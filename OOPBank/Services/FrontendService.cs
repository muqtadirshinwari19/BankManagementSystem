using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.SignalR;
using BankSystem.Services.Interfaces;
using BankSystem.ViewModels;
using BankSystem.Models.OOP;
using BankSystem.Models;
using BankSystem.Hubs;
using BankSystem.Data;

namespace BankSystem.Services
{
    public class FrontendService : IFrontendService
    {
        private readonly IHubContext<NotificationHub> _hubContext;
        private readonly AppDbContext _context;

        public FrontendService(AppDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        #region Home
        public async Task<FrontendView> GetDashboardAsync()
        {
            var model = new FrontendView();

            model.TotalCustomers = await _context.Customers.CountAsync();
            model.TotalAccounts = await _context.Accounts.CountAsync();
            model.TotalTransactions = await _context.Transactions.CountAsync();
            model.TotalLoans = await _context.Loans.CountAsync();
            model.TotalCards = await _context.Cards.CountAsync();
            model.TotalBalance = await _context.Accounts.SumAsync(x => x.Balance) ?? 0;

            model.RecentTransactions = await _context.Transactions
                .Include(x => x.Account)
                .ThenInclude(x => x.Customer)
                .OrderByDescending(x => x.TransactionDate)
                .Take(4)
                .ToListAsync();

            model.TopAccounts = await _context.Accounts
                .Include(x => x.Customer)
                .OrderByDescending(x => x.Balance)
                .Take(5)
                .ToListAsync();

            return model;
        }
        #endregion

        #region Customers
        public async Task<List<Customer>> GetCustomersAsync()
        {
            return await _context.Customers
                .OrderByDescending(x => x.CustomerId)
                .ToListAsync();
        }

        public async Task AddCustomerAsync(Customer customer)
        {
            customer.CreatedDate = DateTime.Now;
            customer.Status = "Active";

            await _context.Customers.AddAsync(customer);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Accounts
        public async Task<List<Account>> GetAccountsAsync()
        {
            return await _context.Accounts
                .Include(x => x.Customer)
                .OrderByDescending(x => x.AccountId)
                .ToListAsync();
        }

        public async Task<List<Customer>> GetActiveCustomersAsync()
        {
            return await _context.Customers
                .Where(x => x.Status == "Active" || x.Status == null || x.Status == "")
                .OrderBy(x => x.FirstName)
                .ToListAsync();
        }

        public async Task AddAccountAsync(Account account)
        {
            account.CreatedDate = DateTime.Now;
            account.Status = "Active";

            if (string.IsNullOrWhiteSpace(account.AccountNumber))
            {
                account.AccountNumber = "AC-" + DateTime.Now.ToString("yyyyMMddHHmmss");
            }

            await _context.Accounts.AddAsync(account);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Transaction
        public async Task<List<Transaction>> GetTransactionsAsync()
        {
            return await _context.Transactions
                .Include(x => x.Account)
                .ThenInclude(x => x.Customer)
                .OrderByDescending(x => x.TransactionId)
                .ToListAsync();
        }
        #endregion

        #region Transfer
        private BankAccount CreateBankAccountObject(Account account)
        {
            if (account.AccountType == "Savings")
            {
                return new SavingsAccount(account.Balance ?? 0);
            }

            return new CurrentAccount(account.Balance ?? 0);
        }

        public async Task<bool> TransferMoneyAsync(int fromAccountId, int toAccountId, decimal amount, string? note)
        {
            if (fromAccountId == toAccountId)
                return false;

            var fromAccountEntity = await _context.Accounts
                .FirstOrDefaultAsync(x => x.AccountId == fromAccountId);

            var toAccountEntity = await _context.Accounts
                .FirstOrDefaultAsync(x => x.AccountId == toAccountId);

            if (fromAccountEntity == null || toAccountEntity == null)
                return false;

            BankAccount fromAccount = CreateBankAccountObject(fromAccountEntity);
            BankAccount toAccount = CreateBankAccountObject(toAccountEntity);

            bool isTransferred = fromAccount.Transfer(toAccount, amount);

            if (!isTransferred)
                return false;

            fromAccountEntity.Balance = fromAccount.GetBalance();
            toAccountEntity.Balance = toAccount.GetBalance();

            var debitTransaction = new Transaction
            {
                AccountId = fromAccountId,
                TransactionType = "Transfer Out",
                Amount = amount,
                Description = note ?? "Fund transferred",
                TransactionDate = DateTime.Now
            };

            var creditTransaction = new Transaction
            {
                AccountId = toAccountId,
                TransactionType = "Transfer In",
                Amount = amount,
                Description = note ?? "Fund received",
                TransactionDate = DateTime.Now
            };

            await _context.Transactions.AddAsync(debitTransaction);
            await _context.Transactions.AddAsync(creditTransaction);

            await _context.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync(
                   "ReceiveNotification",
                   $"Transfer completed: ${amount:N2} has been sent successfully."
               );

            return true;
        }
        #endregion

        #region Loans
        public async Task<List<Loan>> GetLoansAsync()
        {
            return await _context.Loans
                .Include(x => x.Customer)
                .OrderByDescending(x => x.LoanId)
                .ToListAsync();
        }

        public async Task AddLoanAsync(Loan loan)
        {
            loan.CreatedDate = DateTime.Now;
            loan.LoanStatus = "Pending";

            if (loan.LoanAmount > 0 && loan.InterestRate > 0 && loan.LoanTerm > 0)
            {
                var totalInterest = loan.LoanAmount * (loan.InterestRate / 100);
                var totalPayable = loan.LoanAmount + totalInterest;
                loan.MonthlyInstallment = totalPayable / loan.LoanTerm;
            }

            await _context.Loans.AddAsync(loan);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Cards
        #region Cards

        public async Task<List<Card>> GetCardsAsync()
        {
            return await _context.Cards
                .Include(x => x.Customer)
                .OrderByDescending(x => x.CardId)
                .ToListAsync();
        }

        public async Task AddCardAsync(Card card)
        {
            card.CardStatus = "Pending";

            await _context.Cards.AddAsync(card);
            await _context.SaveChangesAsync();
        }

        #endregion
        #endregion

        #region Report
        public async Task<FrontendView> GetReportDashboardAsync()
        {
            var model = new FrontendView();

            model.TotalCustomers = await _context.Customers.CountAsync();

            model.TotalAccounts = await _context.Accounts.CountAsync();

            model.TotalTransactions = await _context.Transactions.CountAsync();

            model.TotalBalance = await _context.Accounts.SumAsync(x => x.Balance) ?? 0;

            model.RecentTransactions = await _context.Transactions
                .Include(x => x.Account)
                .ThenInclude(x => x.Customer)
                .OrderByDescending(x => x.TransactionDate)
                .Take(10)
                .ToListAsync();

            return model;
        }
        #endregion
    }
}