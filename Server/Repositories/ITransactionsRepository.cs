
using Microsoft.AspNetCore.Identity;
using Shared.Models;
using static Shared.Models.ServiceResponses;

public interface ITransactionsRepository<T>
{
    Task<IEnumerable<T>> GetTransactionsInCurrentMonth(IEnumerable<string> keywords);
    Task<IEnumerable<decimal>> GetTransactionsInCurrentYear(IEnumerable<string> keywords);
    Task<IEnumerable<decimal>> GetTransactionsByYear(IEnumerable<string> keywords, int year);
    Task<IEnumerable<T>> GetAllTransactionsAsync();
    Task<GeneralResponse> PostTransaction(T data);
    Task<IEnumerable<T>> GetTransactionsByKeywordsAsync(IEnumerable<string> keywords);
}