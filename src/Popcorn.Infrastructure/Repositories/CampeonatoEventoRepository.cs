using Microsoft.EntityFrameworkCore;
using Popcorn.Application.Interfaces.Repositories;
using Popcorn.Domain.Entities;
using Popcorn.Infrastructure.Data;

namespace Popcorn.Infrastructure.Repositories;

public class CampeonatoEventoRepository : Repository<CampeonatoEvento>, ICampeonatoEventoRepository
{
    public CampeonatoEventoRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<CampeonatoEvento>> GetByCampeonatoAsync(int campeonatoId) =>
        await _dbSet
            .Include(ce => ce.Evento)
            .Where(ce => ce.CampeonatoId == campeonatoId)
            .ToListAsync();
}
