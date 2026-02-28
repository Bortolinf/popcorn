using Microsoft.EntityFrameworkCore;
using Popcorn.Application.Interfaces.Repositories;
using Popcorn.Domain.Entities;
using Popcorn.Domain.Enums;
using Popcorn.Infrastructure.Data;

namespace Popcorn.Infrastructure.Repositories;

public class EventoRepository : Repository<Evento>, IEventoRepository
{
    public EventoRepository(AppDbContext context) : base(context) { }

    public override async Task<IEnumerable<Evento>> GetAllAsync() =>
        await _dbSet.Include(e => e.Modalidade).ToListAsync();

    public async Task<IEnumerable<Evento>> GetByStatusAsync(StatusEvento status) =>
        await _dbSet.Where(e => e.Status == status).ToListAsync();

    public async Task<Evento?> GetWithTrajetosECategoriasAsync(int id) =>
        await _dbSet
            .Include(e => e.Modalidade)
            .Include(e => e.Trajetos)
                .ThenInclude(t => t.EventoCategorias)
                    .ThenInclude(ec => ec.Categoria)
            .Include(e => e.Categorias)
                .ThenInclude(ec => ec.Categoria)
            .FirstOrDefaultAsync(e => e.Id == id);
}
