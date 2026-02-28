using Popcorn.Application.Interfaces.Repositories;
using Popcorn.Application.Interfaces.Services;
using Popcorn.Domain.Entities;
using Popcorn.Domain.Enums;

namespace Popcorn.Application.Services;

public class ResultadoService : IResultadoService
{
    private readonly IEventoRepository _eventoRepository;
    private readonly IInscricaoAtletaRepository _inscricaoRepository;
    private readonly IChegadaRepository _chegadaRepository;
    private readonly IResultadoRepository _resultadoRepository;
    private readonly IEventoCategoriaRepository _eventoCategoriaRepository;

    public ResultadoService(
        IEventoRepository eventoRepository,
        IInscricaoAtletaRepository inscricaoRepository,
        IChegadaRepository chegadaRepository,
        IResultadoRepository resultadoRepository,
        IEventoCategoriaRepository eventoCategoriaRepository)
    {
        _eventoRepository = eventoRepository;
        _inscricaoRepository = inscricaoRepository;
        _chegadaRepository = chegadaRepository;
        _resultadoRepository = resultadoRepository;
        _eventoCategoriaRepository = eventoCategoriaRepository;
    }

    public async Task CalcularResultadosAsync(int eventoId)
    {
        var evento = await _eventoRepository.GetByIdAsync(eventoId)
            ?? throw new InvalidOperationException($"Evento {eventoId} não encontrado.");

        var inscricoes = (await _inscricaoRepository.GetByEventoAsync(eventoId)).ToList();
        var chegadas = (await _chegadaRepository.GetByEventoAsync(eventoId)).ToList();
        var resultadosExistentes = (await _resultadoRepository.GetClassificacaoGeralAsync(eventoId)).ToList();

        var resultados = new List<Resultado>();

        foreach (var inscricao in inscricoes)
        {
            var chegadasAtleta = chegadas
                .Where(c => c.AtletaId == inscricao.AtletaId && !c.FlagDNS && !c.FlagDNF && !c.FlagDSQ)
                .OrderBy(c => c.Volta)
                .ToList();

            var ultimaChegada = chegadasAtleta.LastOrDefault();

            var chegadaStatus = chegadas
                .FirstOrDefault(c => c.AtletaId == inscricao.AtletaId
                    && (c.FlagDNS || c.FlagDNF || c.FlagDSQ));

            var resultado = resultadosExistentes
                .FirstOrDefault(r => r.AtletaId == inscricao.AtletaId)
                ?? new Resultado { AtletaId = inscricao.AtletaId, EventoId = eventoId, CreatedAt = DateTime.Now };

            if (chegadaStatus != null)
            {
                resultado.Status = chegadaStatus.FlagDNS ? StatusResultado.DNS
                    : chegadaStatus.FlagDNF ? StatusResultado.DNF
                    : StatusResultado.DSQ;
                resultado.TempoBruto = null;
                resultado.TempoLiquido = null;
                resultado.TempoTotal = null;
            }
            else if (ultimaChegada != null && evento.HoraLargada.HasValue)
            {
                var horaLargadaAtleta = ResolverHoraLargada(inscricao, evento);

                resultado.TempoBruto = ultimaChegada.HoraChegada - evento.HoraLargada.Value;
                resultado.TempoLiquido = ultimaChegada.HoraChegada - horaLargadaAtleta;
                resultado.TempoTotal = evento.ClassificarTempoBruto
                    ? resultado.TempoBruto
                    : resultado.TempoLiquido;
                resultado.Status = StatusResultado.Finalizado;
            }

            resultados.Add(resultado);
        }

        // Classificação geral (somente finalizados)
        var finalizados = resultados
            .Where(r => r.Status == StatusResultado.Finalizado && r.TempoTotal.HasValue)
            .OrderBy(r => r.TempoTotal)
            .ToList();

        for (int i = 0; i < finalizados.Count; i++)
            finalizados[i].PosicaoGeral = i + 1;

        // Classificação por categoria
        var categorias = await _eventoCategoriaRepository.GetByEventoAsync(eventoId);
        foreach (var categoria in categorias)
        {
            var inscricoesCategoria = inscricoes
                .Where(i => i.EventoCategoriaId == categoria.Id)
                .Select(i => i.AtletaId)
                .ToHashSet();

            var finalizadosCategoria = finalizados
                .Where(r => inscricoesCategoria.Contains(r.AtletaId))
                .OrderBy(r => r.TempoTotal)
                .ToList();

            for (int i = 0; i < finalizadosCategoria.Count; i++)
                finalizadosCategoria[i].PosicaoCategoria = i + 1;
        }

        // Persiste resultados
        foreach (var resultado in resultados)
        {
            if (resultado.Id == 0)
                await _resultadoRepository.AddAsync(resultado);
            else
                await _resultadoRepository.UpdateAsync(resultado);
        }

        await _resultadoRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<Resultado>> GetClassificacaoGeralAsync(int eventoId) =>
        await _resultadoRepository.GetClassificacaoGeralAsync(eventoId);

    public async Task<IEnumerable<Resultado>> GetClassificacaoCategoriaAsync(int eventoId, int categoriaId) =>
        await _resultadoRepository.GetClassificacaoCategoriaAsync(eventoId, categoriaId);

    private DateTime ResolverHoraLargada(InscricaoAtleta inscricao, Evento evento)
    {
        if (inscricao.HoraLargada.HasValue)
            return inscricao.HoraLargada.Value;

        if (evento.HoraLargada.HasValue)
            return evento.HoraLargada.Value;

        return DateTime.Now;
    }
}
