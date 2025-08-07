using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shuffle.DataProcessor
{
    public class Account
    {
        public Guid Id { get; set; }
    }

    public class AccountResponse
    {
        public List<Guid> Accounts { get; set; }
    }

    public class TransactionResponse 
    { 
        public PaylodMetadata Metadata { get; set; }

        public Payload Payload { get; set; }
    }

    public record Payload(List<Transaction> Pending, List<Transaction> Booked);


    public class Transaction
    {
        public string TransactionId { get; set; }
        public string BookingDate { get; set; }
        public DateTime BookingDateTime { get; set; }
        public TransactionAmount? TransactionAmount { get; set; }
        public CurrencyExchange? CurrencyExchange { get; set; }
        public string CreditorName { get; set; }
        public string RemittanceInformationUnstructured { get; set; }
        public string ProprietaryBankTransactionCode { get; set; }
        public string MerchantCategoryCode { get; set; }
        public string InternalTransactionId { get; set; }
    }


    public record TransactionAmount(decimal Amount, string Currency);
    public record InstructedAmount(decimal Amount, string Currency);
    public record CurrencyExchange(InstructedAmount InstructedAmount, string SourceCurrency, decimal ExchangeRage);
    public record PaylodMetadata(Guid AccountId, Guid RequisitingId, DateTime CreatedAt, Guid TraceId);
}
