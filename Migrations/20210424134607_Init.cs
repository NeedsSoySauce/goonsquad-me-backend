using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace hackathon.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Goons",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    LastSeenOnUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Goonsquads",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Goonsquads", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedOnUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    GoonId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.UniqueConstraint("UNIQUE_GoonId", x => x.GoonId);
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jobs_Goons_GoonId",
                        column: x => x.GoonId,
                        principalTable: "Goons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GoonGoonsquad",
                columns: table => new
                {
                    GoonsId = table.Column<string>(type: "text", nullable: false),
                    GoonsquadsId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GoonGoonsquad", x => new { x.GoonsId, x.GoonsquadsId });
                    table.ForeignKey(
                        name: "FK_GoonGoonsquad_Goons_GoonsId",
                        column: x => x.GoonsId,
                        principalTable: "Goons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GoonGoonsquad_Goonsquads_GoonsquadsId",
                        column: x => x.GoonsquadsId,
                        principalTable: "Goonsquads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SentOnUtc = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    GoonsquadId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Goonsquads_GoonsquadId",
                        column: x => x.GoonsquadId,
                        principalTable: "Goonsquads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GoonGoonsquad_GoonsquadsId",
                table: "GoonGoonsquad",
                column: "GoonsquadsId");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_GoonId",
                table: "Jobs",
                column: "GoonId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_GoonsquadId",
                table: "Messages",
                column: "GoonsquadId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GoonGoonsquad");

            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Goons");

            migrationBuilder.DropTable(
                name: "Goonsquads");
        }
    }
}
