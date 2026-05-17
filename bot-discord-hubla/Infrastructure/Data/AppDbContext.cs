using bot_discord_hubla.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace bot_discord_hubla.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Membro> Membros => Set<Membro>();
    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<Inscricao> Inscricoes => Set<Inscricao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
