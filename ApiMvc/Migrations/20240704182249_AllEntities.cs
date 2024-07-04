using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiMvc.Migrations
{
    /// <inheritdoc />
    public partial class AllEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "Titles",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "Titles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "gender",
                table: "Titles",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "Profiles",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Profiles",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "Episodes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_idTitle",
                table: "Seasons",
                column: "idTitle");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_idUser",
                table: "Profiles",
                column: "idUser");

            migrationBuilder.CreateIndex(
                name: "IX_Episodes_idSeason",
                table: "Episodes",
                column: "idSeason");

            migrationBuilder.AddForeignKey(
                name: "FK_Episodes_Seasons_idSeason",
                table: "Episodes",
                column: "idSeason",
                principalTable: "Seasons",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Users_idUser",
                table: "Profiles",
                column: "idUser",
                principalTable: "Users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Seasons_Titles_idTitle",
                table: "Seasons",
                column: "idTitle",
                principalTable: "Titles",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Episodes_Seasons_idSeason",
                table: "Episodes");

            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Users_idUser",
                table: "Profiles");

            migrationBuilder.DropForeignKey(
                name: "FK_Seasons_Titles_idTitle",
                table: "Seasons");

            migrationBuilder.DropIndex(
                name: "IX_Seasons_idTitle",
                table: "Seasons");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_idUser",
                table: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_Episodes_idSeason",
                table: "Episodes");

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "Titles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "Titles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "gender",
                table: "Titles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "Episodes",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);
        }
    }
}
