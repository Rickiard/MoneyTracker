using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MoneyTracker.Migrations
{
    /// <inheritdoc />
    public partial class SqLiteDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Icon = table.Column<string>(type: "TEXT", nullable: false),
                    ColorHex = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Value = table.Column<decimal>(type: "TEXT", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsIncome = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Transactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "ColorHex", "Icon", "Name" },
                values: new object[,]
                {
                    { 1, "#FF6B6B", "🍔", "Food & Dining" },
                    { 2, "#4ECDC4", "🚗", "Transportation" },
                    { 3, "#95E1D3", "🏥", "Health" },
                    { 4, "#A8E6CF", "📚", "Education" },
                    { 5, "#FFD3B6", "🎮", "Entertainment" },
                    { 6, "#FFAAA5", "💡", "Utilities" },
                    { 7, "#FF8B94", "👕", "Clothing" },
                    { 8, "#8E99F3", "🏠", "Housing" },
                    { 9, "#B19CD9", "✈️", "Travel" },
                    { 10, "#52C41A", "💰", "Salary" },
                    { 11, "#FAAD14", "🎁", "Bonus" },
                    { 12, "#D9D9D9", "📌", "Other" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                column: "Id",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CategoryId",
                table: "Transactions",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_UserId",
                table: "Transactions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
