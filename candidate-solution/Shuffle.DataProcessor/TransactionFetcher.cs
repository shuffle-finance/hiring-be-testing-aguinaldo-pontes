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

        public async Task<TransactionResponse> GetTransactions(Guid accountId, int page = 1, int perPage = 100)
        {
            var payload = await _transactionsClient.GetFromJsonAsync<TransactionResponse>($"/accounts/{accountId}/transactions?page={page}&per_page={perPage}");


            return payload;
        }


        public async Task<List<Transactions>> GetAllTransactions(Guid accountId)
        {
            var response = new List<Transactions>();
            int page = 1;
            do
            {
                var payload = await GetTransactions(accountId, page: page++);

                response.AddRange(payload.Transactions);

                if (!payload.Pagination.HasNext)
                    break;
            } while (true);


            return response;
        }
    }

}
