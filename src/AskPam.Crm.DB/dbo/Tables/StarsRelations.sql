CREATE TABLE [dbo].[StarsRelations] (
    [Id]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [ContactId]      BIGINT         NULL,
    [ConversationId] BIGINT         NULL,
    [CreatedAt]      DATETIME2 (7)  NULL,
    [CreatedById]    NVARCHAR (MAX) NULL,
    [UserId]         NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_StarsRelations] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_StarsRelations_Contacts_ContactId] FOREIGN KEY ([ContactId]) REFERENCES [dbo].[Contacts] ([Id]),
    CONSTRAINT [FK_StarsRelations_Conversations_ConversationId] FOREIGN KEY ([ConversationId]) REFERENCES [dbo].[Conversations] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_StarsRelations_ConversationId]
    ON [dbo].[StarsRelations]([ConversationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_StarsRelations_ContactId]
    ON [dbo].[StarsRelations]([ContactId] ASC);

