using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Popcorn.Application.Interfaces.Repositories;
using Popcorn.Application.Interfaces.Services;
using Popcorn.Application.Services;
using Popcorn.Infrastructure.Data;
using Popcorn.Infrastructure.Repositories;

namespace Popcorn.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseMySql(
                connectionString,
                new MariaDbServerVersion(new Version(10, 4, 28))
            ));

        // Repositories
        services.AddScoped<IModalidadeRepository, ModalidadeRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<ICategoriaRepository, CategoriaRepository>();
        services.AddScoped<IAtletaRepository, AtletaRepository>();
        services.AddScoped<IEventoRepository, EventoRepository>();
        services.AddScoped<IEventoTrajetoRepository, EventoTrajetoRepository>();
        services.AddScoped<IEventoCategoriaRepository, EventoCategoriaRepository>();
        services.AddScoped<ICampeonatoRepository, CampeonatoRepository>();
        services.AddScoped<ICampeonatoEventoRepository, CampeonatoEventoRepository>();
        services.AddScoped<IInscricaoAtletaRepository, InscricaoAtletaRepository>();
        services.AddScoped<IChegadaRepository, ChegadaRepository>();
        services.AddScoped<IResultadoRepository, ResultadoRepository>();

        // Services
        services.AddScoped<IEventoService, EventoService>();
        services.AddScoped<ICronometragemService, CronometragemService>();
        services.AddScoped<IResultadoService, ResultadoService>();

        return services;
    }
}
