using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MrAndMissUniversity.Migrations
{
    /// <inheritdoc />
    public partial class Initial01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    TelegramId = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 40, nullable: true),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 40, nullable: true),
                    Patronymic = table.Column<string>(type: "TEXT", maxLength: 40, nullable: true),
                    Year = table.Column<short>(type: "INTEGER", nullable: true),
                    Group = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
                    NameOfSpecialty = table.Column<string>(type: "TEXT", maxLength: 60, nullable: true),
                    Photograph = table.Column<byte[]>(type: "BLOB", nullable: true),
                    BriefIntroduction = table.Column<string>(type: "TEXT", nullable: true),
                    Reason = table.Column<string>(type: "TEXT", nullable: true),
                    RegistrationStep = table.Column<short>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.TelegramId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
