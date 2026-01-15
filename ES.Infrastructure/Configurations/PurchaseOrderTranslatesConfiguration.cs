using AKM.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ES.Infrastructure.Configurations
{
    public class PurchaseOrderTranslatesConfiguration : IEntityTypeConfiguration<PurchaseOrderTranslate>
    {
        public void Configure(EntityTypeBuilder<PurchaseOrderTranslate> builder)
        {
            builder.HasOne(e => e.PurchaseOrder)
                   .WithMany(e => e.PurchaseOrderTranslates)
                   .HasForeignKey(e => e.PurchaseOrderId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Language)
                  .WithMany()
                  .HasForeignKey(e => e.LanguageId)
                  .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
