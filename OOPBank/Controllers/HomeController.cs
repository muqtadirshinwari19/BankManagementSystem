using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BankSystem.Services.Interfaces;
using System.Diagnostics;
using BankSystem.Models;
using BankSystem.Data;
using Microsoft.AspNetCore.Authorization;

namespace OOPBank.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFrontendService _frontendService;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IFrontendService frontendService    )
        {
            _logger = logger;
            _frontendService = frontendService;
        }

        #region Home
        public async Task<IActionResult> Index()
        {
            var model = await _frontendService.GetDashboardAsync();
            return View(model);
        }
        #endregion

        #region Customers
        public async Task<IActionResult> Customers()
        {
            var customers = await _frontendService.GetCustomersAsync();
            return View( customers );
        }
       

        [HttpPost]
        public async Task<IActionResult> AddCustomer(Customer customer)
        {
            await _frontendService.AddCustomerAsync(customer);
            return RedirectToAction(nameof(Customers));
        }
        #endregion
        [Authorize(Roles ="Admin")]
        #region Accounts
        public async Task<IActionResult> Accounts()
        {
            ViewBag.Customers = await _frontendService.GetActiveCustomersAsync();

            var accounts = await _frontendService.GetAccountsAsync();

            return View(accounts);
        }

        [HttpPost]
        public async Task<IActionResult> AddAccount(Account account)
        {
            await _frontendService.AddAccountAsync(account);

            return RedirectToAction(nameof(Accounts));
        }

        #endregion

        #region Transaction
        public async Task<IActionResult> Transactions()
        {
            var transactions = await _frontendService.GetTransactionsAsync();
            return View(transactions);
        }
        #endregion

        #region Transfer
        public async Task<IActionResult> Transfers()
        {
            ViewBag.Accounts = await _frontendService.GetAccountsAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> TransferMoney(int fromAccountId, int toAccountId, decimal amount, string? note)
        {
            var result = await _frontendService.TransferMoneyAsync(fromAccountId, toAccountId, amount, note);

            if (result)
                TempData["Success"] = "Money transferred successfully.";
            else
                TempData["Error"] = "Transfer failed. Please check balance or account details.";

            return RedirectToAction(nameof(Transfers));
        }

        #endregion
        [Authorize(Roles = "Admin")]

        #region Loans
        public async Task<IActionResult> Loans()
        {
            ViewBag.Customers = await _frontendService.GetActiveCustomersAsync();

            var loans = await _frontendService.GetLoansAsync();

            return View(loans);
        }

        [HttpPost]
        public async Task<IActionResult> AddLoan(Loan loan)
        {
            await _frontendService.AddLoanAsync(loan);

            return RedirectToAction(nameof(Loans));
        }
        #endregion

        #region Cards

        public async Task<IActionResult> Cards()
        {
            ViewBag.Customers = await _frontendService.GetActiveCustomersAsync();

            var cards = await _frontendService.GetCardsAsync();

            return View(cards);
        }

        [HttpPost]
        public async Task<IActionResult> AddCard(Card card)
        {
            await _frontendService.AddCardAsync(card);

            return RedirectToAction(nameof(Cards));
        }

        #endregion

        #region Report
        public async Task<IActionResult> Reports()
        {
            var model = await _frontendService.GetReportDashboardAsync();

            return View(model);
        }
        #endregion

        #region Setting
        public IActionResult Settings()
        {
            return View();
        }
        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
