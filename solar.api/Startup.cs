using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Elmah;
using ElmahCore.Mvc;
using ElmahCore.Sql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using solar.generics.Providers;
using solar.generics.DataHelper;
using solar.irepo.Extensions;
using solar.iservices;
using solar.iservices.Extensions;
using solar.messaging.Model;
using System.IO;
using Microsoft.OpenApi.Models;

namespace solar.api
{
    public class Startup
    {
        public List<Type> TypesToRegister { get; }
        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            DBServer.configuration = Configuration;
            DBServer.setConnectionString();
            TypesToRegister = Assembly.LoadFrom(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\solar.services.dll")
                                      .GetTypes()
                                      .Where(x => !string.IsNullOrEmpty(x.Namespace))
                                      .Where(x => x.IsClass)
                                      .Where(x => x.Namespace.StartsWith("solar.services")).ToList();
            TypesToRegister.AddRange(Assembly.LoadFrom(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\solar.repo.dll")
                                      .GetTypes()
                                      .Where(x => !string.IsNullOrEmpty(x.Namespace))
                                      .Where(x => x.IsClass)
                                      .Where(x => x.Namespace.StartsWith("solar.repo")));
            SystemConstants.JWT = Configuration.GetSection("SystemKeys").GetValue<String>("JWT");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = "https://localhost:44308",
                        ValidAudience = "https://localhost:44308",
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SystemConstants.JWT))
                    };
                });
            services.AddMvc(option => option.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddElmah();
            services.AddSignalR();
            services.AddElmah<ElmahCore.Sql.SqlErrorLog>(options =>
            {
                options.ConnectionString = Configuration.GetConnectionString("simbayuConn"); // DB structure see here: https://bitbucket.org/project-elmah/main/downloads/ELMAH-1.2-db-SQLServer.sql
            });
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.Configure<EmailSettings>(Configuration.GetSection("EmailSettings"));
            // Tenant Services
            // Classes to register
            TypesToRegister.ForEach(x => { if (x.DeclaringType == null) { services.AddScoped(x); } });
            RegisterRepos.registerReposDI(ref services, TypesToRegister);
            RegisterServices.registerServicesDI(ref services, TypesToRegister);
            //services.AddScopedDynamic<IProductService>(TypesToRegister);
            // Global Service provider
            services.AddScoped(typeof(IServicesProvider<>), typeof(ServicesProvider<>));
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
        "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
                });
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseCors(builder =>
            {
                builder.WithOrigins("http://localhost:4600", "https://dev.simbayu.in")
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials();
            });
            app.UseCors("CorsPolicy");
            app.UseMvc();
            app.UseElmah();
            app.UseSignalR(route =>
            {
                route.MapHub<NotificationHub>("/notify");
                route.MapHub<ChartHub>("/chart");
            });
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

        }
    }
}
