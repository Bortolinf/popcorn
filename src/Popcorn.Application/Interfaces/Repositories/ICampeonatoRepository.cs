using Popcorn.Domain.Entities;

namespace Popcorn.Application.Interfaces.Repositories;

public interface ICampeonatoRepository : IRepository<Campeonato>
{
    Task<Campeonato?> GetWithEventosAsync(int id);
}
