namespace Popcorn.Domain.Entities;

public class Chegada : BaseEntity
{
    public int AtletaId { get; set; }
    public Atleta Atleta { get; set; } = null!;
    public int EventoId { get; set; }
    public Evento Evento { get; set; } = null!;
    public int Volta { get; set; }
    public DateTime HoraChegada { get; set; }
    public bool FlagDNS { get; set; }
    public bool FlagDNF { get; set; }
    public bool FlagDSQ { get; set; }
}
