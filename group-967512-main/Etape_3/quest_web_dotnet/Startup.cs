using System;
using System.Text.Json.Serialization;
using System.Text ;
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
using Microsoft.AspNetCore.Authentication.JwtBearer ;
using quest_web.Utils ;
using Microsoft.AspNetCore.Http ;

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
            
            services.AddSingleton<Utils.JwtTokenUtil>(); 
            
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

            //jwt
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)    
                    .AddJwtBearer(options => 
                    {
                        options.TokenValidationParameters = JwtTokenUtil.TokenValidationParameters ;
                        /*options.Events = new JwtBearerEvents
                        {
                            OnChallenge = async context =>
                            {
                                // Call this to skip the default logic and avoid using the default response
                                context.HandleResponse();

                                // Write to the response in any way you wish
                                context.Response.StatusCode = 401;
                                await context.Response.WriteAsync("Non autorise");
                            }
                        };*/

                    });  


            
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

           app.Use(async (context, next) =>
            {
                await next();
            
                if (context.Response.StatusCode == 401 ) //(int)HttpStatusCode.Unauthorized) // 401
                {
                    context.Response.ContentType = "application/json";
                    await context.Response.WriteAsync("{ \"Erreur\" : \"Vous n'etes pas autorise\" }");
                }
            });

            app.UseAuthentication();
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
