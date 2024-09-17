using System.Data;
using Newtonsoft.Json;

namespace Shared.Models;


public class ChaseTransactionsDTO
{
    public string Details { get; set; }

    public DateOnly PostingDate { get; set; }

    public string Description { get; set; }

    public float Amount { get; set; }

    public string Type { get; set; }

    public float? Balance { get; set; }

    public string? CheckOrSlip { get; set; }
}




