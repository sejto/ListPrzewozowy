create view KontrahentNIPView as
select k.kontrid as ID,k.Nazwa, Ulica, Nrdomu, kod, miasto, Nip, Telefon, 
(Select top 1  case when tekst like '%pel%' then 'Pełnomocnictwo' else 'Brak' end as Pelnomocnictwo From OTD.dbo.KontrOpis ko where KO.KontrId = K.KontrId and Znaczenie = 76) as Pelnomocnictwo,
(Select top 1  case when tekst like '%osw%' then 'Oswiadczenie' else 'Brak' end as Oswiadczenie From OTD.dbo.KontrOpis ko where KO.KontrId = K.KontrId and Znaczenie = 76) as Oswiadczenie 
from OTD.dbo.kontrahent k 
----------------------------------------------
Create view KontrahentNazwaView as
select k.kontrid as ID, k.Nazwa, Ulica, Nrdomu, kod, miasto, Nip, Telefon, 
(Select top 1  case when tekst like '%pel%' then 'Pełnomocnictwo' else 'Brak' end as Pelnomocnictwo From OTD.dbo.KontrOpis ko where KO.KontrId = K.KontrId and Znaczenie = 76) as Pelnomocnictwo,
(Select top 1  case when tekst like '%osw%' then 'Oswiadczenie' else 'Brak' end as Oswiadczenie From OTD.dbo.KontrOpis ko where KO.KontrId = K.KontrId and Znaczenie = 76) as Oswiadczenie 
from OTD.dbo.kontrahent k
-----------------------------------------------
create view ListyView as
SELECT L.Dokid,D.Data,SUM(CASE WHEN paliwoid = 1 THEN ilosc END) IloscON,SUM(CASE WHEN paliwoid = 2 THEN ilosc END) IloscONA,
SUM(CASE WHEN paliwoid = 3 THEN ilosc END) IloscOP,count(nrwz) as Klienci,Nazwa as Wystawiajacy FROM list L inner join dok D on D.id=L.dokid 
inner join Uzytkownik U on U.id=D.Userid where L.aktywny=1 GROUP BY L.Dokid, D.data, U.nazwa 
--------------------------------------------------------
create view WZView as
select l.ID,L.dokid,d.data,K.Kontrid,K.Nazwa,K.Ulica,K.nrdomu,K.Kod, K.Miasto,K.Nip, K.telefon,P.PaliwoID,P.nazwa as Paliwo,L.Ilosc,L.Cena, L.FormaPlat, L.Termin, L.Sent, L.DostUlica,L.DostNr,L.DostMiasto,
L.DostKod,L.DostPoczta,L.DostKraj,L.DostPlanRozp,L.DostRozp,L.DostPlanZak,L.Uwagi,L.nrWZ from List L 
inner join Dok D on D.id = L.dokid inner join Uzytkownik U on U.id = D.userID inner join Paliwo P on P.paliwoID = L.paliwoid inner join OTD.dbo.kontrahent K
on k.kontrid = L.kontrID  where L.aktywny=1
----------------------------
select * from WZView where dokid=55

select * from ListyView where dokid>55
-----------------------------------------------------
--- Wyświetlenie wszystkich kontrahentów z listu przewozowego
select d.ID as Nr_listu, K.Nazwa, k.Ulica, k.Nrdomu, k.Nrlokalu,k.Kod,k.Miasto,p.nazwa as Paliwo, l.Ilosc, l.Cena, l.Formaplat,l.Termin, l.Sent,U.Nazwa as Wystawił from dok D
inner join List L
on L.dokID=D.ID
inner join OTD.dbo.kontrahent K
on k.kontrid=L.kontrID
inner join paliwo P
on p.paliwoID=L.paliwoID
inner join Uzytkownik U
on D.userId=U.id
------------------------------
select d.ID as Nr_listu,d.Data, count(K.Nazwa)as Kontrahentów,p.nazwa as Paliwo, sum(l.Ilosc) as Litrów from dok D
inner join List L
on L.dokID=D.ID
inner join OTD.dbo.kontrahent K
on k.kontrid=L.kontrID
inner join paliwo P
on p.paliwoID=L.paliwoID
group by d.ID, d.Data,p.nazwa


-------------test3-------------------
select * from 
(select dokid, sum(ilosc) as Ilosc, paliwoid from list where paliwoid=1 group by dokid, paliwoid) as  _ON,
(select dokid, sum(ilosc) as Ilosc, paliwoid from list where paliwoid=2 group by id,dokid, paliwoid) as _ONA
----
select * from (select dokid, sum(_ON.ilosc),
(select dokid, sum(ilosc) as Ilosc, paliwoid from list where paliwoid=1 group by id,dokid, paliwoid) as  _ON,
(select dokid, sum(ilosc) as Ilosc, paliwoid from list where paliwoid=2 group by id,dokid, paliwoid) as _ONA,
(select dokid, sum(ilosc) as Ilosc, paliwoid from list where paliwoid=3 group by id,dokid, paliwoid) as _OP
from k
inner join List L
on L.dokID=D.ID
)k

---
select * from 
(select dokid, sum(ilosc) as Ilosc, paliwoid from list where paliwoid=1 group by id,dokid, paliwoid) as  _ON
inner join
(select dokid, sum(ilosc) as Ilosc, paliwoid from list where paliwoid=2 group by id,dokid, paliwoid) as _ONA
on id._ON=id._ONA
------------*********-----------------
select _ON.id, IloscON, IloscONA from
(select id,sum(ilosc) as IloscON from list where paliwoid=1 group by id) as _ON
full outer join
(select id,sum(ilosc) as IloscONA from list where paliwoid=2 group by id) as _ONA
on _ON.id=_ONA.id
-----------------------------------------
select _ON.dokid, IloscON, IloscONA, IloscOP from
(select id,dokid,sum(ilosc) as IloscON from list where paliwoid=1 group by id,dokid) as _ON
full outer join
(select id,dokid,sum(ilosc) as IloscONA from list where paliwoid=2 group by id,dokid) as _ONA
on _ON.id=_ONA.id
full outer join
(select id,dokid,sum(ilosc) as IloscOP from list where paliwoid=3 group by id,dokid) as _OP
on _ON.id=_OP.id
------------------------------------------------------
select e1.dokid, e1.paliwoid, e1.ilosc as IloscON,e2.ilosc as IloscONA
from list e1 left outer join list e2 on e1.id=e2.id
------------------------------------------------------
select * from(
select e1.dokid, e1.paliwoid, e1.ilosc as IloscON,e2.ilosc as IloscONA
from list e1 left outer join list e2 on e1.id=e2.id)k
where paliwoid=1
***********************************************
SELECT  Dokid as nrWZ,
        SUM(CASE WHEN paliwoid = 1 THEN ilosc END) iloscON,
        SUM(CASE WHEN paliwoid = 2 THEN ilosc END) iloscONA,
        SUM(CASE WHEN paliwoid = 3 THEN ilosc END) iloscOP
FROM list
GROUP BY Dokid;
***********************************************
SELECT L.Dokid,D.Data,
        SUM(CASE WHEN paliwoid = 1 THEN ilosc END) iloscON,
        SUM(CASE WHEN paliwoid = 2 THEN ilosc END) iloscONA,
        SUM(CASE WHEN paliwoid = 3 THEN ilosc END) iloscOP,
Nazwa as Wystawiajacy
FROM list L
inner join dok D
on D.id=L.dokid
inner join Uzytkownik U
on U.id=D.Userid
GROUP BY L.Dokid, D.data, U.nazwa;
---------------wczytywanie do zmiennej----------------------
select d.data,K.Nazwa,K.Ulica,K.nrdomu, K.Miasto, K.Nip, K.telefon,P.nazwa,L.Ilosc,L.Cena, L.FormaPlat, L.Termin, L.Sent, L.DostUlica,L.DostNr from List L
inner join Dok D
on D.id=L.dokid
inner join Uzytkownik U
on U.id=D.userID
inner join Paliwo P
on P.paliwoID=L.paliwoid
inner join OTD.dbo.kontrahent K
on k.kontrid=L.kontrID
 where dokid=1
------------###############----------------


CREATE TABLE [dbo].[Dok](
	[ID] int IDENTITY(1,1) primary key,
	[Data] [varchar](10) NOT NULL,
	[UserID]int NOT NULL)

------------------
CREATE TABLE [dbo].[Paliwo](
	[PaliwoID]  int IDENTITY(1,1) primary key,
	[Nazwa][varchar](50))
-----------
CREATE TABLE [dbo].[Uzytkownik](
	[ID]  int IDENTITY(1,1) primary key,
	[Nazwa][varchar](50))
------------

CREATE TABLE [dbo].[List](
	[ID] int IDENTITY(1,1) primary key,
	[DokID]int NOT NULL,
	[KontrId] [numeric](9,0) NOT NULL,
	[PaliwoID] [int] NOT NULL,
	[Ilosc] [int] NOT NULL,
	[Cena][varchar](6) NULL,
	[FormaPlat][varchar](8) NULL,
	[Termin][int] NULL,
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
	[NrWZ][int] NOT NULL,
	[Aktywny][bit]NOT NULL)


--drop table [List]
--drop table [Dok]
--drop table [Paliwo]
--drop table [Uzytkownik]

select * from KontrahentNazwaView where nazwa like '%sej%'
select * from ListyView
select * from List where aktywny = 1
update list set aktywny=1 where dokid=59
ALTER TABLE List ADD aktywny BIT

update list set aktywny=1 where id=4

select * from Dok
select * from List
select * from WZView where dokid =1
select top 1 isnull(nrwz,1) from list order by nrwz desc


select top 1 isnull(nrwz,1) from List L inner join dok D on D.id = L.dokid order by L.id desc
SELECT COALESCE(MAX(nrwz), '0') FROM List where aktywny=1 
select COALESCE(MAX(id), '0') from dok 
SELECT COALESCE(MAX(nrwz), '0') FROM List
SELECT COALESCE(MAX(nrwz), '0') FROM List where dokid=7
select top 1 nrwz from List L inner join dok D on D.id = L.dokid order by L.id desc
select top 1 nrwz from List L inner join dok D on D.id = L.dokid where dokid=1

SELECT COALESCE(MAX(dokid), '0') FROM List where aktywny=1 


DECLARE @i As int
SET @i = 0
UPDATE List
SET @i= NrWZ= @i+1
where aktywny=1


UPDATE list
SET nrwz = (rowNumber +628)
FROM list
INNER JOIN 
(SELECT ID, row_number() OVER (ORDER BY dokID ) as rowNumber
FROM list where aktywny=1) drRowNumbers ON drRowNumbers.ID = list.ID

update list set nrwz=17 where id=23
select * from List where aktywny=1

SELECT UUID FROM PocztaSent where UUID='11535345.24522.23424'
select uniqueidentifier
[klucz] [uniqueidentifier] NOT NULL)



select * from PocztaSent