using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarBlazor.Migrations
{
    /// <inheritdoc />
    public partial class Statuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Loan");

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Loan",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "LoanStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanStatus", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "LoanStatus",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Active" },
                    { 2, "Paid" },
                    { 3, "Defaulted" },
                    { 4, "Refinanced" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Loan_StatusId",
                table: "Loan",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loan_LoanStatus_StatusId",
                table: "Loan",
                column: "StatusId",
                principalTable: "LoanStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loan_LoanStatus_StatusId",
                table: "Loan");

            migrationBuilder.DropTable(
                name: "LoanStatus");

            migrationBuilder.DropIndex(
                name: "IX_Loan_StatusId",
                table: "Loan");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Loan");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Loan",
                type: "TEXT",
                nullable: true);
        }
    }
}
