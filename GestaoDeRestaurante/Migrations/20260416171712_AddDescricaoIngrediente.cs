using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoDeRestaurante.Migrations
{
    /// <inheritdoc />
    public partial class AddDescricaoIngrediente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Descricao",
                table: "Ingredientes",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Descricao",
                table: "Ingredientes");
        }
    }
}
