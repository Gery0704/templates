using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.IO;

namespace Animals.Models
{
    public class DbInitializer
    {
            private static MyDbContext _context;
            private static UserManager<MyUsers> _userManager;

        public static void Initialize(IServiceProvider serviceProvider, string imageDirectory)
            {
                _context = serviceProvider.GetRequiredService<MyDbContext>();
                _userManager = serviceProvider.GetRequiredService<UserManager<MyUsers>>();
            
                _context.Database.Migrate();
            
                if (_context.Animals.Any())
                {
                    return; 
                }

                SeedUsers();
                SeedAnimals(imageDirectory);
            }

            /// <summary>
            /// Városok inicializálása.
            /// </summary>
            private static void SeedUsers()
            {

                var Users = new MyUsers[]
                {
                new MyUsers {FullName = "Teszt Elek1", tender = false, UserName = "Teszt1", Email = "Teszt1@teszt.hu"},
                new MyUsers {FullName = "Teszt Elek2", tender = false, UserName = "Teszt2", Email = "Teszt2@teszt.hu"},
                new MyUsers {FullName = "Teszt Elek3", tender = false, UserName = "Teszt3", Email = "Teszt3@teszt.hu"},
                new MyUsers {FullName = "Teszt Elek4", tender = true, UserName = "Teszt4", Email = "Teszt4@teszt.hu"},
                new MyUsers {FullName = "Teszt Elek5", tender = true, UserName = "Teszt5", Email = "Teszt5@teszt.hu"}
                };
                foreach (MyUsers c in Users)
                {
                var psw = "Teszt1Teszt1";
                var result = _userManager.CreateAsync(c, psw).Result;
                }
            _context.SaveChanges();

            }

        /// <summary>
        /// Épületek inicializálása.
        /// </summary>
        private static void SeedAnimals(string imageDirectory)
        {
            // Ellenőrizzük, hogy képek könyvtára létezik-e.
            if (Directory.Exists(imageDirectory))
            {
                var animals = new List<Animals>();

                var largePath = Path.Combine(imageDirectory, "petra_1.png");
                if (File.Exists(largePath))
                {
                    animals.Add(new Animals
                    {
                        Name = "Rex",
                        Picture = File.ReadAllBytes(largePath),
                        free = true,
                        Type = "Németjuhász",
                        BirthDate = DateTime.Now
                    });
                }

                largePath = Path.Combine(imageDirectory, "petra_2.png");
                if (File.Exists(largePath))
                {
                    animals.Add(new Animals
                    {
                        Name = "Bodri",
                        Picture = File.ReadAllBytes(largePath),
                        free = true,
                        Type = "Palotapincsi",
                        BirthDate = DateTime.Now
                    });
                }
                largePath = Path.Combine(imageDirectory, "cavallino_1.png");
                if (File.Exists(largePath))
                {
                    animals.Add(new Animals
                    {
                        Name = "Picur",
                        Picture = File.ReadAllBytes(largePath),
                        free = true,
                        Type = "Gyilkos bulldog",
                        BirthDate = DateTime.Now
                    });
                }


                foreach (var a in animals)
                {
                    _context.Animals.Add(a);
                }

                var types = new List<AnimalType>()
                {
                    new AnimalType{Name = "Németjuhász"},
                    new AnimalType{Name = "Kétfarkúkutya"},
                    new AnimalType{Name = "Gyilkos hörcsög"}
                };

                _context.SaveChanges();
            }
        }
    }
}
