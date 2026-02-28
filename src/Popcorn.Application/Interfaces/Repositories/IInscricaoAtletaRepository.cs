using Popcorn.Domain.Entities;

namespace Popcorn.Application.Interfaces.Repositories;

public interface IInscricaoAtletaRepository : IRepository<InscricaoAtleta>
{
    Task<IEnumerable<InscricaoAtleta>> GetByEventoAsync(int eventoId);
    Task<IEnumerable<InscricaoAtleta>> GetByAtletaAsync(int atletaId);
    Task<InscricaoAtleta?> GetByNumeroAsync(int eventoId, int numero);
    Task<InscricaoAtleta?> GetByTagRfidAsync(int eventoId, string tagRfid);
}
