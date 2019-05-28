using System;
using ELTE.TravelAgency.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ELTE.TravelAgency
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

	        // Dependency injection beállítása a Google konfiguráció kollekcióhoz
	        services.Configure<GoogleConfig>(Configuration.GetSection("Google"));

			// Dependency injection beállítása az utazással kapcsolatos szolgáltatáshoz
			services.AddTransient<ITravelService, TravelService>();
	        // Dependency injection beállítása a felhasználókezeléssel kapcsolatos szolgáltatáshoz
			services.AddTransient<IAccountService, AccountService>();

	        services.AddMvc();

			// Munkamenetkezelés beállítésa
	        services.AddDistributedMemoryCache();
	        services.AddSession(options =>
	        {
				options.IdleTimeout = TimeSpan.FromMinutes(15); // max. 15 percig él a munkamenet
			});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

			// Munkamentek használata
	        app.UseSession();

			// Statikus fájlkiszolgálás használata
			app.UseStaticFiles();

			// Dinamikus, MVC alapú útvonalkiszolgálás hazsnálata
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            
            // Adatbázis inicializálása
            DbInitializer.Initialize(serviceProvider, Configuration.GetValue<string>("ImageStore"));
        }
    }
}
