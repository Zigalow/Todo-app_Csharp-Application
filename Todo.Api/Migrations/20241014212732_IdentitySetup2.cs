using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Todo.Api.Migrations
{
    /// <inheritdoc />
    public partial class IdentitySetup2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_TodoItems_TodoItemId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_TodoItemId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "TodoItemId",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TodoItemId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_TodoItemId",
                table: "AspNetUsers",
                column: "TodoItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_TodoItems_TodoItemId",
                table: "AspNetUsers",
                column: "TodoItemId",
                principalTable: "TodoItems",
                principalColumn: "Id");
        }
    }
}
