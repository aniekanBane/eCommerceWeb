using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eCommerceWeb.Migrator.StoreMigrations
{
    /// <inheritdoc />
    public partial class CatalogAggUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "media_file");

            migrationBuilder.DropIndex(
                name: "ix_product_normalised_name",
                table: "product");

            migrationBuilder.DropIndex(
                name: "ix_product_normalised_sku",
                table: "product");

            migrationBuilder.DropCheckConstraint(
                name: "ck_price",
                table: "product");

            migrationBuilder.DropCheckConstraint(
                name: "ck_stock",
                table: "product");

            migrationBuilder.DropCheckConstraint(
                name: "ck_unit_price_value",
                table: "product");

            migrationBuilder.DropIndex(
                name: "ix_category_normalised_name",
                table: "category");

            migrationBuilder.DropColumn(
                name: "normalised_name",
                table: "product");

            migrationBuilder.DropColumn(
                name: "normalised_name",
                table: "category");

            migrationBuilder.DropColumn(
                name: "is_parent_category",
                table: "category");
            
            migrationBuilder.DropColumn(
                name: "normalised_name",
                table: "tag");

            migrationBuilder.RenameColumn(
                name: "normalised_sku",
                table: "product",
                newName: "normalized_sku");

            migrationBuilder.RenameColumn(
                name: "editor",
                table: "product",
                newName: "last_modified_by");

            migrationBuilder.RenameColumn(
                name: "author",
                table: "product",
                newName: "created_by");

            migrationBuilder.RenameColumn(
                name: "editor",
                table: "category",
                newName: "last_modified_by");

            migrationBuilder.RenameColumn(
                name: "author",
                table: "category",
                newName: "created_by");

            migrationBuilder.AlterColumn<string>(
                name: "product_id",
                table: "tag",
                type: "character(32)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character(36)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "tag",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60);
            
            migrationBuilder.AddColumn<string>(
                name: "normalized_name",
                table: "tag",
                type: "text",
                nullable: false,
                computedColumnSql: "UPPER(name)",
                stored: true);

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "product_review",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.AlterColumn<string>(
                name: "review_text",
                table: "product_review",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "product_id",
                table: "product_review",
                type: "character(32)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character(36)");

            migrationBuilder.AlterColumn<string>(
                name: "related_products_id",
                table: "product_product",
                type: "character(32)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character(36)");

            migrationBuilder.AlterColumn<string>(
                name: "product_id",
                table: "product_product",
                type: "character(32)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character(36)");

            migrationBuilder.AlterColumn<string>(
                name: "product_id",
                table: "product_image",
                type: "character(32)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character(36)");

            migrationBuilder.AlterColumn<string>(
                name: "product_id",
                table: "product_category",
                type: "character(32)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character(36)");

            migrationBuilder.AlterColumn<string>(
                name: "id",
                table: "product",
                type: "character(32)",
                fixedLength: true,
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character(36)",
                oldFixedLength: true,
                oldMaxLength: 36);

            migrationBuilder.AddColumn<string>(
                name: "normalized_name",
                table: "product",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<bool>(
                name: "is_visible",
                table: "category",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_enabled",
                table: "category",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "normalized_name",
                table: "category",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_tag_normalized_name",
                table: "tag",
                column: "normalized_name")
                .Annotation("Npgsql:IndexInclude", new[] { "name" });

            migrationBuilder.CreateIndex(
                name: "ix_product_normalized_name",
                table: "product",
                column: "normalized_name",
                unique: true)
                .Annotation("Npgsql:IndexInclude", new[] { "name" });

            migrationBuilder.CreateIndex(
                name: "ix_product_normalized_sku",
                table: "product",
                column: "normalized_sku",
                unique: true)
                .Annotation("Npgsql:IndexInclude", new[] { "sku" });

            migrationBuilder.AddCheckConstraint(
                name: "ck_sale_price",
                table: "product",
                sql: "(on_sale = TRUE AND sale_price > 0 AND sale_price < unit_price) OR (on_sale = FALSE AND sale_price = 0)");

            migrationBuilder.AddCheckConstraint(
                name: "ck_stock_quantity",
                table: "product",
                sql: "(has_unlimited_stock = FALSE AND stock_quantity >= 0) OR (has_unlimited_stock AND stock_quantity IS NULL)");

            migrationBuilder.AddCheckConstraint(
                name: "ck_unit_price",
                table: "product",
                sql: "unit_price >= 0");

            migrationBuilder.CreateIndex(
                name: "ix_category_normalized_name",
                table: "category",
                column: "normalized_name",
                unique: true)
                .Annotation("Npgsql:IndexInclude", new[] { "name" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_product_normalized_name",
                table: "product");

            migrationBuilder.DropIndex(
                name: "ix_product_normalized_sku",
                table: "product");

            migrationBuilder.DropCheckConstraint(
                name: "ck_sale_price",
                table: "product");

            migrationBuilder.DropCheckConstraint(
                name: "ck_stock_quantity",
                table: "product");

            migrationBuilder.DropCheckConstraint(
                name: "ck_unit_price",
                table: "product");

            migrationBuilder.DropIndex(
                name: "ix_category_normalized_name",
                table: "category");

            migrationBuilder.DropColumn(
                name: "normalized_name",
                table: "product");

            migrationBuilder.DropColumn(
                name: "normalized_name",
                table: "category");

            migrationBuilder.DropColumn(
                name: "normalized_name",
                table: "tag");

            migrationBuilder.RenameColumn(
                name: "normalized_sku",
                table: "product",
                newName: "normalised_sku");

            migrationBuilder.RenameColumn(
                name: "last_modified_by",
                table: "product",
                newName: "editor");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "product",
                newName: "author");

            migrationBuilder.RenameColumn(
                name: "last_modified_by",
                table: "category",
                newName: "editor");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "category",
                newName: "author");

            migrationBuilder.AlterColumn<string>(
                name: "product_id",
                table: "tag",
                type: "character(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character(32)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "tag",
                type: "character varying(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldMaxLength: 64);
            
            migrationBuilder.AddColumn<string>(
                name: "normalised_name",
                table: "tag",
                type: "text",
                nullable: false,
                computedColumnSql: "UPPER(name)",
                stored: true);

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "product_review",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "review_text",
                table: "product_review",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "product_id",
                table: "product_review",
                type: "character(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character(32)");

            migrationBuilder.AlterColumn<string>(
                name: "related_products_id",
                table: "product_product",
                type: "character(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character(32)");

            migrationBuilder.AlterColumn<string>(
                name: "product_id",
                table: "product_product",
                type: "character(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character(32)");

            migrationBuilder.AlterColumn<string>(
                name: "product_id",
                table: "product_image",
                type: "character(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character(32)");

            migrationBuilder.AlterColumn<string>(
                name: "product_id",
                table: "product_category",
                type: "character(36)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character(32)");

            migrationBuilder.AlterColumn<string>(
                name: "id",
                table: "product",
                type: "character(36)",
                fixedLength: true,
                maxLength: 36,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character(32)",
                oldFixedLength: true,
                oldMaxLength: 32);

            migrationBuilder.AlterColumn<bool>(
                name: "is_visible",
                table: "category",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<bool>(
                name: "is_enabled",
                table: "category",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<bool>(
                name: "is_parent_category",
                table: "category",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "normalised_name",
                table: "product",
                type: "text",
                nullable: false,
                computedColumnSql: "UPPER(name)",
                stored: true);

            migrationBuilder.AddColumn<string>(
                name: "normalised_name",
                table: "category",
                type: "text",
                nullable: false,
                computedColumnSql: "UPPER(name)",
                stored: true);

            migrationBuilder.CreateTable(
                name: "media_file",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    author = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    content_type = table.Column<string>(type: "text", nullable: false),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    editor = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    file_location = table.Column<string>(type: "text", nullable: false),
                    file_name = table.Column<string>(type: "text", nullable: false),
                    file_size = table.Column<long>(type: "bigint", nullable: false),
                    file_type = table.Column<string>(type: "text", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    last_modified_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    title = table.Column<string>(type: "character varying(125)", maxLength: 125, nullable: true),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_media_file", x => x.id);
                    table.CheckConstraint("ck_file_size", "file_size <= 1048576000");
                    table.CheckConstraint("ck_file_type", "file_type IN ('Document', 'Image', 'Video')");
                });

            migrationBuilder.CreateIndex(
                name: "ix_tag_normalised_name",
                table: "tag",
                column: "normalised_name");

            migrationBuilder.CreateIndex(
                name: "ix_product_normalised_name",
                table: "product",
                column: "normalised_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_product_normalised_sku",
                table: "product",
                column: "normalised_sku",
                unique: true);

            migrationBuilder.AddCheckConstraint(
                name: "ck_price",
                table: "product",
                sql: "(on_sale = TRUE AND sale_price > 0 AND sale_price < unit_price) OR (on_sale = FALSE AND sale_price = 0)");

            migrationBuilder.AddCheckConstraint(
                name: "ck_stock",
                table: "product",
                sql: "(has_unlimited_stock = FALSE AND stock_quantity >= 0) OR (has_unlimited_stock AND stock_quantity IS NULL)");

            migrationBuilder.AddCheckConstraint(
                name: "ck_unit_price_value",
                table: "product",
                sql: "unit_price >= 0");

            migrationBuilder.CreateIndex(
                name: "ix_category_normalised_name",
                table: "category",
                column: "normalised_name",
                unique: true);
        }
    }
}
