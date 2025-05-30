﻿// <auto-generated />
using System;
using ExamensArbete_BA_WIN23.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ExamensArbete_BA_WIN23.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20250505101204_init")]
    partial class init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ExamensArbete_BA_WIN23.Business.Dtos.ChangeRequestDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTimeOffset>("Created")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("Customer")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset?>("DateSentBV")
                        .HasColumnType("datetimeoffset");

                    b.Property<Guid?>("FirstApprover")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Region")
                        .HasColumnType("int");

                    b.Property<Guid?>("SecondApprover")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("Updated")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("isSignCompleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("ChangeRequest");
                });
#pragma warning restore 612, 618
        }
    }
}
