CREATE TABLE [dbo].[Channels] (
    [Id]             BIGINT         IDENTITY (1, 1) NOT NULL,
    [Active]         BIT            NOT NULL,
    [AvatarUrl]      NVARCHAR (MAX) NULL,
    [DisplayName]    NVARCHAR (MAX) NULL,
    [LastSeen]       DATETIME2 (7)  NULL,
    [Primary]        BIT            NOT NULL,
    [Type]           NVARCHAR (MAX) NULL,
    [SmoochId]       NVARCHAR (MAX) NULL,
    [ConversationId] BIGINT         NULL,
    CONSTRAINT [PK_Channels] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Channels_Conversations_ConversationId] FOREIGN KEY ([ConversationId]) REFERENCES [dbo].[Conversations] ([Id])
);










GO
CREATE NONCLUSTERED INDEX [IX_Channels_ConversationId]
    ON [dbo].[Channels]([ConversationId] ASC);


GO



GO


