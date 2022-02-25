using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using N5.Exam.Infrastructure.EntityConfigurations;
using N5.Exam.Infrastructure.Models;
using N5.Exam.Shared;
using N5.Exam.Shared.Repositories;

namespace N5.Exam.Infrastructure
{
    public class N5ExamContext: DbContext, IUnitOfWork
    {
        private const string DataSeedFolderName = "DataSeed";


        public DbSet<Permission> Permissions { get; set; }

        public DbSet<PermissionType> PermissionTypes { get; set; }

        public N5ExamContext()
        {
            
        }

        public N5ExamContext(DbContextOptions<N5ExamContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new PermissionEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PermissionTypeEntityTypeConfiguration());

            modelBuilder.Entity<PermissionType>().InitializeData(GetType(), $"{DataSeedFolderName}.{nameof(PermissionType)}.json");
        }

        public Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
