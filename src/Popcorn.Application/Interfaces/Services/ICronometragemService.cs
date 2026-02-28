using Popcorn.Domain.Entities;

namespace Popcorn.Application.Interfaces.Services;

public interface ICronometragemService
{
    Task<Evento> IniciarEventoAsync(int eventoId);
    Task<Evento> EncerrarEventoAsync(int eventoId);

    Task<Chegada> RegistrarChegadaAsync(int eventoId, int atletaId, int volta);
    Task<Chegada> RegistrarChegadaPorNumeroAsync(int eventoId, int numero);
    Task<Chegada> RegistrarChegadaPorTagRfidAsync(int eventoId, string tagRfid);

    Task MarcarDNSAsync(int eventoId, int atletaId);
    Task MarcarDNFAsync(int eventoId, int atletaId);
    Task MarcarDSQAsync(int eventoId, int atletaId);

    Task<IEnumerable<Chegada>> GetChegadasAsync(int eventoId);
}
