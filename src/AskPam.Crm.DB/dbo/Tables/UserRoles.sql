CREATE TABLE [dbo].[UserRoles] (
    [Id]             BIGINT           IDENTITY (1, 1) NOT NULL,
    [OrganizationId] UNIQUEIDENTIFIER NULL,
    [RoleId]         BIGINT           NOT NULL,
    [UserId]         NVARCHAR (450)   NOT NULL,
    [Default]        BIT              DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserRoles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);








GO
CREATE NONCLUSTERED INDEX [IX_UserRoles_RoleId]
    ON [dbo].[UserRoles]([RoleId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UserRoles_UserId]
    ON [dbo].[UserRoles]([UserId] ASC);

