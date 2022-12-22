using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AIStore.DAL.Migrations
{
    public partial class added_isEmailConfirm : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsEmailСonfirm",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsEmailСonfirm",
                table: "Users");
        }
    }
}
