using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace N5.Exam.Shared.DbFactories
{
    public abstract class BaseContextFactory<T> : IDesignTimeDbContextFactory<T> where T : DbContext, new()
    {
        private readonly string _connectionName;

        protected BaseContextFactory(string connectionString)
        {
            _connectionName = connectionString;
        }

        public virtual T CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddUserSecrets<T>()
                .Build();

            var builder = new DbContextOptionsBuilder<T>();

            builder = builder.EnableDetailedErrors().EnableSensitiveDataLogging();

            var connectionString = configuration.GetConnectionString(_connectionName);

            builder.UseSqlServer(connectionString);

            return Activator.CreateInstance(typeof(T), builder.Options) as T ?? new T();
        }
    }
}
