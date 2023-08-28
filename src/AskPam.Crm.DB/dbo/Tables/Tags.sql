CREATE TABLE [dbo].[Tags] (
    [Id]             BIGINT           IDENTITY (1, 1) NOT NULL,
    [Category]       NVARCHAR (MAX)   NULL,
    [CreatedAt]      DATETIME2 (7)    NULL,
    [CreatedById]    NVARCHAR (MAX)   NULL,
    [IsDeleted]      BIT              NOT NULL,
    [Name]           NVARCHAR (MAX)   NULL,
    [OrganizationId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_Tags] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Tags_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations] ([Id]) ON DELETE CASCADE
);






GO
CREATE NONCLUSTERED INDEX [IX_Tags_OrganizationId]
    ON [dbo].[Tags]([OrganizationId] ASC);

