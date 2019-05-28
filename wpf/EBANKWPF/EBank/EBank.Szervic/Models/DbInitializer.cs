using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EBank.Szervic.Models
{
    public static class DbInitializer
    {
        private static EBankContext _context;
        private static UserManager<Clients> _userManager;
        private static RoleManager<IdentityRole<int>> _roleManager;

        /// <summary>
        /// Adatbázis inicializálása.
        /// </summary>
        /// <param name="context">Adatbázis kontextus.</param>
        /// <param name="imageDirectory">Képek útvonala.</param>
        public static void Initialize(EBankContext context,
            UserManager<Clients> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;

            // Adatbázis migrációk végrehajtása, amennyiben szükséges
            _context.Database.Migrate();

            // Felhasználók inicializálása
            if (!_context.Users.Any())
            {
                SeedUsers();
            }
            // Városok, épületek, apartmanok inicializálás
            if (!_context.BankAccounts.Any())
            {
                SeedAccounts();
            }

        }

        /// <summary>
        /// Adminisztrátorok inicializálása.
        /// </summary>
        private static void SeedUsers()
        {
            var adminUser = new Clients
            {
                UserName = "admin",
                FullName = "Adminisztrátor",
                Email = "admin@example.com",
                PhoneNumber = "+36123456789",
                PinCode = "1234",
                PrimaryAccountNumber = "11111111-12345678-12345601"
            };
            var adminPassword = "Admin123";
            var adminRole = new IdentityRole<int>("administrator");

            var result1 = _userManager.CreateAsync(adminUser, adminPassword).Result;
            var result2 = _roleManager.CreateAsync(adminRole).Result;
            var result3 = _userManager.AddToRoleAsync(adminUser, adminRole.Name).Result;
        }

        /// <summary>
        /// Városok inicializálása.
        /// </summary>
        private static void SeedAccounts()
        {

            var bankaccounts = new BankAccounts[]
            {
                new BankAccounts
                {
                    AccountNumber = "11111111-12345678-12345601",
                    Balance = 5000,
                    Created = DateTime.Now,
                    isLocked = false,
                    ClientID = _context.Users.FirstOrDefault(u => u.UserName == "admin").Id
                }
            };
            foreach (BankAccounts c in bankaccounts)
            {
                _context.BankAccounts.Add(c);
            }

            _context.SaveChanges();
        }
    }
}
