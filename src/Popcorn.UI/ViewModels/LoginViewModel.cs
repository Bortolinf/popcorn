using Popcorn.Application.Interfaces.Repositories;
using Popcorn.UI.Commands;
using Popcorn.UI.ViewModels.Base;
using System.Windows.Input;

namespace Popcorn.UI.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private readonly IUsuarioRepository _usuarioRepository;

    private string _login = string.Empty;
    private string _senha = string.Empty;
    private string _errorMessage = string.Empty;
    private bool _isLoading;

    public string Login
    {
        get => _login;
        set => SetProperty(ref _login, value);
    }

    public string Senha
    {
        get => _senha;
        set => SetProperty(ref _senha, value);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public bool IsLoading
    {
        get => _isLoading;
        set => SetProperty(ref _isLoading, value);
    }

    public ICommand LoginCommand { get; }

    public event Action? LoginSucesso;

    public LoginViewModel(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
        LoginCommand = new AsyncRelayCommand(ExecuteLoginAsync, _ => !IsLoading);
    }

    private async Task ExecuteLoginAsync(object? parameter)
    {
        ErrorMessage = string.Empty;
        IsLoading = true;

        try
        {
            var usuario = await _usuarioRepository.GetByLoginAsync(Login);

            if (usuario == null || !usuario.Ativo)
            {
                ErrorMessage = "Usuário não encontrado ou inativo.";
                return;
            }

            if (usuario.SenhaHash != HashSenha(Senha))
            {
                ErrorMessage = "Senha incorreta.";
                return;
            }

            LoginSucesso?.Invoke();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Erro ao conectar: {ex.Message}";
        }
        finally
        {
            IsLoading = false;
        }
    }

    private static string HashSenha(string senha)
    {
        using var sha = System.Security.Cryptography.SHA256.Create();
        var bytes = System.Text.Encoding.UTF8.GetBytes(senha);
        return Convert.ToHexString(sha.ComputeHash(bytes));
    }
}
