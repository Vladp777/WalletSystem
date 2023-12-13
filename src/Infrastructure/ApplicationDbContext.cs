using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options) { }

        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<TransactionTag> TransactionTags { get; set; }
        public virtual DbSet<TransactionType> TransactionTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}
