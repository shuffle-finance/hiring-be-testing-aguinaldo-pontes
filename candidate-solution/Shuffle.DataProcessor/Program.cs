using Shuffle.DataProcessor;

var fetcher = new TransactionFetcher();

var accounts = await fetcher.GetAccounts();

Console.WriteLine(accounts.Count);