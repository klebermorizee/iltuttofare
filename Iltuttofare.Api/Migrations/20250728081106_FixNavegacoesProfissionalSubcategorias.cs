using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iltuttofare.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixNavegacoesProfissionalSubcategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfissionalSubcategorias_Profissionais_ProfissionalId1",
                table: "ProfissionalSubcategorias");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfissionalSubcategorias_Subcategorias_SubcategoriaId1",
                table: "ProfissionalSubcategorias");

            migrationBuilder.DropIndex(
                name: "IX_ProfissionalSubcategorias_ProfissionalId1",
                table: "ProfissionalSubcategorias");

            migrationBuilder.DropIndex(
                name: "IX_ProfissionalSubcategorias_SubcategoriaId1",
                table: "ProfissionalSubcategorias");

            migrationBuilder.DropColumn(
                name: "ProfissionalId1",
                table: "ProfissionalSubcategorias");

            migrationBuilder.DropColumn(
                name: "SubcategoriaId1",
                table: "ProfissionalSubcategorias");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProfissionalId1",
                table: "ProfissionalSubcategorias",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SubcategoriaId1",
                table: "ProfissionalSubcategorias",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProfissionalSubcategorias_ProfissionalId1",
                table: "ProfissionalSubcategorias",
                column: "ProfissionalId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProfissionalSubcategorias_SubcategoriaId1",
                table: "ProfissionalSubcategorias",
                column: "SubcategoriaId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfissionalSubcategorias_Profissionais_ProfissionalId1",
                table: "ProfissionalSubcategorias",
                column: "ProfissionalId1",
                principalTable: "Profissionais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfissionalSubcategorias_Subcategorias_SubcategoriaId1",
                table: "ProfissionalSubcategorias",
                column: "SubcategoriaId1",
                principalTable: "Subcategorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
