CREATE TABLE [dbo].[Messages] (
    [Bcc]              NVARCHAR (MAX)   NULL,
    [Cc]               NVARCHAR (MAX)   NULL,
    [From]             NVARCHAR (MAX)   NULL,
    [Header]           NVARCHAR (MAX)   NULL,
    [HtmlBody]         NVARCHAR (MAX)   NULL,
    [IsBodyHtml]       BIT              NULL,
    [IsReplied]        BIT              NULL,
    [TextBody]         NVARCHAR (MAX)   NULL,
    [Thread]           NVARCHAR (MAX)   NULL,
    [To]               NVARCHAR (MAX)   NULL,
    [Id]               BIGINT           IDENTITY (1, 1) NOT NULL,
    [Author]           NVARCHAR (MAX)   NULL,
    [AuthorId]         NVARCHAR (MAX)   NULL,
    [Avatar]           NVARCHAR (MAX)   NULL,
    [ConversationId]   BIGINT           NOT NULL,
    [CreatedUserId]    NVARCHAR (450)   NULL,
    [Date]             DATETIME2 (7)    NOT NULL,
    [Discriminator]    NVARCHAR (MAX)   NOT NULL,
    [IsAutomaticReply] BIT              NOT NULL,
    [IsDeleted]        BIT              NOT NULL,
    [PostmarkId]       UNIQUEIDENTIFIER NULL,
    [ReplyTo]          BIGINT           NULL,
    [Seen]             BIT              NULL,
    [SmoochMessageId]  NVARCHAR (MAX)   NULL,
    [Subject]          NVARCHAR (MAX)   NULL,
    [Text]             NVARCHAR (MAX)   NULL,
    [Channel]          NVARCHAR (MAX)   NULL,
    [Status]           NVARCHAR (MAX)   NULL,
    [Type]             NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_Messages] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Messages_Conversations_ConversationId] FOREIGN KEY ([ConversationId]) REFERENCES [dbo].[Conversations] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Messages_Users_CreatedUserId] FOREIGN KEY ([CreatedUserId]) REFERENCES [dbo].[Users] ([Id])
);




















GO



GO
CREATE NONCLUSTERED INDEX [IX_Messages_ConversationId]
    ON [dbo].[Messages]([ConversationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Messages_CreatedUserId]
    ON [dbo].[Messages]([CreatedUserId] ASC);

