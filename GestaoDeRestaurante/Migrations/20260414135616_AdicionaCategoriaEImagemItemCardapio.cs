using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoDeRestaurante.Migrations
{
    /// <inheritdoc />
    public partial class AdicionaCategoriaEImagemItemCardapio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Categoria",
                table: "ItensCardapio",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImagemBase64",
                table: "ItensCardapio",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Categoria",
                table: "ItensCardapio");

            migrationBuilder.DropColumn(
                name: "ImagemBase64",
                table: "ItensCardapio");
        }
    }
}
