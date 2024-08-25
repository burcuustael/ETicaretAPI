using System.Linq.Expressions;
using ETicaret.Domain.Entities.Common;
using ETicaretAPI.Application.Repositories;
using ETicaretAPI.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;

namespace ETicaretAPI.Persistence.Repositories;

public class ReadRepository<T> : IReadRepository<T> where T : BaseEntity
{
    private readonly ETicaretAPIDbContext _context;

    public ReadRepository(ETicaretAPIDbContext context)
    {
        _context = context;
    }

    public DbSet<T> Table => _context.Set<T>();

    public IQueryable<T> GetAll(bool tracking = true)
        => Table;

    public IQueryable<T> GetWhere(Expression<Func<T, bool>> method, bool tracking = true)
        => Table.Where(method);

    public async Task<T> GetSingleAsync(Expression<Func<T, bool>> method, bool tracking = true)
        => await Table.FirstOrDefaultAsync(method);

    public async Task<T> GetByIdAsync(string id, bool tracking = true)
        => await Table.FirstOrDefaultAsync(data => data.Id == Guid.Parse(id));
}