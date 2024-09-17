
using Microsoft.AspNetCore.Identity;
using Shared.Models;
using static Shared.Models.ServiceResponses;

public interface ITransactionsRepository<T>
{
    Task<IEnumerable<T>> GetAllTransactions();
    Task<GeneralResponse> PostTransaction(T data);
}