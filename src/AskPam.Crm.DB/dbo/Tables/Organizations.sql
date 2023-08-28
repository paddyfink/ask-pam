CREATE TABLE [dbo].[Organizations] (
    [Id]           UNIQUEIDENTIFIER NOT NULL,
    [BrainDates]   BIT              NOT NULL,
    [CreatedAt]    DATETIME2 (7)    NULL,
    [CreatedById]  NVARCHAR (MAX)   NULL,
    [DeletedAt]    DATETIME2 (7)    NULL,
    [DeletedById]  NVARCHAR (MAX)   NULL,
    [FullContact]  BIT              NOT NULL,
    [ImageUrl]     NVARCHAR (MAX)   NULL,
    [IsActive]     BIT              NOT NULL,
    [IsDeleted]    BIT              NOT NULL,
    [Klik]         BIT              NOT NULL,
    [ModifiedAt]   DATETIME2 (7)    NULL,
    [ModifiedById] NVARCHAR (MAX)   NULL,
    [Name]         NVARCHAR (MAX)   NULL,
    [Stay22]       BIT              NOT NULL,
    [Type]         INT              NULL,
    CONSTRAINT [PK_Organizations] PRIMARY KEY CLUSTERED ([Id] ASC)
);



























