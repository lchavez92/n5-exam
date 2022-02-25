using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using N5.Exam.Infrastructure.Models;

namespace N5.Exam.Infrastructure.EntityConfigurations
{
    internal sealed class PermissionTypeEntityTypeConfiguration: IEntityTypeConfiguration<PermissionType>
    {
        public void Configure(EntityTypeBuilder<PermissionType> builder)
        {
            builder.ToTable(nameof(PermissionType));
            builder.Property(x => x.Id).ValueGeneratedNever();
            builder.Property(x => x.Description).IsRequired();
        }
    }
}
