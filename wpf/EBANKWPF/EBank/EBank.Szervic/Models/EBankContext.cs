using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBank.Szervic.Models
{
    public class EBankContext : IdentityDbContext<Clients, IdentityRole<int>, int>
    {
        public EBankContext() // Mockoláshoz szükséges
        { }
        public EBankContext(DbContextOptions<EBankContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Clients>().ToTable("Clients");
            // A felhasználói tábla alapértelemezett neve AspNetUsers lenne az adatbázisban, de ezt felüldefiniálhatjuk.
        }

        public virtual DbSet<EBank.Szervic.Models.BankAccounts> BankAccounts { get; set; }

        public virtual DbSet<EBank.Szervic.Models.Transactions> Transactions { get; set; }
        
    }
}
