CREATE TABLE [dbo].[Notifications] (
    [Id]               BIGINT           IDENTITY (1, 1) NOT NULL,
    [CreatedAt]        DATETIME2 (7)    NULL,
    [CreatedById]      NVARCHAR (MAX)   NULL,
    [CreatedUserId]    NVARCHAR (450)   NULL,
    [Data]             NVARCHAR (MAX)   NULL,
    [EntityId]         NVARCHAR (MAX)   NULL,
    [EntityType]       NVARCHAR (MAX)   NULL,
    [NotificationType] NVARCHAR (MAX)   NULL,
    [OrganizationId]   UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Notifications] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Notifications_Users_CreatedUserId] FOREIGN KEY ([CreatedUserId]) REFERENCES [dbo].[Users] ([Id])
);






GO
CREATE NONCLUSTERED INDEX [IX_Notifications_CreatedUserId]
    ON [dbo].[Notifications]([CreatedUserId] ASC);

