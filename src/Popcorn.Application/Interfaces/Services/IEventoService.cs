using Popcorn.Domain.Entities;
using Popcorn.Domain.Enums;

namespace Popcorn.Application.Interfaces.Services;

public interface IEventoService
{
    Task<Evento?> GetByIdAsync(int id);
    Task<IEnumerable<Evento>> GetAllAsync();
    Task<IEnumerable<Evento>> GetByStatusAsync(StatusEvento status);
    Task<Evento> CreateAsync(Evento evento);
    Task UpdateAsync(Evento evento);
    Task DeleteAsync(int id);

    Task<EventoTrajeto> AddTrajetoAsync(EventoTrajeto trajeto);
    Task UpdateTrajetoAsync(EventoTrajeto trajeto);
    Task RemoveTrajetoAsync(int trajetoId);

    Task<EventoCategoria> AddCategoriaAsync(EventoCategoria eventoCategoria);
    Task UpdateCategoriaAsync(EventoCategoria eventoCategoria);
    Task RemoveCategoriaAsync(int eventoCategoriaId);

    Task<InscricaoAtleta> InscreverAtletaAsync(InscricaoAtleta inscricao);
    Task CancelarInscricaoAsync(int inscricaoId);
    Task<IEnumerable<InscricaoAtleta>> GetInscricoesAsync(int eventoId);
}
