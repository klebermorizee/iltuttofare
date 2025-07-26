using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Iltuttofare.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailToProfissional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Profissionais",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Profissionais");
        }
    }
}
