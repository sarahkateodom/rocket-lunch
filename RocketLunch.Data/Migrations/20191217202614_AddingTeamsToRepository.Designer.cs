﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RocketLunch.data;

namespace RocketLunch.data.Migrations
{
    [DbContext(typeof(LunchContext))]
    [Migration("20191217202614_AddingTeamsToRepository")]
    partial class AddingTeamsToRepository
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("RocketLunch.data.entities.TeamEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .HasColumnName("name");

                    b.Property<string>("Zip")
                        .HasColumnName("zip");

                    b.HasKey("Id")
                        .HasName("pk_teams");

                    b.ToTable("teams");
                });

            modelBuilder.Entity("RocketLunch.data.entities.UserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Email")
                        .HasColumnName("email");

                    b.Property<string>("GoogleId")
                        .HasColumnName("google_id");

                    b.Property<string>("Name")
                        .HasColumnName("name");

                    b.Property<string>("Nopes")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("nopes")
                        .HasDefaultValue("[]");

                    b.Property<string>("PhotoUrl")
                        .HasColumnName("photo_url");

                    b.Property<string>("Zip")
                        .HasColumnName("zip");

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.ToTable("users");
                });

            modelBuilder.Entity("RocketLunch.data.entities.UserTeamEntity", b =>
                {
                    b.Property<int>("TeamId")
                        .HasColumnName("team_id");

                    b.Property<int>("UserId")
                        .HasColumnName("user_id");

                    b.HasKey("TeamId", "UserId")
                        .HasName("pk_user_teams");

                    b.HasIndex("UserId")
                        .HasName("ix_user_teams_user_id");

                    b.ToTable("user_teams");
                });

            modelBuilder.Entity("RocketLunch.data.entities.UserTeamEntity", b =>
                {
                    b.HasOne("RocketLunch.data.entities.TeamEntity", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .HasConstraintName("fk_user_teams_teams_team_id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RocketLunch.data.entities.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_user_teams_users_user_id")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}