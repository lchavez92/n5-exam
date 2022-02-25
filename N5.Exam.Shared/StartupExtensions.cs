using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Polly;


namespace N5.Exam.Shared
{
    public static class StartupExtensions
    {
        public static DataBuilder<T> InitializeData<T>(this EntityTypeBuilder<T> entity, Type seedType, string jsonName) where T : class
        {
            var assembly = seedType.Assembly;
            string resourceName = $"{seedType.Namespace}.{jsonName}";
            using var stream = assembly.GetManifestResourceStream(resourceName);

            if (stream == null)
            {
                throw new FileNotFoundException("Unable to find the JSON file from an embedded resource", resourceName);
            }

            using var reader = new StreamReader(stream);
            string content = reader.ReadToEnd();

            var seed = JsonConvert.DeserializeObject<IEnumerable<T>>(content);
            return entity.HasData(seed.ToArray());
        }

        public static IHost MigrateDbContext<TContext>(
            this IHost host) where TContext : DbContext
        {
            using var scope = host.Services.CreateScope();

            var services = scope.ServiceProvider;
            var logger = services.GetRequiredService<ILogger<TContext>>();
            var context = services.GetService<TContext>();

            try
            {
                logger.LogInformation($"Migrating database associated with context {typeof(TContext).Name}");

                var retry = Policy.Handle<Exception>().WaitAndRetry(new[]
                {
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10),
                    TimeSpan.FromSeconds(15),
                });

                retry.Execute(() =>
                {
                    context?.Database.Migrate();
                });

                logger.LogInformation($"Migrated database associated with context {typeof(TContext).Name}");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"An error occurred while migrating the database used on context {typeof(TContext).Name}");
            }

            return host;
        }
    }
}
