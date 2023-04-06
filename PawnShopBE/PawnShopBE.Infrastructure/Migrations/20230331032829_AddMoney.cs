using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PawnShopBE.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMoney : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
   

            migrationBuilder.CreateTable(
                name: "Money",
                columns: table => new
                {
                    MoneyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    DateCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MoneyInput = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Money", x => x.MoneyId);
                    table.ForeignKey(
                        name: "FK_Money_Branch_BranchId",
                        column: x => x.BranchId,
                        principalTable: "Branch",
                        principalColumn: "BranchId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Money_BranchId",
                table: "Money",
                column: "BranchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Money");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "UserPermissionGroups");
        }
    }
}
