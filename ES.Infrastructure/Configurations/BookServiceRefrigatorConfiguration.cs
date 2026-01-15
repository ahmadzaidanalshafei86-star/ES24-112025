using ES.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ES.Infrastructure.Configurations
{
    public class BookServiceRefrigatorConfiguration : IEntityTypeConfiguration<BookServiceRefrigator>
    {
        public void Configure(EntityTypeBuilder<BookServiceRefrigator> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Name)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.HasOne(r => r.BookServiceRequest)
                   .WithMany(b => b.Refrigators)
                   .HasForeignKey(r => r.BookServiceRequestId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
