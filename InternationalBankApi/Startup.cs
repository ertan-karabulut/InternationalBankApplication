using BusinessLayer.Dto;
using BusinessLayer.Mappers;
using CoreLayer.DataAccess.Abstrack;
using CoreLayer.DataAccess.Concrete.Repository;
using CoreLayer.Utilities.Cache.Abstrack;
using CoreLayer.Utilities.Cache.Concreate;
using CoreLayer.Utilities.Ioc;
using CoreLayer.Utilities.Logger;
using CoreLayer.Utilities.Messages;
using DataAccessLayer.Abstract;
using DataAccessLayer.Concreate;
using EntityLayer.Models.EntityFremework;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InternationalBankApi
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

            services.AddCors();

            services.AddAutoMapper(typeof(CustomMapping));

            services.AddMemoryCache();
            byte[] key = Encoding.UTF8.GetBytes(Configuration[StaticValue.TokenKey]);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x => {
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

            services.Configure<DatabaseSttings>(Configuration.GetSection("DatabaseSettings"));


            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "InternationalBankApi", Version = "v1" });
            });

            //services.Configure<RedisSettings>(Configuration.GetSection("RedisSettings"));
            //services.AddSingleton<ICacheWorkFlow, RedisCacheWorkFlow>(sp =>
            //{
            //    var options = sp.GetRequiredService<IOptions<RedisSettings>>().Value;
            //    var redis = new RedisCacheWorkFlow(options.Host, options.Port, options.Db);
            //    return redis;
            //});
            services.AddSingleton<ICacheWorkFlow, MemoryCacheWorkFlow>();
            services.AddSingleton<IDatabaseSttings>(sp =>
            {
                return sp.GetRequiredService<IOptions<DatabaseSttings>>().Value;
            });
            services.AddScoped<LogMessage>();
            ServiceTool.Create(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "InternationalBankApi v1"));
            }

            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

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
                endpoints.MapControllers();
            });
        }
    }
}
