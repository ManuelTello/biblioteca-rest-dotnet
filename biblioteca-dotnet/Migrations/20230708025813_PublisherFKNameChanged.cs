using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace biblioteca_dotnet.Migrations
{
    /// <inheritdoc />
    public partial class PublisherFKNameChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EPublisherId",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EPublisherId",
                table: "Books");
        }
    }
}
