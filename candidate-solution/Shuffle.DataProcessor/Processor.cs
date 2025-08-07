using System.Text.Json;

namespace Shuffle.DataProcessor
{
    public class Processor
    {
        private static int TransactionId = 1;
        private readonly  TransactionFetcher _fetcher = new();
        public Dictionary<Guid, List<Transaction>> _transactions = new();
        public Dictionary<Guid, User> _users = new();
        public List<Transactions> _rawTransactions = new();

        public Dictionary<User, List<ProcessedTransaction>> _processed = new();

        public async Task Process()
        {
            var accounts = await _fetcher.GetAccounts();

            foreach (var account in accounts)
            {
                var user = new User { Id = account };
                _users[account] = user;
                var userTransactions = new List<Transaction>();

                var transactions = await _fetcher.GetAllTransactions(account);

                foreach (var transaction in transactions)
                {
                    _rawTransactions.Add(transaction);
                    foreach (var booked in transaction.Payload.Booked)
                    {
                        booked.TransactionType = TransactionType.Booked;
                        userTransactions.Add(booked);
                    }

                    foreach (var pending in transaction.Payload.Pending)
                    {
                        pending.TransactionType = TransactionType.Pending;
                        userTransactions.Add(pending);
                    }

                }

                _transactions[account] = userTransactions;

            }


            foreach (var userTransaction in _transactions)
            {
                var userId = userTransaction.Key;
                var transactions = userTransaction.Value;

                var deduplicated = transactions.OrderByDescending(t => t.TransactionType).DistinctBy(t => new { t.BookingDate, t.TransactionAmount.Amount, t.RemittanceInformationUnstructured });
                var userTransactions = new List<ProcessedTransaction>();

                ProcessedTransaction lastTransaction = null;
                int count = 0;
                decimal balance = 0;
                foreach (var transaction in deduplicated)
                {
                    var processedTransaction = new ProcessedTransaction
                    {
                        Id = TransactionId++,
                        UserId = userId,
                        Amount = transaction.TransactionAmount.Amount,
                        Currency = transaction.TransactionAmount.Currency,
                        TimeStamp = DateTime.Parse(transaction.BookingDate),
                        Description = transaction.RemittanceInformationUnstructured,
                        CreatedAt = DateTime.UtcNow,
                        Type = transaction.RemittanceInformationUnstructured,
                        Status = transaction.TransactionType.ToString(),
                        RawData = JsonSerializer.Serialize(transaction),
                    };

                    count++;
                    balance += processedTransaction.Amount;
                    lastTransaction = processedTransaction;
                    userTransactions.Add(lastTransaction);
                }

                var user = new User
                {
                    Currency = lastTransaction.Currency,
                    Id = userId,
                    LastUpdated = lastTransaction.TimeStamp,
                    Balance = balance,
                    TransactionCount = count
                };

                _processed[user] = userTransactions;
            }
        }
    }
}

public class User
{
    public Guid Id { get; set; }
    public decimal Balance { get; set; }
    public string Currency { get; set; }
    public int TransactionCount { get; set; }
    public DateTime LastUpdated { get; set; }
}

public class ProcessedTransaction
{
    public long Id { get; set; }
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public DateTime TimeStamp { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
    public string Type { get; set; }
    public string RawData { get; set; }
    public DateTime CreatedAt { get; set; }
}