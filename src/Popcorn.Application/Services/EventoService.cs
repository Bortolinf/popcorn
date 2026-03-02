using Popcorn.Application.Interfaces.Repositories;
using Popcorn.Application.Interfaces.Services;
using Popcorn.Domain.Entities;
using Popcorn.Domain.Enums;

namespace Popcorn.Application.Services;

public class EventoService : IEventoService
{
    private readonly IEventoRepository _eventoRepository;
    private readonly IEventoTrajetoRepository _trajetoRepository;
    private readonly IEventoCategoriaRepository _eventoCategoriaRepository;
    private readonly IInscricaoAtletaRepository _inscricaoRepository;

    public EventoService(
        IEventoRepository eventoRepository,
        IEventoTrajetoRepository trajetoRepository,
        IEventoCategoriaRepository eventoCategoriaRepository,
        IInscricaoAtletaRepository inscricaoRepository)
    {
        _eventoRepository = eventoRepository;
        _trajetoRepository = trajetoRepository;
        _eventoCategoriaRepository = eventoCategoriaRepository;
        _inscricaoRepository = inscricaoRepository;
    }

    public async Task<Evento?> GetByIdAsync(int id) =>
        await _eventoRepository.GetWithTrajetosECategoriasAsync(id);

    public async Task<IEnumerable<Evento>> GetAllAsync() =>
        await _eventoRepository.GetAllAsync();

    public async Task<IEnumerable<Evento>> GetByStatusAsync(StatusEvento status) =>
        await _eventoRepository.GetByStatusAsync(status);

    public async Task<Evento> CreateAsync(Evento evento)
    {
        evento.Status = StatusEvento.Pendente;
        evento.CreatedAt = DateTime.Now;
        await _eventoRepository.AddAsync(evento);
        await _eventoRepository.SaveChangesAsync();
        return evento;
    }

    public async Task UpdateAsync(Evento evento)
    {
        await _eventoRepository.UpdateAsync(evento);
        await _eventoRepository.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var evento = await _eventoRepository.GetByIdAsync(id)
            ?? throw new InvalidOperationException($"Evento {id} não encontrado.");

        await _eventoRepository.DeleteAsync(evento);
        await _eventoRepository.SaveChangesAsync();
    }

    public async Task<EventoTrajeto> AddTrajetoAsync(EventoTrajeto trajeto)
    {
        trajeto.CreatedAt = DateTime.Now;
        await _trajetoRepository.AddAsync(trajeto);
        await _trajetoRepository.SaveChangesAsync();
        return trajeto;
    }

    public async Task UpdateTrajetoAsync(EventoTrajeto trajeto)
    {
        await _trajetoRepository.UpdateAsync(trajeto);
        await _trajetoRepository.SaveChangesAsync();
    }

    public async Task RemoveTrajetoAsync(int trajetoId)
    {
        var trajeto = await _trajetoRepository.GetByIdAsync(trajetoId)
            ?? throw new InvalidOperationException($"Trajeto {trajetoId} não encontrado.");

        await _trajetoRepository.DeleteAsync(trajeto);
        await _trajetoRepository.SaveChangesAsync();
    }

    public async Task<EventoCategoria> AddCategoriaAsync(EventoCategoria eventoCategoria)
    {
        eventoCategoria.CreatedAt = DateTime.Now;
        await _eventoCategoriaRepository.AddAsync(eventoCategoria);
        await _eventoCategoriaRepository.SaveChangesAsync();
        return eventoCategoria;
    }

    public async Task UpdateCategoriaAsync(EventoCategoria eventoCategoria)
    {
        await _eventoCategoriaRepository.UpdateAsync(eventoCategoria);
        await _eventoCategoriaRepository.SaveChangesAsync();
    }

    public async Task RemoveCategoriaAsync(int eventoCategoriaId)
    {
        var eventoCategoria = await _eventoCategoriaRepository.GetByIdAsync(eventoCategoriaId)
            ?? throw new InvalidOperationException($"EventoCategoria {eventoCategoriaId} não encontrada.");

        await _eventoCategoriaRepository.DeleteAsync(eventoCategoria);
        await _eventoCategoriaRepository.SaveChangesAsync();
    }

    public async Task<InscricaoAtleta> InscreverAtletaAsync(InscricaoAtleta inscricao)
    {
        inscricao.CreatedAt = DateTime.Now;
        await _inscricaoRepository.AddAsync(inscricao);
        await _inscricaoRepository.SaveChangesAsync();
        return inscricao;
    }

    public async Task CancelarInscricaoAsync(int inscricaoId)
    {
        var inscricao = await _inscricaoRepository.GetByIdAsync(inscricaoId)
            ?? throw new InvalidOperationException($"Inscrição {inscricaoId} não encontrada.");

        await _inscricaoRepository.DeleteAsync(inscricao);
        await _inscricaoRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<InscricaoAtleta>> GetInscricoesAsync(int eventoId) =>
        await _inscricaoRepository.GetByEventoAsync(eventoId);
}
