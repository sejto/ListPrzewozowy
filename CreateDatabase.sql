create database ListPrzewozowy
go


CREATE TABLE [dbo].[List](
	[ID] int IDENTITY(1,1) primary key,
	[Data] [varchar](10) NOT NULL,
	[KontrId] [numeric](9,0) NOT NULL,
	[KontrNazwa] [varchar](120) NULL,
	[Paliwo] [varchar](45) NOT NULL,
	[Ilosc] [int] NOT NULL,
	[Cena][varchar](6) NULL,
	[FormaPlat][varchar](8) NULL,
	[Termin][varchar](6) NULL,
	[Sent][varchar](20) NULL,
	[DostUlica][varchar](40) NULL,
	[DostNr][varchar](8) NULL,
	[DostMiasto][varchar](40) NULL,
	[DostKod][varchar](10) NULL,
	[DostPoczta][varchar](40) NULL,
	[DostKraj][varchar](15) NULL,
	[DostPlanRozp][varchar](10) NULL,
	[DostRozp][varchar](10) NULL,
	[DostPlanZak][varchar](10) NULL,
	[Uwagi][varchar](250) NULL,
	[NrWZ][varchar](10) NOT NULL)



--drop table [List]
---------------------------------------------------------

--Temp Table: 
CREATE TABLE dbo.#Cars 
   ( 
   Car_id int NOT NULL, 
   ColorCode varchar(10), 
   ModelName varchar(20), 
   Code int, 
   DateEntered datetime 
   ) 

INSERT INTO dbo.#Cars (Car_id, ColorCode, ModelName, Code, DateEntered) 
VALUES (1,'BlueGreen', 'Austen', 200801, GETDATE()) 

SELECT Car_id, ColorCode, ModelName, Code, DateEntered FROM #Cars 

DROP TABLE dbo.[#Cars]
