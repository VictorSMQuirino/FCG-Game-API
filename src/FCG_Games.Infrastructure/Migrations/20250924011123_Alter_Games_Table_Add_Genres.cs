using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCG_Games.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Alter_Games_Table_Add_Genres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int[]>(
                name: "Genres",
                table: "Games",
                type: "integer[]",
                nullable: false,
                defaultValue: new int[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Genres",
                table: "Games");
        }
    }
}
