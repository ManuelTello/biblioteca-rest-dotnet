using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace biblioteca_dotnet.Migrations
{
    /// <inheritdoc />
    public partial class BooksRentedColumnAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Rented",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rented",
                table: "Books");
        }
    }
}
