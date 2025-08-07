using System.Net.Http.Json;

namespace Shuffle.DataProcessor
{
    internal class TransactionFetcher
    {
        private static HttpClient _transactionsClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:8000")
        };


        public async Task<List<Guid>> GetAccounts()
        {
            var accounts = await  _transactionsClient.GetFromJsonAsync<AccountResponse>("/accounts");

            return accounts.Accounts;
        }

        public async Task<TransactionResponse> GetTransactions(Guid accountId)
        {
            var payload = await _transactionsClient.GetFromJsonAsync<TransactionResponse>($"/accounts/{accountId}/transactions");


            return payload;
        }
    }

}
