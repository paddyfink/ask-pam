CREATE TABLE [dbo].[Contacts] (
    [Id]                           BIGINT           IDENTITY (1, 1) NOT NULL,
    [Gender]                       NVARCHAR (MAX)   NULL,
    [Bio]                          NVARCHAR (MAX)   NULL,
    [Company]                      NVARCHAR (MAX)   NULL,
    [CreatedAt]                    DATETIME2 (7)    NULL,
    [CreatedById]                  NVARCHAR (MAX)   NULL,
    [MaritalStatus]                NVARCHAR (MAX)   NULL,
    [DateOfBirth]                  DATETIME2 (7)    NULL,
    [DeletedAt]                    DATETIME2 (7)    NULL,
    [DeletedById]                  NVARCHAR (MAX)   NULL,
    [EmailAddress]                 NVARCHAR (MAX)   NULL,
    [ExternalId]                   NVARCHAR (MAX)   NULL,
    [FirstName]                    NVARCHAR (MAX)   NOT NULL,
    [FullName]                     AS               (([FirstName]+' ')+[LastName]),
    [GroupId]                      BIGINT           NULL,
    [IsDeleted]                    BIT              NOT NULL,
    [JobTitle]                     NVARCHAR (MAX)   NULL,
    [LastName]                     NVARCHAR (MAX)   NOT NULL,
    [ModifiedAt]                   DATETIME2 (7)    NULL,
    [ModifiedById]                 NVARCHAR (MAX)   NULL,
    [OrganizationId]               UNIQUEIDENTIFIER NOT NULL,
    [PrimaryLanguage]              NVARCHAR (MAX)   NULL,
    [SecondaryLanguage]            NVARCHAR (MAX)   NULL,
    [SmoochUserId]                 NVARCHAR (MAX)   NULL,
    [Address_Address1]             NVARCHAR (MAX)   NULL,
    [Address_Address2]             NVARCHAR (MAX)   NULL,
    [Address_City]                 NVARCHAR (MAX)   NULL,
    [Address_Country]              NVARCHAR (MAX)   NULL,
    [Address_PostalCode]           NVARCHAR (MAX)   NULL,
    [Address_Province]             NVARCHAR (MAX)   NULL,
    [BusinessPhone_CountryCode]    NVARCHAR (MAX)   NULL,
    [BusinessPhone_NationalFormat] NVARCHAR (MAX)   NULL,
    [BusinessPhone_Number]         NVARCHAR (MAX)    NULL,
    [MobilePhone_CountryCode]      NVARCHAR (MAX)   NULL,
    [MobilePhone_NationalFormat]   NVARCHAR (MAX)   NULL,
    [MobilePhone_Number]           NVARCHAR (MAX)    NULL,
    [Uid]                          UNIQUEIDENTIFIER DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [AssignedToDate]               DATETIME2 (7)    NULL,
    [AssignedToId]                 NVARCHAR (450)   NULL,
    [CustomFields]                 NVARCHAR (MAX)   NULL,
    [EmailAddress2]                NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_Contacts] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Contacts_ContactGroups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [dbo].[ContactGroups] ([Id]),
    CONSTRAINT [FK_Contacts_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Contacts_Users_AssignedToId] FOREIGN KEY ([AssignedToId]) REFERENCES [dbo].[Users] ([Id])
);








































GO
CREATE NONCLUSTERED INDEX [IX_Contacts_OrganizationId]
    ON [dbo].[Contacts]([OrganizationId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Contacts_GroupId]
    ON [dbo].[Contacts]([GroupId] ASC);


GO



GO
CREATE NONCLUSTERED INDEX [IX_Contacts_AssignedToId]
    ON [dbo].[Contacts]([AssignedToId] ASC);

