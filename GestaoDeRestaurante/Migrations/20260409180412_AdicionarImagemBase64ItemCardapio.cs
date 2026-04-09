using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoDeRestaurante.Migrations
{
    /// <inheritdoc />
    public partial class AdicionarImagemBase64ItemCardapio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "ImagemBase64",
                table: "ItensCardapio");
        }
    }
}
