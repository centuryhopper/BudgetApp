
using Microsoft.AspNetCore.Identity;
using Shared.Models;
using static Shared.Models.ServiceResponses;

public interface ITransactionsRepository<T>
{
    Task<IEnumerable<T>> GetAllTransactionsAsync();
    Task<GeneralResponse> PostTransaction(T data);
    Task<IEnumerable<T>> GetTransactionsByKeywordsAsync(IEnumerable<string> keywords);
}