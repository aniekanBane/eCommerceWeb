using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eCommerceWeb.Migrator.StoreMigrations
{
    /// <inheritdoc />
    public partial class CustomerAggInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "customer",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    firstname = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    lastname = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    fullname = table.Column<string>(type: "text", nullable: false, computedColumnSql: "firstname || ' ' || lastname", stored: true),
                    email_address = table.Column<string>(type: "character varying(256)", nullable: false),
                    phone_number = table.Column<string>(type: "character varying(15)", nullable: false),
                    accepts_marketing = table.Column<bool>(type: "boolean", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    created_by = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_modified_by = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    last_modified_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customer", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_customer_email_address",
                table: "customer",
                column: "email_address",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_customer_fullname",
                table: "customer",
                column: "fullname")
                .Annotation("Npgsql:IndexInclude", new[] { "firstname", "lastname" });

            migrationBuilder.CreateIndex(
                name: "ix_customer_phone_number",
                table: "customer",
                column: "phone_number");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "customer");
        }
    }
}
