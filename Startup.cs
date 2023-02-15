using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PriceService.DataBase;
using PriceService.SignalR;

namespace PriceService
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
            Console.WriteLine(Configuration.GetConnectionString("mssql"));

            services.AddScoped<IPricesRepository, PriceRepository>(provider => new PriceRepository(Configuration.GetConnectionString("mssql")));
            
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddSingleton<ClientHub>(provider => new ClientHub(provider.GetRequiredService<IPricesRepository>(), Configuration.GetConnectionString("signalR")));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PriceService", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PriceService v1"));
            }
            
            app.Use(async (context, next) =>
            {
                Console.WriteLine($"Method: {context.Request.Method}");
                Console.WriteLine($"Path: {context.Request.Path.Value}");
                await next();
            });


            app.UseRouting();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            PrepDb.UpdateDB(app);
        }
    }
}