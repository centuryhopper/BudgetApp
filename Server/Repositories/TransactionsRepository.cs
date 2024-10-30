
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

    private async Task<ChaseTransactionsDTO?> GetTransactionAsync(ChaseTransactionsDTO dto)
    {
        return (await BudgetDBContext.Transactions.FirstOrDefaultAsync(t => 
            t.Postingdate == dto.PostingDate &&
            t.Description == dto.Description &&
            t.Amount.ToString() == dto.Amount
        ))?.ToChaseTransactionsDTO();
    }

    public async Task<GeneralResponse> PostTransaction(ChaseTransactionsDTO dto)
    {
        try
        {
            // do not allow duplicates. We can check for duplicates with column combos
            var transaction = await GetTransactionAsync(dto);

            if (transaction is not null)
            {
                throw new Exception("transaction already exists for: " + transaction.Description);
            }

            await BudgetDBContext.Transactions.AddAsync(dto.ToTransactions());
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

    public async Task<IEnumerable<ChaseTransactionsDTO>> GetTransactionsInCurrentMonth(IEnumerable<string> keywords)
    {
        var query = BudgetDBContext.Transactions.AsQueryable();
        query = query.Where(t => 
        keywords.Any(keyword => t.Description.ToLower().Contains(keyword.ToLower()))
        &&
        t.Postingdate.GetValueOrDefault().Month == DateTime.Now.Month
        );
        return await query.Select(t=>t.ToChaseTransactionsDTO()).ToListAsync();
    }

    public async Task<IEnumerable<decimal>> GetTransactionsInCurrentYear(IEnumerable<string> keywords)
    {
        var query = BudgetDBContext.Transactions.AsQueryable();
        query = query.Where(t =>
            keywords.Any(keyword => t.Description.ToLower().Contains(keyword.ToLower()))
            && 
            t.Postingdate.HasValue && t.Postingdate.Value.Year == DateTime.Now.Year
        );

        List<decimal> data = Enumerable.Repeat(0m, 12).ToList();
        var groups = query.GroupBy(t => new {t.Postingdate.Value.Month}).ToArray();

        foreach (var group in groups)
        {
            //group.ToList().ForEach(t => System.Console.WriteLine(Math.Abs(t.Amount.Value)));
            data[group.Key.Month - 1] = group.Sum(t => Math.Abs(t.Amount.Value));
        }

        return data;
    }
}

