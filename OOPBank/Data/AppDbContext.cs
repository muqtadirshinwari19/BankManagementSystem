using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using BankSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BankSystem.Models.Identities;
using BankSystem.Models.AI;

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
        public DbSet<Conversation> Conversations { get; set; }

        public DbSet<ChattMessage> ChatMessages { get; set; }
        public DbSet<KnowledgeDocument> KnowledgeDocuments { get; set; }
        public DbSet<KnowledgeChunk> KnowledgeChunks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Conversation>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ChattMessage>()
                .HasOne(m => m.Conversation)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<KnowledgeChunk>()
           .HasOne(chunk => chunk.KnowledgeDocument)
           .WithMany(document => document.Chunks)
           .HasForeignKey(chunk => chunk.KnowledgeDocumentId)
           .OnDelete(DeleteBehavior.Cascade);
        }


    }


}