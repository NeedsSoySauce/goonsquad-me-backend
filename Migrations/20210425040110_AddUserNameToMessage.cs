using Microsoft.EntityFrameworkCore.Migrations;

namespace hackathon.Migrations
{
    public partial class AddUserNameToMessage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GoonUsername",
                table: "Messages",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GoonUsername",
                table: "Messages");
        }
    }
}
