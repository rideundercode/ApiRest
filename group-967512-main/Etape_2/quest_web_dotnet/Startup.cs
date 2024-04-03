using System;
using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;

namespace quest_web
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
 
            //var connectionString = "server=127.0.0.1;database=quest_web;user=application;password=password";
            //var serverVersion = ServerVersion.AutoDetect(connectionString) ; 
            //var serverVersion =  new MySqlServerVersion(new Version(8, 0, 28));
            
            services.AddControllers()
                    .AddJsonOptions
                    (
                        options => options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())
                    );

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "quest_web", Version = "v1" });
            });
             
            services.AddDbContext<APIDbContext>(
            //dbContextOptions => dbContextOptions
                //.UseMySql(connectionString, serverVersion)
                // The following three options help with debugging, but should
                // be changed or removed for production.
                //.LogTo(Console.WriteLine, LogLevel.Information)
                //.EnableSensitiveDataLogging()
                //.EnableDetailedErrors()
            );

            services.AddScoped<APIDbContext>() ;
            

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, APIDbContext DbContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "quest_web v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            }) ;

            DbContext.Database.EnsureCreated();

        }
    }
}
