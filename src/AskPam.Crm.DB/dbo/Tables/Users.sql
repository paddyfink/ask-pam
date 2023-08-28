CREATE TABLE [dbo].[Users] (
    [Id]            NVARCHAR (450) NOT NULL,
    [CreatedAt]     DATETIME2 (7)  NULL,
    [CreatedById]   NVARCHAR (MAX) NULL,
    [DeletedAt]     DATETIME2 (7)  NULL,
    [DeletedById]   NVARCHAR (MAX) NULL,
    [Email]         NVARCHAR (MAX) NULL,
    [EmailVerified] BIT            NULL,
    [ExternalId]    NVARCHAR (MAX) NULL,
    [FirstName]     NVARCHAR (MAX) NULL,
    [FullName]      AS             (([FirstName]+' ')+[LastName]),
    [IsActive]      BIT            NOT NULL,
    [IsDeleted]     BIT            NOT NULL,
    [LastName]      NVARCHAR (MAX) NULL,
    [ModifiedAt]    DATETIME2 (7)  NULL,
    [ModifiedById]  NVARCHAR (MAX) NULL,
    [PhoneNumber]   NVARCHAR (MAX) NULL,
    [Picture]       NVARCHAR (MAX) NULL,
    [Signature]     NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC)
);













