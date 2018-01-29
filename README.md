# ib-api-sync
C# library for sync use of IB API. It wraps up basic methods for trading by nicer interface. In conjunction with [Gateway Controller](https://github.com/mastercs999/gateway-controller) you can control your entire trading from C#.

## Features
* Easy sync use
* Error handling
* Extensive logging
* Library and IB API uses decimals to prevent any inaccuracy
* Whole library uses DateTimeOffset in UTC which prevents any inconvenience when dealing with date and time
* You can get trading hours of any product to determine next trading day

## Cons
* Not all IB API functions are supported. If you lack something, please open an issue. Otherwise you can implement the function yourself. It should be quite straightforward :)
* Not all IB API functions were properly tested. Please make sure it works as you expect before deployment.

## Example
Example of basic usage. This you can also find in [Program.cs](src/ExampleCli/Program.cs).
```cs
// Create api instance
ApiClient api = new ApiClient("DUXXXXXX", 0, 4002, new SilentLogger());

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
```

## License
This library was developed under MIT license. IB API code itself subjects to [IB API Non-Commercial License](http://interactivebrokers.github.io/).
