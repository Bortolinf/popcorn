using Popcorn.Domain.Enums;

namespace Popcorn.Domain.Entities;

public class Usuario : BaseEntity
{
    public string Nome { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public string SenhaHash { get; set; } = string.Empty;
    public TipoUsuario Tipo { get; set; }
    public bool Ativo { get; set; } = true;
}
