using Microsoft.EntityFrameworkCore;
using Popcorn.Application.Interfaces.Repositories;
using Popcorn.Domain.Entities;
using Popcorn.Infrastructure.Data;

namespace Popcorn.Infrastructure.Repositories;

public class InscricaoAtletaRepository : Repository<InscricaoAtleta>, IInscricaoAtletaRepository
{
    public InscricaoAtletaRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<InscricaoAtleta>> GetByEventoAsync(int eventoId) =>
        await _dbSet
            .Include(i => i.Atleta)
            .Include(i => i.EventoCategoria)
                .ThenInclude(ec => ec.Categoria)
            .Where(i => i.EventoId == eventoId)
            .ToListAsync();

    public async Task<IEnumerable<InscricaoAtleta>> GetByAtletaAsync(int atletaId) =>
        await _dbSet
            .Include(i => i.Evento)
            .Where(i => i.AtletaId == atletaId)
            .ToListAsync();

    public async Task<InscricaoAtleta?> GetByNumeroAsync(int eventoId, int numero) =>
        await _dbSet
            .Include(i => i.Atleta)
            .FirstOrDefaultAsync(i => i.EventoId == eventoId && i.Numero == numero);

    public async Task<InscricaoAtleta?> GetByTagRfidAsync(int eventoId, string tagRfid) =>
        await _dbSet
            .Include(i => i.Atleta)
            .FirstOrDefaultAsync(i => i.EventoId == eventoId && i.TagRfid == tagRfid);
}
