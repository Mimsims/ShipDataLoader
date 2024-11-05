USE [ShipDataDb]
GO

/****** Object: Table [dbo].[BoxesSpec] Script Date: 05.11.2024 16:36:56 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[BoxesSpec];


GO
CREATE TABLE [dbo].[BoxesSpec] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [BoxId]    CHAR (12)      NULL,
    [PoNumber] NVARCHAR (15)  NULL,
    [Isbn]     NVARCHAR (13)  NULL,
    [Quantity] DECIMAL (6, 2) NULL
);


