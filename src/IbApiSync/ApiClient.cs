using IbApiSync.ApiWrapper;
using IbApiSync.Models;
using IbApiSync.Support;
using IbApiSync.Support.Exceptions;
using IbApiSync.Support.Loggers;
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
    /// <summary>
    /// This class wraps up common IB API. It provides access to all necessary methods in a nice, simple and sync way.
    /// </summary>
    public class ApiClient
    {
        private string AccountName;
        private int Port;
        private int ClientId;
        private ILogger _____________________________________________________________________________Logger;
        private IBWrapper IBWrapper;

        private int NextOrderId = -1;

        /// <summary>
        /// Creates instance of this class and nothing else.
        /// </summary>
        /// <param name="accountName">Name of the IB accout. e.g.: DU816890</param>
        /// <param name="clientId">Client ID as described in IB docs. For most of cases it will be 0</param>
        /// <param name="port">Port on which IB Gateway is running. Usually 4001 or 4002.</param>
        /// <param name="logger">Instance of logger for events logging. If you don't want any logging, just pass <see cref="IbApiSync.Support.Loggers.SilentLogger"/> here.</param>
        public ApiClient(string accountName, int clientId, int port, ILogger logger)
        {
            AccountName = accountName;
            ClientId = clientId;
            Port = port;
            _____________________________________________________________________________Logger = logger;

            IBWrapper = new IBWrapper(AccountName, ClientId, Port, _____________________________________________________________________________Logger);
        }

        /// <summary>
        /// Connects to IB Gateway
        /// </summary>
        /// <exception cref="IBException">Thrown when account given in constructor was not found.</exception>
        public void Connect()
        {
            IBWrapper.Connect();
        }

        /// <summary>
        /// Connects to IB Gateway, but also performs various check to make sure the connections is valid.
        /// Following actions are taknes: Tries to reach https://www.interactivebrokers.com, disconnects if already connected, gets server time from IB Gateway.
        /// </summary>
        /// <exception cref="ConnectionException">Thrown if any check fails.</exception>
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

        /// <summary>
        /// Disconnects from IB Gateway
        /// </summary>
        public void Disconnect()
        {
            IBWrapper.Disconnect();
        }




        /// <summary>
        /// Searches for product by given symbol and product type. By default searches on SMART exchange and in USD currency.
        /// </summary>
        /// <param name="symbol">Name of the product</param>
        /// <param name="productType">Type of the product (stock, CFD)</param>
        /// <returns>Instance of the product</returns>
        /// <exception cref="InvalidOperationException">Thrown when no such product or more products are found.</exception>
        public Product FindProduct(string symbol, ProductType productType)
        {
            return FindProduct(symbol, productType, "SMART");
        }

        /// <summary>
        /// Looks up specified product in USD currency.
        /// </summary>
        /// <param name="symbol">Name of the product</param>
        /// <param name="productType">Type of the product (stock, CFD)</param>
        /// <param name="exchange">Name of the exchange where the product should be traded.</param>
        /// <returns>Instance of the product</returns>
        /// <exception cref="InvalidOperationException">Thrown when no such product or more products are found.</exception>
        public Product FindProduct(string symbol, ProductType productType, string exchange)
        {
            return FindProduct(symbol, productType, exchange, "USD");
        }

        /// <summary>
        /// Looks up specified product.
        /// </summary>
        /// <param name="symbol">Name of the product</param>
        /// <param name="productType">Type of the product (stock, CFD)</param>
        /// <param name="exchange">Name of the exchange where the product should be traded.</param>
        /// <param name="currency">Name of the product's currency. Usually USD.</param>
        /// <returns>Instance of the product</returns>
        /// <exception cref="InvalidOperationException">Thrown when no such product or more products are found.</exception>
        public Product FindProduct(string symbol, ProductType productType, string exchange, string currency)
        {
            return IBWrapper.FindProduct(symbol, productType, exchange, currency);
        }




        /// <summary>
        /// Places an order for the connected account. This function call may take more time then you expect, because it waits until
        /// the order ends - filled, cancelled etc.
        /// </summary>
        /// <param name="order">Instance of any order inhereiting from <see cref="Order"/>.</param>
        public void PlaceOrder(Order order)
        {
            PlaceOrder(order, true);
        }

        /// <summary>
        /// Places an order for the connected account.
        /// </summary>
        /// <param name="order">Instance of any order inhereiting from <see cref="Order"/>.</param>
        /// <param name="waitTillFinishes">True if the function should wait until order ends - cancelled, filled etc.</param>
        public void PlaceOrder(Order order, bool waitTillFinishes)
        {
            // Get order id
            int orderId = NextOrderId == -1 ? NextOrderId = IBWrapper.GetNextOrderId() : ++NextOrderId;

            // Find needed product
            _____________________________________________________________________________Logger.Info($"We are about to place an order {orderId} for {order.Product.Symbol} with action {order.Action.Text()} and quantity {order.Quantity}. To do so, we have to find the product...");
            order.Product = FindProduct(order.Product.Symbol, Utils.GetEnumArray<ProductType>().Single(x => x.Text() == order.Product.IBContract.SecType));
            _____________________________________________________________________________Logger.Info($"We have found the product for the order {orderId}");

            // Place order
            IBWrapper.PlaceOrder(orderId, order, waitTillFinishes);
        }




        /// <summary>
        /// Cancels specified order.
        /// </summary>
        /// <param name="order">Instance of the order to be cancelled.</param>
        public void CancelOrder(Order order)
        {
            IBWrapper.CancelOrder(order);
        }

        /// <summary>
        /// Cancels all submitted orders.
        /// </summary>
        public void CancelAllOrders()
        {
            IBWrapper.CancelAllOrders();
        }




        /// <summary>
        /// Gets all opened positions for this account.
        /// </summary>
        /// <returns>List of positions with non-zero size.</returns>
        public List<Position> GetAllPositions()
        {
            return IBWrapper.GetAllPositions().Where(x => x.Account == AccountName && Math.Abs(x.Size) > 0m).ToList();
        }




        /// <summary>
        /// Gets account details about the account. 
        /// </summary>
        /// <returns>Object with all finance information about the account.</returns>
        public Account GetAccountSummary()
        {
            return IBWrapper.GetAccountSummary();
        }
    }
}
