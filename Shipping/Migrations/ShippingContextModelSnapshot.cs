﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Shipping.Models;

#nullable disable

namespace Shipping.Migrations
{
    [DbContext(typeof(ShippingContext))]
    partial class ShippingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Shipping.Models.ApplicationRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Shipping.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Shipping.Models.Branch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Mobile")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Branches");
                });

            modelBuilder.Entity("Shipping.Models.BranchMerchant", b =>
                {
                    b.Property<int>("Merchant_Id")
                        .HasColumnType("int");

                    b.Property<int>("Branch_Id")
                        .HasColumnType("int");

                    b.HasKey("Merchant_Id", "Branch_Id");

                    b.HasIndex("Branch_Id");

                    b.ToTable("BranchMerchants");
                });

            modelBuilder.Entity("Shipping.Models.City", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Government_Id")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal?>("PickupShipping")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal?>("StandardShipping")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("Government_Id");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("Shipping.Models.Delivery", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AppUser_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Branch_Id")
                        .HasColumnType("int");

                    b.Property<decimal>("CompanyPercentage")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("DiscountType")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasMaxLength(100)
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("AppUser_Id")
                        .IsUnique();

                    b.HasIndex("Branch_Id");

                    b.ToTable("Deliveries");
                });

            modelBuilder.Entity("Shipping.Models.DeliveryGovernment", b =>
                {
                    b.Property<int>("Delivery_Id")
                        .HasColumnType("int");

                    b.Property<int>("Government_Id")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.HasKey("Delivery_Id", "Government_Id");

                    b.HasIndex("Government_Id");

                    b.ToTable("DeliveryGovernments");
                });

            modelBuilder.Entity("Shipping.Models.Employee", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AppUser_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Branch_Id")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("AppUser_Id")
                        .IsUnique();

                    b.HasIndex("Branch_Id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("Shipping.Models.Government", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Branch_Id")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Branch_Id");

                    b.ToTable("Governments");
                });

            modelBuilder.Entity("Shipping.Models.Merchant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AppUser_Id")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Government")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<decimal>("PickupCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("RejectedOrderPercentage")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("StoreName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("AppUser_Id")
                        .IsUnique();

                    b.ToTable("Merchants");
                });

            modelBuilder.Entity("Shipping.Models.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("City_Id")
                        .HasColumnType("int");

                    b.Property<string>("ClientAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClientEmail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClientName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClientPhone1")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClientPhone2")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("DeliverToVillage")
                        .HasColumnType("bit");

                    b.Property<int>("Delivery_Id")
                        .HasColumnType("int");

                    b.Property<int>("Dovernment_Id")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int>("Merchant_Id")
                        .HasColumnType("int");

                    b.Property<int>("OrderStatus")
                        .HasColumnType("int");

                    b.Property<int>("OrderType")
                        .HasColumnType("int");

                    b.Property<int>("PaymentType")
                        .HasColumnType("int");

                    b.Property<int>("RejectReason_ID")
                        .HasColumnType("int");

                    b.Property<int>("Setting_Id")
                        .HasColumnType("int");

                    b.Property<decimal>("ShippingCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("ShippingType_Id")
                        .HasColumnType("int");

                    b.Property<float>("TotalWeight")
                        .HasColumnType("real");

                    b.Property<int>("WeightPricing_Id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("City_Id");

                    b.HasIndex("Delivery_Id");

                    b.HasIndex("Dovernment_Id");

                    b.HasIndex("Merchant_Id");

                    b.HasIndex("RejectReason_ID");

                    b.HasIndex("Setting_Id");

                    b.HasIndex("ShippingType_Id");

                    b.HasIndex("WeightPricing_Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Shipping.Models.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("Shipping.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<float>("ItemWeight")
                        .HasColumnType("real");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("OrderId")
                        .HasColumnType("int");

                    b.Property<int>("Product_Id")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OrderId");

                    b.HasIndex("Product_Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Shipping.Models.RejectReason", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Reason")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RejectReason");
                });

            modelBuilder.Entity("Shipping.Models.RolePermission", b =>
                {
                    b.Property<int>("Permission_Id")
                        .HasColumnType("int");

                    b.Property<string>("Role_Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("CanAdd")
                        .HasColumnType("bit");

                    b.Property<bool>("CanDelete")
                        .HasColumnType("bit");

                    b.Property<bool>("CanEdit")
                        .HasColumnType("bit");

                    b.Property<bool>("CanView")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.HasKey("Permission_Id", "Role_Id");

                    b.HasIndex("Role_Id");

                    b.ToTable("RolePermissions");
                });

            modelBuilder.Entity("Shipping.Models.Setting", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("DeliveryAutoAccept")
                        .HasColumnType("bit");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<decimal>("ShippingToVillageCost")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.ToTable("ShippingToVillages");
                });

            modelBuilder.Entity("Shipping.Models.ShippingType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Cost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("ShippingTypes");
                });

            modelBuilder.Entity("Shipping.Models.SpecialShippingRate", b =>
                {
                    b.Property<int>("City_Id")
                        .HasColumnType("int");

                    b.Property<int>("Merchant_Id")
                        .HasColumnType("int");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<decimal>("SpecialPrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("City_Id", "Merchant_Id");

                    b.HasIndex("Merchant_Id");

                    b.ToTable("SpecialShippingRates");
                });

            modelBuilder.Entity("Shipping.Models.WeightPricing", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<decimal>("AdditionalKgPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("DefaultPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<float>("DefaultWeight")
                        .HasColumnType("real");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("WeightPricings");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Shipping.Models.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Shipping.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Shipping.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Shipping.Models.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shipping.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Shipping.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Shipping.Models.BranchMerchant", b =>
                {
                    b.HasOne("Shipping.Models.Branch", "Branch")
                        .WithMany("BranchMerchants")
                        .HasForeignKey("Branch_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shipping.Models.Merchant", "Merchant")
                        .WithMany("BranchMerchants")
                        .HasForeignKey("Merchant_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Branch");

                    b.Navigation("Merchant");
                });

            modelBuilder.Entity("Shipping.Models.City", b =>
                {
                    b.HasOne("Shipping.Models.Government", "Government")
                        .WithMany("Cities")
                        .HasForeignKey("Government_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Government");
                });

            modelBuilder.Entity("Shipping.Models.Delivery", b =>
                {
                    b.HasOne("Shipping.Models.ApplicationUser", "ApplicationUser")
                        .WithOne("Delivery")
                        .HasForeignKey("Shipping.Models.Delivery", "AppUser_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shipping.Models.Branch", "Branch")
                        .WithMany()
                        .HasForeignKey("Branch_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApplicationUser");

                    b.Navigation("Branch");
                });

            modelBuilder.Entity("Shipping.Models.DeliveryGovernment", b =>
                {
                    b.HasOne("Shipping.Models.Delivery", "Delivery")
                        .WithMany("DeliveryGovernments")
                        .HasForeignKey("Delivery_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shipping.Models.Government", "Government")
                        .WithMany()
                        .HasForeignKey("Government_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Delivery");

                    b.Navigation("Government");
                });

            modelBuilder.Entity("Shipping.Models.Employee", b =>
                {
                    b.HasOne("Shipping.Models.ApplicationUser", "ApplicationUser")
                        .WithOne("Employee")
                        .HasForeignKey("Shipping.Models.Employee", "AppUser_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shipping.Models.Branch", "Branch")
                        .WithMany("Employees")
                        .HasForeignKey("Branch_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApplicationUser");

                    b.Navigation("Branch");
                });

            modelBuilder.Entity("Shipping.Models.Government", b =>
                {
                    b.HasOne("Shipping.Models.Branch", "Branch")
                        .WithMany("Governments")
                        .HasForeignKey("Branch_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Branch");
                });

            modelBuilder.Entity("Shipping.Models.Merchant", b =>
                {
                    b.HasOne("Shipping.Models.ApplicationUser", "ApplicationUser")
                        .WithOne("Merchant")
                        .HasForeignKey("Shipping.Models.Merchant", "AppUser_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApplicationUser");
                });

            modelBuilder.Entity("Shipping.Models.Order", b =>
                {
                    b.HasOne("Shipping.Models.City", "City")
                        .WithMany("Orders")
                        .HasForeignKey("City_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shipping.Models.Delivery", "Delivery")
                        .WithMany("Orders")
                        .HasForeignKey("Delivery_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shipping.Models.Government", "Government")
                        .WithMany("Orders")
                        .HasForeignKey("Dovernment_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shipping.Models.Merchant", "Merchant")
                        .WithMany("Orders")
                        .HasForeignKey("Merchant_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shipping.Models.RejectReason", "RejectReason")
                        .WithMany("ORders")
                        .HasForeignKey("RejectReason_ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shipping.Models.Setting", "Setting")
                        .WithMany("Orders")
                        .HasForeignKey("Setting_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shipping.Models.ShippingType", "ShippingType")
                        .WithMany("Orders")
                        .HasForeignKey("ShippingType_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shipping.Models.WeightPricing", "WeightPricing")
                        .WithMany("Orders")
                        .HasForeignKey("WeightPricing_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");

                    b.Navigation("Delivery");

                    b.Navigation("Government");

                    b.Navigation("Merchant");

                    b.Navigation("RejectReason");

                    b.Navigation("Setting");

                    b.Navigation("ShippingType");

                    b.Navigation("WeightPricing");
                });

            modelBuilder.Entity("Shipping.Models.Product", b =>
                {
                    b.HasOne("Shipping.Models.Order", null)
                        .WithMany("Products")
                        .HasForeignKey("OrderId");

                    b.HasOne("Shipping.Models.Product", "product")
                        .WithMany()
                        .HasForeignKey("Product_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("product");
                });

            modelBuilder.Entity("Shipping.Models.RolePermission", b =>
                {
                    b.HasOne("Shipping.Models.Permission", "Permission")
                        .WithMany("RolePermissions")
                        .HasForeignKey("Permission_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shipping.Models.ApplicationRole", "Role")
                        .WithMany("RolePermissions")
                        .HasForeignKey("Role_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Permission");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Shipping.Models.SpecialShippingRate", b =>
                {
                    b.HasOne("Shipping.Models.City", "City")
                        .WithMany("SpecialShippingRates")
                        .HasForeignKey("City_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Shipping.Models.Merchant", "Merchant")
                        .WithMany("SpecialShippingRates")
                        .HasForeignKey("Merchant_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("City");

                    b.Navigation("Merchant");
                });

            modelBuilder.Entity("Shipping.Models.ApplicationRole", b =>
                {
                    b.Navigation("RolePermissions");
                });

            modelBuilder.Entity("Shipping.Models.ApplicationUser", b =>
                {
                    b.Navigation("Delivery");

                    b.Navigation("Employee");

                    b.Navigation("Merchant");
                });

            modelBuilder.Entity("Shipping.Models.Branch", b =>
                {
                    b.Navigation("BranchMerchants");

                    b.Navigation("Employees");

                    b.Navigation("Governments");
                });

            modelBuilder.Entity("Shipping.Models.City", b =>
                {
                    b.Navigation("Orders");

                    b.Navigation("SpecialShippingRates");
                });

            modelBuilder.Entity("Shipping.Models.Delivery", b =>
                {
                    b.Navigation("DeliveryGovernments");

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Shipping.Models.Government", b =>
                {
                    b.Navigation("Cities");

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Shipping.Models.Merchant", b =>
                {
                    b.Navigation("BranchMerchants");

                    b.Navigation("Orders");

                    b.Navigation("SpecialShippingRates");
                });

            modelBuilder.Entity("Shipping.Models.Order", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("Shipping.Models.Permission", b =>
                {
                    b.Navigation("RolePermissions");
                });

            modelBuilder.Entity("Shipping.Models.RejectReason", b =>
                {
                    b.Navigation("ORders");
                });

            modelBuilder.Entity("Shipping.Models.Setting", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Shipping.Models.ShippingType", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("Shipping.Models.WeightPricing", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
