using Microsoft.EntityFrameworkCore;
using Popcorn.Application.Interfaces.Repositories;
using Popcorn.Domain.Entities;
using Popcorn.Infrastructure.Data;

namespace Popcorn.Infrastructure.Repositories;

public class ChegadaRepository : Repository<Chegada>, IChegadaRepository
{
    public ChegadaRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Chegada>> GetByEventoAsync(int eventoId) =>
        await _dbSet
            .Include(c => c.Atleta)
            .Where(c => c.EventoId == eventoId)
            .OrderBy(c => c.HoraChegada)
            .ToListAsync();

    public async Task<IEnumerable<Chegada>> GetByAtletaAsync(int atletaId, int eventoId) =>
        await _dbSet
            .Where(c => c.AtletaId == atletaId && c.EventoId == eventoId)
            .OrderBy(c => c.Volta)
            .ToListAsync();
}
