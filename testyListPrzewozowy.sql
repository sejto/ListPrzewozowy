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

------------test-------------
select * from (
select d.ID as Nr_listu,d.Data, count(K.Nazwa)as Kontrahentów,
(select sum(ilosc) from list where paliwoid=1) as Olej_napedowy,
(select sum(ilosc) from list where paliwoid=2) as Olej_arktyczny,
(select sum(ilosc) from list where paliwoid=3) as Olej_opałowy
from dok D
inner join List L
on L.dokID=D.ID
inner join OTD.dbo.kontrahent K
on k.kontrid=L.kontrID
inner join paliwo P
on p.paliwoID=L.paliwoID
group by d.ID, d.Data)k
---


select NrListu, K.nazwa, paliwo, ilosc, cena from list L 


select * from
(
select kontrid from List	
union all
select kontrid,nazwa from OTD.dbo.kontrahent )
as aa
where kontrid=35

select  nazwa, paliwo, ilosc, cena, termin, sent
from OTD.dbo.kontrahent 
join list
on list.kontrid=OTD.dbo.kontrahent.kontrid

where nazwa like '%trans%'


select  top 1 isnull (nrlistu,1) from list order by ID desc

SELECT CASE WHEN COUNT(1) =0  THEN 1 else ((select top 1 nrlistu from list order by ID desc)+1 )END AS nrlistu FROM List

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