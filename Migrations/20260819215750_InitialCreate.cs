using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UbecSolicitacoes.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Alunos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Nome = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Matricula = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Ativo = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alunos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Solicitacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AlunoId = table.Column<int>(type: "int", nullable: false),
                    TipoDocumento = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Status = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CriadaEm = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PrazoLimite = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicitacoes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Solicitacoes_Alunos_AlunoId",
                        column: x => x.AlunoId,
                        principalTable: "Alunos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Alunos",
                columns: new[] { "Id", "Ativo", "Matricula", "Nome" },
                values: new object[,]
                {
                    { 1, true, "2024001", "João Silva" },
                    { 2, true, "2024002", "Maria Oliveira" },
                    { 3, true, "2024003", "Pedro Santos" },
                    { 4, false, "2024004", "Ana Costa" }
                });

            migrationBuilder.InsertData(
                table: "Solicitacoes",
                columns: new[] { "Id", "AlunoId", "CriadaEm", "PrazoLimite", "Status", "TipoDocumento" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2026, 8, 19, 18, 57, 47, 554, DateTimeKind.Local).AddTicks(3021), new DateTime(2026, 8, 21, 18, 57, 47, 554, DateTimeKind.Local).AddTicks(3021), "Pendente", "Declaração de Matrícula" },
                    { 2, 2, new DateTime(2026, 8, 15, 18, 57, 47, 554, DateTimeKind.Local).AddTicks(3021), new DateTime(2026, 8, 18, 18, 57, 47, 554, DateTimeKind.Local).AddTicks(3021), "Pendente", "Atestado de Frequência" },
                    { 3, 1, new DateTime(2026, 8, 14, 18, 57, 47, 554, DateTimeKind.Local).AddTicks(3021), new DateTime(2026, 8, 19, 18, 57, 47, 554, DateTimeKind.Local).AddTicks(3021), "Concluída", "Histórico Escolar" },
                    { 4, 3, new DateTime(2026, 8, 14, 18, 57, 47, 554, DateTimeKind.Local).AddTicks(3021), new DateTime(2026, 8, 16, 18, 57, 47, 554, DateTimeKind.Local).AddTicks(3021), "Pendente", "Declaração de Matrícula" },
                    { 5, 2, new DateTime(2026, 8, 19, 18, 57, 47, 554, DateTimeKind.Local).AddTicks(3021), new DateTime(2026, 8, 24, 18, 57, 47, 554, DateTimeKind.Local).AddTicks(3021), "Cancelada", "Histórico Escolar" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Solicitacoes_AlunoId",
                table: "Solicitacoes",
                column: "AlunoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Solicitacoes");

            migrationBuilder.DropTable(
                name: "Alunos");
        }
    }
}
