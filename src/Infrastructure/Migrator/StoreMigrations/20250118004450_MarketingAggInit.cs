using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace eCommerceWeb.Migrator.StoreMigrations
{
    /// <inheritdoc />
    public partial class MarketingAggInit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mailing_list",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: false),
                    normalized_name = table.Column<string>(type: "text", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    created_by = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    last_modified_by = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    last_modified_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mailing_list", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mailing_list_subcriber",
                columns: table => new
                {
                    mailing_list_id = table.Column<int>(type: "integer", nullable: false),
                    subcriber_id = table.Column<int>(type: "integer", nullable: false),
                    subcribed_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mailing_list_subcriber", x => new { x.mailing_list_id, x.subcriber_id });
                    table.ForeignKey(
                        name: "fk_mailing_list_subcriber_mailing_list_mailing_list_id",
                        column: x => x.mailing_list_id,
                        principalTable: "mailing_list",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_mailing_list_subcriber_subcriber_subcriber_id",
                        column: x => x.subcriber_id,
                        principalTable: "subcriber",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_mailing_list_normalized_name",
                table: "mailing_list",
                column: "normalized_name",
                unique: true)
                .Annotation("Npgsql:IndexInclude", new[] { "name" });

            migrationBuilder.CreateIndex(
                name: "ix_mailing_list_subcriber_subcriber_id",
                table: "mailing_list_subcriber",
                column: "subcriber_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mailing_list_subcriber");

            migrationBuilder.DropTable(
                name: "mailing_list");
        }
    }
}
