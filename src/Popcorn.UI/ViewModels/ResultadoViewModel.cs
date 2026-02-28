using Microsoft.Win32;
using Popcorn.Application.Interfaces.Repositories;
using Popcorn.Application.Interfaces.Services;
using Popcorn.Domain.Entities;
using Popcorn.Domain.Enums;
using Popcorn.UI.Commands;
using Popcorn.UI.ViewModels.Base;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Input;

namespace Popcorn.UI.ViewModels;

public class ResultadoDisplay
{
    public int Posicao         { get; set; }
    public string AtletaNome   { get; set; } = string.Empty;
    public string Equipe       { get; set; } = string.Empty;
    public string Categoria    { get; set; } = string.Empty;
    public string TempoTotal   { get; set; } = string.Empty;
    public string TempoLiquido { get; set; } = string.Empty;
    public string TempoBruto   { get; set; } = string.Empty;
    public string Status       { get; set; } = string.Empty;
}

public class CategoriaItem
{
    public int Id              { get; set; }
    public string Descricao    { get; set; } = string.Empty;
    public override string ToString() => Descricao;
}

public class ResultadoViewModel : ViewModelBase, ILoadable
{
    private readonly IResultadoService _resultadoService;
    private readonly IEventoService _eventoService;
    private readonly IInscricaoAtletaRepository _inscricaoRepository;
    private readonly IEventoCategoriaRepository _eventoCategoriaRepository;

    // Dados brutos carregados
    private List<Resultado> _todosResultados = new();
    private Dictionary<int, string> _atletaCategoria = new();   // AtletaId → nome da categoria
    private Dictionary<int, int> _atletaNumero = new();         // AtletaId → nº de peito

    // ---- Seleção de evento ----
    private ObservableCollection<Evento> _eventos = new();
    private Evento? _eventoSelecionado;

    // ---- Modo de visualização ----
    private bool _mostrarGeral = true;
    private bool _mostrarPorCategoria;

    // ---- Categorias ----
    private ObservableCollection<CategoriaItem> _categorias = new();
    private CategoriaItem? _categoriaSelecionada;

    // ---- Resultados exibidos ----
    private ObservableCollection<ResultadoDisplay> _resultados = new();

    // ---- Resumo ----
    private int _totalFinalizados;
    private int _totalDNS;
    private int _totalDNF;
    private int _totalDSQ;

    // ---- Estado ----
    private bool _isBusy;
    private string _mensagem = string.Empty;

    // ================================================================
    // Properties
    // ================================================================

    public ObservableCollection<Evento> Eventos
    {
        get => _eventos;
        set => SetProperty(ref _eventos, value);
    }

    public Evento? EventoSelecionado
    {
        get => _eventoSelecionado;
        set { SetProperty(ref _eventoSelecionado, value); _ = CarregarResultadosAsync(null); }
    }

    public bool MostrarGeral
    {
        get => _mostrarGeral;
        set
        {
            SetProperty(ref _mostrarGeral, value);
            if (value) AplicarFiltro();
        }
    }

    public bool MostrarPorCategoria
    {
        get => _mostrarPorCategoria;
        set
        {
            SetProperty(ref _mostrarPorCategoria, value);
            if (value) AplicarFiltro();
        }
    }

    public ObservableCollection<CategoriaItem> Categorias
    {
        get => _categorias;
        set => SetProperty(ref _categorias, value);
    }

    public CategoriaItem? CategoriaSelecionada
    {
        get => _categoriaSelecionada;
        set { SetProperty(ref _categoriaSelecionada, value); if (value != null) AplicarFiltro(); }
    }

    public ObservableCollection<ResultadoDisplay> Resultados
    {
        get => _resultados;
        set => SetProperty(ref _resultados, value);
    }

    public int TotalFinalizados { get => _totalFinalizados; set => SetProperty(ref _totalFinalizados, value); }
    public int TotalDNS         { get => _totalDNS;         set => SetProperty(ref _totalDNS, value); }
    public int TotalDNF         { get => _totalDNF;         set => SetProperty(ref _totalDNF, value); }
    public int TotalDSQ         { get => _totalDSQ;         set => SetProperty(ref _totalDSQ, value); }
    public bool IsBusy          { get => _isBusy;           set => SetProperty(ref _isBusy, value); }
    public string Mensagem      { get => _mensagem;         set => SetProperty(ref _mensagem, value); }

    // ================================================================
    // Commands
    // ================================================================

    public ICommand CarregarCommand   { get; }
    public ICommand RecalcularCommand { get; }
    public ICommand ExportarCsvCommand { get; }

    // ================================================================
    // Constructor
    // ================================================================

    public ResultadoViewModel(
        IResultadoService resultadoService,
        IEventoService eventoService,
        IInscricaoAtletaRepository inscricaoRepository,
        IEventoCategoriaRepository eventoCategoriaRepository)
    {
        _resultadoService          = resultadoService;
        _eventoService             = eventoService;
        _inscricaoRepository       = inscricaoRepository;
        _eventoCategoriaRepository = eventoCategoriaRepository;

        CarregarCommand    = new AsyncRelayCommand(CarregarResultadosAsync,
            _ => EventoSelecionado != null && !IsBusy);

        RecalcularCommand  = new AsyncRelayCommand(RecalcularAsync,
            _ => EventoSelecionado != null && !IsBusy);

        ExportarCsvCommand = new AsyncRelayCommand(ExportarCsvAsync,
            _ => EventoSelecionado != null && Resultados.Count > 0 && !IsBusy);
    }

    // ================================================================
    // ILoadable
    // ================================================================

    public async Task LoadAsync()
    {
        var todos = await _eventoService.GetAllAsync();
        Eventos = new ObservableCollection<Evento>(todos);
    }

    // ================================================================
    // Carregar resultados
    // ================================================================

    private async Task CarregarResultadosAsync(object? _)
    {
        if (EventoSelecionado == null) return;
        IsBusy = true;
        Mensagem = string.Empty;

        try
        {
            // Carrega resultados brutos
            _todosResultados = (await _resultadoService
                .GetClassificacaoGeralAsync(EventoSelecionado.Id)).ToList();

            // Carrega inscrições para mapear categoria e número
            var inscricoes = (await _inscricaoRepository
                .GetByEventoAsync(EventoSelecionado.Id)).ToList();

            var eventoCategorias = (await _eventoCategoriaRepository
                .GetByEventoAsync(EventoSelecionado.Id)).ToList();

            _atletaCategoria = inscricoes.ToDictionary(
                i => i.AtletaId,
                i => eventoCategorias
                         .FirstOrDefault(ec => ec.Id == i.EventoCategoriaId)
                         ?.Categoria?.Descricao ?? "—");

            _atletaNumero = inscricoes.ToDictionary(
                i => i.AtletaId,
                i => i.Numero);

            // Monta lista de categorias para o filtro
            var cats = eventoCategorias
                .Where(ec => ec.Categoria != null)
                .Select(ec => new CategoriaItem
                {
                    Id = ec.Categoria!.Id,
                    Descricao = ec.Categoria.Descricao
                })
                .DistinctBy(c => c.Id)
                .OrderBy(c => c.Descricao)
                .ToList();

            Categorias = new ObservableCollection<CategoriaItem>(cats);
            if (cats.Count > 0) CategoriaSelecionada = cats[0];

            // Resumo
            TotalFinalizados = _todosResultados.Count(r => r.Status == StatusResultado.Finalizado);
            TotalDNS = _todosResultados.Count(r => r.Status == StatusResultado.DNS);
            TotalDNF = _todosResultados.Count(r => r.Status == StatusResultado.DNF);
            TotalDSQ = _todosResultados.Count(r => r.Status == StatusResultado.DSQ);

            AplicarFiltro();
        }
        catch (Exception ex) { Mensagem = $"Erro ao carregar: {ex.Message}"; }
        finally { IsBusy = false; }
    }

    // ================================================================
    // Recalcular
    // ================================================================

    private async Task RecalcularAsync(object? _)
    {
        if (EventoSelecionado == null) return;
        IsBusy = true;
        try
        {
            await _resultadoService.CalcularResultadosAsync(EventoSelecionado.Id);
            await CarregarResultadosAsync(null);
            Mensagem = "Resultados recalculados com sucesso.";
        }
        catch (Exception ex) { Mensagem = $"Erro: {ex.Message}"; }
        finally { IsBusy = false; }
    }

    // ================================================================
    // Filtro Geral / Por Categoria
    // ================================================================

    private void AplicarFiltro()
    {
        if (!_todosResultados.Any()) return;

        IEnumerable<Resultado> fonte;

        if (MostrarGeral)
        {
            fonte = _todosResultados.OrderBy(r => r.PosicaoGeral ?? int.MaxValue);
        }
        else
        {
            if (CategoriaSelecionada == null) return;

            // Filtra atletas que pertencem à categoria selecionada
            var atletasDaCategoria = _atletaCategoria
                .Where(kv => kv.Value == CategoriaSelecionada.Descricao)
                .Select(kv => kv.Key)
                .ToHashSet();

            fonte = _todosResultados
                .Where(r => atletasDaCategoria.Contains(r.AtletaId))
                .OrderBy(r => r.PosicaoCategoria ?? int.MaxValue);
        }

        var lista = fonte.Select((r, idx) => new ResultadoDisplay
        {
            Posicao     = (MostrarGeral ? r.PosicaoGeral : r.PosicaoCategoria) ?? (idx + 1),
            AtletaNome  = r.Atleta?.Nome ?? $"Atleta {r.AtletaId}",
            Equipe      = r.Atleta?.Equipe ?? "—",
            Categoria   = _atletaCategoria.GetValueOrDefault(r.AtletaId, "—"),
            TempoTotal  = FormatarTempo(MostrarGeral ? r.TempoTotal : r.TempoLiquido),
            TempoLiquido = FormatarTempo(r.TempoLiquido),
            TempoBruto  = FormatarTempo(r.TempoBruto),
            Status      = r.Status.ToString()
        }).ToList();

        Resultados = new ObservableCollection<ResultadoDisplay>(lista);
    }

    // ================================================================
    // Exportar CSV
    // ================================================================

    private async Task ExportarCsvAsync(object? _)
    {
        var dlg = new SaveFileDialog
        {
            Title      = "Exportar Resultados",
            Filter     = "CSV (*.csv)|*.csv",
            FileName   = $"resultados_{EventoSelecionado?.Nome}_{DateTime.Now:yyyyMMdd_HHmm}.csv"
        };

        if (dlg.ShowDialog() != true) return;

        IsBusy = true;
        try
        {
            await Task.Run(() =>
            {
                var sb = new StringBuilder();
                sb.AppendLine("Posicao,Atleta,Equipe,Categoria,Tempo Total,Tempo Liquido,Tempo Bruto,Status");

                foreach (var r in Resultados)
                {
                    sb.AppendLine(string.Join(",",
                        r.Posicao,
                        CsvEscape(r.AtletaNome),
                        CsvEscape(r.Equipe),
                        CsvEscape(r.Categoria),
                        r.TempoTotal,
                        r.TempoLiquido,
                        r.TempoBruto,
                        r.Status));
                }

                File.WriteAllText(dlg.FileName, sb.ToString(), Encoding.UTF8);
            });

            Mensagem = $"Exportado: {dlg.FileName}";
        }
        catch (Exception ex) { Mensagem = $"Erro ao exportar: {ex.Message}"; }
        finally { IsBusy = false; }
    }

    // ================================================================
    // Helpers
    // ================================================================

    private static string FormatarTempo(TimeSpan? tempo)
    {
        if (!tempo.HasValue) return "—";
        var t = tempo.Value;
        return t.TotalHours >= 1
            ? $"{(int)t.TotalHours:D2}:{t.Minutes:D2}:{t.Seconds:D2}"
            : $"{t.Minutes:D2}:{t.Seconds:D2}.{t.Milliseconds / 10:D2}";
    }

    private static string CsvEscape(string value)
    {
        if (value.Contains(',') || value.Contains('"') || value.Contains('\n'))
            return $"\"{value.Replace("\"", "\"\"")}\"";
        return value;
    }
}
