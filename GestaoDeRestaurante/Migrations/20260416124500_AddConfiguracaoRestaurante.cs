using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoDeRestaurante.Migrations
{
    /// <inheritdoc />
    public partial class AddConfiguracaoRestaurante : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfiguracoesRestaurante",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AlmocoInicio = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, defaultValue: "11:00"),
                    AlmocoFim = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, defaultValue: "14:00"),
                    JantarInicio = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, defaultValue: "18:00"),
                    JantarFim = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, defaultValue: "22:00"),
                    ReservaInicio = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, defaultValue: "11:00"),
                    ReservaFim = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: false, defaultValue: "14:00"),
                    AntecedenciaMinimaDias = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracoesRestaurante", x => x.Id);
                });

            // Insere registro padrão
            migrationBuilder.InsertData(
                table: "ConfiguracoesRestaurante",
                columns: new[] { "AlmocoInicio", "AlmocoFim", "JantarInicio", "JantarFim", "ReservaInicio", "ReservaFim", "AntecedenciaMinimaDias" },
                values: new object[] { "11:00", "14:00", "18:00", "22:00", "11:00", "14:00", 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfiguracoesRestaurante");
        }
    }
}
