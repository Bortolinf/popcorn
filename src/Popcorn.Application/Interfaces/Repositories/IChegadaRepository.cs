using Popcorn.Domain.Entities;

namespace Popcorn.Application.Interfaces.Repositories;

public interface IChegadaRepository : IRepository<Chegada>
{
    Task<IEnumerable<Chegada>> GetByEventoAsync(int eventoId);
    Task<IEnumerable<Chegada>> GetByAtletaAsync(int atletaId, int eventoId);
}
