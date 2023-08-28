CREATE TABLE [dbo].[Attachments] (
    [Id]            BIGINT          IDENTITY (1, 1) NOT NULL,
    [Content]       VARBINARY (MAX) NULL,
    [ContentLength] INT             NOT NULL,
    [ContentType]   NVARCHAR (MAX)  NULL,
    [CreatedAt]     DATETIME2 (7)   NULL,
    [CreatedById]   NVARCHAR (MAX)  NULL,
    [MessageId]     BIGINT          NOT NULL,
    [Name]          NVARCHAR (MAX)  NULL,
    [Url]           NVARCHAR (MAX)  NULL,
    CONSTRAINT [PK_Attachments] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Attachments_Messages_MessageId] FOREIGN KEY ([MessageId]) REFERENCES [dbo].[Messages] ([Id]) ON DELETE CASCADE
);






GO
CREATE NONCLUSTERED INDEX [IX_Attachments_MessageId]
    ON [dbo].[Attachments]([MessageId] ASC);

