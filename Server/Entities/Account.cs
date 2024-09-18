using System;
using System.Collections.Generic;

namespace Server.Entities;

public partial class Account
{
    public int Accountid { get; set; }

    public int? Userid { get; set; }

    public string Accounttype { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public virtual User? User { get; set; }
}
