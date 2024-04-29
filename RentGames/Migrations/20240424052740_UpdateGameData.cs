using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RentGames.Migrations
{
    /// <inheritdoc />
    public partial class UpdateGameData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Preco",
                table: "Games",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "Games",
                newName: "Name");

            migrationBuilder.AddColumn<bool>(
                name: "IsStock",
                table: "Games",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsStock",
                table: "Games");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Games",
                newName: "Preco");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Games",
                newName: "Nome");
        }
    }
}
