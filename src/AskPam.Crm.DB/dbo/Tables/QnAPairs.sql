CREATE TABLE [dbo].[QnAPairs] (
    [Id]             BIGINT           IDENTITY (1, 1) NOT NULL,
    [Answer]         NVARCHAR (MAX)   NULL,
    [IsDeleted]      BIT              NOT NULL,
    [OrganizationId] UNIQUEIDENTIFIER NOT NULL,
    [Question]       NVARCHAR (MAX)   NULL,
    CONSTRAINT [PK_QnAPairs] PRIMARY KEY CLUSTERED ([Id] ASC)
);



