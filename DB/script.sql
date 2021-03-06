
CREATE DATABASE [CRUD_OperationForImageDB]
 
USE [CRUD_OperationForImageDB]

CREATE TABLE [dbo].[TableImage](
	[id] [int] NOT NULL,
	[Name] [varchar](50) NULL,
	[Picture] [image] NULL,
 CONSTRAINT [PK_TableImage] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

