using Popcorn.Domain.Entities;

namespace Popcorn.Application.Interfaces.Repositories;

public interface ICampeonatoEventoRepository : IRepository<CampeonatoEvento>
{
    Task<IEnumerable<CampeonatoEvento>> GetByCampeonatoAsync(int campeonatoId);
}
