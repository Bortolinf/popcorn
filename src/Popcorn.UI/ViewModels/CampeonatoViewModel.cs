using Popcorn.Application.Interfaces.Repositories;
using Popcorn.Domain.Entities;
using Popcorn.UI.Commands;
using Popcorn.UI.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Popcorn.UI.ViewModels;

public class CampeonatoViewModel : ViewModelBase, ILoadable
{
    private readonly ICampeonatoRepository _campeonatoRepository;

    private ObservableCollection<Campeonato> _campeonatos = new();
    private Campeonato? _selectedCampeonato;
    private string _nome = string.Empty;
    private DateTime _dataInicio = DateTime.Today;
    private DateTime _dataFim = DateTime.Today.AddMonths(3);
    private string? _descricao;
    private bool _isEditing;
    private string _mensagem = string.Empty;

    public ObservableCollection<Campeonato> Campeonatos { get => _campeonatos; set => SetProperty(ref _campeonatos, value); }
    public Campeonato? SelectedCampeonato
    {
        get => _selectedCampeonato;
        set { SetProperty(ref _selectedCampeonato, value); if (value != null) PreencherFormulario(value); }
    }

    public string Nome          { get => _nome;        set => SetProperty(ref _nome, value); }
    public DateTime DataInicio  { get => _dataInicio;  set => SetProperty(ref _dataInicio, value); }
    public DateTime DataFim     { get => _dataFim;     set => SetProperty(ref _dataFim, value); }
    public string? Descricao    { get => _descricao;   set => SetProperty(ref _descricao, value); }
    public bool IsEditing       { get => _isEditing;   set => SetProperty(ref _isEditing, value); }
    public string Mensagem      { get => _mensagem;    set => SetProperty(ref _mensagem, value); }

    public ICommand NovoCommand    { get; }
    public ICommand SalvarCommand  { get; }
    public ICommand ExcluirCommand { get; }
    public ICommand CancelarCommand { get; }

    public CampeonatoViewModel(ICampeonatoRepository campeonatoRepository)
    {
        _campeonatoRepository = campeonatoRepository;
        NovoCommand     = new RelayCommand(_ => Novo());
        SalvarCommand   = new AsyncRelayCommand(SalvarAsync);
        ExcluirCommand  = new AsyncRelayCommand(ExcluirAsync, _ => SelectedCampeonato != null);
        CancelarCommand = new RelayCommand(_ => Cancelar());
    }

    public async Task LoadAsync()
    {
        var lista = await _campeonatoRepository.GetAllAsync();
        Campeonatos = new ObservableCollection<Campeonato>(lista);
    }

    private void Novo()
    {
        SelectedCampeonato = null;
        Nome = string.Empty; DataInicio = DateTime.Today;
        DataFim = DateTime.Today.AddMonths(3); Descricao = null;
        IsEditing = true; Mensagem = string.Empty;
    }

    private async Task SalvarAsync(object? _)
    {
        try
        {
            if (SelectedCampeonato == null)
            {
                var novo = new Campeonato
                {
                    Nome = Nome, DataInicio = DataInicio,
                    DataFim = DataFim, Descricao = Descricao
                };
                await _campeonatoRepository.AddAsync(novo);
                await _campeonatoRepository.SaveChangesAsync();
            }
            else
            {
                SelectedCampeonato.Nome = Nome; SelectedCampeonato.DataInicio = DataInicio;
                SelectedCampeonato.DataFim = DataFim; SelectedCampeonato.Descricao = Descricao;
                await _campeonatoRepository.UpdateAsync(SelectedCampeonato);
                await _campeonatoRepository.SaveChangesAsync();
            }

            Mensagem = "Salvo com sucesso."; IsEditing = false;
            await LoadAsync();
        }
        catch (Exception ex) { Mensagem = $"Erro: {ex.Message}"; }
    }

    private async Task ExcluirAsync(object? _)
    {
        if (SelectedCampeonato == null) return;
        try
        {
            await _campeonatoRepository.DeleteAsync(SelectedCampeonato);
            await _campeonatoRepository.SaveChangesAsync();
            Mensagem = "Excluído com sucesso."; IsEditing = false;
            await LoadAsync();
        }
        catch (Exception ex) { Mensagem = $"Erro: {ex.Message}"; }
    }

    private void Cancelar() { IsEditing = false; Mensagem = string.Empty; }

    private void PreencherFormulario(Campeonato c)
    {
        Nome = c.Nome; DataInicio = c.DataInicio; DataFim = c.DataFim; Descricao = c.Descricao;
        IsEditing = true;
    }
}
