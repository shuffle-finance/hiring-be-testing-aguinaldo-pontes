using Shuffle.DataProcessor;

var fetcher = new TransactionFetcher();

var accounts = await fetcher.GetAccounts();

foreach (var account in accounts)
{
    var transactions = await fetcher.GetTransactions(account);
    Console.WriteLine(transactions.Transactions);
}