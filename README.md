# Popcorn

Aplicação desktop Windows para cronometragem de eventos esportivos.

## Funcionalidades

- Cadastro e gerenciamento de atletas
- Organização de eventos e campeonatos
- Controle de categorias (individual, duplas, trios, quartetos)
- Cronometragem precisa com registro de chegadas por volta
- Cálculo automático de classificação geral e por categoria
- Exportação de resultados em CSV
- Operação offline com banco de dados local (MariaDB)

## Stack

| Camada         | Tecnologia                          |
|----------------|-------------------------------------|
| UI             | WPF (.NET 9, MVVM)                  |
| Application    | C# — Services e Interfaces          |
| Domain         | C# — Entidades e Enums              |
| Infrastructure | Entity Framework Core 9 + MariaDB   |

## Pré-requisitos

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9)
- [XAMPP](https://www.apachefriends.org/) com MariaDB 10.4+

## Configuração

### 1. Banco de dados

Inicie o MariaDB pelo XAMPP Control Panel e crie o banco:

```sql
CREATE DATABASE popcorn;
```

A connection string padrão está em:

- `src/Popcorn.Infrastructure/DependencyInjection/ServiceCollectionExtensions.cs`
- `src/Popcorn.UI/App.xaml.cs`

```
Server=127.0.0.1;Database=popcorn;User=root;Password=;SslMode=None;
```

### 2. Migrations

```bash
dotnet ef database update --project src/Popcorn.Infrastructure --startup-project src/Popcorn.Infrastructure
```

### 3. Rodar a aplicação

```bash
dotnet run --project src/Popcorn.UI/Popcorn.UI.csproj
```

## Login padrão

| Campo | Valor  |
|-------|--------|
| Login | admin  |
| Senha | 123456 |

## Estrutura do projeto

```
Popcorn/
├── src/
│   ├── Popcorn.Domain/          # Entidades e Enums
│   ├── Popcorn.Application/     # Interfaces e Services
│   ├── Popcorn.Infrastructure/  # EF Core, Repositories, Migrations
│   └── Popcorn.UI/              # WPF — Views, ViewModels, XAML
└── docs/
    └── cronometragem-desktop-fluxo.md
```

## Fluxo operacional

```
Criar Evento → Criar Trajetos → Criar Categorias → Cadastrar Atletas
→ Inscrever Atletas → Iniciar Cronometragem → Registrar Chegadas
→ Calcular Resultados → Exportar Classificação
```
