using Microsoft.EntityFrameworkCore.Migrations;

namespace FiorellaBackToFrontProject.Migrations
{
    public partial class AddAboutContextAndAboutIconTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AboutIcons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IconSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IconContext = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutIcons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AboutContexts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VideoSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AboutIconId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutContexts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AboutContexts_AboutIcons_AboutIconId",
                        column: x => x.AboutIconId,
                        principalTable: "AboutIcons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AboutContexts_AboutIconId",
                table: "AboutContexts",
                column: "AboutIconId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutContexts");

            migrationBuilder.DropTable(
                name: "AboutIcons");
        }
    }
}
