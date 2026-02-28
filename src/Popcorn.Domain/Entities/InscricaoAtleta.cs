namespace Popcorn.Domain.Entities;

public class InscricaoAtleta : BaseEntity
{
    public int AtletaId { get; set; }
    public Atleta Atleta { get; set; } = null!;
    public int EventoId { get; set; }
    public Evento Evento { get; set; } = null!;
    public int EventoCategoriaId { get; set; }
    public EventoCategoria EventoCategoria { get; set; } = null!;
    public int Numero { get; set; }
    public string? TagRfid { get; set; }
    public string? Camisa { get; set; }
    public bool RetirouKit { get; set; }
    public DateTime? HoraLargada { get; set; }
    public string? Observacao { get; set; }
    public bool Organizador { get; set; }
}
