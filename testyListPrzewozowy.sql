select * from List	

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


insert into list (NrListu, KontrId, Paliwo, Ilosc,Cena, FormaPlat,Termin, Sent, NrWZ) values ((SELECT CASE WHEN COUNT(1) =0  THEN 1 else ((select top 1 nrlistu from list order by ID desc)+1 )END AS nrlistu FROM List
), 35, 'Olej napedowy', 250,'3,48','gotówka',0,'My zamykamy','148')


CREATE TABLE [dbo].[List](
	[ID] int IDENTITY(1,1) primary key,
	[Data] [varchar](10) NOT NULL,
	[KontrId] [numeric](9,0) NOT NULL,
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

