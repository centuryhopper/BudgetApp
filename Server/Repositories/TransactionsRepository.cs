
using Microsoft.EntityFrameworkCore;
using Server.Contexts;
using Server.Utils;
using Shared.Models;
using static Shared.Models.ServiceResponses;
using Transaction = Server.Entities.Transaction;

namespace Server.Repositories;

public class TransactionsRepository(BudgetDBContext BudgetDBContext) : ITransactionsRepository<ChaseTransactionsDTO>
{
    public async Task<IEnumerable<ChaseTransactionsDTO>> GetAllTransactionsAsync()
    {
        return await BudgetDBContext.Transactions.Select(t => t.ToChaseTransactionsDTO()).ToListAsync();
    }

    public async Task<GeneralResponse> PostTransaction(ChaseTransactionsDTO data)
    {
        try
        {
            await BudgetDBContext.Transactions.AddAsync(data.ToTransactions());
            await BudgetDBContext.SaveChangesAsync();
        }
        catch (System.Exception ex)
        {
            return new GeneralResponse(Flag: false, Message: ex.Message);
        }

        return new GeneralResponse(Flag: true, Message: "Transaction successfully added!");
    }

    public async Task<IEnumerable<ChaseTransactionsDTO>> GetTransactionsByKeywordsAsync(IEnumerable<string> keywords)
    {
        // Assuming the `Description` field is what you're filtering by
        var query = BudgetDBContext.Transactions.AsQueryable();
        query = query.Where(t => keywords.Any(keyword => t.Description.ToLower().Contains(keyword.ToLower())));
        return await query.Select(t=>t.ToChaseTransactionsDTO()).ToListAsync();
    }
}

