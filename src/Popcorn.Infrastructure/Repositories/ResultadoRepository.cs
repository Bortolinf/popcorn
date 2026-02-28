using Microsoft.EntityFrameworkCore;
using Popcorn.Application.Interfaces.Repositories;
using Popcorn.Domain.Entities;
using Popcorn.Infrastructure.Data;

namespace Popcorn.Infrastructure.Repositories;

public class ResultadoRepository : Repository<Resultado>, IResultadoRepository
{
    public ResultadoRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Resultado>> GetClassificacaoGeralAsync(int eventoId) =>
        await _dbSet
            .Include(r => r.Atleta)
            .Where(r => r.EventoId == eventoId)
            .OrderBy(r => r.PosicaoGeral)
            .ToListAsync();

    public async Task<IEnumerable<Resultado>> GetClassificacaoCategoriaAsync(int eventoId, int categoriaId) =>
        await _dbSet
            .Include(r => r.Atleta)
            .Where(r => r.EventoId == eventoId)
            .OrderBy(r => r.PosicaoCategoria)
            .ToListAsync();
}
