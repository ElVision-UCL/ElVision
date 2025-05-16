using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ElVision.Data.Migrations.UserMigrations
{
    /// <inheritdoc />
    public partial class AddAPIKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ElOverblikApiKey",
                table: "AspNetUsers",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ElOverblikApiKey",
                table: "AspNetUsers");
        }
    }
}
