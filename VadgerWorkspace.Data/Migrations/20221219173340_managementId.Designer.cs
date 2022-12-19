﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VadgerWorkspace.Data;

#nullable disable

namespace VadgerWorkspace.Data.Migrations
{
    [DbContext(typeof(VadgerContext))]
    [Migration("20221219173340_managementId")]
    partial class managementId
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.0");

            modelBuilder.Entity("VadgerWorkspace.Data.Entities.Client", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long?>("EmployeeId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("Link")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Service")
                        .HasColumnType("TEXT");

                    b.Property<byte?>("Stage")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Town")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("VadgerWorkspace.Data.Entities.Employee", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("ClientId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsLocalAdmin")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("IsVerified")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("ManagementId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<byte>("Stage")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Town")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Employees");
                });

            modelBuilder.Entity("VadgerWorkspace.Data.Entities.SavedMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long?>("ClientId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("EmployeeId")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("IsFromClient")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Text")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("Time")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Messages");
                });
#pragma warning restore 612, 618
        }
    }
}
