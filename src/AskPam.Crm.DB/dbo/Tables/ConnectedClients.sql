CREATE TABLE [dbo].[ConnectedClients] (
    [Id]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [ConnectionId] NVARCHAR (100) NULL,
    [LastActivity] DATETIME2 (7)  NOT NULL,
    [UserAgent]    NVARCHAR (MAX) NULL,
    [UserId]       NVARCHAR (450) NULL,
    CONSTRAINT [PK_ConnectedClients] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_ConnectedClients_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_ConnectedClients_UserId]
    ON [dbo].[ConnectedClients]([UserId] ASC);

