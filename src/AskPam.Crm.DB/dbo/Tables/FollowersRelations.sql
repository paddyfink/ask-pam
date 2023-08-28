CREATE TABLE [dbo].[FollowersRelations] (
    [Id]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [ConversationId] BIGINT         NULL,
    [CreatedAt]      DATETIME2 (7)  NULL,
    [CreatedById]    NVARCHAR (MAX) NULL,
    [PostId]         BIGINT         NULL,
    [UserId]         NVARCHAR (450) NULL,
    CONSTRAINT [PK_FollowersRelations] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_FollowersRelations_Conversations_ConversationId] FOREIGN KEY ([ConversationId]) REFERENCES [dbo].[Conversations] ([Id]),
    CONSTRAINT [FK_FollowersRelations_Posts_PostId] FOREIGN KEY ([PostId]) REFERENCES [dbo].[Posts] ([Id]),
    CONSTRAINT [FK_FollowersRelations_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);








GO
CREATE NONCLUSTERED INDEX [IX_FollowersRelations_UserId]
    ON [dbo].[FollowersRelations]([UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FollowersRelations_ConversationId]
    ON [dbo].[FollowersRelations]([ConversationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FollowersRelations_PostId]
    ON [dbo].[FollowersRelations]([PostId] ASC);

