namespace Popcorn.Domain.Entities;

public class Modalidade : BaseEntity
{
    public string Descricao { get; set; } = string.Empty;
    public bool PossuiVoltas { get; set; }
    public bool LargadaBaterias { get; set; }
    public ICollection<Evento> Eventos { get; set; } = new List<Evento>();
}
