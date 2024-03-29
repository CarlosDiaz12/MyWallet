using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MyWallet.Data;
using MyWallet.ErrorHandlers;
using NLog.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWallet
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
            // response compression
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
            });

            services.AddControllers(x =>
            {
                x.Filters.Add<WebAPIExceptionFilter>();
            });
            services.AddDbContext<DailyExpensesContext>(opts => opts.UseInMemoryDatabase("WalletDB"));
            services.AddSingleton<Serilog.ILogger>(_ =>
            {
                var connString = Configuration["SerilogConfig:ConnectionString"];
                var tableName = Configuration["SerilogConfig:TableName"];
                return new LoggerConfiguration().WriteTo.MSSqlServer(connString, tableName).CreateLogger();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            // response compression
            app.UseResponseCompression();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
