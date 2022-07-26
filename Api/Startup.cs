using Autofac;
using BusinessLayer.Abstrack;
using BusinessLayer.Concreate;
using BusinessLayer.Concreate.WorkFlow;
using BusinessLayer.DependencyResolvers.Autofac;
using BusinessLayer.Model;
using BusinessLayer.Validation;
using CoreLayer.DataAccess.Abstrack;
using CoreLayer.Utilities.Cache.Abstrack;
using CoreLayer.Utilities.Cache.Concreate;
using CoreLayer.Utilities.Ioc;
using CoreLayer.Utilities.Logger;
using CoreLayer.Utilities.Messages;
using CoreLayer.Utilities.Result.Abstrack;
using CoreLayer.Utilities.Result.Concreate;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concreate;
using EntityLayer.Models;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Startup>());

            services.AddDbContext<MyBankContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString(StaticValue.ConnectionKey));
            }, ServiceLifetime.Scoped);

            services.AddMemoryCache();
            byte[] key = Encoding.UTF8.GetBytes(Configuration[StaticValue.TokenKey]);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x=> {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
                
            });
            services.AddSingleton<ICacheWorkFlow, MemoryCacheWorkFlow>();
            services.AddSingleton<ILogModel, LogModel>();
            services.AddScoped<LogMessage>();
            ServiceTool.Create(services);
        }

        public async void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors(x => x
                .AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowed(origin => true)
                .AllowCredentials());

            //app.UseHttpsRedirection();

            app.Use(async (context, next) =>
            {
                var message = ServiceTool.ServiceProvider.GetService<LogMessage>();
                message.EmptyLog();

                await next();
            });

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{Id?}");
            });

           
        }
    }
}
