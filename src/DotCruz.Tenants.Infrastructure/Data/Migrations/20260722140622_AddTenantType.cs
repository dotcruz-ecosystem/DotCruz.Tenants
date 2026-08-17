using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DotCruz.Tenants.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddTenantType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "type",
                table: "tenants",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "type",
                table: "tenants");
        }
    }
}
