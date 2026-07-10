using System;
using System.Collections.Generic;

namespace BankSystem.Models;

public partial class Loan
{
    public int LoanId { get; set; }

    public int CustomerId { get; set; }

    public string? LoanType { get; set; }

    public decimal? LoanAmount { get; set; }

    public decimal? InterestRate { get; set; }

    public int? LoanTerm { get; set; }

    public decimal? MonthlyInstallment { get; set; }

    public string? LoanStatus { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual Customer Customer { get; set; } = null!;
}
