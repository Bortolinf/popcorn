using Microsoft.EntityFrameworkCore;
using Popcorn.Application.Interfaces.Repositories;
using Popcorn.Domain.Entities;
using Popcorn.Infrastructure.Data;

namespace Popcorn.Infrastructure.Repositories;

public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(AppDbContext context) : base(context) { }

    public async Task<Usuario?> GetByLoginAsync(string login) =>
        await _dbSet.FirstOrDefaultAsync(u => u.Login == login);
}
