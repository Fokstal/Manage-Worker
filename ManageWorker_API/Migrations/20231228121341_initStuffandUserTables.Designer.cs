﻿// <auto-generated />
using System;
using ManageWorker_API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ManageWorker_API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231228121341_initStuffandUserTables")]
    partial class initStuffandUserTables
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("ManageWorker_API.Models.Stuff", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Stuff");
                });

            modelBuilder.Entity("ManageWorker_API.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("User");
                });

            modelBuilder.Entity("ManageWorker_API.Models.Worker", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("StuffId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("StuffId");

                    b.ToTable("Worker");
                });

            modelBuilder.Entity("ManageWorker_API.Models.Worker", b =>
                {
                    b.HasOne("ManageWorker_API.Models.Stuff", "Stuff")
                        .WithMany("WorkerList")
                        .HasForeignKey("StuffId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Stuff");
                });

            modelBuilder.Entity("ManageWorker_API.Models.Stuff", b =>
                {
                    b.Navigation("WorkerList");
                });
#pragma warning restore 612, 618
        }
    }
}
