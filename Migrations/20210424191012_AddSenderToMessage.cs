using Microsoft.EntityFrameworkCore.Migrations;

namespace hackathon.Migrations
{
    public partial class AddSenderToMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GoonId",
                table: "Messages",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_GoonId",
                table: "Messages",
                column: "GoonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Goons_GoonId",
                table: "Messages",
                column: "GoonId",
                principalTable: "Goons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Goons_GoonId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_GoonId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "GoonId",
                table: "Messages");
        }
    }
}
