using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using BankSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BankSystem.Models.Identities;

namespace BankSystem.Data
{
    public partial class AppDbContext : IdentityDbContext<User,UserRole,string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<Card> Cards { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Loan> Loans { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
      

    }
}