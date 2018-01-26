using Common;
using Common.Loggers;
using Common.Extensions;
using IbApiSync.Models;
using IbApiSync.Support;
using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TimeZoneConverter;
using System.Globalization;

namespace IbApiSync.ApiWrapper
{
    internal class IBWrapper : EWrapper
    {
        // Connection
        private int ClientId;
        private int Port;
        private EReaderSignal Signal = new EReaderMonitorSignal();
        private EClientSocket Socket;
        
        // Synchronizing
        private Dictionary<string, ManualResetEvent> Locks = new Dictionary<string, ManualResetEvent>();
        private static object MessageLock = new object();
        private static Random Rng = new Random();

        // Logging
        private ILogger _____________________________________________________________________________Logger;

        // Results
        private string[] AccountNames;
        private string AccountName;
        private int NextOrderId;
        private Dictionary<int, List<Product>> ProductDetails = new Dictionary<int, List<Product>>();
        private Dictionary<int, IbApiSync.Models.Order> PlacedOrders = new Dictionary<int, IbApiSync.Models.Order>();
        private List<Position> ReceivedPositions = new List<Position>();
        private Account AccountSummary;
        private List<string> AccountSummarySetProperties = new List<string>();
        private DateTimeOffset CurrentTime;

        public IBWrapper(string accountName, int clientId, int port, ILogger logger)
        {
            AccountName = accountName;
            ClientId = clientId;
            Port = port;
            Socket = new EClientSocket(this, Signal)
            {
                AsyncEConnect = false
            };

            _____________________________________________________________________________Logger = logger;
        }

        #region Sync methods

        public void Connect()
        {
            // Connect
            _____________________________________________________________________________Logger.Info("Connecting to IB...");
            Locks[(nameof(connectAck))] = new ManualResetEvent(false);
            Socket.eConnect("127.0.0.1", Port, ClientId);
            Locks[nameof(connectAck)].WaitOne();
            _____________________________________________________________________________Logger.Info("Connecting to IB was successful");

            // Start api
            _____________________________________________________________________________Logger.Info("Starting IB API...");
            Socket.startApi();
            _____________________________________________________________________________Logger.Info("IB API has started");

            // Start reader loop
            _____________________________________________________________________________Logger.Info("Starting listenning to IB messages...");
            Locks[nameof(managedAccounts)] = new ManualResetEvent(false);
            Locks[nameof(nextValidId)] = new ManualResetEvent(false);
            EReader reader = new EReader(Socket, Signal);
            reader.Start();
            new Thread((mainThread) =>
            {
                // Because of parsing we have to set this culture
                Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

                try
                {
                    while (Socket.IsConnected())
                    {
                        Signal.waitForSignal();

                        lock (MessageLock)
                        {
                            reader.processMsgs();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ThreadMessage.ThrownException = ex;
                    (mainThread as Thread).Interrupt();
                }
            })
            { IsBackground = true }.Start(Thread.CurrentThread);
            _____________________________________________________________________________Logger.Info("We are listenning to IB messages now");

            // Wait for starting info
            _____________________________________________________________________________Logger.Info("Waiting for starting info");
            Locks[nameof(managedAccounts)].WaitOne();
            Locks[nameof(nextValidId)].WaitOne();
            _____________________________________________________________________________Logger.Info("Starting info was received");

            // Check if account exists
            if (!AccountNames.Contains(AccountName))
                throw new Exception(String.Format("Given account {0} not found among these accounts found: {1}", AccountName, String.Join(", ", AccountNames)));
        }
        public bool IsConnected()
        {
            return Socket.IsConnected();
        }
        public void Disconnect()
        {
            _____________________________________________________________________________Logger.Info("Disconnecting API client...");
            Locks[nameof(connectionClosed)] = new ManualResetEvent(false);
            Socket.eDisconnect();
            Locks[nameof(connectionClosed)].WaitOne();
            _____________________________________________________________________________Logger.Info("API client was disconnected");

        }

        public Product FindProduct(string symbol, ProductType productType, string exchange, string currency)
        {
            // Start request
            int requestId = Rng.Next();
            _____________________________________________________________________________Logger.Info($"Searching for product: {symbol}, {productType.Text()}, {exchange}, {currency}. Request ID is {requestId}");
            ProductDetails[requestId] = new List<Product>();
            Locks[nameof(contractDetailsEnd) + requestId] = new ManualResetEvent(false);
            Socket.reqContractDetails(requestId, new Contract()
            {
                Symbol = symbol,
                SecType = productType.Text(),
                Exchange = exchange,
                Currency = currency
            });
            Locks[nameof(contractDetailsEnd) + requestId].WaitOne();
            _____________________________________________________________________________Logger.Info("We have found following products:", String.Join("\n", ProductDetails.TryGetValue(requestId, out List<Product> products) ? products.Select(x => x.Symbol) : new string[] { "NONE" }));

            // We should find just one product
            Product product = ProductDetails[requestId].Single();
            ProductDetails.Remove(requestId);

            return product; 
        }

        public int GetNextOrderId()
        {
            _____________________________________________________________________________Logger.Info("Getting next order ID...");
            Locks[nameof(nextValidId)] = new ManualResetEvent(false);
            Socket.reqIds(0);
            Locks[nameof(nextValidId)].WaitOne();
            _____________________________________________________________________________Logger.Info($"Next order ID is {NextOrderId}");

            return NextOrderId;
        }

        public void PlaceOrder(int orderId, IbApiSync.Models.Order order, bool waitTillFinishes)
        {
            // Set account and order id
            order.IBOrderToPlace.Account = AccountName;
            order.IBOrderToPlace.OrderId = orderId;

            // Save order ID into dictionary
            PlacedOrders.Add(orderId, order);

            // Set lock for completing information
            order.FinishLock = new ManualResetEvent(false);
            order.CommissionLock = new ManualResetEvent(false);
            order.AverageFillPriceLock = new ManualResetEvent(false);

            // Place an order
            _____________________________________________________________________________Logger.Info($"Placing the order {orderId}");
            Locks[nameof(openOrder) + orderId] = new ManualResetEvent(false);
            Socket.placeOrder(orderId, order.Product.IBContract, order.IBOrderToPlace);
            Locks[nameof(openOrder) + orderId].WaitOne();
            _____________________________________________________________________________Logger.Info($"We have placed the order {orderId}");

            // Wait until order is complete
            if (waitTillFinishes)
            {
                _____________________________________________________________________________Logger.Info($"We're going to wait till order {orderId} finishes...");
                order.WailTillFinishes();
                _____________________________________________________________________________Logger.Info($"Order {orderId} finished with status {order.Status.Text()}");
            }
            else
                _____________________________________________________________________________Logger.Info("Waiting for order execution wasn't requested");
        }

        public void CancelOrder(Models.Order order)
        {
            _____________________________________________________________________________Logger.Info($"Cancelling an order {order.IBOrderToPlace.OrderId}");
            Socket.cancelOrder(order.IBOrderToPlace.OrderId);
        }

        public void CancelAllOrders()
        {
            _____________________________________________________________________________Logger.Info("Cancelling all orders...");
            Socket.reqGlobalCancel();
        }

        public List<Position> GetAllPositions()
        {
            // Start from the beginning
            ReceivedPositions = new List<Position>();

            // Initiate positions request
            _____________________________________________________________________________Logger.Info("Getting all positions in IB...");
            Locks[nameof(positionEnd)] = new ManualResetEvent(false);
            Socket.reqPositions();
            Locks[nameof(positionEnd)].WaitOne();
            _____________________________________________________________________________Logger.Info("All positions were received. We got this:", String.Join("\n", ReceivedPositions.Select(x => $"{x.Product.Symbol} has size {x.Size}")));

            // End of subscription
            _____________________________________________________________________________Logger.Info("Cancelling all positions subscriptions...");
            Socket.cancelPositions();
            _____________________________________________________________________________Logger.Info("Subscriptions was cancelled");

            // Lose reference
            List<Position> positions = ReceivedPositions;
            ReceivedPositions = new List<Position>();

            return positions;
        }

        public Account GetAccountSummary()
        {
            // Start request
            AccountSummary = new Account();
            AccountSummarySetProperties = new List<string>();

            _____________________________________________________________________________Logger.Info("Getting account summary...");
            Locks[nameof(accountDownloadEnd)] = new ManualResetEvent(false);
            Socket.reqAccountUpdates(true, AccountName);
            Locks[nameof(accountDownloadEnd)].WaitOne();
            _____________________________________________________________________________Logger.Info("We have received account summary:", AccountSummary.Dump());

            // Cancel subscription
            Socket.reqAccountUpdates(false, AccountName);

            // Check whether we have set all properties
            List<string> notSetProperties = typeof(Account).GetProperties().Select(x => x.Name).Except(AccountSummarySetProperties).ToList();
            if (notSetProperties.Any())
                _____________________________________________________________________________Logger.Warning($"Not all properties were set in {nameof(Account)}, specifically these: " + String.Join(", ", notSetProperties));

            // Lose reference
            Account account = AccountSummary;
            AccountSummary = new Account();

            return account;
        }

        public DateTimeOffset GetServerTime()
        {
            _____________________________________________________________________________Logger.Info("Requesting IB time...");
            Locks[nameof(currentTime)] = new ManualResetEvent(false);
            Socket.reqCurrentTime();
            Locks[nameof(currentTime)].WaitOne();
            _____________________________________________________________________________Logger.Info("We have received time: " + CurrentTime);

            return CurrentTime;
        }


        #endregion

        #region API

        public void accountDownloadEnd(string account)
        {
            _____________________________________________________________________________Logger.WriteMethod(nameof(accountDownloadEnd), nameof(account), account);

            Locks[nameof(accountDownloadEnd)].Set();
        }

        public void accountSummary(int reqId, string account, string tag, string value, string currency)
        {
            throw new NotImplementedException();
        }

        public void accountSummaryEnd(int reqId)
        {
            throw new NotImplementedException();
        }

        public void accountUpdateMulti(int requestId, string account, string modelCode, string key, string value, string currency)
        {
            throw new NotImplementedException();
        }

        public void accountUpdateMultiEnd(int requestId)
        {
            throw new NotImplementedException();
        }

        public void bondContractDetails(int reqId, ContractDetails contract)
        {
            throw new NotImplementedException();
        }

        public void commissionReport(CommissionReport commissionReport)
        {
            _____________________________________________________________________________Logger.WriteMethod(nameof(commissionReport), nameof(commissionReport), commissionReport);
        }

        public void connectAck()
        {
            _____________________________________________________________________________Logger.WriteMethod(nameof(connectAck));

            Locks[nameof(connectAck)].Set();
        }

        public void connectionClosed()
        {
            _____________________________________________________________________________Logger.WriteMethod(nameof(connectionClosed));

            Locks[nameof(connectionClosed)].Set();
        }

        public void contractDetails(int reqId, ContractDetails contractDetails)
        {
            _____________________________________________________________________________Logger.WriteMethod(nameof(contractDetails), nameof(reqId), reqId, nameof(contractDetails), contractDetails);

            // Parse trading hours
            List<DateRange> tradingHours = new List<DateRange>();
            if (contractDetails.LiquidHours != null)
                foreach (string dateStr in contractDetails.LiquidHours.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    // Skip if it is closed this date
                    if (dateStr.Substring(9) == "CLOSED")
                        continue;

                    // Check length
                    if (dateStr.Length != 18)
                        throw new Exception("Unknown liquid hours datetime format.");

                    // Parse, example: 20170515:0930-1600
                    int year = int.Parse(dateStr.Substring(0, 4));
                    int month = int.Parse(dateStr.Substring(4, 2));
                    int day = int.Parse(dateStr.Substring(6, 2));
                    int hourFrom = int.Parse(dateStr.Substring(9, 2));
                    int minutesFrom = int.Parse(dateStr.Substring(11, 2));
                    int hourTo = int.Parse(dateStr.Substring(14, 2));
                    int minutesTo = int.Parse(dateStr.Substring(16, 2));

                    // Is it today?
                    DateTime from = new DateTime(year, month, day, hourFrom, minutesFrom, 0);
                    DateTime to = new DateTime(year, month, day, hourTo, minutesTo, 0);

                    // Convert to UTC
                    TimeZoneInfo sourceTimeZone = TimeZoneInfo.FindSystemTimeZoneById(TZConvert.IanaToWindows(contractDetails.TimeZoneId));
                    from = TimeZoneInfo.ConvertTimeToUtc(from, sourceTimeZone);
                    to = TimeZoneInfo.ConvertTimeToUtc(to, sourceTimeZone);

                    // Save
                    tradingHours.Add(new DateRange()
                    {
                        From = from,
                        To = to
                    });
                }

            // Set the product
            ProductDetails[reqId].Add(new Product()
            {
                Id = contractDetails.Summary.ConId,
                Symbol = contractDetails.Summary.Symbol,
                Exchange = contractDetails.Summary.Exchange,
                Currency = contractDetails.Summary.Currency,
                TradingHours = tradingHours,

                IBContract = contractDetails.Summary,
                IBContractDetails = contractDetails
            });
        }

        public void contractDetailsEnd(int reqId)
        {
            _____________________________________________________________________________Logger.WriteMethod(nameof(contractDetailsEnd), nameof(reqId), reqId);

            Locks[nameof(contractDetailsEnd) + reqId].Set();
        }

        public void currentTime(long time)
        {
            _____________________________________________________________________________Logger.WriteMethod(nameof(currentTime), nameof(time), time);

            // Convert UNIX time to DateTimeOffset
            CurrentTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(time);

            Locks[nameof(currentTime)].Set();
        }

        public void deltaNeutralValidation(int reqId, UnderComp underComp)
        {
            throw new NotImplementedException();
        }

        public void displayGroupList(int reqId, string groups)
        {
            throw new NotImplementedException();
        }

        public void displayGroupUpdated(int reqId, string contractInfo)
        {
            throw new NotImplementedException();
        }

        public void error(Exception e)
        {
            _____________________________________________________________________________Logger.WriteMethod(nameof(error), nameof(e), e.Message + " ___ " + e.InnerException?.Message);

            throw e;
        }

        public void error(string str)
        {
            _____________________________________________________________________________Logger.WriteMethod(nameof(error), nameof(str), str);

            throw new IBException(str);
        }

        public void error(int id, int errorCode, string errorMsg)
        {
            _____________________________________________________________________________Logger.WriteMethod(nameof(error), nameof(id), id, nameof(errorCode), errorCode, nameof(errorMsg), errorMsg);

            // This is just info message
            if (id == -1 && errorCode == 2100 && errorMsg == "API client has been unsubscribed from account data.")
                return;

            throw new IBException(errorMsg, id, errorCode);
        }

        public void execDetails(int reqId, Contract contract, Execution execution)
        {
            _____________________________________________________________________________Logger.WriteMethod(nameof(execDetails), nameof(reqId), reqId, nameof(contract), contract, nameof(execution), execution);

            if (PlacedOrders.ContainsKey(execution.OrderId))
            {
                Models.Order order = PlacedOrders[execution.OrderId];

                order.AverageFillPrice = execution.AvgPrice;
                order.AverageFillPriceLock.Set();

                if (execution.CumQty == order.Quantity)
                {
                    order.Status = OrderStatus.Filled;
                    if (order.Status.IsLastStatus())
                        order.FinishLock.Set();
                }
            }
        }

        public void execDetailsEnd(int reqId)
        {
            throw new NotImplementedException();
        }

        public void fundamentalData(int reqId, string data)
        {
            throw new NotImplementedException();
        }

        public void historicalData(int reqId, string date, decimal open, decimal high, decimal low, decimal close, int volume, int count, decimal WAP, bool hasGaps)
        {
            throw new NotImplementedException();
        }

        public void historicalDataEnd(int reqId, string start, string end)
        {
            throw new NotImplementedException();
        }

        public void managedAccounts(string accountsList)
        {
            _____________________________________________________________________________Logger.WriteMethod(nameof(managedAccounts), nameof(accountsList), accountsList);

            AccountNames = accountsList.Split(',');

            Locks[nameof(managedAccounts)].Set();
        }

        public void marketDataType(int reqId, int marketDataType)
        {
            throw new NotImplementedException();
        }

        public void nextValidId(int orderId)
        {
            _____________________________________________________________________________Logger.WriteMethod(nameof(nextValidId), nameof(orderId), orderId);

            NextOrderId = orderId;

            Locks[nameof(nextValidId)].Set();
        }

        public void openOrder(int orderId, Contract contract, IBApi.Order order, OrderState orderState)
        {
            _____________________________________________________________________________Logger.WriteMethod(nameof(openOrder), nameof(orderId), orderId, nameof(contract), contract, nameof(order), order, nameof(orderState), orderState);

            if (PlacedOrders.ContainsKey(orderId))
            {
                Models.Order modelOrder = PlacedOrders[orderId];

                OrderStatus orderStatus = EnumUtilities.GetArray<OrderStatus>().Single(x => x.Text() == orderState.Status);
                if (orderStatus.IsLastStatus() && orderState.Commission != decimal.MaxValue)
                {
                    modelOrder.Commission = orderState.Commission;
                    modelOrder.CommissionLock.Set();
                }

                modelOrder.Status = orderStatus;
                if (modelOrder.Status.IsLastStatus())
                    modelOrder.FinishLock.Set();
            }

            Locks[nameof(openOrder) + orderId].Set();
        }

        public void openOrderEnd()
        {
            throw new NotImplementedException();
        }

        public void orderStatus(int orderId, string status, decimal filled, decimal remaining, decimal avgFillPrice, int permId, int parentId, decimal lastFillPrice, int clientId, string whyHeld)
        {
            _____________________________________________________________________________Logger.WriteMethod(nameof(orderStatus), nameof(orderId), orderId, nameof(status), status, nameof(filled), filled, nameof(remaining), remaining, nameof(avgFillPrice), avgFillPrice, nameof(permId), permId, nameof(parentId), parentId, nameof(lastFillPrice), lastFillPrice, nameof(clientId), clientId, nameof(whyHeld), whyHeld);

            if (PlacedOrders.ContainsKey(orderId))
            {
                Models.Order order = PlacedOrders[orderId];

                order.AverageFillPrice = avgFillPrice;
                if (order.AverageFillPrice != 0)
                    order.AverageFillPriceLock.Set();

                order.Status = EnumUtilities.GetArray<OrderStatus>().Single(x => x.Text() == status);
                if (order.Status.IsLastStatus())
                    order.FinishLock.Set();
            }
        }

        public void position(string account, Contract contract, decimal pos, decimal avgCost)
        {
            _____________________________________________________________________________Logger.WriteMethod(nameof(position), nameof(account), account, nameof(contract), contract, nameof(pos), pos, nameof(avgCost), avgCost);

            ReceivedPositions.Add(new Position()
            {
                Product = new Product()
                {
                    Id = contract.ConId,
                    Symbol = contract.Symbol,
                    Exchange = contract.Exchange,

                    IBContract = contract
                },
                Size = pos,
                UnitCost = avgCost,

                Account = account
            });
        }

        public void positionEnd()
        {
            _____________________________________________________________________________Logger.WriteMethod(nameof(positionEnd));

            Locks[nameof(positionEnd)].Set();
        }

        public void positionMulti(int requestId, string account, string modelCode, Contract contract, decimal pos, decimal avgCost)
        {
            throw new NotImplementedException();
        }

        public void positionMultiEnd(int requestId)
        {
            throw new NotImplementedException();
        }

        public void realtimeBar(int reqId, long time, decimal open, decimal high, decimal low, decimal close, long volume, decimal WAP, int count)
        {
            throw new NotImplementedException();
        }

        public void receiveFA(int faDataType, string faXmlData)
        {
            throw new NotImplementedException();
        }

        public void scannerData(int reqId, int rank, ContractDetails contractDetails, string distance, string benchmark, string projection, string legsStr)
        {
            throw new NotImplementedException();
        }

        public void scannerDataEnd(int reqId)
        {
            throw new NotImplementedException();
        }

        public void scannerParameters(string xml)
        {
            throw new NotImplementedException();
        }

        public void securityDefinitionOptionParameter(int reqId, string exchange, int underlyingConId, string tradingClass, string multiplier, HashSet<string> expirations, HashSet<decimal> strikes)
        {
            throw new NotImplementedException();
        }

        public void securityDefinitionOptionParameterEnd(int reqId)
        {
            throw new NotImplementedException();
        }

        public void softDollarTiers(int reqId, SoftDollarTier[] tiers)
        {
            throw new NotImplementedException();
        }

        public void tickEFP(int tickerId, int tickType, decimal basisPoints, string formattedBasisPoints, decimal impliedFuture, int holdDays, string futureLastTradeDate, decimal dividendImpact, decimal dividendsToLastTradeDate)
        {
            throw new NotImplementedException();
        }

        public void tickGeneric(int tickerId, int field, decimal value)
        {
            throw new NotImplementedException();
        }

        public void tickOptionComputation(int tickerId, int field, decimal impliedVolatility, decimal delta, decimal optPrice, decimal pvDividend, decimal gamma, decimal vega, decimal theta, decimal undPrice)
        {
            throw new NotImplementedException();
        }

        public void tickPrice(int tickerId, int field, decimal price, int canAutoExecute)
        {
            throw new NotImplementedException();
        }

        public void tickSize(int tickerId, int field, int size)
        {
            throw new NotImplementedException();
        }

        public void tickSnapshotEnd(int tickerId)
        {
            throw new NotImplementedException();
        }

        public void tickString(int tickerId, int field, string value)
        {
            throw new NotImplementedException();
        }

        public void updateAccountTime(string timestamp)
        {
            _____________________________________________________________________________Logger.WriteMethod(nameof(updateAccountTime), nameof(timestamp), timestamp);
        }

        public void updateAccountValue(string key, string value, string currency, string accountName)
        {
            _____________________________________________________________________________Logger.WriteMethod(nameof(updateAccountValue), nameof(key), key, nameof(value), value, nameof(currency), currency, nameof(accountName), accountName);

            // Check if accountName is correct
            if (accountName != AccountName)
                throw new ArgumentException($"We received information for account {accountName} even though we requested account information for {AccountName}");

            // Parse value
            PropertyInfo property = typeof(Account).GetProperty(key.Replace('-', '_').Replace('+', '_'));
            object parsedValue = Utils.ParseValue(property.PropertyType, value);

            // Set property value
            property.SetValue(AccountSummary, parsedValue);

            // Note that this property was set
            AccountSummarySetProperties.Add(property.Name);
        }

        public void updateMktDepth(int tickerId, int position, int operation, int side, decimal price, int size)
        {
            throw new NotImplementedException();
        }

        public void updateMktDepthL2(int tickerId, int position, string marketMaker, int operation, int side, decimal price, int size)
        {
            throw new NotImplementedException();
        }

        public void updateNewsBulletin(int msgId, int msgType, string message, string origExchange)
        {
            _____________________________________________________________________________Logger.WriteMethod(nameof(updateNewsBulletin), nameof(msgId), msgId, nameof(msgType), msgType, nameof(message), message, nameof(origExchange), origExchange);
        }

        public void updatePortfolio(Contract contract, decimal position, decimal marketPrice, decimal marketValue, decimal averageCost, decimal unrealisedPNL, decimal realisedPNL, string accountName)
        {
            _____________________________________________________________________________Logger.WriteMethod(nameof(updatePortfolio), nameof(contract), contract, nameof(position), position, nameof(marketPrice), marketPrice, nameof(marketValue), marketValue, nameof(averageCost), averageCost, nameof(unrealisedPNL), unrealisedPNL, nameof(realisedPNL), realisedPNL, nameof(accountName), accountName);
        }

        public void verifyAndAuthCompleted(bool isSuccessful, string errorText)
        {
            throw new NotImplementedException();
        }

        public void verifyAndAuthMessageAPI(string apiData, string xyzChallenge)
        {
            throw new NotImplementedException();
        }

        public void verifyCompleted(bool isSuccessful, string errorText)
        {
            throw new NotImplementedException();
        }

        public void verifyMessageAPI(string apiData)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
