using bot_discord_hubla.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bot_discord_hubla.Infrastructure.Data.Configuration;

public class ProdutoConfiguration : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.ToTable("Produtos");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.HublaProductId).HasMaxLength(100).IsRequired();
        builder.Property(p => p.RoleName).HasMaxLength(100).IsRequired();

        builder.HasIndex(p => p.HublaProductId).IsUnique();
    }
}
