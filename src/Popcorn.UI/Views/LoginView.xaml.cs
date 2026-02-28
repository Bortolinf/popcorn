using Microsoft.Extensions.DependencyInjection;
using Popcorn.UI.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace Popcorn.UI.Views;

public partial class LoginView : Window
{
    private readonly LoginViewModel _viewModel;

    public LoginView(LoginViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;

        _viewModel.LoginSucesso += () =>
        {
            var mainWindow = App.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
            Close();
        };
    }

    private void SenhaBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (sender is PasswordBox pb)
            _viewModel.Senha = pb.Password;
    }
}
