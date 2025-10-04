using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FCG_Games.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Alter_UserGames_Table_Add_GameAccessState_Column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameAccessState",
                table: "UserGames",
                type: "integer",
                nullable: false,
                defaultValue: 2);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameAccessState",
                table: "UserGames");
        }
    }
}
