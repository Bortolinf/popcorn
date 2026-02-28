using Microsoft.EntityFrameworkCore;
using Popcorn.Application.Interfaces.Repositories;
using Popcorn.Domain.Entities;
using Popcorn.Infrastructure.Data;

namespace Popcorn.Infrastructure.Repositories;

public class EventoTrajetoRepository : Repository<EventoTrajeto>, IEventoTrajetoRepository
{
    public EventoTrajetoRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<EventoTrajeto>> GetByEventoAsync(int eventoId) =>
        await _dbSet.Where(t => t.EventoId == eventoId).ToListAsync();
}
