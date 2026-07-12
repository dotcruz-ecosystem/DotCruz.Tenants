using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotCruz.Tenants.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:citext", ",,");

            migrationBuilder.CreateTable(
                name: "tenants",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    status = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    slug = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    document_number = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    document_type = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    contact_email = table.Column<string>(type: "citext", maxLength: 255, nullable: false),
                    contact_phone_country_code = table.Column<int>(type: "integer", nullable: false),
                    contact_phone_national_number = table.Column<string>(type: "character varying(14)", maxLength: 14, nullable: false),
                    subscription_plan = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    subscription_start_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    subscription_end_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    subscription_trial_end_date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    subscription_max_users = table.Column<int>(type: "integer", nullable: false),
                    subscription_max_emails_per_month = table.Column<int>(type: "integer", nullable: false),
                    suspension_reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    deleted_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    address = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tenants", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_tenants_document_number",
                table: "tenants",
                column: "document_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_tenants_slug",
                table: "tenants",
                column: "slug",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tenants");
        }
    }
}
