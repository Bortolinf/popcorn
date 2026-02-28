using Popcorn.Domain.Entities;

namespace Popcorn.Application.Interfaces.Repositories;

public interface IEventoCategoriaRepository : IRepository<EventoCategoria>
{
    Task<IEnumerable<EventoCategoria>> GetByEventoAsync(int eventoId);
}
