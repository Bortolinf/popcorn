namespace Popcorn.Domain.Entities;

public class Campeonato : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
    public string? Descricao { get; set; }
    public int Pontuacao1 { get; set; }
    public int Pontuacao2 { get; set; }
    public int Pontuacao3 { get; set; }
    public int Pontuacao4 { get; set; }
    public int Pontuacao5 { get; set; }
    public int Pontuacao6 { get; set; }
    public int Pontuacao7 { get; set; }
    public int Pontuacao8 { get; set; }
    public int Pontuacao9 { get; set; }
    public int Pontuacao10 { get; set; }
    public int Pontuacao11 { get; set; }
    public int Pontuacao12 { get; set; }
    public int Pontuacao13 { get; set; }
    public int Pontuacao14 { get; set; }
    public int Pontuacao15 { get; set; }
    public int PontuacaoParticipacao { get; set; }
    public int PontuacaoOrganizador { get; set; }

    public ICollection<CampeonatoEvento> CampeonatoEventos { get; set; } = new List<CampeonatoEvento>();
}
