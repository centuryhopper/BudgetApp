using System;
using System.Collections.Generic;

namespace Server.Entities;

public partial class Transaction
{
    public int Transactionsid { get; set; }

    public int? Userid { get; set; }

    public string? Details { get; set; }

    public DateOnly? Postingdate { get; set; }

    public string? Description { get; set; }

    public decimal? Amount { get; set; }

    public string? Type { get; set; }

    public decimal? Balance { get; set; }

    public int? Checkorslip { get; set; }

    public virtual User? User { get; set; }
}
