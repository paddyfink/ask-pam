using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AskPam.Crm.Migrations
{
    public partial class toto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channel_Contacts_ContactId",
                table: "Channel");

            migrationBuilder.DropIndex(
                name: "IX_Channel_ContactId",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "ContactId",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "OrganizationId",
                table: "Channel");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ContactId",
                table: "Channel",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OrganizationId",
                table: "Channel",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Channel_ContactId",
                table: "Channel",
                column: "ContactId");

            migrationBuilder.AddForeignKey(
                name: "FK_Channel_Contacts_ContactId",
                table: "Channel",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
