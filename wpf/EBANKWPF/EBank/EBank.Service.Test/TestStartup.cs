using EBank.Szervic.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace EBank.Service.Test
{
    public class TestStartup
    {
        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Dependency injection beállítása az adatbázis kontextushoz
            services.AddDbContext<EBankContext>(options =>
                options.UseInMemoryDatabase("EbankTest"));

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();

            // adatok inicializációja
            var dbContext = serviceProvider.GetRequiredService<EBankContext>();
            dbContext.BankAccounts.AddRange(EBankIntegrationTest.BankAccountsData);
            dbContext.Users.AddRange(EBankIntegrationTest.ClientsData);
            dbContext.SaveChanges();
        }
    }
}
