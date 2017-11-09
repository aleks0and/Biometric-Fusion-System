if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tblImgData]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblImgData]
GO

CREATE TABLE [dbo].[tblImgData] (
	[ID] [int] NOT NULL ,
	[Name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Picture] [image] NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

