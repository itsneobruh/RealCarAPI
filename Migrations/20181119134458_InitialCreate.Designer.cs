﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RealCarAPI.Models;

namespace RealCarAPI.Migrations
{
    [DbContext(typeof(RealCarAPIContext))]
    [Migration("20181119134458_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024");

            modelBuilder.Entity("RealCarAPI.Models.CarItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Cylinders");

                    b.Property<string>("Engine");

                    b.Property<string>("Height");

                    b.Property<string>("Tags");

                    b.Property<string>("Title");

                    b.Property<string>("Url");

                    b.Property<string>("Width");

                    b.HasKey("Id");

                    b.ToTable("CarItem");
                });
#pragma warning restore 612, 618
        }
    }
}
