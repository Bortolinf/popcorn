using Popcorn.UI.ViewModels;
using System.Windows;

namespace Popcorn.UI;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
