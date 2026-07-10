using BankSystem.Models;
using BankSystem.ViewModels;

namespace BankSystem.Services.Interfaces
{
    public interface IFrontendService
    {
        Task<List<Customer>> GetCustomersAsync();
        Task AddCustomerAsync(Customer customer);
        Task<List<Account>> GetAccountsAsync();
        Task AddAccountAsync(Account account);
        Task<List<Customer>> GetActiveCustomersAsync();
        Task<List<Transaction>> GetTransactionsAsync();
        Task<bool> TransferMoneyAsync(int fromAccountId, int toAccountId, decimal amount, string? note);
        Task<List<Loan>> GetLoansAsync();
        Task AddLoanAsync(Loan loan);
        Task<List<Card>> GetCardsAsync();
        Task AddCardAsync(Card card);
        Task<FrontendView> GetReportDashboardAsync();
        Task<FrontendView> GetDashboardAsync();
    }
}