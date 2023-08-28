using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace AskPam.Crm.Migrations
{
    public partial class renametable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Message_MessageId",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Channel_Conversations_ConversationId",
                table: "Channel");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryStatus_Message_MessageId",
                table: "DeliveryStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowersRelation_Conversations_ConversationId",
                table: "FollowersRelation");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowersRelation_Posts_PostId",
                table: "FollowersRelation");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowersRelation_Users_UserId",
                table: "FollowersRelation");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Conversations_ConversationId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_Message_Users_CreatedUserId",
                table: "Message");

            migrationBuilder.DropForeignKey(
                name: "FK_StarsRelation_Contacts_ContactId",
                table: "StarsRelation");

            migrationBuilder.DropForeignKey(
                name: "FK_StarsRelation_Conversations_ConversationId",
                table: "StarsRelation");

            migrationBuilder.DropForeignKey(
                name: "FK_TagsRelation_Contacts_ContactId",
                table: "TagsRelation");

            migrationBuilder.DropForeignKey(
                name: "FK_TagsRelation_Conversations_ConversationId",
                table: "TagsRelation");

            migrationBuilder.DropForeignKey(
                name: "FK_TagsRelation_LibraryItems_LibraryItemId",
                table: "TagsRelation");

            migrationBuilder.DropForeignKey(
                name: "FK_TagsRelation_Posts_PostId",
                table: "TagsRelation");

            migrationBuilder.DropForeignKey(
                name: "FK_TagsRelation_Tags_TagId",
                table: "TagsRelation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TagsRelation",
                table: "TagsRelation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StarsRelation",
                table: "StarsRelation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Message",
                table: "Message");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FollowersRelation",
                table: "FollowersRelation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Channel",
                table: "Channel");

            migrationBuilder.RenameTable(
                name: "TagsRelation",
                newName: "TagsRelations");

            migrationBuilder.RenameTable(
                name: "StarsRelation",
                newName: "StarsRelations");

            migrationBuilder.RenameTable(
                name: "Message",
                newName: "Messages");

            migrationBuilder.RenameTable(
                name: "FollowersRelation",
                newName: "FollowersRelations");

            migrationBuilder.RenameTable(
                name: "Channel",
                newName: "Channels");

            migrationBuilder.RenameIndex(
                name: "IX_TagsRelation_TagId",
                table: "TagsRelations",
                newName: "IX_TagsRelations_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_TagsRelation_PostId",
                table: "TagsRelations",
                newName: "IX_TagsRelations_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_TagsRelation_LibraryItemId",
                table: "TagsRelations",
                newName: "IX_TagsRelations_LibraryItemId");

            migrationBuilder.RenameIndex(
                name: "IX_TagsRelation_ConversationId",
                table: "TagsRelations",
                newName: "IX_TagsRelations_ConversationId");

            migrationBuilder.RenameIndex(
                name: "IX_TagsRelation_ContactId",
                table: "TagsRelations",
                newName: "IX_TagsRelations_ContactId");

            migrationBuilder.RenameIndex(
                name: "IX_StarsRelation_ConversationId",
                table: "StarsRelations",
                newName: "IX_StarsRelations_ConversationId");

            migrationBuilder.RenameIndex(
                name: "IX_StarsRelation_ContactId",
                table: "StarsRelations",
                newName: "IX_StarsRelations_ContactId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_CreatedUserId",
                table: "Messages",
                newName: "IX_Messages_CreatedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Message_ConversationId",
                table: "Messages",
                newName: "IX_Messages_ConversationId");

            migrationBuilder.RenameIndex(
                name: "IX_FollowersRelation_UserId",
                table: "FollowersRelations",
                newName: "IX_FollowersRelations_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FollowersRelation_PostId",
                table: "FollowersRelations",
                newName: "IX_FollowersRelations_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_FollowersRelation_ConversationId",
                table: "FollowersRelations",
                newName: "IX_FollowersRelations_ConversationId");

            migrationBuilder.RenameIndex(
                name: "IX_Channel_ConversationId",
                table: "Channels",
                newName: "IX_Channels_ConversationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TagsRelations",
                table: "TagsRelations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StarsRelations",
                table: "StarsRelations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Messages",
                table: "Messages",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FollowersRelations",
                table: "FollowersRelations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Channels",
                table: "Channels",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Messages_MessageId",
                table: "Attachments",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Channels_Conversations_ConversationId",
                table: "Channels",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryStatus_Messages_MessageId",
                table: "DeliveryStatus",
                column: "MessageId",
                principalTable: "Messages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FollowersRelations_Conversations_ConversationId",
                table: "FollowersRelations",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FollowersRelations_Posts_PostId",
                table: "FollowersRelations",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FollowersRelations_Users_UserId",
                table: "FollowersRelations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Conversations_ConversationId",
                table: "Messages",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_CreatedUserId",
                table: "Messages",
                column: "CreatedUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StarsRelations_Contacts_ContactId",
                table: "StarsRelations",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StarsRelations_Conversations_ConversationId",
                table: "StarsRelations",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TagsRelations_Contacts_ContactId",
                table: "TagsRelations",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TagsRelations_Conversations_ConversationId",
                table: "TagsRelations",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TagsRelations_LibraryItems_LibraryItemId",
                table: "TagsRelations",
                column: "LibraryItemId",
                principalTable: "LibraryItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TagsRelations_Posts_PostId",
                table: "TagsRelations",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TagsRelations_Tags_TagId",
                table: "TagsRelations",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Messages_MessageId",
                table: "Attachments");

            migrationBuilder.DropForeignKey(
                name: "FK_Channels_Conversations_ConversationId",
                table: "Channels");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryStatus_Messages_MessageId",
                table: "DeliveryStatus");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowersRelations_Conversations_ConversationId",
                table: "FollowersRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowersRelations_Posts_PostId",
                table: "FollowersRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_FollowersRelations_Users_UserId",
                table: "FollowersRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Conversations_ConversationId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_CreatedUserId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_StarsRelations_Contacts_ContactId",
                table: "StarsRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_StarsRelations_Conversations_ConversationId",
                table: "StarsRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_TagsRelations_Contacts_ContactId",
                table: "TagsRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_TagsRelations_Conversations_ConversationId",
                table: "TagsRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_TagsRelations_LibraryItems_LibraryItemId",
                table: "TagsRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_TagsRelations_Posts_PostId",
                table: "TagsRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_TagsRelations_Tags_TagId",
                table: "TagsRelations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TagsRelations",
                table: "TagsRelations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StarsRelations",
                table: "StarsRelations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Messages",
                table: "Messages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FollowersRelations",
                table: "FollowersRelations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Channels",
                table: "Channels");

            migrationBuilder.RenameTable(
                name: "TagsRelations",
                newName: "TagsRelation");

            migrationBuilder.RenameTable(
                name: "StarsRelations",
                newName: "StarsRelation");

            migrationBuilder.RenameTable(
                name: "Messages",
                newName: "Message");

            migrationBuilder.RenameTable(
                name: "FollowersRelations",
                newName: "FollowersRelation");

            migrationBuilder.RenameTable(
                name: "Channels",
                newName: "Channel");

            migrationBuilder.RenameIndex(
                name: "IX_TagsRelations_TagId",
                table: "TagsRelation",
                newName: "IX_TagsRelation_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_TagsRelations_PostId",
                table: "TagsRelation",
                newName: "IX_TagsRelation_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_TagsRelations_LibraryItemId",
                table: "TagsRelation",
                newName: "IX_TagsRelation_LibraryItemId");

            migrationBuilder.RenameIndex(
                name: "IX_TagsRelations_ConversationId",
                table: "TagsRelation",
                newName: "IX_TagsRelation_ConversationId");

            migrationBuilder.RenameIndex(
                name: "IX_TagsRelations_ContactId",
                table: "TagsRelation",
                newName: "IX_TagsRelation_ContactId");

            migrationBuilder.RenameIndex(
                name: "IX_StarsRelations_ConversationId",
                table: "StarsRelation",
                newName: "IX_StarsRelation_ConversationId");

            migrationBuilder.RenameIndex(
                name: "IX_StarsRelations_ContactId",
                table: "StarsRelation",
                newName: "IX_StarsRelation_ContactId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_CreatedUserId",
                table: "Message",
                newName: "IX_Message_CreatedUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_ConversationId",
                table: "Message",
                newName: "IX_Message_ConversationId");

            migrationBuilder.RenameIndex(
                name: "IX_FollowersRelations_UserId",
                table: "FollowersRelation",
                newName: "IX_FollowersRelation_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_FollowersRelations_PostId",
                table: "FollowersRelation",
                newName: "IX_FollowersRelation_PostId");

            migrationBuilder.RenameIndex(
                name: "IX_FollowersRelations_ConversationId",
                table: "FollowersRelation",
                newName: "IX_FollowersRelation_ConversationId");

            migrationBuilder.RenameIndex(
                name: "IX_Channels_ConversationId",
                table: "Channel",
                newName: "IX_Channel_ConversationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TagsRelation",
                table: "TagsRelation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StarsRelation",
                table: "StarsRelation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Message",
                table: "Message",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FollowersRelation",
                table: "FollowersRelation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Channel",
                table: "Channel",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Message_MessageId",
                table: "Attachments",
                column: "MessageId",
                principalTable: "Message",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Channel_Conversations_ConversationId",
                table: "Channel",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryStatus_Message_MessageId",
                table: "DeliveryStatus",
                column: "MessageId",
                principalTable: "Message",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FollowersRelation_Conversations_ConversationId",
                table: "FollowersRelation",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FollowersRelation_Posts_PostId",
                table: "FollowersRelation",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FollowersRelation_Users_UserId",
                table: "FollowersRelation",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Conversations_ConversationId",
                table: "Message",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Users_CreatedUserId",
                table: "Message",
                column: "CreatedUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StarsRelation_Contacts_ContactId",
                table: "StarsRelation",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StarsRelation_Conversations_ConversationId",
                table: "StarsRelation",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TagsRelation_Contacts_ContactId",
                table: "TagsRelation",
                column: "ContactId",
                principalTable: "Contacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TagsRelation_Conversations_ConversationId",
                table: "TagsRelation",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TagsRelation_LibraryItems_LibraryItemId",
                table: "TagsRelation",
                column: "LibraryItemId",
                principalTable: "LibraryItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TagsRelation_Posts_PostId",
                table: "TagsRelation",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TagsRelation_Tags_TagId",
                table: "TagsRelation",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
