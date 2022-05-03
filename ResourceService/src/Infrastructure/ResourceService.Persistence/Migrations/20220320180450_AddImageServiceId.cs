using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ResourceService.Persistence.Migrations
{
    public partial class AddImageServiceId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageServiceId",
                table: "Images",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageServiceId",
                table: "Images");
        }
    }
}
