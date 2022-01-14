using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OfficeOpenXml;
using Schedent.BusinessLogic.Services;
using Schedent.Common;
using Schedent.DataAccess;
using Schedent.DataAccess.Repositories;
using Schedent.Domain.Entities;
using Schedent.Domain.Interfaces;
using Schedent.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schedent.WorkerService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    Settings.SetConfig(hostContext.Configuration);

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

                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                });
    }
}
