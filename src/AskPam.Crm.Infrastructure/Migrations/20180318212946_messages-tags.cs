using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AskPam.Crm.Migrations
{
    public partial class messagestags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ChannelTypeId",
                table: "DeliveryStatus",
                newName: "Type");

            migrationBuilder.AddColumn<long>(
                name: "MessageId",
                table: "TagsRelations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TagsRelations_MessageId",
                table: "TagsRelations",
                column: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_TagsRelations_Messages_MessageId",
                table: "TagsRelations",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TagsRelations_Messages_MessageId",
                table: "TagsRelations");

            migrationBuilder.DropIndex(
                name: "IX_TagsRelations_MessageId",
                table: "TagsRelations");

            migrationBuilder.DropColumn(
                name: "MessageId",
                table: "TagsRelations");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "DeliveryStatus",
                newName: "ChannelTypeId");
        }
    }
}
