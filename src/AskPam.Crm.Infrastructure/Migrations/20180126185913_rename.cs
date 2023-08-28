using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AskPam.Crm.Migrations
{
    public partial class rename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaritalStatusId",
                table: "Contacts",
                newName: "MaritalStatus");

            migrationBuilder.RenameColumn(
                name: "GenderId",
                table: "Contacts",
                newName: "Gender");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MaritalStatus",
                table: "Contacts",
                newName: "MaritalStatusId");

            migrationBuilder.RenameColumn(
                name: "Gender",
                table: "Contacts",
                newName: "GenderId");
        }
    }
}
