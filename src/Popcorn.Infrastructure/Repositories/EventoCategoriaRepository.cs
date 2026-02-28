using Microsoft.EntityFrameworkCore;
using Popcorn.Application.Interfaces.Repositories;
using Popcorn.Domain.Entities;
using Popcorn.Infrastructure.Data;

namespace Popcorn.Infrastructure.Repositories;

public class EventoCategoriaRepository : Repository<EventoCategoria>, IEventoCategoriaRepository
{
    public EventoCategoriaRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<EventoCategoria>> GetByEventoAsync(int eventoId) =>
        await _dbSet
            .Include(ec => ec.Categoria)
            .Include(ec => ec.EventoTrajeto)
            .Where(ec => ec.EventoId == eventoId)
            .ToListAsync();
}
