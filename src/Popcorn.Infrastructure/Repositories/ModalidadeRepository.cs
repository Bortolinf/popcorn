using Popcorn.Application.Interfaces.Repositories;
using Popcorn.Domain.Entities;
using Popcorn.Infrastructure.Data;

namespace Popcorn.Infrastructure.Repositories;

public class ModalidadeRepository : Repository<Modalidade>, IModalidadeRepository
{
    public ModalidadeRepository(AppDbContext context) : base(context) { }
}
