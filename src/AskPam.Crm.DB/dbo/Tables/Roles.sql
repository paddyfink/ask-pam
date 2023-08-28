CREATE TABLE [dbo].[Roles] (
    [Id]             BIGINT           IDENTITY (1, 1) NOT NULL,
    [DisplayName]    NVARCHAR (64)    NOT NULL,
    [IsDefault]      BIT              NOT NULL,
    [IsStatic]       BIT              NOT NULL,
    [Name]           NVARCHAR (MAX)   NULL,
    [OrganizationId] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [PK_Roles] PRIMARY KEY CLUSTERED ([Id] ASC)
);



