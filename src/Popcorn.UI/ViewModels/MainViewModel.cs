using Microsoft.Extensions.DependencyInjection;
using Popcorn.UI.Commands;
using Popcorn.UI.ViewModels.Base;
using System.Windows.Input;

namespace Popcorn.UI.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private ViewModelBase? _currentViewModel;
    private string _currentPageTitle = "Dashboard";

    public ViewModelBase? CurrentViewModel
    {
        get => _currentViewModel;
        set => SetProperty(ref _currentViewModel, value);
    }

    public string CurrentPageTitle
    {
        get => _currentPageTitle;
        set => SetProperty(ref _currentPageTitle, value);
    }

    public ICommand NavigateDashboardCommand { get; }
    public ICommand NavigateEventosCommand { get; }
    public ICommand NavigateAtletasCommand { get; }
    public ICommand NavigateCategoriasCommand { get; }
    public ICommand NavigateCampeonatosCommand { get; }
    public ICommand NavigateModalidadesCommand { get; }
    public ICommand NavigateCronometragemCommand { get; }
    public ICommand NavigateResultadosCommand { get; }

    public MainViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        NavigateDashboardCommand    = new RelayCommand(_ => Navigate<DashboardViewModel>("Dashboard"));
        NavigateEventosCommand      = new RelayCommand(_ => Navigate<EventoViewModel>("Gestão de Eventos"));
        NavigateAtletasCommand      = new RelayCommand(_ => Navigate<AtletaViewModel>("Gestão de Atletas"));
        NavigateCategoriasCommand   = new RelayCommand(_ => Navigate<CategoriaViewModel>("Gestão de Categorias"));
        NavigateCampeonatosCommand  = new RelayCommand(_ => Navigate<CampeonatoViewModel>("Gestão de Campeonatos"));
        NavigateModalidadesCommand   = new RelayCommand(_ => Navigate<ModalidadeViewModel>("Modalidades"));
        NavigateCronometragemCommand = new RelayCommand(_ => Navigate<CronometragemViewModel>("Cronometragem"));
        NavigateResultadosCommand   = new RelayCommand(_ => Navigate<ResultadoViewModel>("Resultados"));

        Navigate<DashboardViewModel>("Dashboard");
    }

    private void Navigate<TViewModel>(string title) where TViewModel : ViewModelBase
    {
        CurrentPageTitle = title;
        CurrentViewModel = _serviceProvider.GetRequiredService<TViewModel>();

        if (CurrentViewModel is ILoadable loadable)
            _ = loadable.LoadAsync();
    }
}

public interface ILoadable
{
    Task LoadAsync();
}
