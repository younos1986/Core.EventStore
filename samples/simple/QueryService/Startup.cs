using System;
using Autofac;
using Core.EventStore.Managers;
using Core.EventStore.Registration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QueryService.IoCC.Modules;

namespace QueryService
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
            services.AddControllers();
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env , 
            IPersistentSubscriptionClient persistentSubscriptionClient 
            )
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

            // eventStoreConnectionManager.Start();
            persistentSubscriptionClient.Start();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Use and configure Autofac
            builder.RegisterModule(new EventStoreModule());

            // //builder.Update();
            // var reader = services.GetService<Core.EventStore.Dependencies.IEventStoreReader>();
            // var streams = reader.PerformAll().GetAwaiter();

        }
    }
}
