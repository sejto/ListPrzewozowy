Create Database ListPrzewozowyLOKALNA;

USE [ListPrzewozowyLOKALNA]
GO

/****** Object:  Table [dbo].[Dok]    Script Date: 2018-07-09 12:46:39 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Dok](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Data] [varchar](10) NOT NULL,
	[UserID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

--================
USE [ListPrzewozowyLOKALNA]
GO

/****** Object:  Table [dbo].[List]    Script Date: 2018-07-09 12:46:54 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[List](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DokID] [int] NOT NULL,
	[KontrId] [numeric](9, 0) NOT NULL,
	[PaliwoID] [int] NOT NULL,
	[Ilosc] [int] NOT NULL,
	[Cena] [varchar](6) NULL,
	[FormaPlat] [varchar](8) NULL,
	[Termin] [int] NULL,
	[Sent] [varchar](20) NULL,
	[DostUlica] [varchar](40) NULL,
	[DostNr] [varchar](8) NULL,
	[DostMiasto] [varchar](40) NULL,
	[DostKod] [varchar](10) NULL,
	[DostPoczta] [varchar](40) NULL,
	[DostKraj] [varchar](15) NULL,
	[DostPlanRozp] [varchar](10) NULL,
	[DostRozp] [varchar](10) NULL,
	[DostPlanZak] [varchar](10) NULL,
	[Uwagi] [varchar](250) NULL,
	[NrWZ] [int] NOT NULL,
	[Aktywny] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

--=========================

USE [ListPrzewozowyLOKALNA]
GO

/****** Object:  Table [dbo].[Paliwo]    Script Date: 2018-07-09 12:47:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Paliwo](
	[PaliwoID] [int] IDENTITY(1,1) NOT NULL,
	[Nazwa] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[PaliwoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

--=================
USE [ListPrzewozowyLOKALNA]
GO

/****** Object:  Table [dbo].[PocztaSent]    Script Date: 2018-07-09 12:47:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[PocztaSent](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UUID] [varchar](40) NOT NULL,
	[Subject] [varchar](255) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO
--===========================
USE [ListPrzewozowyLOKALNA]
GO

/****** Object:  Table [dbo].[Uzytkownik]    Script Date: 2018-07-09 12:47:42 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Uzytkownik](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Nazwa] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

--===========E N D===============