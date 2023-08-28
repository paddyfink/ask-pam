CREATE TABLE [dbo].[Posts] (
    [Id]             BIGINT           IDENTITY (1, 1) NOT NULL,
    [CreatedAt]      DATETIME2 (7)    NULL,
    [CreatedById]    NVARCHAR (MAX)   NULL,
    [CreatedUserId]  NVARCHAR (450)   NULL,
    [DeletedAt]      DATETIME2 (7)    NULL,
    [DeletedById]    NVARCHAR (MAX)   NULL,
    [Description]    NVARCHAR (MAX)   NULL,
    [IsDeleted]      BIT              NOT NULL,
    [ModifiedAt]     DATETIME2 (7)    NULL,
    [ModifiedById]   NVARCHAR (MAX)   NULL,
    [OrganizationId] UNIQUEIDENTIFIER NOT NULL,
    [Title]          NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_Posts] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Posts_Users_CreatedUserId] FOREIGN KEY ([CreatedUserId]) REFERENCES [dbo].[Users] ([Id])
);




GO
CREATE NONCLUSTERED INDEX [IX_Posts_CreatedUserId]
    ON [dbo].[Posts]([CreatedUserId] ASC);

