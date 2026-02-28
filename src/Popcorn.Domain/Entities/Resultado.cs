using Popcorn.Domain.Enums;

namespace Popcorn.Domain.Entities;

public class Resultado : BaseEntity
{
    public int AtletaId { get; set; }
    public Atleta Atleta { get; set; } = null!;
    public int EventoId { get; set; }
    public Evento Evento { get; set; } = null!;
    public TimeSpan? TempoBruto { get; set; }
    public TimeSpan? TempoLiquido { get; set; }
    public TimeSpan? TempoTotal { get; set; }
    public int? PosicaoGeral { get; set; }
    public int? PosicaoCategoria { get; set; }
    public StatusResultado Status { get; set; }
}
