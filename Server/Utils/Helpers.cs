

using CsvHelper.Configuration;
using Shared.Models;

namespace Server.Utils;

public static class Helpers
{
    public class ChaseTransactionsDTOMapper : ClassMap<ChaseTransactionsDTO>
    {   
        public ChaseTransactionsDTOMapper()
        {
            Map(m=>m.Details).Name("Details");
            Map(m=>m.PostingDate).Name("Posting Date");
            Map(m=>m.Description).Name("Description");
            Map(m=>m.Amount).Name("Amount");
            Map(m=>m.Type).Name("Type");
            Map(m=>m.Balance).Name("Balance");
            Map(m=>m.CheckOrSlip).Name("Check or Slip #");
        }
    }
}