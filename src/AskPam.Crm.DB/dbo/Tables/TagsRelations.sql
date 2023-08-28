CREATE TABLE [dbo].[TagsRelations] (
    [Id]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [ContactId]      BIGINT         NULL,
    [ConversationId] BIGINT         NULL,
    [CreatedAt]      DATETIME2 (7)  NULL,
    [CreatedById]    NVARCHAR (MAX) NULL,
    [LibraryItemId]  BIGINT         NULL,
    [PostId]         BIGINT         NULL,
    [TagId]          BIGINT         NOT NULL,
    [MessageId]      BIGINT         NULL,
    CONSTRAINT [PK_TagsRelations] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_TagsRelations_Contacts_ContactId] FOREIGN KEY ([ContactId]) REFERENCES [dbo].[Contacts] ([Id]),
    CONSTRAINT [FK_TagsRelations_Conversations_ConversationId] FOREIGN KEY ([ConversationId]) REFERENCES [dbo].[Conversations] ([Id]),
    CONSTRAINT [FK_TagsRelations_LibraryItems_LibraryItemId] FOREIGN KEY ([LibraryItemId]) REFERENCES [dbo].[LibraryItems] ([Id]),
    CONSTRAINT [FK_TagsRelations_Messages_MessageId] FOREIGN KEY ([MessageId]) REFERENCES [dbo].[Messages] ([Id]),
    CONSTRAINT [FK_TagsRelations_Posts_PostId] FOREIGN KEY ([PostId]) REFERENCES [dbo].[Posts] ([Id]),
    CONSTRAINT [FK_TagsRelations_Tags_TagId] FOREIGN KEY ([TagId]) REFERENCES [dbo].[Tags] ([Id]) ON DELETE CASCADE
);










GO
CREATE NONCLUSTERED INDEX [IX_TagsRelations_TagId]
    ON [dbo].[TagsRelations]([TagId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TagsRelations_LibraryItemId]
    ON [dbo].[TagsRelations]([LibraryItemId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TagsRelations_ConversationId]
    ON [dbo].[TagsRelations]([ConversationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TagsRelations_ContactId]
    ON [dbo].[TagsRelations]([ContactId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TagsRelations_PostId]
    ON [dbo].[TagsRelations]([PostId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TagsRelations_MessageId]
    ON [dbo].[TagsRelations]([MessageId] ASC);

