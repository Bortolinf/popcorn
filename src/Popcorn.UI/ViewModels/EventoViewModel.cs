using Popcorn.Application.Interfaces.Repositories;
using Popcorn.Application.Interfaces.Services;
using Popcorn.Domain.Entities;
using Popcorn.UI.Commands;
using Popcorn.UI.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Popcorn.UI.ViewModels;

public class EventoViewModel : ViewModelBase, ILoadable
{
    private readonly IEventoService _eventoService;
    private readonly IEventoRepository _eventoRepository;
    private readonly IModalidadeRepository _modalidadeRepository;
    private readonly ICategoriaRepository _categoriaRepository;

    // --- Eventos ---
    private ObservableCollection<Evento> _eventos = new();
    private Evento? _selectedEvento;
    private string _nome = string.Empty;
    private DateTime _data = DateTime.Today;
    private bool _isEditing;
    private string _mensagem = string.Empty;

    // --- Modalidades ---
    private ObservableCollection<Modalidade> _modalidades = new();
    private Modalidade? _selectedModalidade;

    // --- Trajetos ---
    private ObservableCollection<EventoTrajeto> _trajetos = new();
    private EventoTrajeto? _selectedTrajeto;

    // --- Mini-form Trajeto ---
    private string _novoTrajetoNome = string.Empty;
    private decimal _novoTrajetoDistancia;
    private DateTime? _novoTrajetoHoraLargada;
    private bool _isAddingTrajeto;

    // --- Categorias do Trajeto ---
    private ObservableCollection<EventoCategoria> _categoriasDoTrajeto = new();
    private EventoCategoria? _selectedCategoriaDoTrajeto;

    // --- Mini-form Categoria ---
    private ObservableCollection<Categoria> _todasCategorias = new();
    private ObservableCollection<CategoriaSelecionavel> _categoriasSelecionaveis = new();
    private bool _isAddingCategoria;

    // Properties — Eventos
    public ObservableCollection<Evento> Eventos { get => _eventos; set => SetProperty(ref _eventos, value); }
    public Evento? SelectedEvento
    {
        get => _selectedEvento;
        set
        {
            SetProperty(ref _selectedEvento, value);
            if (value != null)
                _ = PreencherFormularioAsync(value);
            else
            {
                IsEditing = false;
                Trajetos.Clear();
                CategoriasDoTrajeto.Clear();
            }
            OnPropertyChanged(nameof(HasSelectedEvento));
        }
    }

    public string Nome    { get => _nome;      set => SetProperty(ref _nome, value); }
    public DateTime Data  { get => _data;      set => SetProperty(ref _data, value); }
    public bool IsEditing { get => _isEditing; set => SetProperty(ref _isEditing, value); }
    public string Mensagem { get => _mensagem; set => SetProperty(ref _mensagem, value); }
    public bool HasSelectedEvento => _selectedEvento != null;

    // Properties — Modalidades
    public ObservableCollection<Modalidade> Modalidades { get => _modalidades; set => SetProperty(ref _modalidades, value); }
    public Modalidade? SelectedModalidade
    {
        get => _selectedModalidade;
        set { SetProperty(ref _selectedModalidade, value); OnPropertyChanged(nameof(PossuiVoltas)); }
    }
    public bool PossuiVoltas => _selectedModalidade?.PossuiVoltas ?? false;

    // Properties — Trajetos
    public ObservableCollection<EventoTrajeto> Trajetos { get => _trajetos; set => SetProperty(ref _trajetos, value); }
    public EventoTrajeto? SelectedTrajeto
    {
        get => _selectedTrajeto;
        set
        {
            SetProperty(ref _selectedTrajeto, value);
            CarregarCategoriasDoTrajeto(value);
            IsAddingCategoria = false;
            OnPropertyChanged(nameof(HasSelectedTrajeto));
        }
    }
    public bool HasSelectedTrajeto => _selectedTrajeto != null;

    // Properties — Mini-form Trajeto
    public string NovoTrajetoNome       { get => _novoTrajetoNome;       set => SetProperty(ref _novoTrajetoNome, value); }
    public decimal NovoTrajetoDistancia { get => _novoTrajetoDistancia;  set => SetProperty(ref _novoTrajetoDistancia, value); }
    public DateTime? NovoTrajetoHoraLargada { get => _novoTrajetoHoraLargada; set => SetProperty(ref _novoTrajetoHoraLargada, value); }
    public bool IsAddingTrajeto         { get => _isAddingTrajeto;        set => SetProperty(ref _isAddingTrajeto, value); }

    // Properties — Categorias do Trajeto
    public ObservableCollection<EventoCategoria> CategoriasDoTrajeto { get => _categoriasDoTrajeto; set => SetProperty(ref _categoriasDoTrajeto, value); }
    public EventoCategoria? SelectedCategoriaDoTrajeto { get => _selectedCategoriaDoTrajeto; set => SetProperty(ref _selectedCategoriaDoTrajeto, value); }

    // Properties — Mini-form Categoria
    public ObservableCollection<Categoria> TodasCategorias { get => _todasCategorias; set => SetProperty(ref _todasCategorias, value); }
    public ObservableCollection<CategoriaSelecionavel> CategoriasSelecionaveis { get => _categoriasSelecionaveis; set => SetProperty(ref _categoriasSelecionaveis, value); }
    public bool IsAddingCategoria { get => _isAddingCategoria; set => SetProperty(ref _isAddingCategoria, value); }

    // Commands
    public ICommand NovoCommand    { get; }
    public ICommand SalvarCommand  { get; }
    public ICommand ExcluirCommand { get; }
    public ICommand CancelarCommand { get; }

    public ICommand AddTrajetoCommand       { get; }
    public ICommand RemoveTrajetoCommand    { get; }
    public ICommand ConfirmarTrajetoCommand { get; }
    public ICommand CancelarTrajetoCommand  { get; }

    public ICommand AddCategoriaCommand       { get; }
    public ICommand RemoveCategoriaCommand    { get; }
    public ICommand ConfirmarCategoriaCommand { get; }
    public ICommand CancelarCategoriaCommand  { get; }

    public EventoViewModel(
        IEventoService eventoService,
        IEventoRepository eventoRepository,
        IModalidadeRepository modalidadeRepository,
        ICategoriaRepository categoriaRepository)
    {
        _eventoService        = eventoService;
        _eventoRepository     = eventoRepository;
        _modalidadeRepository = modalidadeRepository;
        _categoriaRepository  = categoriaRepository;

        NovoCommand     = new RelayCommand(_ => Novo());
        SalvarCommand   = new AsyncRelayCommand(SalvarAsync);
        ExcluirCommand  = new AsyncRelayCommand(ExcluirAsync, _ => SelectedEvento != null);
        CancelarCommand = new RelayCommand(_ => Cancelar());

        AddTrajetoCommand       = new RelayCommand(_ => { IsAddingTrajeto = true; LimparFormTrajeto(); }, _ => SelectedEvento != null);
        RemoveTrajetoCommand    = new AsyncRelayCommand(RemoveTrajetoAsync, _ => SelectedTrajeto != null);
        ConfirmarTrajetoCommand = new AsyncRelayCommand(ConfirmarTrajetoAsync, _ => !string.IsNullOrWhiteSpace(NovoTrajetoNome));
        CancelarTrajetoCommand  = new RelayCommand(_ => { IsAddingTrajeto = false; LimparFormTrajeto(); });

        AddCategoriaCommand       = new RelayCommand(_ => { IsAddingCategoria = true; PopularCategoriasSelecionaveis(); }, _ => SelectedTrajeto != null);
        RemoveCategoriaCommand    = new AsyncRelayCommand(RemoveCategoriaAsync, _ => SelectedCategoriaDoTrajeto != null);
        ConfirmarCategoriaCommand = new AsyncRelayCommand(ConfirmarCategoriaAsync);
        CancelarCategoriaCommand  = new RelayCommand(_ => { IsAddingCategoria = false; LimparFormCategoria(); });
    }

    public async Task LoadAsync()
    {
        var eventos    = await _eventoService.GetAllAsync();
        Eventos        = new ObservableCollection<Evento>(eventos);

        var modalidades = await _modalidadeRepository.GetAllAsync();
        Modalidades    = new ObservableCollection<Modalidade>(modalidades);

        var categorias = await _categoriaRepository.GetAllAsync();
        TodasCategorias = new ObservableCollection<Categoria>(categorias);
    }

    private void Novo()
    {
        SelectedEvento     = null;
        Nome               = string.Empty;
        Data               = DateTime.Today;
        SelectedModalidade = null;
        Trajetos.Clear();
        CategoriasDoTrajeto.Clear();
        IsEditing = true;
        Mensagem  = string.Empty;
    }

    private async Task PreencherFormularioAsync(Evento evento)
    {
        Nome               = evento.Nome;
        Data               = evento.Data;
        SelectedModalidade = Modalidades.FirstOrDefault(m => m.Id == evento.ModalidadeId);
        IsEditing          = true;

        var eventoCompleto = await _eventoRepository.GetWithTrajetosECategoriasAsync(evento.Id);
        if (eventoCompleto != null)
            Trajetos = new ObservableCollection<EventoTrajeto>(eventoCompleto.Trajetos);

        SelectedTrajeto = null;
        CategoriasDoTrajeto.Clear();
    }

    private async Task SalvarAsync(object? _)
    {
        try
        {
            if (SelectedEvento == null)
            {
                var novo = new Evento { Nome = Nome, Data = Data, ModalidadeId = SelectedModalidade?.Id };
                await _eventoService.CreateAsync(novo);
            }
            else
            {
                SelectedEvento.Nome        = Nome;
                SelectedEvento.Data        = Data;
                SelectedEvento.ModalidadeId = SelectedModalidade?.Id;
                await _eventoService.UpdateAsync(SelectedEvento);
            }

            Mensagem  = "Salvo com sucesso.";
            IsEditing = false;
            await LoadAsync();
        }
        catch (Exception ex) { Mensagem = $"Erro: {ex.Message}"; }
    }

    private async Task ExcluirAsync(object? _)
    {
        if (SelectedEvento == null) return;
        try
        {
            await _eventoService.DeleteAsync(SelectedEvento.Id);
            Mensagem  = "Excluído com sucesso.";
            IsEditing = false;
            await LoadAsync();
        }
        catch (Exception ex) { Mensagem = $"Erro: {ex.Message}"; }
    }

    private void Cancelar()
    {
        IsEditing = false;
        Mensagem  = string.Empty;
    }

    // Trajeto
    private async Task ConfirmarTrajetoAsync(object? _)
    {
        if (SelectedEvento == null) return;
        try
        {
            var trajeto = new EventoTrajeto
            {
                EventoId    = SelectedEvento.Id,
                Nome        = NovoTrajetoNome,
                Distancia   = NovoTrajetoDistancia,
                HoraLargada = NovoTrajetoHoraLargada
            };
            await _eventoService.AddTrajetoAsync(trajeto);
            IsAddingTrajeto = false;
            LimparFormTrajeto();
            Mensagem = "Trajeto adicionado.";
            await RecarregarTrajetosAsync();
        }
        catch (Exception ex) { Mensagem = $"Erro: {ex.Message}"; }
    }

    private async Task RemoveTrajetoAsync(object? _)
    {
        if (SelectedTrajeto == null) return;
        try
        {
            await _eventoService.RemoveTrajetoAsync(SelectedTrajeto.Id);
            SelectedTrajeto = null;
            Mensagem = "Trajeto removido.";
            await RecarregarTrajetosAsync();
        }
        catch (Exception ex) { Mensagem = $"Erro: {ex.Message}"; }
    }

    // Categoria
    private async Task ConfirmarCategoriaAsync(object? _)
    {
        if (SelectedTrajeto == null || SelectedEvento == null) return;

        var selecionadas = CategoriasSelecionaveis.Where(c => c.Selecionado).ToList();
        if (!selecionadas.Any()) return;

        try
        {
            foreach (var item in selecionadas)
            {
                var ec = new EventoCategoria
                {
                    EventoId        = SelectedEvento.Id,
                    EventoTrajetoId = SelectedTrajeto.Id,
                    CategoriaId     = item.Categoria.Id,
                    NroVoltas       = PossuiVoltas ? item.NroVoltas : null
                };
                await _eventoService.AddCategoriaAsync(ec);
            }

            IsAddingCategoria = false;
            LimparFormCategoria();
            Mensagem = selecionadas.Count == 1
                ? "Categoria adicionada."
                : $"{selecionadas.Count} categorias adicionadas.";
            await RecarregarTrajetosAsync(SelectedTrajeto.Id);
        }
        catch (Exception ex) { Mensagem = $"Erro: {ex.Message}"; }
    }

    private async Task RemoveCategoriaAsync(object? _)
    {
        if (SelectedCategoriaDoTrajeto == null) return;
        try
        {
            var previousTrajetoId = SelectedTrajeto?.Id;
            await _eventoService.RemoveCategoriaAsync(SelectedCategoriaDoTrajeto.Id);
            SelectedCategoriaDoTrajeto = null;
            Mensagem = "Categoria removida.";
            await RecarregarTrajetosAsync(previousTrajetoId);
        }
        catch (Exception ex) { Mensagem = $"Erro: {ex.Message}"; }
    }

    // Helpers
    private async Task RecarregarTrajetosAsync(int? reSelectTrajetoId = null)
    {
        if (SelectedEvento == null) return;
        var eventoCompleto = await _eventoRepository.GetWithTrajetosECategoriasAsync(SelectedEvento.Id);
        if (eventoCompleto == null) return;

        Trajetos = new ObservableCollection<EventoTrajeto>(eventoCompleto.Trajetos);

        if (reSelectTrajetoId.HasValue)
        {
            var trajeto = Trajetos.FirstOrDefault(t => t.Id == reSelectTrajetoId.Value);
            _selectedTrajeto = trajeto;
            OnPropertyChanged(nameof(SelectedTrajeto));
            OnPropertyChanged(nameof(HasSelectedTrajeto));
            CarregarCategoriasDoTrajeto(trajeto);
        }
        else
        {
            SelectedTrajeto = null;
        }
    }

    private void CarregarCategoriasDoTrajeto(EventoTrajeto? trajeto)
    {
        if (trajeto == null)
        {
            CategoriasDoTrajeto.Clear();
            return;
        }
        CategoriasDoTrajeto = new ObservableCollection<EventoCategoria>(trajeto.EventoCategorias);
    }

    private void LimparFormTrajeto()
    {
        NovoTrajetoNome        = string.Empty;
        NovoTrajetoDistancia   = 0;
        NovoTrajetoHoraLargada = null;
    }

    private void PopularCategoriasSelecionaveis()
    {
        var jaAdicionadas = CategoriasDoTrajeto.Select(ec => ec.CategoriaId).ToHashSet();
        CategoriasSelecionaveis = new ObservableCollection<CategoriaSelecionavel>(
            TodasCategorias
                .Where(c => !jaAdicionadas.Contains(c.Id))
                .Select(c => new CategoriaSelecionavel(c))
        );
    }

    private void LimparFormCategoria()
    {
        CategoriasSelecionaveis.Clear();
    }
}
