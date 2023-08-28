using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AskPam.Crm.Migrations
{
    public partial class wathever : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AssignedToDate",
                table: "Contacts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssignedToId",
                table: "Contacts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_AssignedToId",
                table: "Contacts",
                column: "AssignedToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Users_AssignedToId",
                table: "Contacts",
                column: "AssignedToId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Users_AssignedToId",
                table: "Contacts");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_AssignedToId",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "AssignedToDate",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "AssignedToId",
                table: "Contacts");
        }
    }
}
