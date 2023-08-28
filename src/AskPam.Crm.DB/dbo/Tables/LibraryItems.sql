CREATE TABLE [dbo].[LibraryItems] (
    [Id]               BIGINT           IDENTITY (1, 1) NOT NULL,
    [Address1]         NVARCHAR (MAX)   NULL,
    [Address2]         NVARCHAR (MAX)   NULL,
    [Area]             NVARCHAR (MAX)   NULL,
    [City]             NVARCHAR (MAX)   NULL,
    [Country]          NVARCHAR (MAX)   NULL,
    [CreatedAt]        DATETIME2 (7)    NULL,
    [CreatedById]      NVARCHAR (MAX)   NULL,
    [DeletedAt]        DATETIME2 (7)    NULL,
    [DeletedById]      NVARCHAR (MAX)   NULL,
    [Description]      NVARCHAR (MAX)   NULL,
    [Email]            NVARCHAR (MAX)   NULL,
    [EndDate]          DATETIME2 (7)    NULL,
    [Fax]              NVARCHAR (MAX)   NULL,
    [IsActive]         BIT              NOT NULL,
    [IsAllDay]         BIT              NOT NULL,
    [IsDeleted]        BIT              NOT NULL,
    [Menu]             NVARCHAR (MAX)   NULL,
    [ModifiedAt]       DATETIME2 (7)    NULL,
    [ModifiedById]     NVARCHAR (MAX)   NULL,
    [Name]             NVARCHAR (MAX)   NOT NULL,
    [NationalPhone]    NVARCHAR (MAX)   NULL,
    [OpeningHours]     NVARCHAR (MAX)   NULL,
    [OrganizationId]   UNIQUEIDENTIFIER NOT NULL,
    [Phone]            NVARCHAR (MAX)   NULL,
    [PhoneCountryCode] NVARCHAR (MAX)   NULL,
    [PostalCode]       NVARCHAR (MAX)   NULL,
    [Price]            NVARCHAR (MAX)   NULL,
    [Province]         NVARCHAR (MAX)   NULL,
    [StartDate]        DATETIME2 (7)    NULL,
    [Subject]          NVARCHAR (MAX)   NULL,
    [Type]             INT              NULL,
    [Website]          NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_LibraryItems] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_LibraryItems_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations] ([Id]) ON DELETE CASCADE
);


















GO
CREATE NONCLUSTERED INDEX [IX_LibraryItems_OrganizationId]
    ON [dbo].[LibraryItems]([OrganizationId] ASC);

