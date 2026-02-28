using Microsoft.EntityFrameworkCore;
using Popcorn.Domain.Entities;

namespace Popcorn.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Modalidade> Modalidades => Set<Modalidade>();
    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Atleta> Atletas => Set<Atleta>();
    public DbSet<Evento> Eventos => Set<Evento>();
    public DbSet<EventoTrajeto> EventoTrajetos => Set<EventoTrajeto>();
    public DbSet<EventoCategoria> EventoCategorias => Set<EventoCategoria>();
    public DbSet<Campeonato> Campeonatos => Set<Campeonato>();
    public DbSet<CampeonatoEvento> CampeonatoEventos => Set<CampeonatoEvento>();
    public DbSet<InscricaoAtleta> InscricoesAtletas => Set<InscricaoAtleta>();
    public DbSet<Chegada> Chegadas => Set<Chegada>();
    public DbSet<Resultado> Resultados => Set<Resultado>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<EventoTrajeto>()
            .Property(e => e.Distancia)
            .HasPrecision(10, 3);

        modelBuilder.Entity<Resultado>()
            .Property(r => r.TempoBruto)
            .HasColumnType("time(6)");

        modelBuilder.Entity<Resultado>()
            .Property(r => r.TempoLiquido)
            .HasColumnType("time(6)");

        modelBuilder.Entity<Resultado>()
            .Property(r => r.TempoTotal)
            .HasColumnType("time(6)");
    }
}
