CREATE TABLE [dbo].[Settings] (
    [Id]             BIGINT           IDENTITY (1, 1) NOT NULL,
    [CreatedAt]      DATETIME2 (7)    NULL,
    [CreatedById]    NVARCHAR (MAX)   NULL,
    [ModifiedAt]     DATETIME2 (7)    NULL,
    [ModifiedById]   NVARCHAR (MAX)   NULL,
    [Name]           NVARCHAR (256)   NOT NULL,
    [OrganizationId] UNIQUEIDENTIFIER NULL,
    [UserId]         NVARCHAR (450)   NULL,
    [Value]          NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_Settings] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Settings_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations] ([Id]),
    CONSTRAINT [FK_Settings_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id])
);






GO
CREATE NONCLUSTERED INDEX [IX_Settings_UserId]
    ON [dbo].[Settings]([UserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Settings_OrganizationId]
    ON [dbo].[Settings]([OrganizationId] ASC);

