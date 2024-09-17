
using Shared.Models;

namespace Server.Repositories;

public class TransactionsRepository : ITransactionsRepository<ChaseTransactionsDTO>
{
    public Task<IEnumerable<ChaseTransactionsDTO>> GetAllTransactions()
    {
        throw new NotImplementedException();
    }

    public Task<ServiceResponses.GeneralResponse> PostTransaction(ChaseTransactionsDTO data)
    {
        throw new NotImplementedException();
    }
}

