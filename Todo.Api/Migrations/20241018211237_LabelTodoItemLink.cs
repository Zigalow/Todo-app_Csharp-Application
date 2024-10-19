using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Todo.Api.Migrations
{
    /// <inheritdoc />
    public partial class LabelTodoItemLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Labels_TodoItems_TodoItemId",
                table: "Labels");

            migrationBuilder.DropIndex(
                name: "IX_Labels_TodoItemId",
                table: "Labels");

            migrationBuilder.DropColumn(
                name: "TodoItemId",
                table: "Labels");

            migrationBuilder.CreateTable(
                name: "LabelTodoItem",
                columns: table => new
                {
                    LabelsId = table.Column<int>(type: "int", nullable: false),
                    TodoItemsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabelTodoItem", x => new { x.LabelsId, x.TodoItemsId });
                    table.ForeignKey(
                        name: "FK_LabelTodoItem_Labels_LabelsId",
                        column: x => x.LabelsId,
                        principalTable: "Labels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LabelTodoItem_TodoItems_TodoItemsId",
                        column: x => x.TodoItemsId,
                        principalTable: "TodoItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LabelTodoItem_TodoItemsId",
                table: "LabelTodoItem",
                column: "TodoItemsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LabelTodoItem");

            migrationBuilder.AddColumn<int>(
                name: "TodoItemId",
                table: "Labels",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Labels_TodoItemId",
                table: "Labels",
                column: "TodoItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Labels_TodoItems_TodoItemId",
                table: "Labels",
                column: "TodoItemId",
                principalTable: "TodoItems",
                principalColumn: "Id");
        }
    }
}
