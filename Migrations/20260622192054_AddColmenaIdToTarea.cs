using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObligatorioApiario.Migrations
{
    /// <inheritdoc />
    public partial class AddColmenaIdToTarea : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ColmenaId",
                table: "Tareas",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tareas_ColmenaId",
                table: "Tareas",
                column: "ColmenaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tareas_Colmenas_ColmenaId",
                table: "Tareas",
                column: "ColmenaId",
                principalTable: "Colmenas",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tareas_Colmenas_ColmenaId",
                table: "Tareas");

            migrationBuilder.DropIndex(
                name: "IX_Tareas_ColmenaId",
                table: "Tareas");

            migrationBuilder.DropColumn(
                name: "ColmenaId",
                table: "Tareas");
        }
    }
}
