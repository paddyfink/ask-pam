CREATE TABLE [dbo].[UserOrganizations] (
    [Id]             BIGINT           IDENTITY (1, 1) NOT NULL,
    [Default]        BIT              NOT NULL,
    [OrganizationId] UNIQUEIDENTIFIER NOT NULL,
    [UserId]         NVARCHAR (450)   NOT NULL,
    CONSTRAINT [PK_UserOrganizations] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_UserOrganizations_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserOrganizations_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
);








GO
CREATE NONCLUSTERED INDEX [IX_UserOrganizations_OrganizationId]
    ON [dbo].[UserOrganizations]([OrganizationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UserOrganizations_UserId]
    ON [dbo].[UserOrganizations]([UserId] ASC);

