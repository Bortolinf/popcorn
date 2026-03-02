using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Popcorn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDefinicaoVoltasToModalidade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DefinicaoVoltas",
                table: "Modalidades",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefinicaoVoltas",
                table: "Modalidades");
        }
    }
}
