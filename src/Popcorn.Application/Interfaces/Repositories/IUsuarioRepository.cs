using Popcorn.Domain.Entities;

namespace Popcorn.Application.Interfaces.Repositories;

public interface IUsuarioRepository : IRepository<Usuario>
{
    Task<Usuario?> GetByLoginAsync(string login);
}
