using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lab2.RezervacijeProstora.Migrations
{
    /// <inheritdoc />
    public partial class AddProstorDatoteke : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProstorDatoteke",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ProstorZaProbuId = table.Column<int>(type: "INTEGER", nullable: false),
                    FileName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    FilePath = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    ContentType = table.Column<string>(type: "TEXT", maxLength: 150, nullable: false),
                    FileSize = table.Column<long>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProstorDatoteke", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProstorDatoteke_Prostori_ProstorZaProbuId",
                        column: x => x.ProstorZaProbuId,
                        principalTable: "Prostori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProstorDatoteke_ProstorZaProbuId",
                table: "ProstorDatoteke",
                column: "ProstorZaProbuId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProstorDatoteke");
        }
    }
}
