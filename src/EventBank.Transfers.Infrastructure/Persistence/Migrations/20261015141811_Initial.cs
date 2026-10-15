using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventBank.Transfers.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "account_directory",
                columns: table => new
                {
                    account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    holder = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    is_open = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_account_directory", x => x.account_id);
                });

            migrationBuilder.CreateTable(
                name: "subscription_checkpoints",
                columns: table => new
                {
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    transaction_id = table.Column<long>(type: "bigint", nullable: false),
                    position = table.Column<long>(type: "bigint", nullable: false),
                    rebuild_requested = table.Column<bool>(type: "boolean", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_subscription_checkpoints", x => x.name);
                });

            migrationBuilder.CreateTable(
                name: "transfer_summaries",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    source_account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    source_holder = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    destination_account_id = table.Column<Guid>(type: "uuid", nullable: false),
                    destination_holder = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    amount = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    failure_reason = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    version = table.Column<long>(type: "bigint", nullable: false),
                    requested_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_transfer_summaries", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_transfer_summaries_destination_account_id",
                table: "transfer_summaries",
                column: "destination_account_id");

            migrationBuilder.CreateIndex(
                name: "ix_transfer_summaries_source_account_id",
                table: "transfer_summaries",
                column: "source_account_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account_directory");

            migrationBuilder.DropTable(
                name: "subscription_checkpoints");

            migrationBuilder.DropTable(
                name: "transfer_summaries");
        }
    }
}
