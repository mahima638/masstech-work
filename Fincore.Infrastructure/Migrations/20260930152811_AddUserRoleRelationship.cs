using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Fincore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddUserRoleRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "rid",
                table: "user",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_user_rid",
                table: "user",
                column: "rid");

            migrationBuilder.AddForeignKey(
                name: "FK_user_role_rid",
                table: "user",
                column: "rid",
                principalTable: "role",
                principalColumn: "rid",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_user_role_rid",
                table: "user");

            migrationBuilder.DropIndex(
                name: "IX_user_rid",
                table: "user");

            migrationBuilder.DropColumn(
                name: "rid",
                table: "user");
        }
    }
}
