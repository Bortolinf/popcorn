using Popcorn.Domain.Entities;

namespace Popcorn.Application.Interfaces.Repositories;

public interface IAtletaRepository : IRepository<Atleta>
{
    Task<Atleta?> GetByDocumentoAsync(string numeroDocumento);
    Task<IEnumerable<Atleta>> SearchByNomeAsync(string nome);
}
