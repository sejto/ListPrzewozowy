create database ListPrzewozowy
go


CREATE TABLE [dbo].[Dok](
	[ID] int IDENTITY(1,1) primary key,
	[Data] [varchar](10) NOT NULL)

------------------
CREATE TABLE [dbo].[Paliwo](
	[PaliwoID] [int] NOT NULL,
	[PaliwoNazwa][varchar](30))
-----------
CREATE TABLE [dbo].[PocztaSent](
	[ID] int IDENTITY(1,1) primary key,
	[UUID] [varchar](40) NOT NULL,
	[Subject] [varchar](255) NULL)
-------------

CREATE TABLE [dbo].[List](
	[ID] int IDENTITY(1,1) primary key,
	[KontrId] [numeric](9,0) NOT NULL,
	[PaliwoID] [int] NOT NULL,
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
	[NrWZ][varchar](10) NOT NULL,
	[SentNumber][varchar](20) NULL,
	[SenderKey][varchar](8) NULL,
	[RecipientKey][varchar](8) NULL,
	[CarrierKey][varchar](8) NULL,
	[klucz] [uniqueidentifier] NULL)

ALTER TABLE List ADD 
ZalUlica [varchar](40) NULL,
ZalNr [varchar](8) NULL,
ZalMiasto [varchar](40) NULL,
ZalKod [varchar](10) NULL
-----------
ALTER view WZView as
select l.ID,L.dokid,d.data,K.Kontrid,K.Nazwa,K.Ulica,K.nrdomu,K.Kod, K.Miasto,K.Nip, K.telefon,P.PaliwoID,P.nazwa as Paliwo,L.Ilosc,L.Cena, L.FormaPlat, L.Termin, L.Sent, L.DostUlica,L.DostNr,L.DostMiasto,
L.DostKod,L.DostPoczta,L.DostKraj,L.DostPlanRozp,L.DostRozp,L.DostPlanZak,L.Uwagi,L.nrWZ,L.ZalUlica, L.ZalNr, L.ZalMiasto, L.ZalKod, L.SentNumber, L.SenderKey, L.RecipientKey, L.CarrierKey from List L 
inner join Dok D on D.id = L.dokid inner join Uzytkownik U on U.id = D.userID inner join Paliwo P on P.paliwoID = L.paliwoid inner join OTD.dbo.kontrahent K
on k.kontrid = L.kontrID  where L.aktywny=1


ADD[SentNumber][varchar](20) NULL

--drop table [List]
--drop table [Dok]
--drop table [Paliwo]



select * from List

select * from WZView



select * from WZView