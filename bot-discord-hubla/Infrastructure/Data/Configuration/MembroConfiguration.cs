using bot_discord_hubla.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bot_discord_hubla.Infrastructure.Data.Configuration;

public class MembroConfiguration : IEntityTypeConfiguration<Membro>
{
    public void Configure(EntityTypeBuilder<Membro> builder)
    {
        builder.ToTable("Membros");
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Email).HasMaxLength(254).IsRequired();
        builder.Property(m => m.DiscordId).HasMaxLength(20);

        builder.HasIndex(m => m.Email).IsUnique();
        builder.HasIndex(m => m.DiscordId);
    }
}
