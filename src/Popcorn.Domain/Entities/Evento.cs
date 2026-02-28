using Popcorn.Domain.Enums;

namespace Popcorn.Domain.Entities;

public class Evento : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public DateTime Data { get; set; }
    public StatusEvento Status { get; set; }
    public int? ModalidadeId { get; set; }
    public Modalidade? Modalidade { get; set; }
    public bool SepararGeral { get; set; }
    public int? QtGeral { get; set; }
    public bool SepGeralMunicip { get; set; }
    public int? QtGeralMunicip { get; set; }
    public string? NomeMunicip { get; set; }
    public bool ClassificarTempoBruto { get; set; }
    public DateTime? HoraLargada { get; set; }

    public ICollection<EventoTrajeto> Trajetos { get; set; } = new List<EventoTrajeto>();
    public ICollection<EventoCategoria> Categorias { get; set; } = new List<EventoCategoria>();
    public ICollection<InscricaoAtleta> Inscricoes { get; set; } = new List<InscricaoAtleta>();
    public ICollection<Chegada> Chegadas { get; set; } = new List<Chegada>();
    public ICollection<Resultado> Resultados { get; set; } = new List<Resultado>();
    public ICollection<CampeonatoEvento> CampeonatoEventos { get; set; } = new List<CampeonatoEvento>();
}
