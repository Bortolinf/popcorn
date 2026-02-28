using Popcorn.Domain.Entities;

namespace Popcorn.Application.Interfaces.Repositories;

public interface IEventoTrajetoRepository : IRepository<EventoTrajeto>
{
    Task<IEnumerable<EventoTrajeto>> GetByEventoAsync(int eventoId);
}
