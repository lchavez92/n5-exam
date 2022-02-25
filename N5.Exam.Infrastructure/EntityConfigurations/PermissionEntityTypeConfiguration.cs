using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using N5.Exam.Infrastructure.Models;

namespace N5.Exam.Infrastructure.EntityConfigurations
{
    internal sealed class PermissionEntityTypeConfiguration : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            builder.ToTable(nameof(Permission));

            builder.Property(x => x.EmployeeForename).IsRequired();
            builder.Property(x => x.EmployeeSurname).IsRequired();
            builder.Property(x => x.PermissionDate).HasColumnType("date");
            builder.HasOne(x => x.PermissionType)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
