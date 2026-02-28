using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Popcorn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Atletas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NumeroDocumento = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataNascimento = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Sexo = table.Column<int>(type: "int", nullable: false),
                    Equipe = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Cidade = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Estado = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Atletas", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Campeonatos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    DataInicio = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DataFim = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Descricao = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Pontuacao1 = table.Column<int>(type: "int", nullable: false),
                    Pontuacao2 = table.Column<int>(type: "int", nullable: false),
                    Pontuacao3 = table.Column<int>(type: "int", nullable: false),
                    Pontuacao4 = table.Column<int>(type: "int", nullable: false),
                    Pontuacao5 = table.Column<int>(type: "int", nullable: false),
                    Pontuacao6 = table.Column<int>(type: "int", nullable: false),
                    Pontuacao7 = table.Column<int>(type: "int", nullable: false),
                    Pontuacao8 = table.Column<int>(type: "int", nullable: false),
                    Pontuacao9 = table.Column<int>(type: "int", nullable: false),
                    Pontuacao10 = table.Column<int>(type: "int", nullable: false),
                    Pontuacao11 = table.Column<int>(type: "int", nullable: false),
                    Pontuacao12 = table.Column<int>(type: "int", nullable: false),
                    Pontuacao13 = table.Column<int>(type: "int", nullable: false),
                    Pontuacao14 = table.Column<int>(type: "int", nullable: false),
                    Pontuacao15 = table.Column<int>(type: "int", nullable: false),
                    PontuacaoParticipacao = table.Column<int>(type: "int", nullable: false),
                    PontuacaoOrganizador = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campeonatos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descricao = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Sexo = table.Column<int>(type: "int", nullable: false),
                    IdadeMin = table.Column<int>(type: "int", nullable: true),
                    IdadeMax = table.Column<int>(type: "int", nullable: true),
                    NaoClassificaGeral = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Eventos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Data = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Modalidade = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SepararGeral = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    QtGeral = table.Column<int>(type: "int", nullable: true),
                    SepGeralMunicip = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    QtGeralMunicip = table.Column<int>(type: "int", nullable: true),
                    NomeMunicip = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ClassificarTempoBruto = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HoraLargada = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Login = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    SenhaHash = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "CampeonatoEventos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CampeonatoId = table.Column<int>(type: "int", nullable: false),
                    EventoId = table.Column<int>(type: "int", nullable: false),
                    PontuacaoEspecial = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Pontuacao1 = table.Column<int>(type: "int", nullable: false),
                    Pontuacao2 = table.Column<int>(type: "int", nullable: false),
                    Pontuacao3 = table.Column<int>(type: "int", nullable: false),
                    Pontuacao4 = table.Column<int>(type: "int", nullable: false),
                    Pontuacao5 = table.Column<int>(type: "int", nullable: false),
                    Pontuacao6 = table.Column<int>(type: "int", nullable: false),
                    Pontuacao7 = table.Column<int>(type: "int", nullable: false),
                    Pontuacao8 = table.Column<int>(type: "int", nullable: false),
                    Pontuacao9 = table.Column<int>(type: "int", nullable: false),
                    Pontuacao10 = table.Column<int>(type: "int", nullable: false),
                    Pontuacao11 = table.Column<int>(type: "int", nullable: false),
                    Pontuacao12 = table.Column<int>(type: "int", nullable: false),
                    Pontuacao13 = table.Column<int>(type: "int", nullable: false),
                    Pontuacao14 = table.Column<int>(type: "int", nullable: false),
                    Pontuacao15 = table.Column<int>(type: "int", nullable: false),
                    PontuacaoParticipacao = table.Column<int>(type: "int", nullable: false),
                    PontuacaoOrganizador = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampeonatoEventos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CampeonatoEventos_Campeonatos_CampeonatoId",
                        column: x => x.CampeonatoId,
                        principalTable: "Campeonatos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CampeonatoEventos_Eventos_EventoId",
                        column: x => x.EventoId,
                        principalTable: "Eventos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Chegadas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AtletaId = table.Column<int>(type: "int", nullable: false),
                    EventoId = table.Column<int>(type: "int", nullable: false),
                    Volta = table.Column<int>(type: "int", nullable: false),
                    HoraChegada = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    FlagDNS = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FlagDNF = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    FlagDSQ = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chegadas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Chegadas_Atletas_AtletaId",
                        column: x => x.AtletaId,
                        principalTable: "Atletas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Chegadas_Eventos_EventoId",
                        column: x => x.EventoId,
                        principalTable: "Eventos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EventoTrajetos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EventoId = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Distancia = table.Column<decimal>(type: "decimal(10,3)", precision: 10, scale: 3, nullable: false),
                    QuantVoltas = table.Column<int>(type: "int", nullable: false),
                    HoraLargada = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventoTrajetos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventoTrajetos_Eventos_EventoId",
                        column: x => x.EventoId,
                        principalTable: "Eventos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Resultados",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AtletaId = table.Column<int>(type: "int", nullable: false),
                    EventoId = table.Column<int>(type: "int", nullable: false),
                    TempoBruto = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    TempoLiquido = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    TempoTotal = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    PosicaoGeral = table.Column<int>(type: "int", nullable: true),
                    PosicaoCategoria = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resultados", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resultados_Atletas_AtletaId",
                        column: x => x.AtletaId,
                        principalTable: "Atletas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Resultados_Eventos_EventoId",
                        column: x => x.EventoId,
                        principalTable: "Eventos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "EventoCategorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    EventoId = table.Column<int>(type: "int", nullable: false),
                    EventoTrajetoId = table.Column<int>(type: "int", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false),
                    NroVoltas = table.Column<int>(type: "int", nullable: true),
                    HoraLargada = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventoCategorias", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventoCategorias_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventoCategorias_EventoTrajetos_EventoTrajetoId",
                        column: x => x.EventoTrajetoId,
                        principalTable: "EventoTrajetos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventoCategorias_Eventos_EventoId",
                        column: x => x.EventoId,
                        principalTable: "Eventos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "InscricoesAtletas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AtletaId = table.Column<int>(type: "int", nullable: false),
                    EventoId = table.Column<int>(type: "int", nullable: false),
                    EventoCategoriaId = table.Column<int>(type: "int", nullable: false),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    TagRfid = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Camisa = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RetirouKit = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    HoraLargada = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    Observacao = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Organizador = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InscricoesAtletas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InscricoesAtletas_Atletas_AtletaId",
                        column: x => x.AtletaId,
                        principalTable: "Atletas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InscricoesAtletas_EventoCategorias_EventoCategoriaId",
                        column: x => x.EventoCategoriaId,
                        principalTable: "EventoCategorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InscricoesAtletas_Eventos_EventoId",
                        column: x => x.EventoId,
                        principalTable: "Eventos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_CampeonatoEventos_CampeonatoId",
                table: "CampeonatoEventos",
                column: "CampeonatoId");

            migrationBuilder.CreateIndex(
                name: "IX_CampeonatoEventos_EventoId",
                table: "CampeonatoEventos",
                column: "EventoId");

            migrationBuilder.CreateIndex(
                name: "IX_Chegadas_AtletaId",
                table: "Chegadas",
                column: "AtletaId");

            migrationBuilder.CreateIndex(
                name: "IX_Chegadas_EventoId",
                table: "Chegadas",
                column: "EventoId");

            migrationBuilder.CreateIndex(
                name: "IX_EventoCategorias_CategoriaId",
                table: "EventoCategorias",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_EventoCategorias_EventoId",
                table: "EventoCategorias",
                column: "EventoId");

            migrationBuilder.CreateIndex(
                name: "IX_EventoCategorias_EventoTrajetoId",
                table: "EventoCategorias",
                column: "EventoTrajetoId");

            migrationBuilder.CreateIndex(
                name: "IX_EventoTrajetos_EventoId",
                table: "EventoTrajetos",
                column: "EventoId");

            migrationBuilder.CreateIndex(
                name: "IX_InscricoesAtletas_AtletaId",
                table: "InscricoesAtletas",
                column: "AtletaId");

            migrationBuilder.CreateIndex(
                name: "IX_InscricoesAtletas_EventoCategoriaId",
                table: "InscricoesAtletas",
                column: "EventoCategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_InscricoesAtletas_EventoId",
                table: "InscricoesAtletas",
                column: "EventoId");

            migrationBuilder.CreateIndex(
                name: "IX_Resultados_AtletaId",
                table: "Resultados",
                column: "AtletaId");

            migrationBuilder.CreateIndex(
                name: "IX_Resultados_EventoId",
                table: "Resultados",
                column: "EventoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CampeonatoEventos");

            migrationBuilder.DropTable(
                name: "Chegadas");

            migrationBuilder.DropTable(
                name: "InscricoesAtletas");

            migrationBuilder.DropTable(
                name: "Resultados");

            migrationBuilder.DropTable(
                name: "Usuarios");

            migrationBuilder.DropTable(
                name: "Campeonatos");

            migrationBuilder.DropTable(
                name: "EventoCategorias");

            migrationBuilder.DropTable(
                name: "Atletas");

            migrationBuilder.DropTable(
                name: "Categorias");

            migrationBuilder.DropTable(
                name: "EventoTrajetos");

            migrationBuilder.DropTable(
                name: "Eventos");
        }
    }
}
