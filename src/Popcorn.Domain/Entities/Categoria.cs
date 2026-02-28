using Popcorn.Domain.Enums;

namespace Popcorn.Domain.Entities;

public class Categoria : BaseEntity
{
    public string Descricao { get; set; } = string.Empty;
    public Sexo Sexo { get; set; }
    public int? IdadeMin { get; set; }
    public int? IdadeMax { get; set; }
    public bool NaoClassificaGeral { get; set; }
    public TipoCategoria Tipo { get; set; }

    public ICollection<EventoCategoria> EventoCategorias { get; set; } = new List<EventoCategoria>();
}
