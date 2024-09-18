using System.Data;
using Newtonsoft.Json;

namespace Shared.Models;


public class ChaseTransactionsDTO
{
    public int Transactionsid { get; set; }
    public int? Userid { get; set; }
    public string Details { get; set; }

    public DateOnly? PostingDate { get; set; }

    public string Description { get; set; }

    public string? Amount { get; set; }

    public string Type { get; set; }

    public string? Balance { get; set; }

    public string? CheckOrSlip { get; set; }
}




