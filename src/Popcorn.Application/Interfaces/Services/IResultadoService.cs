using Popcorn.Domain.Entities;

namespace Popcorn.Application.Interfaces.Services;

public interface IResultadoService
{
    Task CalcularResultadosAsync(int eventoId);
    Task<IEnumerable<Resultado>> GetClassificacaoGeralAsync(int eventoId);
    Task<IEnumerable<Resultado>> GetClassificacaoCategoriaAsync(int eventoId, int categoriaId);
}
