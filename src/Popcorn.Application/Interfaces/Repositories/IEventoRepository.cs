using Popcorn.Domain.Entities;
using Popcorn.Domain.Enums;

namespace Popcorn.Application.Interfaces.Repositories;

public interface IEventoRepository : IRepository<Evento>
{
    Task<IEnumerable<Evento>> GetByStatusAsync(StatusEvento status);
    Task<Evento?> GetWithTrajetosECategoriasAsync(int id);
}
