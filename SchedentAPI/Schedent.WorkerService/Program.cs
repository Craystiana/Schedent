using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OfficeOpenXml;
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

namespace Schedent.WorkerService
{
    public static class Program
    {
        /// <summary>
        /// Run the app
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// Create the host builder
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    Settings.SetConfig(hostContext.Configuration);

                    services.AddHttpClient();

                    services.AddHostedService<Worker>();

                    services.AddDbContext<SchedentContext>(options => options.UseSqlServer(Settings.DatabaseConnectionString));

                    // UnitOfWork and Repositories
                    services.AddTransient<IUnitOfWork, UnitOfWork>(_ => new UnitOfWork(Settings.DatabaseConnectionString));
                    services.AddTransient<IRepository<UserRole>, Repository<UserRole>>();
                    services.AddTransient<IUserRepository, UserRepository>();
                    services.AddTransient<IRepository<Document>, Repository<Document>>();
                    services.AddTransient<IRepository<TimeTable>, Repository<TimeTable>>();
                    services.AddTransient<IRepository<DocumentTimeTable>, Repository<DocumentTimeTable>>();
                    services.AddTransient<IRepository<Faculty>, Repository<Faculty>>();
                    services.AddTransient<IRepository<Section>, Repository<Section>>();
                    services.AddTransient<IRepository<Group>, Repository<Group>>();
                    services.AddTransient<IRepository<Subgroup>, Repository<Subgroup>>();
                    services.AddTransient<IRepository<Subject>, Repository<Subject>>();
                    services.AddTransient<IRepository<Professor>, Repository<Professor>>();
                    services.AddTransient<IRepository<ScheduleType>, Repository<ScheduleType>>();
                    services.AddTransient<IRepository<Schedule>, Repository<Schedule>>();

                    // Services
                    services.AddTransient<ImportService>();
                    services.Configure<GoogleCalendarSettings>(hostContext.Configuration.GetSection(nameof(GoogleCalendarSettings)));
                    services.Configure<FirebaseSettings>(hostContext.Configuration.GetSection(nameof(FirebaseSettings)));

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    FirebaseApp.Create(new AppOptions()
                    {
                        Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "firebase_settings.json")),
                    });
                });
    }
}
