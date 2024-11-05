USE [ShipDataDb]
GO

/****** Object: Table [dbo].[Boxes] Script Date: 05.11.2024 16:36:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

DROP TABLE [dbo].[Boxes];


GO
CREATE TABLE [dbo].[Boxes] (
    [BoxId]      CHAR (12)     NOT NULL,
    [SupplierId] NVARCHAR (10) NULL
);


