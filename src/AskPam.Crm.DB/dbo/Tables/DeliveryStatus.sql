CREATE TABLE [dbo].[DeliveryStatus] (
    [Id]           BIGINT         IDENTITY (1, 1) NOT NULL,
    [Date]         DATETIME2 (7)  NULL,
    [ErrorCode]    NVARCHAR (MAX) NULL,
    [ErrorMessage] NVARCHAR (MAX) NULL,
    [MessageId]    BIGINT         NOT NULL,
    [Open]         BIT            NOT NULL,
    [OpenDate]     DATETIME2 (7)  NULL,
    [Success]      BIT            NOT NULL,
    [Channel]         NVARCHAR (MAX) NULL,
    CONSTRAINT [PK_DeliveryStatus] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_DeliveryStatus_Messages_MessageId] FOREIGN KEY ([MessageId]) REFERENCES [dbo].[Messages] ([Id]) ON DELETE CASCADE
);




GO
CREATE NONCLUSTERED INDEX [IX_DeliveryStatus_MessageId]
    ON [dbo].[DeliveryStatus]([MessageId] ASC);

