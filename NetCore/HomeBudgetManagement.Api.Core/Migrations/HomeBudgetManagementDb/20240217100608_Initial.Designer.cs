﻿// <auto-generated />
using System;
using HomeBudgetManagement.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HomeBudgetManagement.Api.Core.Migrations.HomeBudgetManagementDb
{
    [DbContext(typeof(HomeBudgetManagementDbContext))]
    [Migration("20240217100608_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("HomeBudgetManagement.Core.Domain.BudgetAggregate.Budget", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ItemType")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Budget");
                });

            modelBuilder.Entity("HomeBudgetManagement.Core.Domain.BudgetAggregate.FileAttachment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("BudgetId")
                        .HasColumnType("int");

                    b.Property<byte[]>("Content")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("FileExtension")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BudgetId")
                        .IsUnique();

                    b.ToTable("FileAttachment");
                });

            modelBuilder.Entity("HomeBudgetManagement.Core.Domain.BudgetAggregate.FileAttachment", b =>
                {
                    b.HasOne("HomeBudgetManagement.Core.Domain.BudgetAggregate.Budget", null)
                        .WithOne("FileAttachment")
                        .HasForeignKey("HomeBudgetManagement.Core.Domain.BudgetAggregate.FileAttachment", "BudgetId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("HomeBudgetManagement.Core.Domain.BudgetAggregate.Budget", b =>
                {
                    b.Navigation("FileAttachment");
                });
#pragma warning restore 612, 618
        }
    }
}