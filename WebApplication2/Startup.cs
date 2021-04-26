using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.Configure<CAPAzureServiceBusConfiguration>(Configuration.GetSection(nameof(CAPAzureServiceBusConfiguration)));

            IConfigurationSection capAzureServiceBusConfiguration = Configuration.GetSection(nameof(CAPAzureServiceBusConfiguration));

            var azureServiceBusConnectionString = capAzureServiceBusConfiguration.GetValue<string>(nameof(CAPAzureServiceBusConfiguration.ConnectionString));

            var azureServiceBusTopicPath = capAzureServiceBusConfiguration.GetValue<string>(nameof(CAPAzureServiceBusConfiguration.TopicPath));

            serviceCollection.AddCap(x =>
            {
                x.UseAzureServiceBus(config =>
                {
                    config.ConnectionString = azureServiceBusConnectionString;
                    config.TopicPath = azureServiceBusTopicPath;
                });
                x.UseSqlServer(config =>
                {
                    config.ConnectionString = Configuration.GetSection($"{nameof(SqlDatabaseConfiguration)}:{nameof(SqlDatabaseConfiguration.MbDataServicesConnectionString)}").Value;
                });
            });

            serviceCollection.AddControllers();
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
