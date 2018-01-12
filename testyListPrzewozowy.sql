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
select K.Nazwa,K.Ulica, K.Miasto, K.Nip, K.telefon,P.nazwa,L.Ilosc,L.Cena, L.FormaPlat, L.Termin, L.Sent, L.DostUlica from List L
inner join Dok D
on D.id=L.dokid
inner join Uzytkownik U
on U.id=D.userID
inner join Paliwo P
on P.paliwoID=L.paliwoid
inner join OTD.dbo.kontrahent K
on k.kontrid=L.kontrID

------------###############----------------
insert into dok (Data,userID)values('2017-12-10',1)
insert into Uzytkownik (nazwa) values ('Alina Spura')
insert into Uzytkownik (nazwa) values ('Małgorzata Zakrzewska')
insert into Paliwo (Nazwa) values ('Olej napędowy')
insert into Paliwo (Nazwa) values ('Olej napędowy arktyczny')
insert into Paliwo (Nazwa) values ('Olej napędowy do celów opałowych')
insert into list (DokID, KontrId, PaliwoID, Ilosc,Cena, FormaPlat,Termin, Sent, NrWZ) values (1,83,1, 1000,'3,99','przelew',3,'My zamykamy','148')
insert into list (DokID, KontrId, PaliwoID, Ilosc,Cena, FormaPlat,Termin, Sent, NrWZ) values (1,86,1, 1000,'3,99','przelew',3,'My zamykamy','149')
insert into list (DokID, KontrId, PaliwoID, Ilosc,Cena, FormaPlat,Termin, Sent, NrWZ) values (1,93,2, 2000,'4,8','przelew',2,'Oni zamykają','150')
insert into list (DokID, KontrId, PaliwoID, Ilosc,Cena, FormaPlat,Termin, Sent, NrWZ) values (1,96,2, 1500,'4,85','przelew',3,'Oni zamykają','151')
insert into list (DokID, KontrId, PaliwoID, Ilosc,Cena, FormaPlat,Termin, Sent, NrWZ) values (1,183,2, 500,'4,28','przelew',2,'Oni zamykają','152')
--------#####################-----------

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
--drop table [Dok]
--drop table [Paliwo]
--drop table [Uzytkownik]



select * from Dok

select * from Paliwo

select * from List

select * from Uzytkownik

select nazwa from paliwo

delete from list where paliwoid=0

select * from paliwo where paliwoid=1

select cast(ROUND(Cenadet*(1+(CAST(Stawka AS DECIMAL))/10000),2 )AS DECIMAL(5,2)) as cena from towar where towid=1
select * from Regula


select * from List

