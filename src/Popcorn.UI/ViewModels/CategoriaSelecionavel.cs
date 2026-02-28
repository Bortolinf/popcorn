using Popcorn.Domain.Entities;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Popcorn.UI.ViewModels;

public class CategoriaSelecionavel : INotifyPropertyChanged
{
    private bool _selecionado;
    private int? _nroVoltas;

    public Categoria Categoria { get; }

    public bool Selecionado
    {
        get => _selecionado;
        set { _selecionado = value; OnPropertyChanged(); }
    }

    public int? NroVoltas
    {
        get => _nroVoltas;
        set { _nroVoltas = value; OnPropertyChanged(); }
    }

    public CategoriaSelecionavel(Categoria categoria)
    {
        Categoria = categoria;
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
