using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MrAndMissUniversity.Migrations
{
    /// <inheritdoc />
    public partial class Initial04 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StartDeleteMessage",
                table: "Students",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartDeleteMessage",
                table: "Students");
        }
    }
}
