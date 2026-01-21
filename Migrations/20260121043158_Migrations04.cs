using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MrAndMissUniversity.Migrations
{
    /// <inheritdoc />
    public partial class Migrations04 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PhotoExtension",
                table: "Students",
                type: "TEXT",
                maxLength: 10,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoExtension",
                table: "Students");
        }
    }
}
