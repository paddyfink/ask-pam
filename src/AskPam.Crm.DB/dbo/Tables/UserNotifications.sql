CREATE TABLE [dbo].[UserNotifications] (
    [Id]             BIGINT           IDENTITY (1, 1) NOT NULL,
    [NotificationId] BIGINT           NOT NULL,
    [OrganizationId] UNIQUEIDENTIFIER NOT NULL,
    [Read]           BIT              NOT NULL,
    [Seen]           BIT              NOT NULL,
    [UserId]         NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_UserNotifications] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserNotifications_Notifications_NotificationId] FOREIGN KEY ([NotificationId]) REFERENCES [dbo].[Notifications] ([Id]) ON DELETE CASCADE
);






GO
CREATE NONCLUSTERED INDEX [IX_UserNotifications_NotificationId]
    ON [dbo].[UserNotifications]([NotificationId] ASC);

