using System.Linq.Expressions;
using bot_discord_hubla.Domain.Interfaces;
using bot_discord_hubla.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace bot_discord_hubla.Infrastructure.Repositories;

public class GenericRepository<T>(AppDbContext context) : IGenericRepository<T>
    where T : class
{
    protected readonly DbSet<T> DbSet = context.Set<T>();

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        => await DbSet.AsNoTracking().FirstOrDefaultAsync(predicate, ct);

    public async Task<bool> ExisteAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        => await DbSet.AnyAsync(predicate, ct);

    public async Task AdicionarAsync(T entity, CancellationToken ct = default)
        => await DbSet.AddAsync(entity, ct);

    public async Task AdicionarESalvarAsync(T entity, CancellationToken ct = default)
    {
        await DbSet.AddAsync(entity, ct);
        await context.SaveChangesAsync(ct);
    }

    public async Task SalvarAsync(CancellationToken ct = default)
        => await context.SaveChangesAsync(ct);
}
