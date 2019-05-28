using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ELTE.TravelAgency.Service.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ELTE.TravelAgency.Service
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// Dependency injection beállítása az adatbázis kontextushoz
			services.AddDbContext<TravelAgencyContext>(options =>
				options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

			// Dependency injection beállítása az authentikációhoz
			services.AddIdentity<Guest, IdentityRole<int>>()
				.AddEntityFrameworkStores<TravelAgencyContext>() // EF használata a TravelAgencyContext entitás kontextussal
				.AddDefaultTokenProviders(); // Alapértelmezett token generátor használata (pl. SecurityStamp-hez)

			services.Configure<IdentityOptions>(options =>
			{
				// Jelszó komplexitására vonatkozó konfiguráció
				options.Password.RequireDigit = true;
				options.Password.RequiredLength = 8;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireUppercase = true;
				options.Password.RequireLowercase = false;
				options.Password.RequiredUniqueChars = 3;

				// Hibás bejelentkezés esetén az (ideiglenes) kizárásra vonatkozó konfiguráció
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
				options.Lockout.MaxFailedAccessAttempts = 10;
				options.Lockout.AllowedForNewUsers = true;

				// Felhasználókezelésre vonatkozó konfiguráció
				options.User.RequireUniqueEmail = true;
			});

			services.AddMvc();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			// Authentikációs szolgáltatás használata
			app.UseAuthentication();

			app.UseMvc();

			// Adatbázis inicializálása
			var dbContext = serviceProvider.GetRequiredService<TravelAgencyContext>();
			var userManager = serviceProvider.GetRequiredService<UserManager<Guest>>();
			var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
			DbInitializer.Initialize(dbContext, userManager, roleManager,
				Configuration.GetValue<string>("ImageStore"));
		}
	}
}
