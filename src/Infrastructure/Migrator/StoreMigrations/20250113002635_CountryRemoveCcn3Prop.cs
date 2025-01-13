using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eCommerceWeb.Migrator.StoreMigrations
{
    /// <inheritdoc />
    public partial class CountryRemoveCcn3Prop : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_country_ccn3",
                table: "country");

            migrationBuilder.DropColumn(
                name: "ccn3",
                table: "country");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ccn3",
                table: "country",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_country_ccn3",
                table: "country",
                column: "ccn3",
                unique: true);
        }
    }
}
