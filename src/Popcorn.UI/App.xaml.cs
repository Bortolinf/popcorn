using Microsoft.Extensions.DependencyInjection;
using Popcorn.Application.Interfaces.Repositories;
using Popcorn.Infrastructure.DependencyInjection;
using Popcorn.UI.ViewModels;
using Popcorn.UI.Views;
using System.Windows;

namespace Popcorn.UI;

public partial class App : System.Windows.Application
{
    public static IServiceProvider Services { get; private set; } = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();
        ConfigureServices(services);
        Services = services.BuildServiceProvider();

        var loginView = Services.GetRequiredService<LoginView>();
        loginView.Show();
    }

    private static void ConfigureServices(ServiceCollection services)
    {
        const string connectionString =
            "Server=127.0.0.1;Database=popcorn;User=root;Password=;SslMode=None;";

        services.AddInfrastructure(connectionString);

        // ViewModels
        services.AddTransient<LoginViewModel>();
        services.AddTransient<MainViewModel>();
        services.AddTransient<DashboardViewModel>();
        services.AddTransient<EventoViewModel>();
        services.AddTransient<AtletaViewModel>();
        services.AddTransient<CategoriaViewModel>();
        services.AddTransient<CampeonatoViewModel>();
        services.AddTransient<CronometragemViewModel>();
        services.AddTransient<ResultadoViewModel>();
        services.AddTransient<ModalidadeViewModel>();

        // Windows
        services.AddTransient<ModalidadeView>();
        services.AddTransient<LoginView>();
        services.AddTransient<MainWindow>();
    }
}
