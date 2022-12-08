using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Schedent.BusinessLogic.Config;
using Schedent.BusinessLogic.Services;
using Schedent.Common;
using Schedent.DataAccess;
using Schedent.DataAccess.Repositories;
using Schedent.Domain.Entities;
using Schedent.Domain.Interfaces;
using Schedent.Domain.Interfaces.Repositories;
using System;
using System.IO;
using System.Reflection;

namespace Schedent.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            Settings.SetConfig(configuration);
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Swagger configuration
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Schedent API",
                    Version = Settings.Version,
                    Description = "Description for the API goes here.",
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            // Database configuration using the connection string  from appsettings
            services.AddDbContext<SchedentContext>(options => options.UseSqlServer(Settings.DatabaseConnectionString));

            // Cors configuration
            services.AddCors(options => options.AddPolicy("AllowAllOrigins", builder => builder.AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:8100")));

            // UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>(_ => new UnitOfWork(Settings.DatabaseConnectionString));

            // Repositories
            services.AddScoped<IRepository<UserRole>, Repository<UserRole>>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IRepository<Document>, Repository<Document>>();
            services.AddScoped<IRepository<TimeTable>, Repository<TimeTable>>();
            services.AddScoped<IRepository<DocumentTimeTable>, Repository<DocumentTimeTable>>();
            services.AddScoped<IRepository<Faculty>, Repository<Faculty>>();
            services.AddScoped<IRepository<Section>, Repository<Section>>();
            services.AddScoped<IRepository<Group>, Repository<Group>>();
            services.AddScoped<IRepository<Subgroup>, Repository<Subgroup>>();
            services.AddScoped<IRepository<Subject>, Repository<Subject>>();
            services.AddScoped<IRepository<Professor>, Repository<Professor>>();
            services.AddScoped<IRepository<ScheduleType>, Repository<ScheduleType>>();
            services.AddScoped<IScheduleRepository, ScheduleRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();

            // Services
            services.AddScoped<UserService>();
            services.AddScoped<FacultyService>();
            services.AddScoped<SectionService>();
            services.AddScoped<GroupService>();
            services.AddScoped<SubgroupService>();
            services.AddScoped<DocumentService>();
            services.AddScoped<ScheduleService>();
            services.AddScoped<NotificationService>();

            // This configuration allows the GoogleCalendarSettings section from appsettings to be mapped to the GoogleCalendarSettings dto
            services.Configure<GoogleCalendarSettings>(Configuration.GetSection(nameof(GoogleCalendarSettings)));
            // This configuration allows the FirebaseSettings section from appsettings to be mapped to the FirebaseSettings dto
            services.Configure<FirebaseSettings>(Configuration.GetSection(nameof(FirebaseSettings)));

            // JWT authentication
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Settings.TokenSecretBytes),
                    ValidateIssuer = false,
                    ClockSkew = TimeSpan.Zero,
                    ValidateAudience = false
                };
            });

            // Configure Firebase using the credentials from the firebase_settings.json
            FirebaseApp.Create(new AppOptions()
            {
                Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "firebase_settings.json")),
            });

            // Azure Application Insights configuration
            services.AddApplicationInsightsTelemetry();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(options =>
            {
                // Swagger endpoint
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Schedent API " + Settings.Version);

                // To serve SwaggerUI at application's root page, set the RoutePrefix property to an empty string.
                options.RoutePrefix = string.Empty;
            });

            // Adds middleware for redirecting HTTP requests to  HTTPS
            app.UseHttpsRedirection();

            app.UseRouting();

            // Enable authorization capabilities
            app.UseAuthorization();

            // Allow cross domain requests
            app.UseCors("AllowAllOrigins");

            app.UseEndpoints(endpoints =>
            {
                // Add endpoints for controller actions without specifying any routes
                endpoints.MapControllers();
            });
        }
    }
}
