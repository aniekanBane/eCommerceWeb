﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using eCommerceWeb.Persistence;

#nullable disable

namespace eCommerceWeb.Migrator.StoreMigrations
{
    [DbContext(typeof(StoreDbContext))]
    [Migration("20240523030725_CatalogAggInitMigration")]
    partial class CatalogAggInitMigration
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:CollationDefinition:case_insensitive", "en-u-ks-level2,en-u-ks-level2,icu,False")
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ProductProduct", b =>
                {
                    b.Property<string>("ProductId")
                        .HasColumnType("character(36)")
                        .HasColumnName("product_id");

                    b.Property<string>("RelatedProductsId")
                        .HasColumnType("character(36)")
                        .HasColumnName("related_products_id");

                    b.HasKey("ProductId", "RelatedProductsId")
                        .HasName("pk_product_product");

                    b.HasIndex("RelatedProductsId")
                        .HasDatabaseName("ix_product_product_related_products_id");

                    b.ToTable("product_product", (string)null);
                });

            modelBuilder.Entity("eCommerceWeb.Domain.Entities.CatalogAggregate.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasColumnOrder(0);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));
                    NpgsqlPropertyBuilderExtensions.HasIdentityOptions(b.Property<int>("Id"), 100221L, null, null, null, null, null);

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("author");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on_utc");

                    b.Property<string>("Editor")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("editor");

                    b.Property<bool>("IsEnabled")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true)
                        .HasColumnName("is_enabled");

                    b.Property<bool>("IsParentCategory")
                        .HasColumnType("boolean")
                        .HasColumnName("is_parent_category");

                    b.Property<bool>("IsVisible")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true)
                        .HasColumnName("is_visible");

                    b.Property<DateTime>("LastModifiedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_modified_on_utc");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("name")
                        .UseCollation("case_insensitive");

                    b.Property<string>("NormalisedName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("normalised_name");

                    b.Property<int?>("ParentCategoryId")
                        .HasColumnType("integer")
                        .HasColumnName("parent_category_id");

                    b.Property<uint>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.ComplexProperty<Dictionary<string, object>>("Seo", "eCommerceWeb.Domain.Entities.CatalogAggregate.Category.Seo#Seo", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("MetaDescription")
                                .HasMaxLength(400)
                                .HasColumnType("character varying(400)")
                                .HasColumnName("seo_meta_description");

                            b1.Property<string>("MetaKeywords")
                                .HasMaxLength(256)
                                .HasColumnType("character varying(256)")
                                .HasColumnName("seo_meta_keywords");

                            b1.Property<string>("MetaTitle")
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("seo_meta_title");

                            b1.Property<string>("UrlSlug")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("seo_url_slug");
                        });

                    b.HasKey("Id")
                        .HasName("pk_category");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_category_name");

                    b.HasIndex("ParentCategoryId")
                        .HasDatabaseName("ix_category_parent_category_id");

                    b.ToTable("category", (string)null);
                });

            modelBuilder.Entity("eCommerceWeb.Domain.Entities.CatalogAggregate.Product", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(36)
                        .HasColumnType("character(36)")
                        .HasColumnName("id")
                        .HasColumnOrder(0)
                        .IsFixedLength();

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("author");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on_utc");

                    b.Property<string>("Description")
                        .HasMaxLength(2048)
                        .HasColumnType("character varying(2048)")
                        .HasColumnName("description");

                    b.Property<string>("Editor")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("editor");

                    b.Property<bool>("EnableProductReviews")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(true)
                        .HasColumnName("enable_product_reviews");

                    b.Property<bool>("EnableRelatedProducts")
                        .HasColumnType("boolean")
                        .HasColumnName("enable_related_products");

                    b.Property<bool>("HasUnlimitedStock")
                        .HasColumnType("boolean")
                        .HasColumnName("has_unlimited_stock");

                    b.Property<bool>("IsFeatured")
                        .HasColumnType("boolean")
                        .HasColumnName("is_featured");

                    b.Property<DateTime>("LastModifiedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_modified_on_utc");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("name")
                        .UseCollation("case_insensitive");

                    b.Property<string>("NormalisedName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("normalised_name");

                    b.Property<string>("NormalisedSku")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("normalised_sku");

                    b.Property<bool>("OnSale")
                        .HasColumnType("boolean")
                        .HasColumnName("on_sale");

                    b.Property<decimal>("SalePrice")
                        .HasColumnType("numeric(10,2)")
                        .HasColumnName("sale_price");

                    b.Property<string>("Sku")
                        .IsRequired()
                        .HasMaxLength(9)
                        .HasColumnType("character(9)")
                        .HasColumnName("sku")
                        .IsFixedLength()
                        .UseCollation("case_insensitive");

                    b.Property<string>("SocialImageUrl")
                        .HasColumnType("text")
                        .HasColumnName("social_image_url");

                    b.Property<int?>("StockQuantity")
                        .HasColumnType("integer")
                        .HasColumnName("stock_quantity");

                    b.Property<string>("ThumbnailUri")
                        .HasColumnType("text")
                        .HasColumnName("thumbnail_uri");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("numeric(10,2)")
                        .HasColumnName("unit_price");

                    b.Property<uint>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.Property<string>("Visibility")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("Hidden")
                        .HasColumnName("visibility");

                    b.ComplexProperty<Dictionary<string, object>>("Seo", "eCommerceWeb.Domain.Entities.CatalogAggregate.Product.Seo#Seo", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("MetaDescription")
                                .HasMaxLength(400)
                                .HasColumnType("character varying(400)")
                                .HasColumnName("seo_meta_description");

                            b1.Property<string>("MetaKeywords")
                                .HasMaxLength(256)
                                .HasColumnType("character varying(256)")
                                .HasColumnName("seo_meta_keywords");

                            b1.Property<string>("MetaTitle")
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)")
                                .HasColumnName("seo_meta_title");

                            b1.Property<string>("UrlSlug")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("seo_url_slug");
                        });

                    b.HasKey("Id")
                        .HasName("pk_product");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_product_name");

                    b.HasIndex("Sku")
                        .IsUnique()
                        .HasDatabaseName("ix_product_sku");

                    b.ToTable("product", null, t =>
                        {
                            t.HasCheckConstraint("ck_price", "(on_sale = TRUE AND sale_price > 0 AND sale_price < unit_price) OR (on_sale = FALSE AND sale_price = 0)");

                            t.HasCheckConstraint("ck_stock", "(has_unlimited_stock = FALSE AND stock_quantity >= 0) OR (has_unlimited_stock AND stock_quantity IS NULL)");

                            t.HasCheckConstraint("ck_unit_price_value", "unit_price >= 0");

                            t.HasCheckConstraint("ck_visibility", "visibility IN ('Hidden', 'Public', 'Scheduled')");
                        });
                });

            modelBuilder.Entity("eCommerceWeb.Domain.Entities.CatalogAggregate.ProductCategory", b =>
                {
                    b.Property<int>("CategoryId")
                        .HasColumnType("integer")
                        .HasColumnName("category_id");

                    b.Property<string>("ProductId")
                        .HasColumnType("character(36)")
                        .HasColumnName("product_id");

                    b.HasKey("CategoryId", "ProductId")
                        .HasName("pk_product_category");

                    b.HasIndex("ProductId")
                        .HasDatabaseName("ix_product_category_product_id");

                    b.ToTable("product_category", (string)null);
                });

            modelBuilder.Entity("eCommerceWeb.Domain.Entities.CatalogAggregate.ProductReview", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasColumnOrder(0);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid")
                        .HasColumnName("customer_id");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasColumnType("character(36)")
                        .HasColumnName("product_id");

                    b.Property<int>("Rating")
                        .HasColumnType("integer")
                        .HasColumnName("rating");

                    b.Property<string>("ReviewText")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("review_text");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("character varying(128)")
                        .HasColumnName("title");

                    b.HasKey("Id")
                        .HasName("pk_product_review");

                    b.HasIndex("ProductId")
                        .HasDatabaseName("ix_product_review_product_id");

                    b.ToTable("product_review", null, t =>
                        {
                            t.HasCheckConstraint("ck_rating", "rating BETWEEN 1 AND 5");
                        });
                });

            modelBuilder.Entity("eCommerceWeb.Domain.Entities.Misc.MediaFile", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasColumnOrder(0);

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("author");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("content_type");

                    b.Property<DateTime>("CreatedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on_utc");

                    b.Property<string>("Description")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)")
                        .HasColumnName("description");

                    b.Property<string>("Editor")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("editor");

                    b.Property<string>("FileLocation")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("file_location");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("file_name");

                    b.Property<long>("FileSize")
                        .HasColumnType("bigint")
                        .HasColumnName("file_size");

                    b.Property<string>("FileType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("file_type");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean")
                        .HasColumnName("is_deleted");

                    b.Property<DateTime>("LastModifiedOnUtc")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_modified_on_utc");

                    b.Property<string>("Title")
                        .HasMaxLength(125)
                        .HasColumnType("character varying(125)")
                        .HasColumnName("title");

                    b.Property<uint>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("xid")
                        .HasColumnName("xmin");

                    b.HasKey("Id")
                        .HasName("pk_media_file");

                    b.ToTable("media_file", null, t =>
                        {
                            t.HasCheckConstraint("ck_file_size", "file_size <= 1048576000");

                            t.HasCheckConstraint("ck_file_type", "file_type IN ('Document', 'Image', 'Video')");
                        });
                });

            modelBuilder.Entity("eCommerceWeb.Domain.Entities.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasColumnOrder(0);

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("character varying(13)")
                        .HasColumnName("discriminator");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("character varying(60)")
                        .HasColumnName("name")
                        .UseCollation("case_insensitive");

                    b.Property<string>("NormalisedName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("normalised_name");

                    b.HasKey("Id")
                        .HasName("pk_tag");

                    b.HasIndex("Name")
                        .HasDatabaseName("ix_tag_name");

                    b.ToTable("tag", (string)null);

                    b.HasDiscriminator<string>("Discriminator").HasValue("Tag");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("eCommerceWeb.Domain.Entities.CatalogAggregate.ProductTag", b =>
                {
                    b.HasBaseType("eCommerceWeb.Domain.Entities.Tag");

                    b.Property<string>("ProductId")
                        .HasColumnType("character(36)")
                        .HasColumnName("product_id");

                    b.HasIndex("ProductId")
                        .HasDatabaseName("ix_tag_product_id");

                    b.ToTable("tag", (string)null);

                    b.HasDiscriminator().HasValue("ProductTag");
                });

            modelBuilder.Entity("ProductProduct", b =>
                {
                    b.HasOne("eCommerceWeb.Domain.Entities.CatalogAggregate.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_product_product_product_product_id");

                    b.HasOne("eCommerceWeb.Domain.Entities.CatalogAggregate.Product", null)
                        .WithMany()
                        .HasForeignKey("RelatedProductsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_product_product_product_related_products_id");
                });

            modelBuilder.Entity("eCommerceWeb.Domain.Entities.CatalogAggregate.Category", b =>
                {
                    b.HasOne("eCommerceWeb.Domain.Entities.CatalogAggregate.Category", null)
                        .WithMany("SubCategories")
                        .HasForeignKey("ParentCategoryId")
                        .HasConstraintName("fk_category_category_parent_category_id");
                });

            modelBuilder.Entity("eCommerceWeb.Domain.Entities.CatalogAggregate.Product", b =>
                {
                    b.OwnsMany("eCommerceWeb.Domain.Entities.CatalogAggregate.ProductImage", "Images", b1 =>
                        {
                            b1.Property<string>("ProductId")
                                .HasColumnType("character(36)")
                                .HasColumnName("product_id");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer")
                                .HasColumnName("id");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<string>("AltName")
                                .HasColumnType("text")
                                .HasColumnName("alt_name");

                            b1.Property<int>("DisplayOrder")
                                .HasColumnType("integer")
                                .HasColumnName("display_order");

                            b1.Property<bool>("IsThumbnail")
                                .HasColumnType("boolean")
                                .HasColumnName("is_thumbnail");

                            b1.Property<string>("Uri")
                                .IsRequired()
                                .HasColumnType("text")
                                .HasColumnName("uri");

                            b1.HasKey("ProductId", "Id")
                                .HasName("pk_product_image");

                            b1.ToTable("product_image", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("ProductId")
                                .HasConstraintName("fk_product_image_product_product_id");
                        });

                    b.Navigation("Images");
                });

            modelBuilder.Entity("eCommerceWeb.Domain.Entities.CatalogAggregate.ProductCategory", b =>
                {
                    b.HasOne("eCommerceWeb.Domain.Entities.CatalogAggregate.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_product_category_category_category_id");

                    b.HasOne("eCommerceWeb.Domain.Entities.CatalogAggregate.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_product_category_product_product_id");
                });

            modelBuilder.Entity("eCommerceWeb.Domain.Entities.CatalogAggregate.ProductReview", b =>
                {
                    b.HasOne("eCommerceWeb.Domain.Entities.CatalogAggregate.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_product_review_product_product_id");
                });

            modelBuilder.Entity("eCommerceWeb.Domain.Entities.CatalogAggregate.ProductTag", b =>
                {
                    b.HasOne("eCommerceWeb.Domain.Entities.CatalogAggregate.Product", null)
                        .WithMany("Tags")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("fk_tag_product_product_id");
                });

            modelBuilder.Entity("eCommerceWeb.Domain.Entities.CatalogAggregate.Category", b =>
                {
                    b.Navigation("SubCategories");
                });

            modelBuilder.Entity("eCommerceWeb.Domain.Entities.CatalogAggregate.Product", b =>
                {
                    b.Navigation("Tags");
                });
#pragma warning restore 612, 618
        }
    }
}
