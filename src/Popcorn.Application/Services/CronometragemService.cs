using Popcorn.Application.Interfaces.Repositories;
using Popcorn.Application.Interfaces.Services;
using Popcorn.Domain.Entities;
using Popcorn.Domain.Enums;

namespace Popcorn.Application.Services;

public class CronometragemService : ICronometragemService
{
    private readonly IEventoRepository _eventoRepository;
    private readonly IInscricaoAtletaRepository _inscricaoRepository;
    private readonly IChegadaRepository _chegadaRepository;

    public CronometragemService(
        IEventoRepository eventoRepository,
        IInscricaoAtletaRepository inscricaoRepository,
        IChegadaRepository chegadaRepository)
    {
        _eventoRepository = eventoRepository;
        _inscricaoRepository = inscricaoRepository;
        _chegadaRepository = chegadaRepository;
    }

    public async Task<Evento> IniciarEventoAsync(int eventoId)
    {
        var evento = await _eventoRepository.GetByIdAsync(eventoId)
            ?? throw new InvalidOperationException($"Evento {eventoId} não encontrado.");

        if (evento.Status != StatusEvento.Pendente)
            throw new InvalidOperationException("O evento precisa estar com status Pendente para ser iniciado.");

        evento.Status = StatusEvento.EmAndamento;
        evento.HoraLargada = DateTime.Now;

        await _eventoRepository.UpdateAsync(evento);
        await _eventoRepository.SaveChangesAsync();
        return evento;
    }

    public async Task<Evento> EncerrarEventoAsync(int eventoId)
    {
        var evento = await _eventoRepository.GetByIdAsync(eventoId)
            ?? throw new InvalidOperationException($"Evento {eventoId} não encontrado.");

        if (evento.Status != StatusEvento.EmAndamento)
            throw new InvalidOperationException("O evento precisa estar EmAndamento para ser encerrado.");

        evento.Status = StatusEvento.Encerrado;

        await _eventoRepository.UpdateAsync(evento);
        await _eventoRepository.SaveChangesAsync();
        return evento;
    }

    public async Task<Chegada> RegistrarChegadaAsync(int eventoId, int atletaId, int volta)
    {
        await ValidarEventoEmAndamentoAsync(eventoId);

        var chegada = new Chegada
        {
            EventoId = eventoId,
            AtletaId = atletaId,
            Volta = volta,
            HoraChegada = DateTime.Now,
            CreatedAt = DateTime.Now
        };

        await _chegadaRepository.AddAsync(chegada);
        await _chegadaRepository.SaveChangesAsync();
        return chegada;
    }

    public async Task<Chegada> RegistrarChegadaPorNumeroAsync(int eventoId, int numero)
    {
        await ValidarEventoEmAndamentoAsync(eventoId);

        var inscricao = await _inscricaoRepository.GetByNumeroAsync(eventoId, numero)
            ?? throw new InvalidOperationException($"Atleta com número {numero} não encontrado no evento.");

        var chegadas = await _chegadaRepository.GetByAtletaAsync(inscricao.AtletaId, eventoId);
        var proximaVolta = chegadas.Count() + 1;

        return await RegistrarChegadaAsync(eventoId, inscricao.AtletaId, proximaVolta);
    }

    public async Task<Chegada> RegistrarChegadaPorTagRfidAsync(int eventoId, string tagRfid)
    {
        await ValidarEventoEmAndamentoAsync(eventoId);

        var inscricao = await _inscricaoRepository.GetByTagRfidAsync(eventoId, tagRfid)
            ?? throw new InvalidOperationException($"Tag RFID '{tagRfid}' não encontrada no evento.");

        var chegadas = await _chegadaRepository.GetByAtletaAsync(inscricao.AtletaId, eventoId);
        var proximaVolta = chegadas.Count() + 1;

        return await RegistrarChegadaAsync(eventoId, inscricao.AtletaId, proximaVolta);
    }

    public async Task MarcarDNSAsync(int eventoId, int atletaId)
    {
        var chegada = await CriarChegadaStatusAsync(eventoId, atletaId);
        chegada.FlagDNS = true;
        await _chegadaRepository.SaveChangesAsync();
    }

    public async Task MarcarDNFAsync(int eventoId, int atletaId)
    {
        var chegada = await CriarChegadaStatusAsync(eventoId, atletaId);
        chegada.FlagDNF = true;
        await _chegadaRepository.SaveChangesAsync();
    }

    public async Task MarcarDSQAsync(int eventoId, int atletaId)
    {
        var chegada = await CriarChegadaStatusAsync(eventoId, atletaId);
        chegada.FlagDSQ = true;
        await _chegadaRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<Chegada>> GetChegadasAsync(int eventoId) =>
        await _chegadaRepository.GetByEventoAsync(eventoId);

    private async Task ValidarEventoEmAndamentoAsync(int eventoId)
    {
        var evento = await _eventoRepository.GetByIdAsync(eventoId)
            ?? throw new InvalidOperationException($"Evento {eventoId} não encontrado.");

        if (evento.Status != StatusEvento.EmAndamento)
            throw new InvalidOperationException("O evento não está em andamento.");
    }

    private async Task<Chegada> CriarChegadaStatusAsync(int eventoId, int atletaId)
    {
        var chegada = new Chegada
        {
            EventoId = eventoId,
            AtletaId = atletaId,
            Volta = 0,
            HoraChegada = DateTime.Now,
            CreatedAt = DateTime.Now
        };

        await _chegadaRepository.AddAsync(chegada);
        return chegada;
    }
}
