using Popcorn.Application.Interfaces.Repositories;
using Popcorn.Domain.Enums;
using Popcorn.UI.ViewModels.Base;

namespace Popcorn.UI.ViewModels;

public class DashboardViewModel : ViewModelBase, ILoadable
{
    private readonly IEventoRepository _eventoRepository;
    private readonly IAtletaRepository _atletaRepository;

    private int _totalEventos;
    private int _eventosPendentes;
    private int _eventosEmAndamento;
    private int _eventosEncerrados;
    private int _totalAtletas;

    public int TotalEventos       { get => _totalEventos;       set => SetProperty(ref _totalEventos, value); }
    public int EventosPendentes   { get => _eventosPendentes;   set => SetProperty(ref _eventosPendentes, value); }
    public int EventosEmAndamento { get => _eventosEmAndamento; set => SetProperty(ref _eventosEmAndamento, value); }
    public int EventosEncerrados  { get => _eventosEncerrados;  set => SetProperty(ref _eventosEncerrados, value); }
    public int TotalAtletas       { get => _totalAtletas;       set => SetProperty(ref _totalAtletas, value); }

    public DashboardViewModel(IEventoRepository eventoRepository, IAtletaRepository atletaRepository)
    {
        _eventoRepository = eventoRepository;
        _atletaRepository = atletaRepository;
    }

    public async Task LoadAsync()
    {
        var eventos = (await _eventoRepository.GetAllAsync()).ToList();
        var atletas = (await _atletaRepository.GetAllAsync()).ToList();

        TotalEventos       = eventos.Count;
        EventosPendentes   = eventos.Count(e => e.Status == StatusEvento.Pendente);
        EventosEmAndamento = eventos.Count(e => e.Status == StatusEvento.EmAndamento);
        EventosEncerrados  = eventos.Count(e => e.Status == StatusEvento.Encerrado);
        TotalAtletas       = atletas.Count;
    }
}
