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

        public async Task<TransactionResponse> GetTransactions(Guid accountId, int limit = 50, int offset = 0)
        {
            var payload = await _transactionsClient.GetFromJsonAsync<TransactionResponse>($"/accounts/{accountId}/transactions?limit={limit}&offset={offset}");


            return payload;
        }
    }

}
