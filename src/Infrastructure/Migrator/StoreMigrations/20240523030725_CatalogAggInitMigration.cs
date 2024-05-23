using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace eCommerceWeb.Migrator.StoreMigrations
{
    /// <inheritdoc />
    public partial class CatalogAggInitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:CollationDefinition:case_insensitive", "en-u-ks-level2,en-u-ks-level2,icu,False");

            migrationBuilder.CreateTable(
                name: "category",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:IdentitySequenceOptions", "'100221', '1', '', '', 'False', '1'")
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, collation: "case_insensitive"),
                    normalised_name = table.Column<string>(type: "text", nullable: false),
                    is_parent_category = table.Column<bool>(type: "boolean", nullable: false),
                    parent_category_id = table.Column<int>(type: "integer", nullable: true),
                    is_enabled = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    is_visible = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    seo_meta_description = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    seo_meta_keywords = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    seo_meta_title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    seo_url_slug = table.Column<string>(type: "text", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    author = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    editor = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    last_modified_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_category", x => x.id);
                    table.ForeignKey(
                        name: "fk_category_category_parent_category_id",
                        column: x => x.parent_category_id,
                        principalTable: "category",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "media_file",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "character varying(125)", maxLength: 125, nullable: true),
                    description = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: true),
                    file_size = table.Column<long>(type: "bigint", nullable: false),
                    file_type = table.Column<string>(type: "text", nullable: false),
                    file_location = table.Column<string>(type: "text", nullable: false),
                    file_name = table.Column<string>(type: "text", nullable: false),
                    content_type = table.Column<string>(type: "text", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    author = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    editor = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    last_modified_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_media_file", x => x.id);
                    table.CheckConstraint("ck_file_size", "file_size <= 1048576000");
                    table.CheckConstraint("ck_file_type", "file_type IN ('Document', 'Image', 'Video')");
                });

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    id = table.Column<string>(type: "character(36)", fixedLength: true, maxLength: 36, nullable: false),
                    name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false, collation: "case_insensitive"),
                    normalised_name = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "character varying(2048)", maxLength: 2048, nullable: true),
                    sku = table.Column<string>(type: "character(9)", fixedLength: true, maxLength: 9, nullable: false, collation: "case_insensitive"),
                    normalised_sku = table.Column<string>(type: "text", nullable: false),
                    thumbnail_uri = table.Column<string>(type: "text", nullable: true),
                    on_sale = table.Column<bool>(type: "boolean", nullable: false),
                    unit_price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    sale_price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    stock_quantity = table.Column<int>(type: "integer", nullable: true),
                    has_unlimited_stock = table.Column<bool>(type: "boolean", nullable: false),
                    visibility = table.Column<string>(type: "text", nullable: false, defaultValue: "Hidden"),
                    is_featured = table.Column<bool>(type: "boolean", nullable: false),
                    enable_product_reviews = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    enable_related_products = table.Column<bool>(type: "boolean", nullable: false),
                    social_image_url = table.Column<string>(type: "text", nullable: true),
                    seo_meta_description = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: true),
                    seo_meta_keywords = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    seo_meta_title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    seo_url_slug = table.Column<string>(type: "text", nullable: false),
                    xmin = table.Column<uint>(type: "xid", rowVersion: true, nullable: false),
                    author = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    created_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    editor = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    last_modified_on_utc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product", x => x.id);
                    table.CheckConstraint("ck_price", "(on_sale = TRUE AND sale_price > 0 AND sale_price < unit_price) OR (on_sale = FALSE AND sale_price = 0)");
                    table.CheckConstraint("ck_stock", "(has_unlimited_stock = FALSE AND stock_quantity >= 0) OR (has_unlimited_stock AND stock_quantity IS NULL)");
                    table.CheckConstraint("ck_unit_price_value", "unit_price >= 0");
                    table.CheckConstraint("ck_visibility", "visibility IN ('Hidden', 'Public', 'Scheduled')");
                });

            migrationBuilder.CreateTable(
                name: "product_category",
                columns: table => new
                {
                    category_id = table.Column<int>(type: "integer", nullable: false),
                    product_id = table.Column<string>(type: "character(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_category", x => new { x.category_id, x.product_id });
                    table.ForeignKey(
                        name: "fk_product_category_category_category_id",
                        column: x => x.category_id,
                        principalTable: "category",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_product_category_product_product_id",
                        column: x => x.product_id,
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_image",
                columns: table => new
                {
                    product_id = table.Column<string>(type: "character(36)", nullable: false),
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    is_thumbnail = table.Column<bool>(type: "boolean", nullable: false),
                    alt_name = table.Column<string>(type: "text", nullable: true),
                    uri = table.Column<string>(type: "text", nullable: false),
                    display_order = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_image", x => new { x.product_id, x.id });
                    table.ForeignKey(
                        name: "fk_product_image_product_product_id",
                        column: x => x.product_id,
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_product",
                columns: table => new
                {
                    product_id = table.Column<string>(type: "character(36)", nullable: false),
                    related_products_id = table.Column<string>(type: "character(36)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_product", x => new { x.product_id, x.related_products_id });
                    table.ForeignKey(
                        name: "fk_product_product_product_product_id",
                        column: x => x.product_id,
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_product_product_product_related_products_id",
                        column: x => x.related_products_id,
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_review",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    product_id = table.Column<string>(type: "character(36)", nullable: false),
                    title = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    review_text = table.Column<string>(type: "text", nullable: false),
                    rating = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_review", x => x.id);
                    table.CheckConstraint("ck_rating", "rating BETWEEN 1 AND 5");
                    table.ForeignKey(
                        name: "fk_product_review_product_product_id",
                        column: x => x.product_id,
                        principalTable: "product",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tag",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(60)", maxLength: 60, nullable: false, collation: "case_insensitive"),
                    normalised_name = table.Column<string>(type: "text", nullable: false),
                    discriminator = table.Column<string>(type: "character varying(13)", maxLength: 13, nullable: false),
                    product_id = table.Column<string>(type: "character(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tag", x => x.id);
                    table.ForeignKey(
                        name: "fk_tag_product_product_id",
                        column: x => x.product_id,
                        principalTable: "product",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_category_name",
                table: "category",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_category_parent_category_id",
                table: "category",
                column: "parent_category_id");

            migrationBuilder.CreateIndex(
                name: "ix_product_name",
                table: "product",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_product_sku",
                table: "product",
                column: "sku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_product_category_product_id",
                table: "product_category",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_product_product_related_products_id",
                table: "product_product",
                column: "related_products_id");

            migrationBuilder.CreateIndex(
                name: "ix_product_review_product_id",
                table: "product_review",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_tag_name",
                table: "tag",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "ix_tag_product_id",
                table: "tag",
                column: "product_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "media_file");

            migrationBuilder.DropTable(
                name: "product_category");

            migrationBuilder.DropTable(
                name: "product_image");

            migrationBuilder.DropTable(
                name: "product_product");

            migrationBuilder.DropTable(
                name: "product_review");

            migrationBuilder.DropTable(
                name: "tag");

            migrationBuilder.DropTable(
                name: "category");

            migrationBuilder.DropTable(
                name: "product");
        }
    }
}
