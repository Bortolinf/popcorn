using Popcorn.Domain.Enums;

namespace Popcorn.Domain.Entities;

public class Modalidade : BaseEntity
{
    public string Descricao { get; set; } = string.Empty;
    public bool PossuiVoltas { get; set; }
    public bool LargadaBaterias { get; set; }
    public DefinicaoVoltas DefinicaoVoltas { get; set; } = DefinicaoVoltas.NaCategoria;
    public ICollection<Evento> Eventos { get; set; } = new List<Evento>();
}
