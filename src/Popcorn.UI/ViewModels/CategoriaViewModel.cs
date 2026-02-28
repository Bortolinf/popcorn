using Popcorn.Application.Interfaces.Repositories;
using Popcorn.Domain.Entities;
using Popcorn.Domain.Enums;
using Popcorn.UI.Commands;
using Popcorn.UI.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Popcorn.UI.ViewModels;

public class CategoriaViewModel : ViewModelBase, ILoadable
{
    private readonly ICategoriaRepository _categoriaRepository;

    private ObservableCollection<Categoria> _categorias = new();
    private Categoria? _selectedCategoria;
    private string _descricao = string.Empty;
    private Sexo _sexo = Sexo.Masculino;
    private int? _idadeMin;
    private int? _idadeMax;
    private bool _naoClassificaGeral;
    private TipoCategoria _tipo = TipoCategoria.Individual;
    private bool _isEditing;
    private string _mensagem = string.Empty;

    public ObservableCollection<Categoria> Categorias { get => _categorias; set => SetProperty(ref _categorias, value); }
    public Categoria? SelectedCategoria
    {
        get => _selectedCategoria;
        set { SetProperty(ref _selectedCategoria, value); if (value != null) PreencherFormulario(value); }
    }

    public string Descricao         { get => _descricao;         set => SetProperty(ref _descricao, value); }
    public Sexo Sexo                { get => _sexo;              set => SetProperty(ref _sexo, value); }
    public int? IdadeMin            { get => _idadeMin;          set => SetProperty(ref _idadeMin, value); }
    public int? IdadeMax            { get => _idadeMax;          set => SetProperty(ref _idadeMax, value); }
    public bool NaoClassificaGeral  { get => _naoClassificaGeral; set => SetProperty(ref _naoClassificaGeral, value); }
    public TipoCategoria Tipo       { get => _tipo;              set => SetProperty(ref _tipo, value); }
    public bool IsEditing           { get => _isEditing;         set => SetProperty(ref _isEditing, value); }
    public string Mensagem          { get => _mensagem;          set => SetProperty(ref _mensagem, value); }

    public IEnumerable<Sexo> SexoValues => Enum.GetValues<Sexo>();
    public IEnumerable<TipoCategoria> TipoValues => Enum.GetValues<TipoCategoria>();

    public ICommand NovoCommand    { get; }
    public ICommand SalvarCommand  { get; }
    public ICommand ExcluirCommand { get; }
    public ICommand CancelarCommand { get; }

    public CategoriaViewModel(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
        NovoCommand     = new RelayCommand(_ => Novo());
        SalvarCommand   = new AsyncRelayCommand(SalvarAsync);
        ExcluirCommand  = new AsyncRelayCommand(ExcluirAsync, _ => SelectedCategoria != null);
        CancelarCommand = new RelayCommand(_ => Cancelar());
    }

    public async Task LoadAsync()
    {
        var lista = await _categoriaRepository.GetAllAsync();
        Categorias = new ObservableCollection<Categoria>(lista);
    }

    private void Novo()
    {
        SelectedCategoria = null;
        Descricao = string.Empty; Sexo = Sexo.Masculino;
        IdadeMin = null; IdadeMax = null; NaoClassificaGeral = false;
        Tipo = TipoCategoria.Individual;
        IsEditing = true; Mensagem = string.Empty;
    }

    private async Task SalvarAsync(object? _)
    {
        try
        {
            if (SelectedCategoria == null)
            {
                var nova = new Categoria
                {
                    Descricao = Descricao, Sexo = Sexo, IdadeMin = IdadeMin,
                    IdadeMax = IdadeMax, NaoClassificaGeral = NaoClassificaGeral, Tipo = Tipo
                };
                await _categoriaRepository.AddAsync(nova);
                await _categoriaRepository.SaveChangesAsync();
            }
            else
            {
                SelectedCategoria.Descricao = Descricao; SelectedCategoria.Sexo = Sexo;
                SelectedCategoria.IdadeMin = IdadeMin; SelectedCategoria.IdadeMax = IdadeMax;
                SelectedCategoria.NaoClassificaGeral = NaoClassificaGeral; SelectedCategoria.Tipo = Tipo;
                await _categoriaRepository.UpdateAsync(SelectedCategoria);
                await _categoriaRepository.SaveChangesAsync();
            }

            Mensagem = "Salvo com sucesso.";
            IsEditing = false;
            await LoadAsync();
        }
        catch (Exception ex) { Mensagem = $"Erro: {ex.Message}"; }
    }

    private async Task ExcluirAsync(object? _)
    {
        if (SelectedCategoria == null) return;
        try
        {
            await _categoriaRepository.DeleteAsync(SelectedCategoria);
            await _categoriaRepository.SaveChangesAsync();
            Mensagem = "Excluído com sucesso."; IsEditing = false;
            await LoadAsync();
        }
        catch (Exception ex) { Mensagem = $"Erro: {ex.Message}"; }
    }

    private void Cancelar() { IsEditing = false; Mensagem = string.Empty; }

    private void PreencherFormulario(Categoria c)
    {
        Descricao = c.Descricao; Sexo = c.Sexo; IdadeMin = c.IdadeMin;
        IdadeMax = c.IdadeMax; NaoClassificaGeral = c.NaoClassificaGeral; Tipo = c.Tipo;
        IsEditing = true;
    }
}
