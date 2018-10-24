USE [ListPrzewozowyLOKALNA]
GO

/****** Object:  View [dbo].[KontrahentNazwaView]    Script Date: 2018-07-09 13:06:38 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

Create view [dbo].[KontrahentNazwaView] as
select k.kontrid as ID, k.Nazwa, Ulica, Nrdomu, kod, miasto, Nip, Telefon, 
(Select top 1  case when tekst like '%pel%' then 'Pe³nomocnictwo' else 'Brak' end as Pelnomocnictwo From OTD.dbo.KontrOpis ko where KO.KontrId = K.KontrId and Znaczenie = 76) as Pelnomocnictwo,
(Select top 1  case when tekst like '%osw%' then 'Oswiadczenie' else 'Brak' end as Oswiadczenie From OTD.dbo.KontrOpis ko where KO.KontrId = K.KontrId and Znaczenie = 76) as Oswiadczenie 
from OTD.dbo.kontrahent k
GO

--=======================
USE [ListPrzewozowyLOKALNA]
GO

/****** Object:  View [dbo].[KontrahentNIPView]    Script Date: 2018-07-09 13:07:02 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create view [dbo].[KontrahentNIPView] as
select k.kontrid as ID,k.Nazwa, Ulica, Nrdomu, kod, miasto, Nip, Telefon, 
(Select top 1  case when tekst like '%pel%' then 'Pe³nomocnictwo' else 'Brak' end as Pelnomocnictwo From OTD.dbo.KontrOpis ko where KO.KontrId = K.KontrId and Znaczenie = 76) as Pelnomocnictwo,
(Select top 1  case when tekst like '%osw%' then 'Oswiadczenie' else 'Brak' end as Oswiadczenie From OTD.dbo.KontrOpis ko where KO.KontrId = K.KontrId and Znaczenie = 76) as Oswiadczenie 
from OTD.dbo.kontrahent k
GO

--========================
USE [ListPrzewozowyLOKALNA]
GO

/****** Object:  View [dbo].[ListyView]    Script Date: 2018-07-09 13:07:16 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create view [dbo].[ListyView] as
SELECT L.Dokid,D.Data,SUM(CASE WHEN paliwoid = 1 THEN ilosc END) IloscON,SUM(CASE WHEN paliwoid = 2 THEN ilosc END) IloscONA,
SUM(CASE WHEN paliwoid = 3 THEN ilosc END) IloscOP,count(nrwz) as Klienci,Nazwa as Wystawiajacy FROM list L inner join dok D on D.id=L.dokid 
inner join Uzytkownik U on U.id=D.Userid where L.aktywny=1 GROUP BY L.Dokid, D.data, U.nazwa
GO

--=========================

USE [ListPrzewozowyLOKALNA]
GO

/****** Object:  View [dbo].[WZView]    Script Date: 2018-07-09 13:07:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

create view [dbo].[WZView] as
select l.ID,L.dokid,d.data,K.Kontrid,K.Nazwa,K.Ulica,K.nrdomu,K.Kod, K.Miasto,K.Nip, K.telefon,P.PaliwoID,P.nazwa as Paliwo,L.Ilosc,L.Cena, L.FormaPlat, L.Termin, L.Sent, L.DostUlica,L.DostNr,L.DostMiasto,
L.DostKod,L.DostPoczta,L.DostKraj,L.DostPlanRozp,L.DostRozp,L.DostPlanZak,L.Uwagi,L.nrWZ from List L 
inner join Dok D on D.id = L.dokid inner join Uzytkownik U on U.id = D.userID inner join Paliwo P on P.paliwoID = L.paliwoid inner join OTD.dbo.kontrahent K
on k.kontrid = L.kontrID  where L.aktywny=1
GO

