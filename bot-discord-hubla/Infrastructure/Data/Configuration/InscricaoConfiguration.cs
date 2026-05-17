using bot_discord_hubla.Domain.Entities;
using bot_discord_hubla.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace bot_discord_hubla.Infrastructure.Data.Configuration;

public class InscricaoConfiguration : IEntityTypeConfiguration<Inscricao>
{
    public void Configure(EntityTypeBuilder<Inscricao> builder)
    {
        builder.ToTable("Inscricoes");
        builder.HasKey(i => i.Id);

        builder.Property(i => i.MembroId).IsRequired();
        builder.Property(i => i.ProdutoId).IsRequired();

        builder.Property(i => i.Status)
               .HasConversion(new EnumToStringConverter<StatusInscricao>())
               .HasMaxLength(20)
               .IsRequired();

        builder.Property(i => i.UpdatedAt).IsRequired();

        builder.HasOne(i => i.Membro)
               .WithMany(m => m.Inscricoes)
               .HasForeignKey(i => i.MembroId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(i => i.Produto)
               .WithMany()
               .HasForeignKey(i => i.ProdutoId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

