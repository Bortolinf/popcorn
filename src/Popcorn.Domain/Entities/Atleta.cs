using Popcorn.Domain.Enums;

namespace Popcorn.Domain.Entities;

public class Atleta : BaseEntity
{
    public string NumeroDocumento { get; set; } = string.Empty;
    public string Nome { get; set; } = string.Empty;
    public DateTime DataNascimento { get; set; }
    public Sexo Sexo { get; set; }
    public string? Equipe { get; set; }
    public string? Cidade { get; set; }
    public string? Estado { get; set; }

    public ICollection<InscricaoAtleta> Inscricoes { get; set; } = new List<InscricaoAtleta>();
    public ICollection<Chegada> Chegadas { get; set; } = new List<Chegada>();
    public ICollection<Resultado> Resultados { get; set; } = new List<Resultado>();
}
