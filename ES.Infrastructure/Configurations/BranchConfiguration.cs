
using ES.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace ES.Infrastructure.Configurations
{
    public class BranchConfiguration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {
            builder.HasMany(b => b.RelatedHangars)
        .WithOne(h => h.Branch)
        .HasForeignKey(h => h.BranchId);

            builder.HasMany(b => b.RelatedRefrigators)
                   .WithOne(r => r.Branch)
                   .HasForeignKey(r => r.BranchId);

        }
    }
}

