using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iltuttofare.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddProfissionalSubcategoriasRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProfissionalSubcategoria",
                columns: table => new
                {
                    ProfissionalId = table.Column<int>(type: "integer", nullable: false),
                    SubcategoriasId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfissionalSubcategoria", x => new { x.ProfissionalId, x.SubcategoriasId });
                    table.ForeignKey(
                        name: "FK_ProfissionalSubcategoria_Profissionais_ProfissionalId",
                        column: x => x.ProfissionalId,
                        principalTable: "Profissionais",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProfissionalSubcategoria_Subcategorias_SubcategoriasId",
                        column: x => x.SubcategoriasId,
                        principalTable: "Subcategorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfissionalSubcategoria_SubcategoriasId",
                table: "ProfissionalSubcategoria",
                column: "SubcategoriasId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfissionalSubcategoria");
        }
    }
}
