using Autofac;
using Core.EventStore.Dependencies;
using Core.EventStore.Registration;
using EfCoreQueryService.EfCoreConfig;
using EfCoreQueryService.IoCC.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EfCoreQueryService
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
            
            services.AddDbContext<EfCoreDbContext>(options =>
            {
                //"Data Source=localhost,1433;Initial Catalog=EventStoreDb;Persist Security Info=True;User ID=sa;Password=TTTttt456!@#;Max Pool Size=80;"
                options.UseSqlServer(Configuration["CoreEventStore:SqlServerConfig:ConnectionString"]);
            });
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env , 
            IPersistentSubscriptionClient persistentSubscriptionClient
            //, IEventStoreReader eventStoreReader 
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


            //eventStoreReader.PerformReadStreamEventsForwardAsync("CustomerCreated", 0, 10, false, null);
            
            //eventStoreReader.PerformAll( null);


        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            // Use and configure Autofac
            builder.RegisterModule(new EventStoreModule());
            builder.RegisterModule(new ProjectorsModule());
            

            // //builder.Update();
            // var reader = services.GetService<Core.EventStore.Dependencies.IEventStoreReader>();
            // var streams = reader.PerformAll().GetAwaiter();

        }
    }
}