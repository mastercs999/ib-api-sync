using Common;
using Common.Loggers;
using Common.Extensions;
using IbApiSync.ApiWrapper;
using IbApiSync.Models;
using IbApiSync.Support;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IbApiSync
{
    public class ApiClient
    {
        private string AccountName;
        private int Port;
        private int ClientId;
        private ILogger _____________________________________________________________________________Logger;
        private IBWrapper IBWrapper;

        private int NextOrderId = -1;

        public ApiClient(string accountName, int clientId, int port, ILogger logger)
        {
            AccountName = accountName;
            ClientId = clientId;
            Port = port;
            _____________________________________________________________________________Logger = logger;

            IBWrapper = new IBWrapper(AccountName, ClientId, Port, _____________________________________________________________________________Logger);
        }

        public void Connect()
        {
            IBWrapper.Connect();
        }
        public void ConnectSafe()
        {
            try
            {
                // First check connection
                _____________________________________________________________________________Logger.Info("Checking internet connection by accessing https://www.interactivebrokers.com...");
                using (WebClient webClient = Utils.PrepareBrowserWebClient())
                    webClient.DownloadData("https://www.interactivebrokers.com");
                _____________________________________________________________________________Logger.Info("No exception, internet connection should be fine");

                // Shutdown previous conneciton if exists
                if (IBWrapper.IsConnected())
                {
                    _____________________________________________________________________________Logger.Info("We are already connected. To be sure we'll reconnect. Disconnecting now...");
                    IBWrapper.Disconnect();
                    _____________________________________________________________________________Logger.Info("Disconnection was made");
                }
                else
                    _____________________________________________________________________________Logger.Info("No need to disconnect");

                // Connect again
                IBWrapper.Connect();

                // Get something
                IBWrapper.GetServerTime();
            }
            catch (Exception ex)
            {
                throw new ConnectionException("Somewthing went wrong during safe connection. See inner exception!", ex);
            }
        }
        public void Disconnect()
        {
            IBWrapper.Disconnect();
        }

        public Product FindProduct(string symbol, ProductType productType)
        {
            return FindProduct(symbol, productType, "SMART");
        }
        public Product FindProduct(string symbol, ProductType productType, string exchange)
        {
            return FindProduct(symbol, productType, exchange, "USD");
        }
        public Product FindProduct(string symbol, ProductType productType, string exchange, string currency)
        {
            return IBWrapper.FindProduct(symbol, productType, exchange, currency);
        }

        public void PlaceOrder(Order order)
        {
            PlaceOrder(order, true);
        }
        public void PlaceOrder(Order order, bool waitTillFinishes)
        {
            // Get order id
            int orderId = NextOrderId == -1 ? NextOrderId = IBWrapper.GetNextOrderId() : ++NextOrderId;

            // Find needed product
            _____________________________________________________________________________Logger.Info($"We are about to place an order {orderId} for {order.Product.Symbol} with action {order.Action.Text()} and quantity {order.Quantity}. To do so, we have to find the product...");
            order.Product = FindProduct(order.Product.Symbol, EnumUtilities.GetArray<ProductType>().Single(x => x.Text() == order.Product.IBContract.SecType));
            _____________________________________________________________________________Logger.Info($"We have found the product for the order {orderId}");

            // Place order
            IBWrapper.PlaceOrder(orderId, order, waitTillFinishes);
        }

        public void CancelOrder(Order order)
        {
            IBWrapper.CancelOrder(order);
        }
        public void CancelAllOrders()
        {
            IBWrapper.CancelAllOrders();
        }

        public List<Position> GetAllPositions()
        {
            return IBWrapper.GetAllPositions().Where(x => x.Account == AccountName && Math.Abs(x.Size) > 0m).ToList();
        }

        public Account GetAccountSummary()
        {
            return IBWrapper.GetAccountSummary();
        }
    }
}
