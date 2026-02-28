using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Popcorn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddModalidade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Modalidade",
                table: "Eventos");

            migrationBuilder.AddColumn<int>(
                name: "ModalidadeId",
                table: "Eventos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Modalidades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Descricao = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PossuiVoltas = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LargadaBaterias = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modalidades", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_ModalidadeId",
                table: "Eventos",
                column: "ModalidadeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_Modalidades_ModalidadeId",
                table: "Eventos",
                column: "ModalidadeId",
                principalTable: "Modalidades",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_Modalidades_ModalidadeId",
                table: "Eventos");

            migrationBuilder.DropTable(
                name: "Modalidades");

            migrationBuilder.DropIndex(
                name: "IX_Eventos_ModalidadeId",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "ModalidadeId",
                table: "Eventos");

            migrationBuilder.AddColumn<string>(
                name: "Modalidade",
                table: "Eventos",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
