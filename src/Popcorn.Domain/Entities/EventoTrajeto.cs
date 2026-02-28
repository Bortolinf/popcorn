namespace Popcorn.Domain.Entities;

public class EventoTrajeto : BaseEntity
{
    public int EventoId { get; set; }
    public Evento Evento { get; set; } = null!;
    public string Nome { get; set; } = string.Empty;
    public decimal Distancia { get; set; }
    public int QuantVoltas { get; set; }
    public DateTime? HoraLargada { get; set; }

    public ICollection<EventoCategoria> EventoCategorias { get; set; } = new List<EventoCategoria>();
}
