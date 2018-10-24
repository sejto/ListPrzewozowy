using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Windows.Forms;
using System.Xml.Linq;
using OpenPop.Pop3;
using System.Xml.Serialization;

namespace ListPrzewozowy
{
    public class SENT
    {
        public string SenderName { get; set; }
        public string SenderNIP { get; set; }
        public string SenderStreet { get; set; }
        public string SenderHouseNumber { get; set; }
        public string SenderFlatNumber { get; set; }
        public string SenderCity { get; set; }
        public string SenderPostalCode { get; set; }

        public string CarrierName { get; set; }
        public string CarrierNIP { get; set; }
        public string CarrierStreet { get; set; }
        public string CarrierHouseNumber { get; set; }
        public string CarrierFlatNumber { get; set; }
        public string CarrierCity { get; set; }
        public string CarrierPostalCode { get; set; }

        public string TruckNumber { get; set; }
        public string PermitRoad { get; set; }
        public string GeoLocatorNumber { get; set; }
        public string FailoverCarrierEmail { get; set; }

        public string RecipientName { get; set; }
        public string RecipientNIP { get; set; }
        public string RecipientStreet { get; set; }
        public string RecipientHouseNumber { get; set; }
        public string RecipientCity { get; set; }
        public string RecipientPostalCode { get; set; }
        public string LoadingStreet { get; set; }
        public string LoadingHouseNumber { get; set; }
        public string LoadingFlatNumber { get; set; }
        public string RecipientFlatNumber { get; set; }
        public string LoadingCity { get; set; }
        public string LoadingPostalCode { get; set; }
        public string LoadingPlanStart { get; set; }
        public string StartTransportDate { get; set; }
        public string EndTransportDate { get; set; }
        public string CodeTERC { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }


        public string GoodsName { get; set; }
        public string AmountOfGoods { get; set; }
        public string Comments { get; set; }
        public string DocumentId { get; set; }
        public string email1 { get; set; }
        public string EmailAddress2 { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FileName100 { get; set; }

        public void SENT100_old()
        {
            XNamespace ns = "http://www.mf.gov.pl/SENT/2017/01/18/STypes.xsd";
            XNamespace fc = "http://www.mf.gov.pl/SENT/2017/01/18/SENT_100.xsd";
            XElement root = new XElement(fc + "SENT_100",
                new XAttribute(XNamespace.Xmlns + "ns2", fc),
                new XElement(fc + "GoodsSender",
                   new XElement(ns + "TraderInfo",
                   new XElement(ns + "TraderName", SenderName),
                   new XElement(ns + "TraderIdentityType", "NIP"),
                   new XElement(ns + "TraderIdentityNumber", SenderNIP)),
                   new XElement(ns + "TraderAddress",
                   new XElement(ns + "Street", SenderStreet),
                   new XElement(ns + "HouseNumber", SenderHouseNumber),
                   new XElement(ns + "FlatNumber", SenderFlatNumber),
                   new XElement(ns + "City", SenderCity),
                   new XElement(ns + "Country", "PL"),
                   new XElement(ns + "PostalCode", SenderPostalCode))),

                   new XElement(fc + "GoodsRecipient",
                   new XElement(ns + "TraderInfo",
                   new XElement(ns + "TraderName", RecipientName),
                   new XElement(ns + "TraderIdentityType", "NIP"),
                   new XElement(ns + "TraderIdentityNumber", RecipientNIP)),
                   new XElement(ns + "TraderAddress",
                   new XElement(ns + "Street", RecipientStreet),
                   new XElement(ns + "HouseNumber", RecipientHouseNumber),
                   new XElement(ns + "City", RecipientCity),
                   new XElement(ns + "Country", "PL"),
                   new XElement(ns + "PostalCode", RecipientPostalCode))),

                   new XElement(fc + "Transport",
                   new XElement(ns + "PlaceOfLoading",
                   new XElement(ns + "Street", LoadingStreet),
                   new XElement(ns + "HouseNumber", LoadingHouseNumber),
                   new XElement(ns + "City", LoadingCity),
                   new XElement(ns + "Country", "PL"),
                   new XElement(ns + "PostalCode", LoadingPostalCode)),
                   new XElement(ns + "PlannedStartCarriageDate", LoadingPlanStart)),

                   new XElement(fc + "GoodsInformation",
                   new XElement(ns + "CodeCnClassification", "2710"),
                   new XElement(ns + "GoodsName", GoodsName),
                   new XElement(ns + "AmountOfGoods", AmountOfGoods),
                   new XElement(ns + "UnitOfMeasure", "l")),

                   new XElement(fc + "Comments", Comments),
                   new XElement(fc + "DocumentId", DocumentId),

                   new XElement(fc + "ResponseAddress",
                   new XElement(ns + "EmailChannel",
                   new XElement(ns + "EmailAddress1", email1)),

                   new XElement(ns + "WebServiceChannel",
                   new XElement(ns + "WsFromSISC", "false"))),

                   new XElement(fc + "Statements",
                   new XElement(ns + "Statement1", "true"),
                   new XElement(ns + "FirstName", FirstName),
                   new XElement(ns + "LastName", LastName)));

            root.SetAttributeValue("xmlns", "http://www.mf.gov.pl/SENT/2017/01/18/STypes.xsd");
            root.Save(FileName100);
           // MessageBox.Show("Zapisano xml jako: " + FileName100);
        }
        public void SENT100()
        {
            XNamespace ns = "http://www.mf.gov.pl/SENT/2017/12/08/SENT_100.xsd";
            XNamespace fc = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd";
            XElement root = new XElement(ns + "SENT_100",
            new XAttribute(XNamespace.Xmlns + "ns2", fc),
            new XAttribute("xmlns", ns),
            new XElement(ns + "TypeOfTransport", "1"),
                   new XElement(ns + "GoodsSender",
                   new XElement(fc + "TraderInfo",
                   new XElement(fc + "TraderName", SenderName),
                   new XElement(fc + "TraderIdentityType", "NIP"),
                   new XElement(fc + "TraderIdentityNumber", SenderNIP)),
                   new XElement(fc + "TraderAddress",
                   new XElement(fc + "Street", SenderStreet),
                   new XElement(fc + "HouseNumber", SenderHouseNumber),
                   new XElement(fc + "FlatNumber", SenderFlatNumber),
                   new XElement(fc + "City", SenderCity),
                   new XElement(fc + "Country", "PL"),
                   new XElement(fc + "PostalCode", SenderPostalCode))),

                   new XElement(ns + "GoodsRecipient",
                   new XElement(fc + "TraderInfo",
                   new XElement(fc + "TraderName", RecipientName),
                   new XElement(fc + "TraderIdentityType", "NIP"),
                   new XElement(fc + "TraderIdentityNumber", RecipientNIP)),
                   new XElement(fc + "TraderAddress",
                   new XElement(fc + "Street", RecipientStreet),
                   new XElement(fc + "HouseNumber", RecipientHouseNumber),
                   new XElement(fc + "City", RecipientCity),
                   new XElement(fc + "Country", "PL"),
                   new XElement(fc + "PostalCode", RecipientPostalCode))),

                   new XElement(ns + "Transport",
                   new XElement(fc + "PlannedStartCarriageDate", LoadingPlanStart),
                   new XElement(fc + "PlaceOfLoading",
                   new XElement(fc + "Street", LoadingStreet),
                   new XElement(fc + "HouseNumber", LoadingHouseNumber),
                   new XElement(fc + "City", LoadingCity),
                   new XElement(fc + "Country", "PL"),
                   new XElement(fc + "PostalCode", LoadingPostalCode),
                   new XElement(fc + "CodeTERC", CodeTERC),
                   new XElement(fc + "Latitude", Latitude),
                   new XElement(fc + "Longitude", Longitude))),

                   new XElement(ns + "GoodsInformation",
                   new XElement(fc + "CodeCnClassification", "2710"),
                   new XElement(fc + "GoodsName", GoodsName),
                   new XElement(fc + "AmountOfGoods", AmountOfGoods),
                   new XElement(fc + "UnitOfMeasure", "l")),
                   new XElement(ns + "Comments", Comments),
                   new XElement(ns + "DocumentId", DocumentId),

                   new XElement(ns + "ResponseAddress",
                   new XElement(fc + "EmailChannel",
                   new XElement(fc + "EmailAddress1", email1))),

                   new XElement(ns + "Statements",
                   new XElement(fc + "Statement1", "true"),
                   new XElement(fc + "FirstName", FirstName),
                   new XElement(fc + "LastName", LastName)));

            root.Save(FileName100);
        }

        public void SENT102()
        {

        }

        public void SENT105()
        {
            /*    XNamespace ns = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd";
                //XNamespace fc = "http://www.mf.gov.pl/SENT/2017/12/08/SENT_105.xsd";
                XAttribute fc = new XAttribute("xmlns", "http://www.mf.gov.pl/SENT/2017/12/08/SENT_105.xsd");
                XElement root = new XElement("SENT_105",
                    new XAttribute(XNamespace.Xmlns + "ns2", ns),
                    new XAttribute(XNamespace.Xmlns+ "fc",1),
                    new XElement("TypeOfTransport", "1"),
                    new XElement("GoodsSender"));
              */

            XNamespace ns = "http://www.mf.gov.pl/SENT/2017/12/08/SENT_105.xsd";
            XNamespace fc = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd";

            /* XElement root = new XElement(ns + "SENT_105");
             root.Add(new XAttribute(XNamespace.Xmlns + "ns2", fc));
             root.Add(new XAttribute("xmlns", ns));
             root.Add(new XElement("NewChild", "new content")); */

            XElement root = new XElement(ns + "SENT_105",
            new XAttribute(XNamespace.Xmlns + "ns2", fc),
            new XAttribute("xmlns", ns),
            new XElement(ns+ "TypeOfTransport", "1"),
                   new XElement(ns + "GoodsSender",
                   new XElement(fc + "TraderInfo",
                   new XElement(fc + "TraderName", SenderName),
                   new XElement(fc + "TraderIdentityType", "NIP"),
                   new XElement(fc + "TraderIdentityNumber", SenderNIP)),
                   new XElement(fc + "TraderAddress",
                   new XElement(fc + "Street", SenderStreet),
                   new XElement(fc + "HouseNumber", SenderHouseNumber),
                   new XElement(fc + "FlatNumber", SenderFlatNumber),
                   new XElement(fc + "City", SenderCity),
                   new XElement(fc + "Country", "PL"),
                   new XElement(fc + "PostalCode", SenderPostalCode))),
                   
                   new XElement(ns + "Carrier",
                   new XElement(fc + "TraderInfo",
                   new XElement(fc + "TraderName", CarrierName),
                   new XElement(fc + "TraderIdentityType", "NIP"),
                   new XElement(fc + "TraderIdentityNumber", CarrierNIP)),
                   new XElement(fc + "TraderAddress",
                   new XElement(fc + "Street", CarrierStreet),
                   new XElement(fc + "HouseNumber", CarrierHouseNumber),
                   new XElement(fc + "FlatNumber", CarrierFlatNumber),
                   new XElement(fc + "City", CarrierCity),
                   new XElement(fc + "Country", "PL"),
                   new XElement(fc + "PostalCode", CarrierPostalCode))),

                   new XElement(ns + "GoodsRecipient",
                   new XElement(fc + "TraderInfo",
                   new XElement(fc + "TraderName", RecipientName),
                   new XElement(fc + "TraderIdentityType", "NIP"),
                   new XElement(fc + "TraderIdentityNumber", RecipientNIP)),
                   new XElement(fc + "TraderAddress",
                   new XElement(fc + "Street", RecipientStreet),
                   new XElement(fc + "HouseNumber", RecipientHouseNumber),
                   new XElement(fc + "City", RecipientCity),
                   new XElement(fc + "Country", "PL"),
                   new XElement(fc + "PostalCode", RecipientPostalCode))),

                   new XElement(ns + "MeansOfTransport",
                   new XElement(fc + "TruckOrTrainNumber",TruckNumber),
                   new XElement(fc + "PermitRoad", PermitRoad),
                   new XElement(fc + "GeoLocatorNumber", GeoLocatorNumber),
                   new XElement(fc + "FailoverCarrierEmail", FailoverCarrierEmail)),
                   new XElement(ns + "Transport",
                   new XElement(fc + "PlannedStartCarriageDate", LoadingPlanStart),
                   new XElement(fc + "StartTransportDate", StartTransportDate),
                   new XElement(fc + "EndTransportDate", EndTransportDate),
                   new XElement(fc + "PlaceOfLoading",
                   new XElement(fc + "Street", LoadingStreet),
                   new XElement(fc + "HouseNumber", LoadingHouseNumber),
                   new XElement(fc + "City", LoadingCity),
                   new XElement(fc + "Country", "PL"),
                   new XElement(fc + "PostalCode", LoadingPostalCode),
                   new XElement(fc + "CodeTERC", CodeTERC),
                   new XElement(fc + "Latitude", Latitude),
                   new XElement(fc + "Longitude", Longitude)),
                   new XElement(fc + "PlaceOfDelivery",
                   new XElement(fc + "Street", LoadingStreet),
                   new XElement(fc + "HouseNumber", LoadingHouseNumber),
                   new XElement(fc + "City", LoadingCity),
                   new XElement(fc + "Country", "PL"),
                   new XElement(fc + "PostalCode", LoadingPostalCode),
                   new XElement(fc + "CodeTERC", CodeTERC),
                   new XElement(fc + "Latitude", Latitude),
                   new XElement(fc + "Longitude", Longitude))),
                   

                   new XElement(fc + "GoodsInformation",
                   new XElement(ns + "CodeCnClassification", "2710"),
                   new XElement(ns + "GoodsName", GoodsName),
                   new XElement(ns + "AmountOfGoods", AmountOfGoods),
                   new XElement(ns + "UnitOfMeasure", "l")),
                   //TODO: tutaj skonczylem - porownac dalej sent105
                   new XElement(fc + "Comments", Comments),
                   new XElement(fc + "DocumentId", DocumentId),

                   new XElement(fc + "ResponseAddress",
                   new XElement(ns + "EmailChannel",
                   new XElement(ns + "EmailAddress1", email1)),

                   new XElement(ns + "WebServiceChannel",
                   new XElement(ns + "WsFromSISC", "false"))),

                   new XElement(fc + "Statements",
                   new XElement(ns + "Statement1", "true"),
                   new XElement(ns + "FirstName", FirstName),
                   new XElement(ns + "LastName", LastName)));


            //root.SetAttributeValue(XNamespace.Xmlns + "ns2", fc);
            //root.SetAttributeValue("xmlns", ns);
            root.Save(FileName100);
        }

        public void ReadMail()
        {
           // =====klasa Mail
        }
/*        public static List<Message> FetchAllMessages(string hostname, int port, bool useSsl, string username, string password)
        {
            // The client disconnects from the server when being disposed

            using (Pop3Client client = new Pop3Client())
            {
                // Connect to the server
                client.Connect(hostname, port, useSsl);

                // Authenticate ourselves towards the server
                client.Authenticate(username, password);

                // Get the number of messages in the inbox
                int messageCount = client.GetMessageCount();

                // We want to download all messages
                List<Message> allMessages = new List<Message>(messageCount);

                // Messages are numbered in the interval: [1, messageCount]
                // Ergo: message numbers are 1-based.
                // Most servers give the latest message the highest number
                for (int i = messageCount; i > 0; i--)
                {
                    allMessages.Add(client.GetMessage(i));
                }

                // Now return the fetched messages
                return allMessages;
                
            }
        } */
    }
    public static class SENT102
    {
        static string SentNumber { get; set; }
        static string CarrierKey { get; set; }
        static string SenderNIP { get; set; }
        public static void WyslijSent102(string SentNumber, string CarrierKey)
        {
            XNamespace fc = "http://www.mf.gov.pl/SENT/2017/01/18/STypes.xsd";
            XNamespace ns = "http://www.mf.gov.pl/SENT/2017/01/18/SENT_102.xsd";
            string data = DateTime.Now.ToString("yyyy-MM-dd");
            XElement root = new XElement(ns + "SENT_102",
            new XAttribute(XNamespace.Xmlns + "ns2", fc),
            new XElement(ns + "SentNumber",SentNumber),
            new XElement(ns + "CarrierKey", CarrierKey),
            new XElement(ns + "Carrier",
            new XElement(fc + "TraderInfo",
            new XElement(fc + "TraderName", "OIL TRANSFER DEVELOPMENT STACJA PALIW"),
            new XElement(fc + "TraderIdentityType", "NIP"),
            new XElement(fc + "TraderIdentityNumber", "8442355566")),
            new XElement(fc + "TraderAddress",
            new XElement(fc + "Street", "PUŁASKIEGO"),
            new XElement(fc + "HouseNumber", "107"),
            new XElement(fc + "City", "SUWAŁKI"),
            new XElement(fc + "Country", "PL"),
            new XElement(fc + "PostalCode", "16-400"))),

            new XElement(ns + "MeansOfTransport",
            new XElement(fc + "TruckNumber", "BSU66666"),
            new XElement(fc + "PermitRoad", "BSU66666")),

            new XElement(ns + "GoodsInformation",
            new XElement(fc + "TypeOfTransportDocument", "INNY"),
            new XElement(fc + "NumberOfTransportDocument", "WZ 2422/2018")),

            new XElement(ns + "Transport",
            new XElement(fc + "PlaceOfDelivery",
            new XElement(fc + "Street", "PUŁASKIEGO"),
            new XElement(fc + "HouseNumber", "107"),
            new XElement(fc + "City", "SUWAŁKI"),
            new XElement(fc + "Country", "PL"),
            new XElement(fc + "PostalCode", "16-400")),
            new XElement(fc + "StartTransportDate", data),
            new XElement(fc + "EndTransportDate", data)),

            new XElement(ns + "Comments", "Sprzedaż obwoźna"),
            new XElement(ns + "DocumentId", "WZ 2422/2018"),

            new XElement(ns + "ResponseAddress",
            new XElement(fc + "EmailChannel",
            new XElement(fc + "EmailAddress1", "test_vir2@o2.pl")),

            new XElement(fc + "WebServiceChannel",
            new XElement(fc + "WsFromSISC", "false"))),

            new XElement(ns + "Statements",
            new XElement(fc + "Statement1", "true"),
            new XElement(fc + "FirstName", "Lucyfoj"),
            new XElement(fc + "LastName", "Nieustalony")));

            root.SetAttributeValue("xmlns", "http://www.mf.gov.pl/SENT/2017/01/18/SENT_102.xsd");
            string filename = AppDomain.CurrentDomain.BaseDirectory + @"\xml\SENT_102_"+SentNumber+".xml";
            root.Save(filename);
            MessageBox.Show("Zapisano xml jako: " + filename);
        }
    }
  /*  public static class SENT100
    {
        public static string SenderNIP { get; set; }
        public static string SenderName { get; set; }
        public static string SenderStreet { get; set; }
        public static string SenderHouseNumber { get; set; }
        public static string SenderCity { get; set; }
        public static string SenderPostalCode { get; set; }

        public static string RecipientNIP { get; set; }
        public static string RecipientName { get; set; }
        public static string RecipientStreet { get; set; }
        public static string RecipientHouseNumber { get; set; }
        public static string RecipientCity { get; set; }
        public static string RecipientPostalCode { get; set; }

        public static string LoadingStreet { get; set; }
        public static string LoadingHouseNumber { get; set; }
        public static string LoadingCity { get; set; }
        public static string LoadingPostalCode { get; set; }
        public static string LoadingDate { get; set; }

        public static string GoodsName { get; set; }
        public static string AmountOfGoods { get; set; }

        public static string Comments { get; set; }
        public static string DocumentId { get; set; }

        public static string email1 { get; set; }

        public static string FirstName { get; set; }
        public static string LastName { get; set; }


        public static void WyslijSENT100()
        {
            MessageBox.Show(SenderNIP);
            XNamespace ns = "http://www.mf.gov.pl/SENT/2017/01/18/STypes.xsd";
            XNamespace fc = "http://www.mf.gov.pl/SENT/2017/01/18/SENT_100.xsd";
            XElement root = new XElement(fc + "SENT_100",
                new XAttribute(XNamespace.Xmlns + "ns2", fc),
                new XElement(fc + "GoodsSender",
                   new XElement(ns + "TraderInfo",
                   new XElement(ns + "TraderName", SenderName),
                   new XElement(ns + "TraderIdentityType", "NIP"),
                   new XElement(ns + "TraderIdentityNumber", SenderNIP)),
                   new XElement(ns + "TraderAddress",
                   new XElement(ns + "Street", SenderStreet),
                   new XElement(ns + "HouseNumber", SenderHouseNumber),
                   new XElement(ns + "City", SenderCity),
                   new XElement(ns + "Country", "PL"),
                   new XElement(ns + "PostalCode", SenderPostalCode))),

                   new XElement(fc + "GoodsRecipient",
                   new XElement(ns + "TraderInfo",
                   new XElement(ns + "TraderName", RecipientName),
                   new XElement(ns + "TraderIdentityType", "NIP"),
                   new XElement(ns + "TraderIdentityNumber", RecipientNIP)),
                   new XElement(ns + "TraderAddress",
                   new XElement(ns + "Street", RecipientStreet),
                   new XElement(ns + "HouseNumber", RecipientHouseNumber),
                   new XElement(ns + "City", RecipientCity),
                   new XElement(ns + "Country", "PL"),
                   new XElement(ns + "PostalCode", RecipientPostalCode))),

                   new XElement(fc + "Transport",
                   new XElement(ns + "PlaceOfLoading",
                   new XElement(ns + "Street", LoadingStreet),
                   new XElement(ns + "HouseNumber", LoadingHouseNumber),
                   new XElement(ns + "City", LoadingCity),
                   new XElement(ns + "Country", "PL"),
                   new XElement(ns + "PostalCode", LoadingPostalCode)),
                   new XElement(ns + "PlannedStartCarriageDate", LoadingDate)),

                   new XElement(fc + "GoodsInformation",
                   new XElement(ns + "CodeCnClassification", "2710"),
                   new XElement(ns + "GoodsName", GoodsName),
                   new XElement(ns + "AmountOfGoods", AmountOfGoods),
                   new XElement(ns + "UnitOfMeasure", "l")),

                   new XElement(fc + "Comments", Comments),
                   new XElement(fc + "DocumentId", DocumentId),

                   new XElement(fc + "ResponseAddress",
                   new XElement(ns + "EmailChannel",
                   new XElement(ns + "EmailAddress1", email1)),

                   new XElement(ns + "WebServiceChannel",
                   new XElement(ns + "WsFromSISC", "false"))),

                   new XElement(fc + "Statements",
                   new XElement(ns + "Statement1", "true"),
                   new XElement(ns + "FirstName", FirstName),
                   new XElement(ns + "LastName", LastName)));

            root.SetAttributeValue("xmlns", "http://www.mf.gov.pl/SENT/2017/01/18/STypes.xsd");
            string filename = AppDomain.CurrentDomain.BaseDirectory + @"\xml\SENT_100.xml";
            root.Save(filename);
            MessageBox.Show("Zapisano xml jako: " + filename);
            // Close();
            //===========================================================================================================
        } 
    } */


    //===========================================================
       /* 
    Licensed under the Apache License, Version 2.0
    
    http://www.apache.org/licenses/LICENSE-2.0
    */

        [XmlRoot(ElementName = "TraderInfo", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
        public class TraderInfo
        {
            [XmlElement(ElementName = "TraderName", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string TraderName { get; set; }
            [XmlElement(ElementName = "TraderIdentityType", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string TraderIdentityType { get; set; }
            [XmlElement(ElementName = "TraderIdentityNumber", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string TraderIdentityNumber { get; set; }
        }

        [XmlRoot(ElementName = "TraderAddress", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
        public class TraderAddress
        {
            [XmlElement(ElementName = "Street", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string Street { get; set; }
            [XmlElement(ElementName = "HouseNumber", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string HouseNumber { get; set; }
            [XmlElement(ElementName = "FlatNumber", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string FlatNumber { get; set; }
            [XmlElement(ElementName = "City", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string City { get; set; }
            [XmlElement(ElementName = "Country", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string Country { get; set; }
            [XmlElement(ElementName = "PostalCode", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string PostalCode { get; set; }
        }

        [XmlRoot(ElementName = "GoodsSender", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/SENT_105.xsd")]
        public class GoodsSender
        {
            [XmlElement(ElementName = "TraderInfo", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public TraderInfo TraderInfo { get; set; }
            [XmlElement(ElementName = "TraderAddress", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public TraderAddress TraderAddress { get; set; }
        }

        [XmlRoot(ElementName = "Carrier", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/SENT_105.xsd")]
        public class Carrier
        {
            [XmlElement(ElementName = "TraderInfo", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public TraderInfo TraderInfo { get; set; }
            [XmlElement(ElementName = "TraderAddress", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public TraderAddress TraderAddress { get; set; }
        }

        [XmlRoot(ElementName = "GoodsRecipient", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/SENT_105.xsd")]
        public class GoodsRecipient
        {
            [XmlElement(ElementName = "TraderInfo", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public TraderInfo TraderInfo { get; set; }
            [XmlElement(ElementName = "TraderAddress", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public TraderAddress TraderAddress { get; set; }
        }

        [XmlRoot(ElementName = "MeansOfTransport", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/SENT_105.xsd")]
        public class MeansOfTransport
        {
            [XmlElement(ElementName = "TruckOrTrainNumber", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string TruckOrTrainNumber { get; set; }
            [XmlElement(ElementName = "PermitRoad", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string PermitRoad { get; set; }
            [XmlElement(ElementName = "GeoLocatorNumber", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string GeoLocatorNumber { get; set; }
            [XmlElement(ElementName = "FailoverCarrierEmail", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string FailoverCarrierEmail { get; set; }
        }

        [XmlRoot(ElementName = "PlaceOfLoading", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
        public class PlaceOfLoading
        {
            [XmlElement(ElementName = "Street", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string Street { get; set; }
            [XmlElement(ElementName = "HouseNumber", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string HouseNumber { get; set; }
            [XmlElement(ElementName = "FlatNumber", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string FlatNumber { get; set; }
            [XmlElement(ElementName = "City", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string City { get; set; }
            [XmlElement(ElementName = "Country", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string Country { get; set; }
            [XmlElement(ElementName = "PostalCode", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string PostalCode { get; set; }
            [XmlElement(ElementName = "CodeTERC", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string CodeTERC { get; set; }
            [XmlElement(ElementName = "Latitude", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string Latitude { get; set; }
            [XmlElement(ElementName = "Longitude", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string Longitude { get; set; }
        }

        [XmlRoot(ElementName = "PlaceOfDelivery", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
        public class PlaceOfDelivery
        {
            [XmlElement(ElementName = "Street", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string Street { get; set; }
            [XmlElement(ElementName = "HouseNumber", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string HouseNumber { get; set; }
            [XmlElement(ElementName = "FlatNumber", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string FlatNumber { get; set; }
            [XmlElement(ElementName = "City", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string City { get; set; }
            [XmlElement(ElementName = "Country", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string Country { get; set; }
            [XmlElement(ElementName = "PostalCode", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string PostalCode { get; set; }
            [XmlElement(ElementName = "CodeTERC", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string CodeTERC { get; set; }
            [XmlElement(ElementName = "Latitude", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string Latitude { get; set; }
            [XmlElement(ElementName = "Longitude", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string Longitude { get; set; }
        }

        [XmlRoot(ElementName = "Transport", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/SENT_105.xsd")]
        public class Transport
        {
            [XmlElement(ElementName = "PlannedStartCarriageDate", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string PlannedStartCarriageDate { get; set; }
            [XmlElement(ElementName = "StartTransportDate", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string StartTransportDate { get; set; }
            [XmlElement(ElementName = "EndTransportDate", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string EndTransportDate { get; set; }
            [XmlElement(ElementName = "PlaceOfLoading", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public PlaceOfLoading PlaceOfLoading { get; set; }
            [XmlElement(ElementName = "PlaceOfDelivery", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public PlaceOfDelivery PlaceOfDelivery { get; set; }
        }

        [XmlRoot(ElementName = "GoodsInformation", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/SENT_105.xsd")]
        public class GoodsInformation
        {
            [XmlElement(ElementName = "CodeCnClassification", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string CodeCnClassification { get; set; }
            [XmlElement(ElementName = "GoodsName", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string GoodsName { get; set; }
            [XmlElement(ElementName = "AmountOfGoods", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string AmountOfGoods { get; set; }
            [XmlElement(ElementName = "UnitOfMeasure", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string UnitOfMeasure { get; set; }
        }

        [XmlRoot(ElementName = "GoodsTransportDocuments", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/SENT_105.xsd")]
        public class GoodsTransportDocuments
        {
            [XmlElement(ElementName = "TypeOfTransportDocument", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string TypeOfTransportDocument { get; set; }
            [XmlElement(ElementName = "NumberOfTransportDocument", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string NumberOfTransportDocument { get; set; }
        }

        [XmlRoot(ElementName = "EmailChannel", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
        public class EmailChannel
        {
            [XmlElement(ElementName = "EmailAddress1", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string EmailAddress1 { get; set; }
        }

        [XmlRoot(ElementName = "ResponseAddress", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/SENT_105.xsd")]
        public class ResponseAddress
        {
            [XmlElement(ElementName = "EmailChannel", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public EmailChannel EmailChannel { get; set; }
        }

        [XmlRoot(ElementName = "Statements", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/SENT_105.xsd")]
        public class Statements
        {
            [XmlElement(ElementName = "Statement1", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string Statement1 { get; set; }
            [XmlElement(ElementName = "FirstName", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string FirstName { get; set; }
            [XmlElement(ElementName = "LastName", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/STypes.xsd")]
            public string LastName { get; set; }
        }

        [XmlRoot(ElementName = "SENT_105", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/SENT_105.xsd")]
        public class SENT_105
        {
            [XmlElement(ElementName = "TypeOfTransport", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/SENT_105.xsd")]
            public string TypeOfTransport { get; set; }
            [XmlElement(ElementName = "GoodsSender", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/SENT_105.xsd")]
            public GoodsSender GoodsSender { get; set; }
            [XmlElement(ElementName = "Carrier", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/SENT_105.xsd")]
            public Carrier Carrier { get; set; }
            [XmlElement(ElementName = "GoodsRecipient", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/SENT_105.xsd")]
            public GoodsRecipient GoodsRecipient { get; set; }
            [XmlElement(ElementName = "MeansOfTransport", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/SENT_105.xsd")]
            public MeansOfTransport MeansOfTransport { get; set; }
            [XmlElement(ElementName = "Transport", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/SENT_105.xsd")]
            public Transport Transport { get; set; }
            [XmlElement(ElementName = "GoodsInformation", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/SENT_105.xsd")]
            public GoodsInformation GoodsInformation { get; set; }
            [XmlElement(ElementName = "GoodsTransportDocuments", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/SENT_105.xsd")]
            public GoodsTransportDocuments GoodsTransportDocuments { get; set; }
            [XmlElement(ElementName = "Comments", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/SENT_105.xsd")]
            public string Comments { get; set; }
            [XmlElement(ElementName = "ResponseAddress", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/SENT_105.xsd")]
            public ResponseAddress ResponseAddress { get; set; }
            [XmlElement(ElementName = "Statements", Namespace = "http://www.mf.gov.pl/SENT/2017/12/08/SENT_105.xsd")]
            public Statements Statements { get; set; }
            [XmlAttribute(AttributeName = "ns2", Namespace = "http://www.w3.org/2000/xmlns/")]
            public string Ns2 { get; set; }
            [XmlAttribute(AttributeName = "xmlns")]
            public string Xmlns { get; set; }
        }



    //===========================================================

}