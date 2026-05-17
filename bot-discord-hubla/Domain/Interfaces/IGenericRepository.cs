using System.Linq.Expressions;

namespace bot_discord_hubla.Domain.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    Task<bool> ExisteAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);
    Task AdicionarAsync(T entity, CancellationToken ct = default);
    Task AdicionarESalvarAsync(T entity, CancellationToken ct = default);
    Task SalvarAsync(CancellationToken ct = default);
}
