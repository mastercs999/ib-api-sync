using IbApiSync;
using IbApiSync.Models;
using IbApiSync.Models.Orders;
using IbApiSync.Support;
using IbApiSync.Support.Exceptions;
using IbApiSync.Support.Loggers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CliExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // First start IB Gateway
            // https://github.com/mastercs999/gateway-controller

            // Create api instance
            ApiClient api = new ApiClient("DUXXXXXX", 0, 4002, new SilentLogger());

            // At first you need to connect of course
            api.Connect();

            // Let's demonstrate some basic usage
            BasicUsage(api);

            // Let's demonstrate error handling
            ErrorHandling(api);

            // Let's demonstrate searching for next trading day
            FindNextTradingDay(api);
        }

        private static void BasicUsage(ApiClient api)
        {
            // Get information about your account
            Account account = api.GetAccountSummary();

            // Now find out which position you hold
            List<Position> currentPositions = api.GetAllPositions();

            // Let's say we want to buy 152 shares of AAPL
            // At first we need to find target product
            Product aapl = api.FindProduct("AAPL", ProductType.Stock);

            // Now we can create whatever order we want
            Order order = new Market(aapl, OrderAction.Buy, 152);

            // It's time to place an order - the code doesn't wait till order finishes because of the second parameter.
            api.PlaceOrder(order, false);

            // There can go additional orders
            // ...

            // You placed all orders you wanted (just one order in our case). Now you want to wait till they finish.
            order.WailTillFinishes();

            // But wait, we want to know also all fill details - like commisions and fill price. So instead you may want to call this method:
            order.WaitForExecutionDetails();

            // So what are those fill details?
            Console.WriteLine(order.AverageFillPrice);
            Console.WriteLine(order.Commission);
        }
        private static void ErrorHandling(ApiClient api)
        {
            try
            {
                // We can try to find a product which doesn't exist
                Product product = api.FindProduct("AAPLbbbb", ProductType.Stock);
            }
            catch (Exception ex)
            {
                // The exception comes from different. Look at the agreed location where thrown exception was stored.
                if (ex is ThreadInterruptedException)
                    ex = ThreadMessage.ThrownException;

                // Print exception details
                Console.WriteLine(ex.Message);
                if (ex is IBException ibException)
                {
                    Console.WriteLine($"Id:\t{ibException.Id}");
                    Console.WriteLine($"Code:\t{ibException.ErrorCode}");
                }
            }
        }
        private static void FindNextTradingDay(ApiClient api)
        {
            // Find a product whose trading day we want
            // You can also specify exchange as a third argument
            Product product = api.FindProduct("AAPL", ProductType.Stock);

            // So when is next trading day
            DateRange nextTradingDay = product.TradingHours.OrderBy(x => x.From).First(x => x.From >= DateTimeOffset.UtcNow);

            // Print what we found 
            Console.WriteLine($"From:\t{nextTradingDay.From}");
            Console.WriteLine($"To:\t{nextTradingDay.To}");
        }
    }
}
