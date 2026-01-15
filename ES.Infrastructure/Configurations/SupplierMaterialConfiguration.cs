using ES.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ES.Infrastructure.Configurations
{
    public class SupplierMaterialConfiguration : IEntityTypeConfiguration<SupplierMaterial>
    {
        public void Configure(EntityTypeBuilder<SupplierMaterial> builder)
        {
         

            builder.HasKey(sm => sm.Id);

            builder.HasOne(sm => sm.Supplier)
                   .WithMany(s => s.SupplierMaterials)
                   .HasForeignKey(sm => sm.SupplierId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(sm => sm.Material)
                   .WithMany()
                   .HasForeignKey(sm => sm.MaterialId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
