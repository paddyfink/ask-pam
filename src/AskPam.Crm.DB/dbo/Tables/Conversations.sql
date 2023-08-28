CREATE TABLE [dbo].[Conversations] (
    [Id]                       BIGINT           IDENTITY (1, 1) NOT NULL,
    [AssignedToId]             NVARCHAR (450)   NULL,
    [AvatarColor]              NVARCHAR (MAX)   NULL,
    [BotDisabled]              BIT              NOT NULL,
    [ContactId]                BIGINT           NULL,
    [CreatedAt]                DATETIME2 (7)    NULL,
    [CreatedById]              NVARCHAR (MAX)   NULL,
    [DeletedAt]                DATETIME2 (7)    NULL,
    [DeletedById]              NVARCHAR (MAX)   NULL,
    [IsActive]                 BIT              NOT NULL,
    [IsDeleted]                BIT              NOT NULL,
    [IsFlagged]                BIT              NOT NULL,
    [ModifiedAt]               DATETIME2 (7)    NULL,
    [ModifiedById]             NVARCHAR (MAX)   NULL,
    [Name]                     NVARCHAR (MAX)   NULL,
    [OrganizationId]           UNIQUEIDENTIFIER NOT NULL,
    [Seen]                     BIT              NOT NULL,
    [Uid]                      UNIQUEIDENTIFIER DEFAULT ('00000000-0000-0000-0000-000000000000') NOT NULL,
    [Email]                    NVARCHAR (MAX)   NULL,
    [SmoochUserId]             NVARCHAR (MAX)   NULL,
    [LastLocation_City]        NVARCHAR (MAX)   NULL,
    [LastLocation_Country]     NVARCHAR (MAX)   NULL,
    [LastLocation_CountryCode] NVARCHAR (MAX)   NULL,
    [LastLocation_Ip]          NVARCHAR (MAX)   NULL,
    [LastLocation_Latitude]    REAL             DEFAULT (CONVERT([real],(0))) NOT NULL,
    [LastLocation_Lontitude]   REAL             DEFAULT (CONVERT([real],(0))) NOT NULL,
    [LastLocation_Region]      NVARCHAR (MAX)   NULL,
    [LastLocation_RegionCode]  NVARCHAR (MAX)   NULL,
    [LastLocation_Zip]         NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_Conversations] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Conversations_Contacts_ContactId] FOREIGN KEY ([ContactId]) REFERENCES [dbo].[Contacts] ([Id]),
    CONSTRAINT [FK_Conversations_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_Conversations_Users_AssignedToId] FOREIGN KEY ([AssignedToId]) REFERENCES [dbo].[Users] ([Id])
);






















GO
CREATE NONCLUSTERED INDEX [IX_Conversations_OrganizationId]
    ON [dbo].[Conversations]([OrganizationId] ASC);


GO



GO
CREATE NONCLUSTERED INDEX [IX_Conversations_ContactId]
    ON [dbo].[Conversations]([ContactId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Conversations_AssignedToId]
    ON [dbo].[Conversations]([AssignedToId] ASC);

