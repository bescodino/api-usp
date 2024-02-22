using Microsoft.Extensions.DependencyInjection;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;

namespace LabSid.Migrations
{
    public class Migrator
    {
        private readonly bool _isDevelopment;
        private readonly string connectionString;
        public Migrator(bool isDevelopment, string connectionString)
        {
            _isDevelopment = isDevelopment;
            this.connectionString = connectionString;
        }
        public async Task Migrate()
        {
            var serviceProvider = CreateServiceProvider(connectionString);
            RunMigrations(serviceProvider, _isDevelopment);
        }
        private ServiceProvider CreateServiceProvider(string connectionString) => new ServiceCollection()
            .AddFluentMigratorCore()
               .ConfigureRunner(
                builder => builder
                    .AddPostgres11_0()
                    .WithGlobalConnectionString(connectionString)
                    .ScanIn(typeof(Migrator).Assembly).For.Migrations()
                )
             .AddLogging(loggingBuilder => loggingBuilder.AddFluentMigratorConsole())
            .Configure<RunnerOptions>(cfg =>
            {
                if (_isDevelopment)
                    cfg.Tags = new string[] { "Development" };
            })
            .BuildServiceProvider(false);

        private void RunMigrations(ServiceProvider serviceProvider, bool isDevelopment)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();

                try
                {
                    runner.MigrateUp();
                }
                catch (Exception)
                {
                    if (isDevelopment)
                    {
                        runner.Rollback(1);
                    }

                }

            }
        }
    }
}
