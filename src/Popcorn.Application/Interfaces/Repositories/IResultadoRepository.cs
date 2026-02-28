using Popcorn.Domain.Entities;

namespace Popcorn.Application.Interfaces.Repositories;

public interface IResultadoRepository : IRepository<Resultado>
{
    Task<IEnumerable<Resultado>> GetClassificacaoGeralAsync(int eventoId);
    Task<IEnumerable<Resultado>> GetClassificacaoCategoriaAsync(int eventoId, int categoriaId);
}
