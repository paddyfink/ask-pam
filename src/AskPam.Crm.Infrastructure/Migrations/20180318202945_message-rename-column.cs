using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AskPam.Crm.Migrations
{
    public partial class messagerenamecolumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MessageTypeId",
                table: "Messages",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "MessageStatusId",
                table: "Messages",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "ChannelTypeId",
                table: "Messages",
                newName: "Channel");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Messages",
                newName: "MessageTypeId");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Messages",
                newName: "MessageStatusId");

            migrationBuilder.RenameColumn(
                name: "Channel",
                table: "Messages",
                newName: "ChannelTypeId");
        }
    }
}
