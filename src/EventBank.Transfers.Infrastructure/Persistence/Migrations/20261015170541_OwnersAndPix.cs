using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventBank.Transfers.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class OwnersAndPix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "source_holder",
                table: "transfer_summaries",
                type: "character varying(120)",
                maxLength: 120,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "destination_holder",
                table: "transfer_summaries",
                type: "character varying(120)",
                maxLength: 120,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "holder",
                table: "account_directory",
                type: "character varying(120)",
                maxLength: 120,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "number",
                table: "account_directory",
                type: "character varying(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "owner_id",
                table: "account_directory",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "customer_directory",
                columns: table => new
                {
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    full_name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    cpf = table.Column<string>(type: "character varying(11)", maxLength: 11, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customer_directory", x => x.customer_id);
                });

            migrationBuilder.CreateTable(
                name: "pix_keys",
                columns: table => new
                {
                    key = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    type = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_name = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    registered_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_pix_keys", x => x.key);
                });

            migrationBuilder.CreateIndex(
                name: "ix_account_directory_owner_id",
                table: "account_directory",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "ix_pix_keys_account_id",
                table: "pix_keys",
                column: "account_id");

            migrationBuilder.CreateIndex(
                name: "ix_pix_keys_owner_id",
                table: "pix_keys",
                column: "owner_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customer_directory");

            migrationBuilder.DropTable(
                name: "pix_keys");

            migrationBuilder.DropIndex(
                name: "ix_account_directory_owner_id",
                table: "account_directory");

            migrationBuilder.DropColumn(
                name: "number",
                table: "account_directory");

            migrationBuilder.DropColumn(
                name: "owner_id",
                table: "account_directory");

            migrationBuilder.AlterColumn<string>(
                name: "source_holder",
                table: "transfer_summaries",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(120)",
                oldMaxLength: 120);

            migrationBuilder.AlterColumn<string>(
                name: "destination_holder",
                table: "transfer_summaries",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(120)",
                oldMaxLength: 120);

            migrationBuilder.AlterColumn<string>(
                name: "holder",
                table: "account_directory",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(120)",
                oldMaxLength: 120);
        }
    }
}
