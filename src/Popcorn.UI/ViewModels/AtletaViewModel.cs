using Popcorn.Application.Interfaces.Repositories;
using Popcorn.Domain.Entities;
using Popcorn.Domain.Enums;
using Popcorn.UI.Commands;
using Popcorn.UI.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Popcorn.UI.ViewModels;

public class AtletaViewModel : ViewModelBase, ILoadable
{
    private readonly IAtletaRepository _atletaRepository;

    private ObservableCollection<Atleta> _atletas = new();
    private Atleta? _selectedAtleta;
    private string _busca = string.Empty;
    private string _nome = string.Empty;
    private string _numeroDocumento = string.Empty;
    private DateTime _dataNascimento = DateTime.Today.AddYears(-20);
    private Sexo _sexo = Sexo.Masculino;
    private string? _equipe;
    private string? _cidade;
    private string? _estado;
    private bool _isEditing;
    private string _mensagem = string.Empty;

    public ObservableCollection<Atleta> Atletas { get => _atletas; set => SetProperty(ref _atletas, value); }
    public Atleta? SelectedAtleta
    {
        get => _selectedAtleta;
        set { SetProperty(ref _selectedAtleta, value); if (value != null) PreencherFormulario(value); }
    }

    public string Busca              { get => _busca;           set => SetProperty(ref _busca, value); }
    public string Nome               { get => _nome;            set => SetProperty(ref _nome, value); }
    public string NumeroDocumento    { get => _numeroDocumento; set => SetProperty(ref _numeroDocumento, value); }
    public DateTime DataNascimento   { get => _dataNascimento;  set => SetProperty(ref _dataNascimento, value); }
    public Sexo Sexo                 { get => _sexo;            set => SetProperty(ref _sexo, value); }
    public string? Equipe            { get => _equipe;          set => SetProperty(ref _equipe, value); }
    public string? Cidade            { get => _cidade;          set => SetProperty(ref _cidade, value); }
    public string? Estado            { get => _estado;          set => SetProperty(ref _estado, value); }
    public bool IsEditing            { get => _isEditing;       set => SetProperty(ref _isEditing, value); }
    public string Mensagem           { get => _mensagem;        set => SetProperty(ref _mensagem, value); }

    public IEnumerable<Sexo> SexoValues => Enum.GetValues<Sexo>().Where(s => s != Sexo.Misto);

    public ICommand NovoCommand    { get; }
    public ICommand SalvarCommand  { get; }
    public ICommand ExcluirCommand { get; }
    public ICommand CancelarCommand { get; }
    public ICommand BuscarCommand  { get; }

    public AtletaViewModel(IAtletaRepository atletaRepository)
    {
        _atletaRepository = atletaRepository;
        NovoCommand     = new RelayCommand(_ => Novo());
        SalvarCommand   = new AsyncRelayCommand(SalvarAsync);
        ExcluirCommand  = new AsyncRelayCommand(ExcluirAsync, _ => SelectedAtleta != null);
        CancelarCommand = new RelayCommand(_ => Cancelar());
        BuscarCommand   = new AsyncRelayCommand(BuscarAsync);
    }

    public async Task LoadAsync()
    {
        var lista = await _atletaRepository.GetAllAsync();
        Atletas = new ObservableCollection<Atleta>(lista);
    }

    private async Task BuscarAsync(object? _)
    {
        if (string.IsNullOrWhiteSpace(Busca))
        {
            await LoadAsync();
            return;
        }
        var lista = await _atletaRepository.SearchByNomeAsync(Busca);
        Atletas = new ObservableCollection<Atleta>(lista);
    }

    private void Novo()
    {
        SelectedAtleta = null;
        Nome = string.Empty; NumeroDocumento = string.Empty;
        DataNascimento = DateTime.Today.AddYears(-20);
        Sexo = Sexo.Masculino; Equipe = null; Cidade = null; Estado = null;
        IsEditing = true; Mensagem = string.Empty;
    }

    private async Task SalvarAsync(object? _)
    {
        try
        {
            if (SelectedAtleta == null)
            {
                var novo = new Atleta
                {
                    Nome = Nome, NumeroDocumento = NumeroDocumento,
                    DataNascimento = DataNascimento, Sexo = Sexo,
                    Equipe = Equipe, Cidade = Cidade, Estado = Estado
                };
                await _atletaRepository.AddAsync(novo);
                await _atletaRepository.SaveChangesAsync();
            }
            else
            {
                SelectedAtleta.Nome = Nome; SelectedAtleta.NumeroDocumento = NumeroDocumento;
                SelectedAtleta.DataNascimento = DataNascimento; SelectedAtleta.Sexo = Sexo;
                SelectedAtleta.Equipe = Equipe; SelectedAtleta.Cidade = Cidade; SelectedAtleta.Estado = Estado;
                await _atletaRepository.UpdateAsync(SelectedAtleta);
                await _atletaRepository.SaveChangesAsync();
            }

            Mensagem = "Salvo com sucesso.";
            IsEditing = false;
            await LoadAsync();
        }
        catch (Exception ex) { Mensagem = $"Erro: {ex.Message}"; }
    }

    private async Task ExcluirAsync(object? _)
    {
        if (SelectedAtleta == null) return;
        try
        {
            await _atletaRepository.DeleteAsync(SelectedAtleta);
            await _atletaRepository.SaveChangesAsync();
            Mensagem = "Excluído com sucesso.";
            IsEditing = false;
            await LoadAsync();
        }
        catch (Exception ex) { Mensagem = $"Erro: {ex.Message}"; }
    }

    private void Cancelar() { IsEditing = false; Mensagem = string.Empty; }

    private void PreencherFormulario(Atleta a)
    {
        Nome = a.Nome; NumeroDocumento = a.NumeroDocumento;
        DataNascimento = a.DataNascimento; Sexo = a.Sexo;
        Equipe = a.Equipe; Cidade = a.Cidade; Estado = a.Estado;
        IsEditing = true;
    }
}
