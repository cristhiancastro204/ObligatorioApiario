using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObligatorioApiario.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAltitud : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Altitud",
                table: "Apiarios");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Altitud",
                table: "Apiarios",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
