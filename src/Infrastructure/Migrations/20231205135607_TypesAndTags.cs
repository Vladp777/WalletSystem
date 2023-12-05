using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TypesAndTags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_TransactionTag_TagId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_TransactionType_TypeId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionType",
                table: "TransactionType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionTag",
                table: "TransactionTag");

            migrationBuilder.RenameTable(
                name: "TransactionType",
                newName: "TransactionTypes");

            migrationBuilder.RenameTable(
                name: "TransactionTag",
                newName: "TransactionTags");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionTypes",
                table: "TransactionTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionTags",
                table: "TransactionTags",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_TransactionTags_TagId",
                table: "Transactions",
                column: "TagId",
                principalTable: "TransactionTags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_TransactionTypes_TypeId",
                table: "Transactions",
                column: "TypeId",
                principalTable: "TransactionTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_TransactionTags_TagId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_TransactionTypes_TypeId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionTypes",
                table: "TransactionTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TransactionTags",
                table: "TransactionTags");

            migrationBuilder.RenameTable(
                name: "TransactionTypes",
                newName: "TransactionType");

            migrationBuilder.RenameTable(
                name: "TransactionTags",
                newName: "TransactionTag");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionType",
                table: "TransactionType",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TransactionTag",
                table: "TransactionTag",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_TransactionTag_TagId",
                table: "Transactions",
                column: "TagId",
                principalTable: "TransactionTag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_TransactionType_TypeId",
                table: "Transactions",
                column: "TypeId",
                principalTable: "TransactionType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
