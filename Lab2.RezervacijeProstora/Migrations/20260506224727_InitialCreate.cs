using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Lab2.RezervacijeProstora.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Korisnici",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    KorisnickoIme = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ImePrezime = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    BrojTelefona = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    DatumRegistracije = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TipKorisnika = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Korisnici", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lokacije",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Grad = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Adresa = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    PostanskiBroj = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Drzava = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lokacije", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Oprema",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Naziv = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Proizvodac = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Ispravna = table.Column<bool>(type: "INTEGER", nullable: false),
                    Vrijednost = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oprema", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vlasnici",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ImePrezime = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    BrojTelefona = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    DatumRegistracije = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Oib = table.Column<string>(type: "TEXT", maxLength: 11, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vlasnici", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Prostori",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Naziv = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    KapacitetOsoba = table.Column<int>(type: "INTEGER", nullable: false),
                    CijenaPoSatu = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ImaParking = table.Column<bool>(type: "INTEGER", nullable: false),
                    ImaKlimu = table.Column<bool>(type: "INTEGER", nullable: false),
                    Aktivan = table.Column<bool>(type: "INTEGER", nullable: false),
                    DatumDodavanja = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LokacijaId = table.Column<int>(type: "INTEGER", nullable: false),
                    VlasnikId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prostori", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prostori_Lokacije_LokacijaId",
                        column: x => x.LokacijaId,
                        principalTable: "Lokacije",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prostori_Vlasnici_VlasnikId",
                        column: x => x.VlasnikId,
                        principalTable: "Vlasnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProstorOprema",
                columns: table => new
                {
                    OpremaId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProstoriId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProstorOprema", x => new { x.OpremaId, x.ProstoriId });
                    table.ForeignKey(
                        name: "FK_ProstorOprema_Oprema_OpremaId",
                        column: x => x.OpremaId,
                        principalTable: "Oprema",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProstorOprema_Prostori_ProstoriId",
                        column: x => x.ProstoriId,
                        principalTable: "Prostori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recenzije",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Ocjena = table.Column<int>(type: "INTEGER", nullable: false),
                    Komentar = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    DatumRecenzije = table.Column<DateTime>(type: "TEXT", nullable: false),
                    KorisnikId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProstorId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recenzije", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recenzije_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Recenzije_Prostori_ProstorId",
                        column: x => x.ProstorId,
                        principalTable: "Prostori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rezervacije",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DatumVrijemeOd = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DatumVrijemeDo = table.Column<DateTime>(type: "TEXT", nullable: false),
                    DatumKreiranja = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    BrojSudionika = table.Column<int>(type: "INTEGER", nullable: false),
                    Napomena = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    KorisnikId = table.Column<int>(type: "INTEGER", nullable: false),
                    ProstorId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rezervacije", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rezervacije_Korisnici_KorisnikId",
                        column: x => x.KorisnikId,
                        principalTable: "Korisnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rezervacije_Prostori_ProstorId",
                        column: x => x.ProstorId,
                        principalTable: "Prostori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Placanja",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Iznos = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    DatumPlacanja = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Uspjesno = table.Column<bool>(type: "INTEGER", nullable: false),
                    NacinPlacanja = table.Column<int>(type: "INTEGER", nullable: false),
                    BrojTransakcije = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    RezervacijaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Placanja", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Placanja_Rezervacije_RezervacijaId",
                        column: x => x.RezervacijaId,
                        principalTable: "Rezervacije",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Placanja_RezervacijaId",
                table: "Placanja",
                column: "RezervacijaId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prostori_LokacijaId",
                table: "Prostori",
                column: "LokacijaId");

            migrationBuilder.CreateIndex(
                name: "IX_Prostori_VlasnikId",
                table: "Prostori",
                column: "VlasnikId");

            migrationBuilder.CreateIndex(
                name: "IX_ProstorOprema_ProstoriId",
                table: "ProstorOprema",
                column: "ProstoriId");

            migrationBuilder.CreateIndex(
                name: "IX_Recenzije_KorisnikId",
                table: "Recenzije",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Recenzije_ProstorId",
                table: "Recenzije",
                column: "ProstorId");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacije_KorisnikId",
                table: "Rezervacije",
                column: "KorisnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Rezervacije_ProstorId",
                table: "Rezervacije",
                column: "ProstorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Placanja");

            migrationBuilder.DropTable(
                name: "ProstorOprema");

            migrationBuilder.DropTable(
                name: "Recenzije");

            migrationBuilder.DropTable(
                name: "Rezervacije");

            migrationBuilder.DropTable(
                name: "Oprema");

            migrationBuilder.DropTable(
                name: "Korisnici");

            migrationBuilder.DropTable(
                name: "Prostori");

            migrationBuilder.DropTable(
                name: "Lokacije");

            migrationBuilder.DropTable(
                name: "Vlasnici");
        }
    }
}
