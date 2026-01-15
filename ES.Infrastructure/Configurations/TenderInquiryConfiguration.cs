using ES.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ES.Infrastructure.Configurations
{
    public class TenderInquiryConfiguration : IEntityTypeConfiguration<TenderInquiry>
    {
        public void Configure(EntityTypeBuilder<TenderInquiry> builder)
        {
            builder.ToTable("TenderInquiries");

            // Primary Key
            builder.HasKey(x => x.Id);

            // Required fields + max length
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Phone)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.InquiryText)
                .IsRequired();

            builder.Property(x => x.AttachmentUrl)
                .HasMaxLength(500);

            builder.Property(x => x.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            // Relation (if you want full FK behavior)
            builder.HasOne<Tender>()
                .WithMany()
                .HasForeignKey(x => x.TenderId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
