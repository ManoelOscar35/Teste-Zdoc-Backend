using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestaoPessoalApi.Migrations
{
    /// <inheritdoc />
    public partial class AddColunasHistoricoAlteracao : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Historicos_Funcionarios_FuncionarioId",
                table: "Historicos");

            migrationBuilder.DropIndex(
                name: "IX_Historicos_FuncionarioId",
                table: "Historicos");

            migrationBuilder.DropColumn(
                name: "ValorAntigo",
                table: "Historicos");

            migrationBuilder.AlterColumn<string>(
                name: "ValorNovo",
                table: "Historicos",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CampoAlterado",
                table: "Historicos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ValorAnterior",
                table: "Historicos",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Ferias",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValorAnterior",
                table: "Historicos");

            migrationBuilder.AlterColumn<string>(
                name: "ValorNovo",
                table: "Historicos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.AlterColumn<string>(
                name: "CampoAlterado",
                table: "Historicos",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "ValorAntigo",
                table: "Historicos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Ferias",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.CreateIndex(
                name: "IX_Historicos_FuncionarioId",
                table: "Historicos",
                column: "FuncionarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Historicos_Funcionarios_FuncionarioId",
                table: "Historicos",
                column: "FuncionarioId",
                principalTable: "Funcionarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
