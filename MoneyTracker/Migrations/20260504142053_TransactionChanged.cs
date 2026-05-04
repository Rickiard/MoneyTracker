using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MoneyTracker.Migrations
{
    /// <inheritdoc />
    public partial class TransactionChanged : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Categories_TransactionCategoryId",
                table: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionCategoryId",
                table: "Transactions",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Categories_TransactionCategoryId",
                table: "Transactions",
                column: "TransactionCategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Categories_TransactionCategoryId",
                table: "Transactions");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionCategoryId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Categories_TransactionCategoryId",
                table: "Transactions",
                column: "TransactionCategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
