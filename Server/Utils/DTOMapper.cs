
using System.Transactions;
using Shared.Models;
using Transaction = Server.Entities.Transaction;


namespace Server.Utils;

public static class DTOMapper
{
    public static ChaseTransactionsDTO ToChaseTransactionsDTO(this Transaction transaction)
    {
        return new()
        {
            Transactionsid = transaction.Transactionsid,
            Details = transaction.Details,
            PostingDate = transaction.Postingdate,
            Description = transaction.Description,
            Amount = transaction.Amount.ToString(),
            Type = transaction.Type,
            Balance = transaction.Balance.ToString(),
            CheckOrSlip = transaction.Checkorslip.ToString(),
        };
    }

    public static Transaction ToTransactions(this ChaseTransactionsDTO dto)
    {
        return new()
        {
            Transactionsid = dto.Transactionsid,
            Details = dto.Details,
            Postingdate = dto.PostingDate,
            Description = dto.Description,
            Amount = string.IsNullOrWhiteSpace(dto.Amount) ? null : Convert.ToDecimal(dto.Amount),
            Type = dto.Type,
            Balance = string.IsNullOrWhiteSpace(dto.Balance) ? null : Convert.ToDecimal(dto.Balance),
            Checkorslip = string.IsNullOrWhiteSpace(dto.CheckOrSlip) ? null : Convert.ToInt32(dto.CheckOrSlip),
        };
    }
}
