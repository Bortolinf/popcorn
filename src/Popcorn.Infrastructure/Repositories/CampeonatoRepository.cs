using Microsoft.EntityFrameworkCore;
using Popcorn.Application.Interfaces.Repositories;
using Popcorn.Domain.Entities;
using Popcorn.Infrastructure.Data;

namespace Popcorn.Infrastructure.Repositories;

public class CampeonatoRepository : Repository<Campeonato>, ICampeonatoRepository
{
    public CampeonatoRepository(AppDbContext context) : base(context) { }

    public async Task<Campeonato?> GetWithEventosAsync(int id) =>
        await _dbSet
            .Include(c => c.CampeonatoEventos)
                .ThenInclude(ce => ce.Evento)
            .FirstOrDefaultAsync(c => c.Id == id);
}
