using Popcorn.Application.Interfaces.Services;
using Popcorn.Domain.Entities;
using Popcorn.Domain.Enums;
using Popcorn.UI.Commands;
using Popcorn.UI.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;

namespace Popcorn.UI.ViewModels;

public class ChegadaDisplay
{
    public int Posicao { get; set; }
    public string AtletaNome { get; set; } = string.Empty;
    public int Numero { get; set; }
    public int Volta { get; set; }
    public string HoraChegada { get; set; } = string.Empty;
    public string TempoDecorrido { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class CronometragemViewModel : ViewModelBase, ILoadable
{
    private readonly ICronometragemService _cronometragemService;
    private readonly IEventoService _eventoService;
    private readonly IResultadoService _resultadoService;

    private readonly DispatcherTimer _cronometro = new() { Interval = TimeSpan.FromMilliseconds(100) };
    private DateTime? _horaInicio;

    // Eventos disponíveis
    private ObservableCollection<Evento> _eventos = new();
    private Evento? _eventoSelecionado;

    // Estado
    private string _tempoDecorrido = "00:00:00.0";
    private bool _emAndamento;
    private bool _eventoEncerrado;
    private string _statusTexto = "Pendente";

    private string _mensagem = string.Empty;
    private bool _isBusy;

    // Chegadas
    private ObservableCollection<ChegadaDisplay> _chegadas = new();

    // Inputs
    private string _inputNumero = string.Empty;
    private string _inputTagRfid = string.Empty;

    // ---- Properties ----

    public ObservableCollection<Evento> Eventos
    {
        get => _eventos;
        set => SetProperty(ref _eventos, value);
    }

    public Evento? EventoSelecionado
    {
        get => _eventoSelecionado;
        set
        {
            SetProperty(ref _eventoSelecionado, value);
            _ = AtualizarEstadoEventoAsync();
        }
    }

    public string TempoDecorrido   { get => _tempoDecorrido;  set => SetProperty(ref _tempoDecorrido, value); }
    public bool EmAndamento        { get => _emAndamento;     set => SetProperty(ref _emAndamento, value); }
    public bool EventoEncerrado    { get => _eventoEncerrado; set => SetProperty(ref _eventoEncerrado, value); }
    public string StatusTexto      { get => _statusTexto;     set => SetProperty(ref _statusTexto, value); }
    public string Mensagem         { get => _mensagem;        set => SetProperty(ref _mensagem, value); }
    public bool IsBusy             { get => _isBusy;          set => SetProperty(ref _isBusy, value); }

    public ObservableCollection<ChegadaDisplay> Chegadas
    {
        get => _chegadas;
        set => SetProperty(ref _chegadas, value);
    }

    public string InputNumero
    {
        get => _inputNumero;
        set => SetProperty(ref _inputNumero, value);
    }

    public string InputTagRfid
    {
        get => _inputTagRfid;
        set => SetProperty(ref _inputTagRfid, value);
    }

    // ---- Commands ----

    public ICommand IniciarLargadaCommand     { get; }
    public ICommand EncerrarEventoCommand     { get; }
    public ICommand RegistrarPorNumeroCommand { get; }
    public ICommand RegistrarPorTagCommand    { get; }
    public ICommand MarcarDNSCommand          { get; }
    public ICommand MarcarDNFCommand          { get; }
    public ICommand MarcarDSQCommand          { get; }
    public ICommand CalcularResultadosCommand { get; }
    public ICommand LimparMensagemCommand     { get; }

    public CronometragemViewModel(
        ICronometragemService cronometragemService,
        IEventoService eventoService,
        IResultadoService resultadoService)
    {
        _cronometragemService = cronometragemService;
        _eventoService = eventoService;
        _resultadoService = resultadoService;

        IniciarLargadaCommand     = new AsyncRelayCommand(IniciarLargadaAsync,
            _ => EventoSelecionado != null && !EmAndamento && !EventoEncerrado && !IsBusy);

        EncerrarEventoCommand     = new AsyncRelayCommand(EncerrarEventoAsync,
            _ => EventoSelecionado != null && EmAndamento && !IsBusy);

        RegistrarPorNumeroCommand = new AsyncRelayCommand(RegistrarPorNumeroAsync,
            _ => EmAndamento && !string.IsNullOrWhiteSpace(InputNumero) && !IsBusy);

        RegistrarPorTagCommand    = new AsyncRelayCommand(RegistrarPorTagAsync,
            _ => EmAndamento && !string.IsNullOrWhiteSpace(InputTagRfid) && !IsBusy);

        MarcarDNSCommand          = new AsyncRelayCommand(p => MarcarStatusAsync(p, "DNS"),
            p => EmAndamento && p != null && !IsBusy);

        MarcarDNFCommand          = new AsyncRelayCommand(p => MarcarStatusAsync(p, "DNF"),
            p => EmAndamento && p != null && !IsBusy);

        MarcarDSQCommand          = new AsyncRelayCommand(p => MarcarStatusAsync(p, "DSQ"),
            p => EmAndamento && p != null && !IsBusy);

        CalcularResultadosCommand = new AsyncRelayCommand(CalcularResultadosAsync,
            _ => EventoSelecionado != null && !IsBusy);

        LimparMensagemCommand     = new RelayCommand(_ => Mensagem = string.Empty);

        _cronometro.Tick += (_, _) => AtualizarCronometro();
    }

    // ---- ILoadable ----

    public async Task LoadAsync()
    {
        var todos = await _eventoService.GetAllAsync();
        Eventos = new ObservableCollection<Evento>(
            todos.Where(e => e.Status != StatusEvento.Encerrado));
    }

    // ---- Largada ----

    private async Task IniciarLargadaAsync(object? _)
    {
        if (EventoSelecionado == null) return;
        IsBusy = true;
        try
        {
            var evento = await _cronometragemService.IniciarEventoAsync(EventoSelecionado.Id);
            EventoSelecionado.Status = StatusEvento.EmAndamento;
            EventoSelecionado.HoraLargada = evento.HoraLargada;

            _horaInicio = evento.HoraLargada ?? DateTime.Now;
            EmAndamento = true;
            EventoEncerrado = false;
            AtualizarStatus();
            _cronometro.Start();

            Mensagem = $"Largada registrada: {_horaInicio:HH:mm:ss.fff}";
        }
        catch (Exception ex) { Mensagem = $"Erro: {ex.Message}"; }
        finally { IsBusy = false; }
    }

    // ---- Chegadas ----

    private async Task RegistrarPorNumeroAsync(object? _)
    {
        if (!int.TryParse(InputNumero, out int numero))
        {
            Mensagem = "Número inválido.";
            return;
        }

        IsBusy = true;
        try
        {
            var chegada = await _cronometragemService.RegistrarChegadaPorNumeroAsync(
                EventoSelecionado!.Id, numero);

            AdicionarChegadaDisplay(chegada, numero.ToString());
            InputNumero = string.Empty;
            Mensagem = $"Chegada registrada: nº {numero} — {chegada.HoraChegada:HH:mm:ss.fff}";
        }
        catch (Exception ex) { Mensagem = $"Erro: {ex.Message}"; }
        finally { IsBusy = false; }
    }

    private async Task RegistrarPorTagAsync(object? _)
    {
        if (string.IsNullOrWhiteSpace(InputTagRfid)) return;
        IsBusy = true;
        try
        {
            var chegada = await _cronometragemService.RegistrarChegadaPorTagRfidAsync(
                EventoSelecionado!.Id, InputTagRfid.Trim());

            AdicionarChegadaDisplay(chegada, $"RFID:{InputTagRfid}");
            InputTagRfid = string.Empty;
            Mensagem = $"Chegada RFID registrada: {chegada.HoraChegada:HH:mm:ss.fff}";
        }
        catch (Exception ex) { Mensagem = $"Erro: {ex.Message}"; }
        finally { IsBusy = false; }
    }

    private async Task MarcarStatusAsync(object? param, string tipo)
    {
        if (param is not string numeroStr || !int.TryParse(numeroStr, out int numero)) return;
        IsBusy = true;
        try
        {
            var inscricoes = await _eventoService.GetInscricoesAsync(EventoSelecionado!.Id);
            var inscricao = inscricoes.FirstOrDefault(i => i.Numero == numero);
            if (inscricao == null) { Mensagem = $"Número {numero} não encontrado."; return; }

            switch (tipo)
            {
                case "DNS": await _cronometragemService.MarcarDNSAsync(EventoSelecionado.Id, inscricao.AtletaId); break;
                case "DNF": await _cronometragemService.MarcarDNFAsync(EventoSelecionado.Id, inscricao.AtletaId); break;
                case "DSQ": await _cronometragemService.MarcarDSQAsync(EventoSelecionado.Id, inscricao.AtletaId); break;
            }

            Chegadas.Add(new ChegadaDisplay
            {
                Posicao = Chegadas.Count + 1,
                AtletaNome = inscricao.Atleta?.Nome ?? "—",
                Numero = numero,
                Volta = 0,
                HoraChegada = DateTime.Now.ToString("HH:mm:ss"),
                Status = tipo
            });

            Mensagem = $"Atleta nº {numero} marcado como {tipo}.";
        }
        catch (Exception ex) { Mensagem = $"Erro: {ex.Message}"; }
        finally { IsBusy = false; }
    }

    // ---- Encerrar ----

    private async Task EncerrarEventoAsync(object? _)
    {
        if (EventoSelecionado == null) return;
        IsBusy = true;
        try
        {
            await _cronometragemService.EncerrarEventoAsync(EventoSelecionado.Id);
            _cronometro.Stop();
            EmAndamento = false;
            EventoEncerrado = true;
            AtualizarStatus();
            Mensagem = "Evento encerrado. Clique em Calcular Resultados.";
        }
        catch (Exception ex) { Mensagem = $"Erro: {ex.Message}"; }
        finally { IsBusy = false; }
    }

    // ---- Calcular Resultados ----

    private async Task CalcularResultadosAsync(object? _)
    {
        if (EventoSelecionado == null) return;
        IsBusy = true;
        try
        {
            await _resultadoService.CalcularResultadosAsync(EventoSelecionado.Id);
            Mensagem = "Resultados calculados com sucesso!";
        }
        catch (Exception ex) { Mensagem = $"Erro: {ex.Message}"; }
        finally { IsBusy = false; }
    }

    // ---- Helpers ----

    private void AtualizarCronometro()
    {
        if (_horaInicio == null) return;
        var elapsed = DateTime.Now - _horaInicio.Value;
        TempoDecorrido = $"{(int)elapsed.TotalHours:D2}:{elapsed.Minutes:D2}:{elapsed.Seconds:D2}.{elapsed.Milliseconds / 100}";
    }

    private void AdicionarChegadaDisplay(Chegada chegada, string identificador)
    {
        var elapsed = _horaInicio.HasValue
            ? chegada.HoraChegada - _horaInicio.Value
            : TimeSpan.Zero;

        Chegadas.Insert(0, new ChegadaDisplay
        {
            Posicao = Chegadas.Count + 1,
            AtletaNome = chegada.Atleta?.Nome ?? identificador,
            Numero = chegada.AtletaId,
            Volta = chegada.Volta,
            HoraChegada = chegada.HoraChegada.ToString("HH:mm:ss.fff"),
            TempoDecorrido = $"{(int)elapsed.TotalHours:D2}:{elapsed.Minutes:D2}:{elapsed.Seconds:D2}.{elapsed.Milliseconds / 10:D2}",
            Status = "OK"
        });

        // Renumera posições
        for (int i = 0; i < Chegadas.Count; i++)
            Chegadas[i].Posicao = i + 1;
    }

    private async Task AtualizarEstadoEventoAsync()
    {
        _cronometro.Stop();
        Chegadas.Clear();
        TempoDecorrido = "00:00:00.0";
        Mensagem = string.Empty;

        if (EventoSelecionado == null) { ResetarStatus(); return; }

        EmAndamento    = EventoSelecionado.Status == StatusEvento.EmAndamento;
        EventoEncerrado = EventoSelecionado.Status == StatusEvento.Encerrado;
        AtualizarStatus();

        if (EmAndamento && EventoSelecionado.HoraLargada.HasValue)
        {
            _horaInicio = EventoSelecionado.HoraLargada.Value;
            _cronometro.Start();
        }

        // Carrega chegadas existentes
        var chegadasExistentes = await _cronometragemService.GetChegadasAsync(EventoSelecionado.Id);
        int pos = 1;
        foreach (var c in chegadasExistentes.OrderBy(c => c.HoraChegada))
        {
            var elapsed = _horaInicio.HasValue ? c.HoraChegada - _horaInicio.Value : TimeSpan.Zero;
            Chegadas.Add(new ChegadaDisplay
            {
                Posicao = pos++,
                AtletaNome = c.Atleta?.Nome ?? $"Atleta {c.AtletaId}",
                Numero = c.AtletaId,
                Volta = c.Volta,
                HoraChegada = c.HoraChegada.ToString("HH:mm:ss.fff"),
                TempoDecorrido = $"{(int)elapsed.TotalHours:D2}:{elapsed.Minutes:D2}:{elapsed.Seconds:D2}.{elapsed.Milliseconds / 10:D2}",
                Status = c.FlagDNS ? "DNS" : c.FlagDNF ? "DNF" : c.FlagDSQ ? "DSQ" : "OK"
            });
        }
    }

    private void AtualizarStatus()
    {
        StatusTexto = EventoSelecionado?.Status switch
        {
            StatusEvento.Pendente    => "Pendente",
            StatusEvento.EmAndamento => "Em Andamento",
            StatusEvento.Encerrado   => "Encerrado",
            _                        => "—"
        };
    }

    private void ResetarStatus()
    {
        EmAndamento = false;
        EventoEncerrado = false;
        StatusTexto = "—";
    }
}
