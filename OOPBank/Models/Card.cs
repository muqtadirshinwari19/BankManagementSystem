using System;
using System.Collections.Generic;

namespace BankSystem.Models;

public partial class Card
{
    public int CardId { get; set; }

    public int CustomerId { get; set; }

    public string? CardNumber { get; set; }

    public string? CardType { get; set; }

    public DateOnly? ExpiryDate { get; set; }

    public string? Cvv { get; set; }

    public string? CardStatus { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
