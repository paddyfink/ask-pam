using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AskPam.Crm.Migrations
{
    public partial class geo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternalNotes_Users_CreatedUserId",
                table: "InternalNotes");

            migrationBuilder.DropIndex(
                name: "IX_InternalNotes_CreatedUserId",
                table: "InternalNotes");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "EmailName",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "IsDkimVerified",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "IsSpfVerified",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "PostmarkSenderId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "SmoochAppId",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "SmoochAppToken",
                table: "Organizations");

            migrationBuilder.DropColumn(
                name: "CreatedUserId",
                table: "InternalNotes");

            migrationBuilder.DropColumn(
                name: "ConversationStarted",
                table: "Contacts");

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "InternalNotes",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastLocation_City",
                table: "Conversations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastLocation_Country",
                table: "Conversations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastLocation_CountryCode",
                table: "Conversations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastLocation_Ip",
                table: "Conversations",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "LastLocation_Latitude",
                table: "Conversations",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "LastLocation_Lontitude",
                table: "Conversations",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "LastLocation_Region",
                table: "Conversations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastLocation_RegionCode",
                table: "Conversations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastLocation_Zip",
                table: "Conversations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_InternalNotes_CreatedById",
                table: "InternalNotes",
                column: "CreatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_InternalNotes_Users_CreatedById",
                table: "InternalNotes",
                column: "CreatedById",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InternalNotes_Users_CreatedById",
                table: "InternalNotes");

            migrationBuilder.DropIndex(
                name: "IX_InternalNotes_CreatedById",
                table: "InternalNotes");

            migrationBuilder.DropColumn(
                name: "LastLocation_City",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "LastLocation_Country",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "LastLocation_CountryCode",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "LastLocation_Ip",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "LastLocation_Latitude",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "LastLocation_Lontitude",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "LastLocation_Region",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "LastLocation_RegionCode",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "LastLocation_Zip",
                table: "Conversations");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailName",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDkimVerified",
                table: "Organizations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSpfVerified",
                table: "Organizations",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PostmarkSenderId",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmoochAppId",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmoochAppToken",
                table: "Organizations",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedById",
                table: "InternalNotes",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedUserId",
                table: "InternalNotes",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ConversationStarted",
                table: "Contacts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_InternalNotes_CreatedUserId",
                table: "InternalNotes",
                column: "CreatedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_InternalNotes_Users_CreatedUserId",
                table: "InternalNotes",
                column: "CreatedUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
