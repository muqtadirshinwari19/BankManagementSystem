using BankSystem.Models;

namespace BankSystem.ViewModels
{
    public class FrontendView
    {
        public int TotalCustomers { get; set; }

        public int TotalAccounts { get; set; }

        public int TotalTransactions { get; set; }

        public decimal TotalBalance { get; set; }

        public List<Transaction> RecentTransactions { get; set; } = new();

        public int TotalLoans { get; set; }

        public int TotalCards { get; set; }

        public List<Account> TopAccounts { get; set; } = new();
    }
}