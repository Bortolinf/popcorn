using Microsoft.EntityFrameworkCore;
using Popcorn.Application.Interfaces.Repositories;
using Popcorn.Domain.Entities;
using Popcorn.Infrastructure.Data;

namespace Popcorn.Infrastructure.Repositories;

public class AtletaRepository : Repository<Atleta>, IAtletaRepository
{
    public AtletaRepository(AppDbContext context) : base(context) { }

    public async Task<Atleta?> GetByDocumentoAsync(string numeroDocumento) =>
        await _dbSet.FirstOrDefaultAsync(a => a.NumeroDocumento == numeroDocumento);

    public async Task<IEnumerable<Atleta>> SearchByNomeAsync(string nome) =>
        await _dbSet.Where(a => a.Nome.Contains(nome)).ToListAsync();
}
