using Crayon.ExchangeRates.Api.CustomExceptionMiddleware;
using Crayon.ExchangeRates.BusinessLogic;
using Crayon.ExchangeRates.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;

namespace Crayon.ExchangeRates.Api
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
            services.AddTransient<IHistoricalExchangeRatesBusinessLogic, HistoricalExchangeRatesBusinessLogic>();
            services.
                AddHttpClient<IHistoricalExchangeRatesBusinessLogic, HistoricalExchangeRatesBusinessLogic>(client =>
                {
                    client.BaseAddress = new Uri(Configuration["BaseUrl"]);
                });

            services.AddDistributedMemoryCache();

            services.AddMvc().
                SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(Configuration.GetSection("ServiceData")["Version"], new Info
                {
                    Version = Configuration.GetSection("ServiceData")["Version"],
                    Title = Configuration.GetSection("ServiceData")["ServiceName"],
                    Description = Configuration.GetSection("ServiceData")["UniqueIdentifier"],
                    TermsOfService = Configuration.GetSection("ServiceData")["UniqueIdentifier"],
                    Contact = new Contact
                    {
                        Name = Configuration.GetSection("ServiceData")["Vendor"],
                        Email = Configuration.GetSection("ServiceData")["VendorEmail"],
                        Url = Configuration.GetSection("ServiceData")["VendorUrl"]
                    }
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Crayon ExchangeRates API V1");
            });

            app.UseCustomExceptionMiddleware();

            app.UseMvc();
        }
    }
}
