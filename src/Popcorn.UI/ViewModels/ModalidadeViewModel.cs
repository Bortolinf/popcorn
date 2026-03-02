using Popcorn.Application.Interfaces.Repositories;
using Popcorn.Domain.Entities;
using Popcorn.Domain.Enums;
using Popcorn.UI.Commands;
using Popcorn.UI.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Popcorn.UI.ViewModels;

public class ModalidadeViewModel : ViewModelBase, ILoadable
{
    private readonly IModalidadeRepository _modalidadeRepository;

    private ObservableCollection<Modalidade> _modalidades = new();
    private Modalidade? _selectedModalidade;
    private string _descricao = string.Empty;
    private bool _possuiVoltas;
    private bool _largadaBaterias;
    private DefinicaoVoltas _definicaoVoltas = DefinicaoVoltas.NaCategoria;
    private bool _isEditing;
    private string _mensagem = string.Empty;

    public ObservableCollection<Modalidade> Modalidades { get => _modalidades; set => SetProperty(ref _modalidades, value); }
    public Modalidade? SelectedModalidade
    {
        get => _selectedModalidade;
        set { SetProperty(ref _selectedModalidade, value); if (value != null) PreencherFormulario(value); }
    }

    public string Descricao     { get => _descricao;       set => SetProperty(ref _descricao, value); }
    public bool LargadaBaterias { get => _largadaBaterias; set => SetProperty(ref _largadaBaterias, value); }
    public bool IsEditing       { get => _isEditing;       set => SetProperty(ref _isEditing, value); }
    public string Mensagem      { get => _mensagem;        set => SetProperty(ref _mensagem, value); }

    public bool PossuiVoltas
    {
        get => _possuiVoltas;
        set { SetProperty(ref _possuiVoltas, value); OnPropertyChanged(nameof(DefinicaoVoltasHabilitado)); }
    }

    public bool DefinicaoVoltasHabilitado => _possuiVoltas;

    public bool DefinicaoVoltasNaCategoria
    {
        get => _definicaoVoltas == DefinicaoVoltas.NaCategoria;
        set
        {
            if (value) { _definicaoVoltas = DefinicaoVoltas.NaCategoria; OnPropertyChanged(); OnPropertyChanged(nameof(DefinicaoVoltasNoTrajeto)); }
        }
    }

    public bool DefinicaoVoltasNoTrajeto
    {
        get => _definicaoVoltas == DefinicaoVoltas.NoTrajeto;
        set
        {
            if (value) { _definicaoVoltas = DefinicaoVoltas.NoTrajeto; OnPropertyChanged(); OnPropertyChanged(nameof(DefinicaoVoltasNaCategoria)); }
        }
    }

    public ICommand NovoCommand     { get; }
    public ICommand SalvarCommand   { get; }
    public ICommand ExcluirCommand  { get; }
    public ICommand CancelarCommand { get; }

    public ModalidadeViewModel(IModalidadeRepository modalidadeRepository)
    {
        _modalidadeRepository = modalidadeRepository;
        NovoCommand     = new RelayCommand(_ => Novo());
        SalvarCommand   = new AsyncRelayCommand(SalvarAsync);
        ExcluirCommand  = new AsyncRelayCommand(ExcluirAsync, _ => SelectedModalidade != null);
        CancelarCommand = new RelayCommand(_ => Cancelar());
    }

    public async Task LoadAsync()
    {
        var lista = await _modalidadeRepository.GetAllAsync();
        Modalidades = new ObservableCollection<Modalidade>(lista);
    }

    private void Novo()
    {
        SelectedModalidade = null;
        Descricao = string.Empty;
        PossuiVoltas = false;
        LargadaBaterias = false;
        _definicaoVoltas = DefinicaoVoltas.NaCategoria;
        OnPropertyChanged(nameof(DefinicaoVoltasNaCategoria));
        OnPropertyChanged(nameof(DefinicaoVoltasNoTrajeto));
        IsEditing = true;
        Mensagem = string.Empty;
    }

    private async Task SalvarAsync(object? _)
    {
        try
        {
            if (SelectedModalidade == null)
            {
                var nova = new Modalidade
                {
                    Descricao       = Descricao,
                    PossuiVoltas    = PossuiVoltas,
                    LargadaBaterias = LargadaBaterias,
                    DefinicaoVoltas = _definicaoVoltas
                };
                await _modalidadeRepository.AddAsync(nova);
                await _modalidadeRepository.SaveChangesAsync();
            }
            else
            {
                SelectedModalidade.Descricao       = Descricao;
                SelectedModalidade.PossuiVoltas    = PossuiVoltas;
                SelectedModalidade.LargadaBaterias = LargadaBaterias;
                SelectedModalidade.DefinicaoVoltas = _definicaoVoltas;
                await _modalidadeRepository.UpdateAsync(SelectedModalidade);
                await _modalidadeRepository.SaveChangesAsync();
            }

            Mensagem  = "Salvo com sucesso.";
            IsEditing = false;
            await LoadAsync();
        }
        catch (Exception ex) { Mensagem = $"Erro: {ex.Message}"; }
    }

    private async Task ExcluirAsync(object? _)
    {
        if (SelectedModalidade == null) return;
        try
        {
            await _modalidadeRepository.DeleteAsync(SelectedModalidade);
            await _modalidadeRepository.SaveChangesAsync();
            Mensagem  = "Excluído com sucesso.";
            IsEditing = false;
            await LoadAsync();
        }
        catch (Exception ex) { Mensagem = $"Erro: {ex.Message}"; }
    }

    private void Cancelar() { IsEditing = false; Mensagem = string.Empty; }

    private void PreencherFormulario(Modalidade m)
    {
        Descricao       = m.Descricao;
        PossuiVoltas    = m.PossuiVoltas;
        LargadaBaterias = m.LargadaBaterias;
        _definicaoVoltas = m.DefinicaoVoltas;
        OnPropertyChanged(nameof(DefinicaoVoltasNaCategoria));
        OnPropertyChanged(nameof(DefinicaoVoltasNoTrajeto));
        OnPropertyChanged(nameof(DefinicaoVoltasHabilitado));
        IsEditing = true;
    }
}
