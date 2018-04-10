using System;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ListPrzewozowy
{
    public class SENT
    {
        public string SenderName { get; set; }
        public string SenderNIP { get; set; }
        public string SenderStreet { get; set; }
        public string SenderNumber { get; set; }
        public string SenderCity { get; set; }
        public string SenderCode { get; set; }
        public string RecipientName { get; set; }
        public string RecipientNIP { get; set; }
        public string RecipientStreet { get; set; }
        public string RecipientNumber { get; set; }
        public string RecipientCity { get; set; }
        public string RecipientCode { get; set; }
        public string LoadingStreet { get; set; }
        public string LoadingNumber { get; set; }
        public string LoadingCity { get; set; }
        public string LoadingCode { get; set; }
        public string LoadingPlanStart { get; set; }
        public string GoodsName { get; set; }
        public string AmountOfGoods { get; set; }
        public string Comments { get; set; }
        public string DocumentId { get; set; }
        public string EmailAddress1 { get; set; }
        public string EmailAddress2 { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FileName { get; set; }

        public void SENT100()
        {
           // string file = "SENT100.xml";
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
                   new XElement(ns + "HouseNumber", SenderNumber),
                   new XElement(ns + "City", SenderCity),
                   new XElement(ns + "Country", "PL"),
                   new XElement(ns + "PostalCode", SenderCode))),

                   new XElement(fc + "GoodsRecipient",
                   new XElement(ns + "TraderInfo",
                   new XElement(ns + "TraderName", RecipientName),
                   new XElement(ns + "TraderIdentityType", "NIP"),
                   new XElement(ns + "TraderIdentityNumber", RecipientNIP)),
                   new XElement(ns + "TraderAddress",
                   new XElement(ns + "Street", RecipientStreet),
                   new XElement(ns + "HouseNumber", RecipientNumber),
                   new XElement(ns + "City", RecipientCity),
                   new XElement(ns + "Country", "PL"),
                   new XElement(ns + "PostalCode", RecipientCode))),

                   new XElement(fc + "Transport",
                   new XElement(ns + "PlaceOfLoading",
                   new XElement(ns + "Street", LoadingStreet),
                   new XElement(ns + "HouseNumber", LoadingNumber),
                   new XElement(ns + "City", LoadingCity),
                   new XElement(ns + "Country", "PL"),
                   new XElement(ns + "PostalCode", LoadingCode)),
                   new XElement(ns + "PlannedStartCarriageDate", LoadingPlanStart)), //"2018-04-12+02:00"

                   new XElement(fc + "GoodsInformation",
                   new XElement(ns + "CodeCnClassification", "2710"),
                   new XElement(ns + "GoodsName", GoodsName),
                   new XElement(ns + "AmountOfGoods", AmountOfGoods),
                   new XElement(ns + "UnitOfMeasure", "l")),

                   new XElement(fc + "Comments", Comments),
                   new XElement(fc + "DocumentId", DocumentId),

                   new XElement(fc + "ResponseAddress",
                   new XElement(ns + "EmailChannel",
                   new XElement(ns + "EmailAddress1", EmailAddress1)),

                   new XElement(ns + "WebServiceChannel",
                   new XElement(ns + "WsFromSISC", "false"))),

                   new XElement(fc + "Statements",
                   new XElement(ns + "Statement1", "true"),
                   new XElement(ns + "FirstName", FirstName),
                   new XElement(ns + "LastName", LastName)));

            root.SetAttributeValue("xmlns", "http://www.mf.gov.pl/SENT/2017/01/18/STypes.xsd");
            root.Save(FileName);
            
        }

        public void SENT102()
        {

        }

        public void SendMail(string  Message)
        {
            SmtpClient server = new SmtpClient("poczta.o2.pl");
            server.Credentials = new NetworkCredential("test_vir2@o2.pl", "xxxx");
            MailMessage email = new MailMessage();
            email.From = new MailAddress("test_vir2@o2.pl");
            email.To.Add("test.puesc@mf.gov.pl");
            email.Subject = "SENT100";
            email.Body = Message;
            //TODO dodawanie załącznika xml !!!!
            try
            {
                server.Send(email);
                MessageBox.Show("Wysłano!");
            }
            catch (SmtpFailedRecipientException error)
            {
                MessageBox.Show("error: " + error.Message + "\nFailing recipient: " + error.FailedRecipient);
            }
        }
    }


}