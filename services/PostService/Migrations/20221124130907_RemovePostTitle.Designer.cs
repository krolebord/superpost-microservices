﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PostService.Data;

#nullable disable

namespace PostService.Migrations
{
    [DbContext(typeof(PostsContext))]
    [Migration("20221124130907_RemovePostTitle")]
    partial class RemovePostTitle
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("PostService.Models.Post", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("ParentPostId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ParentPostId");

                    b.HasIndex("UserId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("PostService.Models.Post", b =>
                {
                    b.HasOne("PostService.Models.Post", "ParentPost")
                        .WithMany("SubPosts")
                        .HasForeignKey("ParentPostId");

                    b.Navigation("ParentPost");
                });

            modelBuilder.Entity("PostService.Models.Post", b =>
                {
                    b.Navigation("SubPosts");
                });
#pragma warning restore 612, 618
        }
    }
}
