using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Db.Migrations
{
    public partial class InitilaCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "r_blog",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    url = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "CI_AS")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_R_BLOG", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "r_post",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    title = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: true, collation: "CI_AS"),
                    content = table.Column<string>(type: "text", nullable: true, collation: "CI_AS"),
                    id_blog = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_R_POST", x => x.id);
                    table.ForeignKey(
                        name: "FK_R_POST_R_BLOG",
                        column: x => x.id_blog,
                        principalTable: "r_blog",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_r_post_id_blog",
                table: "r_post",
                column: "id_blog");

            // load some data
            loadData(migrationBuilder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "r_post");

            migrationBuilder.DropTable(
                name: "r_blog");
        }
    }
}
