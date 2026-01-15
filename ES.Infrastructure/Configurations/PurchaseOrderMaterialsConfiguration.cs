using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ES.Core.Entities;

namespace ES.Infrastructure.Configurations
{
    public class PurchaseOrderMaterialsConfiguration : IEntityTypeConfiguration<PurchaseOrderMaterial>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrderMaterial> builder)
        {
            builder.HasKey(tm => new { tm.PurchaseOrderId, tm.MaterialId });

            builder
                .HasOne(tm => tm.PurchaseOrder)
                .WithMany(t => t.Materials)
                .HasForeignKey(tm => tm.PurchaseOrderId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(tm => tm.Material)
                .WithMany(m => m.PurchaseOrders)
                .HasForeignKey(tm => tm.MaterialId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
