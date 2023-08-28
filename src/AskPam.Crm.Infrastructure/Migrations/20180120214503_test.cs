using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AskPam.Crm.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Channel_Integrations_IntegrationId",
                table: "Channel");

            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Users_AssignedToId",
                table: "Contacts");

            migrationBuilder.DropTable(
                name: "Integrations");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_AssignedToId",
                table: "Contacts");

            migrationBuilder.DropIndex(
                name: "IX_Channel_IntegrationId",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "ChannelType",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "MessageType",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "SmoochConversationId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "ChannelType",
                table: "DeliveryStatus");

            migrationBuilder.DropColumn(
                name: "AssignedToDate",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "MaritalStatus",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "IntegrationId",
                table: "Channel");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Channel");

            migrationBuilder.RenameColumn(
                name: "SmoochUserId",
                table: "Message",
                newName: "MessageTypeId");

            migrationBuilder.RenameColumn(
                name: "SmoochType",
                table: "Message",
                newName: "MessageStatusId");

            migrationBuilder.RenameColumn(
                name: "SmoochSource",
                table: "Message",
                newName: "ChannelTypeId");

            migrationBuilder.RenameColumn(
                name: "Data",
                table: "Contacts",
                newName: "MaritalStatusId");

            migrationBuilder.RenameColumn(
                name: "AssignedToId",
                table: "Contacts",
                newName: "GenderId");

            migrationBuilder.RenameColumn(
                name: "Recipient",
                table: "Channel",
                newName: "TypeId");

            migrationBuilder.AddColumn<string>(
                name: "ChannelTypeId",
                table: "DeliveryStatus",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Uid",
                table: "Conversations",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "GenderId",
                table: "Contacts",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ConversationStarted",
                table: "Contacts",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "Uid",
                table: "Contacts",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChannelTypeId",
                table: "DeliveryStatus");

            migrationBuilder.DropColumn(
                name: "Uid",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "ConversationStarted",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "Uid",
                table: "Contacts");

            migrationBuilder.RenameColumn(
                name: "MessageTypeId",
                table: "Message",
                newName: "SmoochUserId");

            migrationBuilder.RenameColumn(
                name: "MessageStatusId",
                table: "Message",
                newName: "SmoochType");

            migrationBuilder.RenameColumn(
                name: "ChannelTypeId",
                table: "Message",
                newName: "SmoochSource");

            migrationBuilder.RenameColumn(
                name: "MaritalStatusId",
                table: "Contacts",
                newName: "Data");

            migrationBuilder.RenameColumn(
                name: "GenderId",
                table: "Contacts",
                newName: "AssignedToId");

            migrationBuilder.RenameColumn(
                name: "TypeId",
                table: "Channel",
                newName: "Recipient");

            migrationBuilder.AddColumn<int>(
                name: "ChannelType",
                table: "Message",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MessageType",
                table: "Message",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Message",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SmoochConversationId",
                table: "Message",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChannelType",
                table: "DeliveryStatus",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "AssignedToId",
                table: "Contacts",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "AssignedToDate",
                table: "Contacts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "Contacts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaritalStatus",
                table: "Contacts",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "IntegrationId",
                table: "Channel",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Channel",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Integrations",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BroidId = table.Column<string>(nullable: true),
                    ChannelType = table.Column<int>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: true),
                    CreatedById = table.Column<string>(nullable: true),
                    DeletedAt = table.Column<DateTime>(nullable: true),
                    DeletedById = table.Column<string>(nullable: true),
                    IsDeleted = table.Column<bool>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: true),
                    ModifiedById = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    OrganizationId = table.Column<Guid>(nullable: false),
                    PostmarkIsDkimVerified = table.Column<bool>(nullable: false),
                    PostmarkIsSpfVerified = table.Column<bool>(nullable: false),
                    PostmarkSenderId = table.Column<int>(nullable: false),
                    Secret = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    Token = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Integrations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Integrations_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_AssignedToId",
                table: "Contacts",
                column: "AssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_Channel_IntegrationId",
                table: "Channel",
                column: "IntegrationId");

            migrationBuilder.CreateIndex(
                name: "IX_Integrations_OrganizationId",
                table: "Integrations",
                column: "OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Channel_Integrations_IntegrationId",
                table: "Channel",
                column: "IntegrationId",
                principalTable: "Integrations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Users_AssignedToId",
                table: "Contacts",
                column: "AssignedToId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
