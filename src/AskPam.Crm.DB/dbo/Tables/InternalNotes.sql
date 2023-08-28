CREATE TABLE [dbo].[InternalNotes] (
    [Id]             BIGINT           IDENTITY (1, 1) NOT NULL,
    [Comment]        NVARCHAR (MAX)   NULL,
    [ContactId]      INT              NULL,
    [ContactId1]     BIGINT           NULL,
    [CreatedAt]      DATETIME2 (7)    NULL,
    [CreatedById]    NVARCHAR (450)   NULL,
    [DeletedAt]      DATETIME2 (7)    NULL,
    [DeletedById]    NVARCHAR (MAX)   NULL,
    [IsDeleted]      BIT              NOT NULL,
    [LibraryId]      BIGINT           NULL,
    [LibraryItemId]  INT              NULL,
    [ModifiedAt]     DATETIME2 (7)    NULL,
    [ModifiedById]   NVARCHAR (MAX)   NULL,
    [OrganizationId] UNIQUEIDENTIFIER NOT NULL,
    [PostId]         INT              NULL,
    [PostId1]        BIGINT           NULL,
    CONSTRAINT [PK_InternalNotes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_InternalNotes_Contacts_ContactId1] FOREIGN KEY ([ContactId1]) REFERENCES [dbo].[Contacts] ([Id]),
    CONSTRAINT [FK_InternalNotes_LibraryItems_LibraryId] FOREIGN KEY ([LibraryId]) REFERENCES [dbo].[LibraryItems] ([Id]),
    CONSTRAINT [FK_InternalNotes_Organizations_OrganizationId] FOREIGN KEY ([OrganizationId]) REFERENCES [dbo].[Organizations] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_InternalNotes_Posts_PostId1] FOREIGN KEY ([PostId1]) REFERENCES [dbo].[Posts] ([Id]),
    CONSTRAINT [FK_InternalNotes_Users_CreatedById] FOREIGN KEY ([CreatedById]) REFERENCES [dbo].[Users] ([Id])
);






GO



GO



GO
CREATE NONCLUSTERED INDEX [IX_InternalNotes_OrganizationId]
    ON [dbo].[InternalNotes]([OrganizationId] ASC);


GO



GO
CREATE NONCLUSTERED INDEX [IX_InternalNotes_PostId1]
    ON [dbo].[InternalNotes]([PostId1] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_InternalNotes_LibraryId]
    ON [dbo].[InternalNotes]([LibraryId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_InternalNotes_ContactId1]
    ON [dbo].[InternalNotes]([ContactId1] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_InternalNotes_CreatedById]
    ON [dbo].[InternalNotes]([CreatedById] ASC);

