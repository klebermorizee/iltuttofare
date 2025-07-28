using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iltuttofare.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddProfissionalSubcategoriaEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfissionalSubcategoria_Profissionais_ProfissionalId",
                table: "ProfissionalSubcategoria");

            migrationBuilder.RenameColumn(
                name: "ProfissionalId",
                table: "ProfissionalSubcategoria",
                newName: "ProfissionaisId");

            migrationBuilder.AddColumn<DateTime>(
                name: "DataCadastro",
                table: "ProfissionalSubcategorias",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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
                name: "FK_ProfissionalSubcategoria_Profissionais_ProfissionaisId",
                table: "ProfissionalSubcategoria",
                column: "ProfissionaisId",
                principalTable: "Profissionais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfissionalSubcategoria_Profissionais_ProfissionaisId",
                table: "ProfissionalSubcategoria");

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
                name: "DataCadastro",
                table: "ProfissionalSubcategorias");

            migrationBuilder.DropColumn(
                name: "ProfissionalId1",
                table: "ProfissionalSubcategorias");

            migrationBuilder.DropColumn(
                name: "SubcategoriaId1",
                table: "ProfissionalSubcategorias");

            migrationBuilder.RenameColumn(
                name: "ProfissionaisId",
                table: "ProfissionalSubcategoria",
                newName: "ProfissionalId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfissionalSubcategoria_Profissionais_ProfissionalId",
                table: "ProfissionalSubcategoria",
                column: "ProfissionalId",
                principalTable: "Profissionais",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
