CREATE TABLE [dbo].[ContactGroups] (
    [Id]             BIGINT           IDENTITY (1, 1) NOT NULL,
    [CreatedAt]      DATETIME2 (7)    NULL,
    [CreatedById]    NVARCHAR (MAX)   NULL,
    [DeletedAt]      DATETIME2 (7)    NULL,
    [DeletedById]    NVARCHAR (MAX)   NULL,
    [IsDeleted]      BIT              NOT NULL,
    [ModifiedAt]     DATETIME2 (7)    NULL,
    [ModifiedById]   NVARCHAR (MAX)   NULL,
    [Name]           NVARCHAR (50)    NOT NULL,
    [OrganizationId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [PK_ContactGroups] PRIMARY KEY CLUSTERED ([Id] ASC)
);



