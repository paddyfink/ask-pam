using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AskPam.Crm.Migrations
{
    public partial class watherver : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Conversations",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmoochUserId",
                table: "Conversations",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ConversationId",
                table: "Channel",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Channel_ConversationId",
                table: "Channel",
                column: "ConversationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Channel_Conversations_ConversationId",
                table: "Channel",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channel_Conversations_ConversationId",
                table: "Channel");

            migrationBuilder.DropIndex(
                name: "IX_Channel_ConversationId",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "SmoochUserId",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "Channel");
        }
    }
}
