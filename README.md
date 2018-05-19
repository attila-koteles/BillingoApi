# A simple API to interact with the Billingo online invoicing system

## Requirements

This library targets [.NET Standard 2.0](https://github.com/dotnet/standard/blob/master/docs/versions/netstandard2.0.md), so you can use it in any of the supported platforms

### Platforms

*   .NET Core 2.0
*   .NET Framework 4.6.1
*   Mono 5.4
*   Xamarin.iOS 10.14
*   Xamarin.Mac 3.8
*   Xamarin.Android 7.5
*   Universal Windows Platform vNext

## Usage

For now this library is focusing on Query features, reading Blocks / Invoices. You are welcome to contribute with additional features.

### Get all available invoice blocks

```cs
// PublicKey and PrivateKey are string values
Blocks blocks = new Blocks(PublicKey, PrivateKey);
List<Block> blocksList = await blocks.LoadAllAsync();

var expected = new List<Block>
{
    new Block
    {
        Name = "Számlák",
        Prefix = "",
        Uid = 44365521
    },
    new Block
    {
        Name = "Other block",
        Prefix = "B2",
        Uid = 4599810
    },
};
```

### Query Invoices

```cs
// PublicKey and PrivateKey are string values
Invoices invoices = new Invoices(PublicKey, PrivateKey);
List<Invoice> invoiceList = await invoices.QueryAsync(
    blockId: 111111111, 
    fromInvoiceNo: 880, 
    yearStart: 2018);
```

#### Parmeters:

*   `blockId`: optional, the ID of the invoice block for filtering
*   `fromInvoiceNo`: optional, list all the invoices from this number, inclusive
*   `limit`: optional, limit of the results, defaults to 50
*   `yearStart`: optional, which year to list, defaults to current year
*   `startDateAsString`: optional, list invoices from this date, example format: "2018-01-01", defaults to "2010-01-01

### DataLoader<T>

There is a lower level class to handle the API communication and to parse the results. You may use it when you implement additional features. For example the `Blocks` class is using the DataLoader the following way:

```cs
public async Task<List<Block>> LoadAllAsync(int limit = 50)
{
    var l = new DataLoader<Block>(connection, "/invoices/blocks");
    return await l.FetchAllAsync(limit);
}
```    

### Connection

This class handles the authentication using the JWT tokens, based on the Billingo specification. This class is used implicitly by the `DataLoader`, which in turn is used by the `Invoices` and `Blocks` classes

### Models

There are various classes that are modelling the exact same datastructures that are defined by Billingo. When you fetch Invoices, it will return the mapped classes with deep structure.

    Invoice
        List<Items>
            Item
                Vat
        Client
            Address

#### Classes

*   Address
*   Bank
*   Block
*   Client
*   Invoice
*   Item
*   Payment
*   Vat