

namespace ES.Infrastructure.Configurations
{
    public class BranchTranslateConfiguration : IEntityTypeConfiguration<BranchTranslate>
    {
        public void Configure(EntityTypeBuilder<BranchTranslate> builder)
        {
            builder.HasOne(e => e.Branch)
                   .WithMany(e => e.BranchesTranslates)
                   .HasForeignKey(e => e.BranchId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(e => e.Language)
                  .WithMany()
                  .HasForeignKey(e => e.LanguageId)
                  .OnDelete(DeleteBehavior.NoAction);
        }
    }
}

