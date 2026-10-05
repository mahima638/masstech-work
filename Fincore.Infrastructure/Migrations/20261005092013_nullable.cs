using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fincore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class nullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_role_rid",
                table: "user");

            migrationBuilder.AlterColumn<int>(
                name: "rid",
                table: "user",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_user_role_rid",
                table: "user",
                column: "rid",
                principalTable: "role",
                principalColumn: "rid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_role_rid",
                table: "user");

            migrationBuilder.AlterColumn<int>(
                name: "rid",
                table: "user",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_user_role_rid",
                table: "user",
                column: "rid",
                principalTable: "role",
                principalColumn: "rid",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
