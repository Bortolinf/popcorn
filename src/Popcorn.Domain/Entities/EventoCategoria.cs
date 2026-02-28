namespace Popcorn.Domain.Entities;

public class EventoCategoria : BaseEntity
{
    public int EventoId { get; set; }
    public Evento Evento { get; set; } = null!;
    public int EventoTrajetoId { get; set; }
    public EventoTrajeto EventoTrajeto { get; set; } = null!;
    public int CategoriaId { get; set; }
    public Categoria Categoria { get; set; } = null!;
    public int? NroVoltas { get; set; }
    public DateTime? HoraLargada { get; set; }

    public ICollection<InscricaoAtleta> Inscricoes { get; set; } = new List<InscricaoAtleta>();
}
