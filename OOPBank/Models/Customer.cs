using System;
using System.Collections.Generic;

namespace BankSystem.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? Email { get; set; }

    public string Phone { get; set; } = null!;

    public string Cnic { get; set; } = null!;

    public string? Gender { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public string? Address { get; set; }

    public string? City { get; set; }

    public string? Country { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string? Status { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();

    public virtual ICollection<Card> Cards { get; set; } = new List<Card>();

    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
