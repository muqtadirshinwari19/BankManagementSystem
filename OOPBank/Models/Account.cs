using System;
using System.Collections.Generic;

namespace BankSystem.Models;

public partial class Account
{
    public int AccountId { get; set; }

    public int CustomerId { get; set; }

    public string AccountNumber { get; set; } = null!;

    public string AccountType { get; set; } = null!;

    public decimal? Balance { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? Status { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
