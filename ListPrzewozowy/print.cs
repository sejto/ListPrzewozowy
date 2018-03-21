using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListPrzewozowy
{
    class Print
    {
        private static readonly XPen pen = new XPen(XColors.Black, 0.5);
        private static readonly XBrush brush = XBrushes.Black;
        private static readonly XFont fontNormal = new XFont("Arial", 9, XFontStyle.Regular, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static readonly XFont fontUwagi = new XFont("Arial", 8, XFontStyle.Regular, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static readonly XFont fontWZ = new XFont("Arial", 11, XFontStyle.Regular, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static readonly XFont fontBold = new XFont("Arial", 10, XFontStyle.Bold, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static readonly XFont fontAmount = new XFont("Arial", 9, XFontStyle.Bold, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static readonly XFont fontHeader = new XFont("Arial", 14, XFontStyle.Bold, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static readonly XFont fontInvoiceType = new XFont("Arial", 12, XFontStyle.Bold, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static readonly XFont fontDates = new XFont("Arial", 10, XFontStyle.Bold, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static readonly XFont fontProductsHeader = new XFont("Arial", 8, XFontStyle.Bold, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static readonly XFont fontWZproduct = new XFont("Arial", 8, XFontStyle.Bold, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static readonly XFont fontSubtitle = new XFont("Arial", 6, XFontStyle.Regular, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static double posX = 30;
        private static double lenListName = 360;
        private static double lenListDischarge = 60;
        private static double posYBody=55;
        //------------WZ---------------------------
        private static double posXWZ1 = 30;
        private static double widthYWZ = 220;
        private static double heightWZ = 76;
        private static double NamePosX1 = 250;
        public string[] lineour = new string[6];
        public string paliwo="";
        public string ilosc="";
        public string cenapaliwa = "";
        public string uwagi="";
        public string uwagiN = "";
        public string line1;
        public string line2;
        public string line3;
        public string line4;
        public string line5;
        public static string sent="";

        public void DrawHeader(PdfPage page, string SaleDate)
        {
            using (XGraphics graphics = XGraphics.FromPdfPage(page))
            {

                XRect listHead = new XRect(posX, 15, 530, 30);
                XRect listHeader = new XRect(posX-50, 10, 530, 30);
                XRect listDate = new XRect(posX+100, 10, 530, 30);
                XRect listShow = new XRect(posX, 30, 530, 10);

                graphics.DrawRectangle(pen, listHead);
                XFont fontNumber = fontDates;

                graphics.DrawString("Wykaz odbiorców UN 1202 w sprzedaży obwoźnej", fontBold, brush, listShow, XStringFormats.Center);
                graphics.DrawString("Załącznik do listu przewozowego z dnia ", fontNormal, brush, listHeader, XStringFormats.Center);
                graphics.DrawString(SaleDate, fontHeader, brush, listDate, XStringFormats.Center);
            }
        }
        public void DrawBody(PdfPage page, int litryON, int litryONA, int litryOP)
        {
            using (XGraphics graphics = XGraphics.FromPdfPage(page))
            {
                XRect listName = new XRect(posX, posYBody, lenListName, 30);
                XRect listName1 = new XRect(posX, posYBody+5, lenListName, 10);
                XRect listName2 = new XRect(posX, posYBody+15, lenListName, 10); 
                XRect listAmountH = new XRect(posX+ lenListName, posYBody, 170, 15);
                XRect listAmountHON = new XRect(posX + lenListName-5, posYBody, 35, 15);
                XRect listAmountON = new XRect(posX + lenListName+5, posYBody, 85, 15);
                XRect listAmountONA = new XRect(posX + lenListName + 55, posYBody, 85, 15);
                XRect listAmountOP = new XRect(posX + lenListName + 105, posYBody, 85, 15);

                XRect listOrdered = new XRect(posX + lenListName, posYBody + 15, 55, 15);
                XRect listDischarge = new XRect(posX + lenListName+55, posYBody+15, 58, 15);
                XRect listRest = new XRect(posX+lenListName+lenListDischarge+53, posYBody+15, 57, 15);

                graphics.DrawRectangle(pen, XBrushes.LightGray, listName);
                graphics.DrawRectangle(pen,  listAmountH);
                graphics.DrawRectangle(pen, XBrushes.LightGray, listOrdered);
                graphics.DrawRectangle(pen, XBrushes.LightGray, listDischarge);
                graphics.DrawRectangle(pen, XBrushes.LightGray, listRest);

                graphics.DrawString("Nazwa i adres odbiorcy", fontNormal, brush, listName1, XStringFormats.Center);
                graphics.DrawString("(miejsce rozładunku)", fontNormal, brush, listName2, XStringFormats.Center);
                graphics.DrawString("Ilość", fontBold, brush, listAmountHON, XStringFormats.Center);
                graphics.DrawString("ON: "+litryON, fontAmount, brush, listAmountON, XStringFormats.Center);
                graphics.DrawString("ONA: " + litryONA, fontAmount, brush, listAmountONA, XStringFormats.Center);
                graphics.DrawString("OP: " + litryOP, fontAmount, brush, listAmountOP, XStringFormats.Center);
                graphics.DrawString("Zamówione", fontNormal, brush, listOrdered, XStringFormats.Center);
                graphics.DrawString("Rozładowane", fontNormal, brush, listDischarge, XStringFormats.Center);
                graphics.DrawString("Pozostało", fontNormal, brush, listRest, XStringFormats.Center);
            }

        }
        public void DrawCustomer(PdfPage page, double posYCustomer, string line1, string line2, string line3, string line4, string line5, string amount, string line6)
        {
            using (XGraphics graphics = XGraphics.FromPdfPage(page))
            {
                XRect listCustomer = new XRect(posX, posYCustomer, lenListName, 70);
                XRect listCust1 = new XRect(posX+10, posYCustomer+5, lenListName, 5);
                XRect listCust2 = new XRect(posX+10, posYCustomer+15, lenListName, 5);
                XRect listCust3 = new XRect(posX+10, posYCustomer+25, lenListName, 5);
                XRect listCust4 = new XRect(posX+10, posYCustomer+35, lenListName, 5);
                XRect listCust5 = new XRect(posX+10, posYCustomer+45, lenListName, 5);
                XRect listCust6 = new XRect(posX+10, posYCustomer + 55, lenListName, 5);
                XRect linesent = new XRect(posX+10, posYCustomer + 55, lenListName, 5); //ustawić współrzedne

                XRect listCustomerOrdered = new XRect(posX+ lenListName, posYCustomer, lenListDischarge-5, 70);
                XRect listCustomerDischarge = new XRect(posX + lenListName + lenListDischarge-5, posYCustomer, 58, 70);
                XRect listCustomerRest = new XRect(posX + lenListName + lenListDischarge+53, posYCustomer, 57, 70);

                graphics.DrawRectangle(pen, listCustomer);

                graphics.DrawRectangle(pen, listCustomerOrdered);
                graphics.DrawRectangle(pen, listCustomerDischarge);
                graphics.DrawRectangle(pen, listCustomerRest);

                graphics.DrawString(line1, fontProductsHeader, brush, listCust1, XStringFormats.TopLeft  );
                graphics.DrawString(line2, fontNormal, brush, listCust2, XStringFormats.TopLeft);
                graphics.DrawString("NIP/PESEL: "+line3, fontNormal, brush, listCust3, XStringFormats.TopLeft);
                graphics.DrawString("Tel: "+line4, fontNormal, brush, listCust4, XStringFormats.TopLeft);
                graphics.DrawString("UWAGI: "+line5, fontBold, brush, listCust5, XStringFormats.TopLeft);
                graphics.DrawString("Uwagi: " + line6, fontNormal, brush, listCust6, XStringFormats.TopLeft);
                graphics.DrawString(amount, fontInvoiceType, brush, listCustomerOrdered, XStringFormats.Center);

            }
        }
        public void DrawFooters(PdfPage page, int numpage)
        {
            string wersja = "20180321";
            using (XGraphics graphics = XGraphics.FromPdfPage(page))
                {
                    graphics.DrawLine(pen, new XPoint(28, page.Height - 26), new XPoint(page.Width - 30.75, page.Height - 26));
                    XRect rectFooter = new XRect(28, page.Height - 25, page.Width - 59, 7);
                XStringFormat formatNear = new XStringFormat
                {
                    Alignment = XStringAlignment.Near
                };
                XStringFormat formatFar = new XStringFormat
                {
                    Alignment = XStringAlignment.Far
                };
                graphics.DrawString("Strona "+ numpage, fontSubtitle, brush, rectFooter, formatFar);
                    graphics.DrawString("List przewozowy v1." + wersja +" ©sejto.pl", fontSubtitle, brush, rectFooter, formatNear);
                }
        }

        public void DrawWZName(PdfPage page, string nrWZ, string data, double posYWZ1)  //, string [] ourname, string[] custname
        {
            using (XGraphics graphics = XGraphics.FromPdfPage(page))
            {
                XRect listCust = new XRect(NamePosX1, posYWZ1, 230, heightWZ);
                XRect listCustTxt = new XRect(NamePosX1, posYWZ1+5, 230, heightWZ);
                XRect listNum = new XRect(NamePosX1 + 230, posYWZ1, 80, heightWZ/2);
                XRect listNumTxt = new XRect(NamePosX1 + 230, posYWZ1+15, 80, heightWZ);
                XRect listDate = new XRect(NamePosX1 + 230, posYWZ1+38, 80, heightWZ/2);
                XRect listDateTxt = new XRect(NamePosX1 + 240, posYWZ1 + 50, 60, heightWZ/2);
                XRect listOurTxt = new XRect(posXWZ1, posYWZ1+5, widthYWZ, heightWZ);
                XRect listOur1 = new XRect(posXWZ1, posYWZ1, widthYWZ, heightWZ);
                XRect listOur2 = new XRect(posXWZ1, posYWZ1 + 15, widthYWZ, 5);
                XRect listOur3 = new XRect(posXWZ1, posYWZ1 + 25, widthYWZ, 5);
                XRect listOur4 = new XRect(posXWZ1, posYWZ1 + 35, widthYWZ, 5);
                XRect listOur5 = new XRect(posXWZ1, posYWZ1 + 45, widthYWZ, 5);
                XRect listOur6 = new XRect(posXWZ1, posYWZ1 + 60, widthYWZ, 5);
                XRect listCust1 = new XRect(NamePosX1, posYWZ1 + 15, widthYWZ, 5);
                XRect listCust2 = new XRect(NamePosX1, posYWZ1 + 25, widthYWZ, 5);
                XRect listCust3 = new XRect(NamePosX1, posYWZ1 + 35, widthYWZ, 5);
                XRect listCust4 = new XRect(NamePosX1, posYWZ1 + 45, widthYWZ, 5);
                XRect listCust5 = new XRect(NamePosX1 + 5, posYWZ1 + 60, widthYWZ, 5);

                graphics.DrawRectangle(pen, listOur1);
                graphics.DrawRectangle(pen, listCust);
                graphics.DrawRectangle(pen, XBrushes.LightGray, listNum);
                graphics.DrawRectangle(pen, listDate);
                XFont fontNumber = fontDates;

                graphics.DrawString(lineour[0], fontWZ, brush, listOurTxt, XStringFormats.TopCenter);
                graphics.DrawString(lineour[1], fontWZ, brush, listOur2, XStringFormats.TopCenter);
                graphics.DrawString(lineour[2], fontWZ, brush, listOur3, XStringFormats.TopCenter);
                graphics.DrawString(lineour[3], fontWZ, brush, listOur4, XStringFormats.TopCenter);
                graphics.DrawString(lineour[4], fontWZ, brush, listOur5, XStringFormats.TopCenter);
                graphics.DrawString(lineour[5], fontBold, brush, listOur6, XStringFormats.TopCenter);
                
                graphics.DrawString(line1, fontNormal, brush, listCustTxt, XStringFormats.TopCenter);
                graphics.DrawString(line2, fontNormal, brush, listCust1, XStringFormats.TopCenter);
                graphics.DrawString(line3, fontNormal, brush, listCust2, XStringFormats.TopCenter);
                graphics.DrawString(line4, fontNormal, brush, listCust3, XStringFormats.TopCenter);
                graphics.DrawString(line5, fontNormal, brush, listCust4, XStringFormats.TopCenter);
                graphics.DrawString("Adres dostawy: "+uwagiN, fontUwagi, brush, listCust5, XStringFormats.TopLeft);

                graphics.DrawString("WZ", fontBold, brush, listNum, XStringFormats.TopCenter);
                graphics.DrawString(nrWZ, fontBold, brush, listNumTxt, XStringFormats.TopCenter);
                graphics.DrawString(data, fontDates, brush, listDateTxt, XStringFormats.TopCenter);

            }
        }

        public void DrawWZBody(PdfPage page, double BodyPosY1, Boolean sentval)
        {
            using (XGraphics graphics = XGraphics.FromPdfPage(page))
            {
                XRect[] tabelka;
                tabelka = new XRect[7];
                double newline = 30;
                XRect Head = new XRect(posXWZ1, BodyPosY1, widthYWZ+ widthYWZ+90, 30);
                XRect listName1 = new XRect(posXWZ1, BodyPosY1 + 5, lenListName, 10);
                XRect Lp = new XRect(posXWZ1, BodyPosY1, newline, 30);
                XRect NazwaTow = new XRect(posXWZ1 + 30, BodyPosY1, 150, 30);
                XRect Ilosc = new XRect(posXWZ1 + 180, BodyPosY1, 200, 15);
                XRect Zadysp = new XRect(posXWZ1 + 180, BodyPosY1 + 15, 80, 15);
                XRect Jm = new XRect(posXWZ1 + 260, BodyPosY1 + 15, 45, 15);
                XRect Wydana = new XRect(posXWZ1 + 305, BodyPosY1 + 15, 75, 15);
                XRect Cena = new XRect(posXWZ1 + 380, BodyPosY1, 70, 30);
                XRect Wartosc = new XRect(posXWZ1 + 450, BodyPosY1, 80, 30);
                XRect Uwagi = new XRect(posXWZ1 + 380, BodyPosY1 + newline*3, 80, 30);
                XRect Sent = new XRect(posXWZ1 + 10, BodyPosY1 + newline*3, 80, 30);

                XRect LpVal = new XRect(posXWZ1, BodyPosY1 + newline, 30, 30);
                XRect NazwaTowVal = new XRect(posXWZ1+30, BodyPosY1 + newline, 150, 30);
                XRect ZadyspVal = new XRect(posXWZ1 + 180, BodyPosY1 + newline, 80, 30);
                XRect JmVal = new XRect(posXWZ1 + 260, BodyPosY1 + newline, 45, 30);
                XRect WydanaVal = new XRect(posXWZ1 + 305, BodyPosY1 + newline, 75, 30);
                XRect CenaVal = new XRect(posXWZ1 + 380, BodyPosY1 + newline, 70, 30);
                XRect TerminVal = new XRect(posXWZ1 + 380, BodyPosY1 + newline, 70, 30);
                XRect WartoscVal = new XRect(posXWZ1 + 450, BodyPosY1 + newline, 80, 30);
                newline = newline + newline;
                for (int t=0; t<2;t++)
                    {
                    tabelka[t] = new XRect(posXWZ1, BodyPosY1 + newline, 30, 30);
                    graphics.DrawRectangle(pen, tabelka[t]);
                    tabelka[t] = new XRect(posXWZ1 + 30, BodyPosY1 + newline, 150, 30);
                    graphics.DrawRectangle(pen, tabelka[t]);
                    tabelka[t] = new XRect(posXWZ1 + 180, BodyPosY1 + newline, 80, 30);
                    graphics.DrawRectangle(pen, tabelka[t]);
                    tabelka[t] = new XRect(posXWZ1 + 260, BodyPosY1 + newline, 45, 30);
                    graphics.DrawRectangle(pen, tabelka[t]);
                    tabelka[t] = new XRect(posXWZ1 + 305, BodyPosY1 + newline, 75, 30);
                    graphics.DrawRectangle(pen, tabelka[t]);
                    tabelka[t] = new XRect(posXWZ1 + 380, BodyPosY1 + newline, 70, 30);
                    graphics.DrawRectangle(pen, tabelka[t]);
                    tabelka[t] = new XRect(posXWZ1 + 450, BodyPosY1 + newline, 80, 30);
                    graphics.DrawRectangle(pen, tabelka[t]);
                    newline = newline + 30;
                }

                graphics.DrawRectangle(pen, XBrushes.LightGray, Head);
                graphics.DrawRectangle(pen, Lp);
                graphics.DrawRectangle(pen, NazwaTow);
                graphics.DrawRectangle(pen, Ilosc);
                graphics.DrawRectangle(pen, Zadysp);
                graphics.DrawRectangle(pen, Jm);
                graphics.DrawRectangle(pen, Wydana);
                graphics.DrawRectangle(pen, Cena);
                graphics.DrawRectangle(pen, Wartosc);
                graphics.DrawRectangle(pen, LpVal);
                graphics.DrawRectangle(pen, NazwaTowVal);
                graphics.DrawRectangle(pen, ZadyspVal);
                graphics.DrawRectangle(pen, JmVal);
                graphics.DrawRectangle(pen, WydanaVal);
                graphics.DrawRectangle(pen, TerminVal);
                graphics.DrawRectangle(pen, WartoscVal);


                graphics.DrawString("Lp", fontProductsHeader, brush, Lp, XStringFormats.Center);
                graphics.DrawString("Nazwa towaru", fontProductsHeader, brush, NazwaTow, XStringFormats.Center);
                graphics.DrawString("Ilość", fontProductsHeader, brush, Ilosc, XStringFormats.Center);
                graphics.DrawString("Zadysponowana", fontProductsHeader, brush, Zadysp, XStringFormats.Center);
                graphics.DrawString("j.m.", fontProductsHeader, brush, Jm, XStringFormats.Center);
                graphics.DrawString("Wydana", fontProductsHeader, brush, Wydana, XStringFormats.Center);
                graphics.DrawString("Cena", fontProductsHeader, brush, Cena, XStringFormats.Center);
                graphics.DrawString("Wartość", fontProductsHeader, brush, Wartosc, XStringFormats.Center);
                graphics.DrawString(cenapaliwa, fontProductsHeader, brush, CenaVal, XStringFormats.Center);
                graphics.DrawString(paliwo, fontWZproduct, brush, NazwaTowVal, XStringFormats.Center);
                graphics.DrawString("1", fontWZproduct, brush, LpVal, XStringFormats.Center);
                graphics.DrawString(ilosc, fontWZproduct, brush, ZadyspVal, XStringFormats.Center);
                graphics.DrawString("l", fontWZproduct, brush, JmVal, XStringFormats.Center);
                if (sentval)
                    graphics.DrawString("SENT", fontWZproduct, brush, Sent, XStringFormats.Center);
                else
                    graphics.DrawString("", fontWZproduct, brush, Sent, XStringFormats.Center);
                graphics.DrawString(uwagi, fontProductsHeader, brush, Uwagi, XStringFormats.Center);
            }
        } //Drukuje zawartość WZ

        public void DrawWZFooter(PdfPage page,string wystawil, double FooterPosY1)
        {
            using (XGraphics graphics = XGraphics.FromPdfPage(page))
            {
                XRect Wystawil = new XRect(posXWZ1, FooterPosY1, 100, 15);
                XRect Zatwierdzil = new XRect(130, FooterPosY1, 100, 15);
                XRect Ilosci = new XRect(230, FooterPosY1, 330, 15);
                XRect Wydal = new XRect(230, FooterPosY1 + 15, 80, 15);
                XRect Data = new XRect(310, FooterPosY1 + 15, 80, 15);
                XRect Odebral = new XRect(390, FooterPosY1 + 15, 170, 15);
                XRect WystawilVal = new XRect(posXWZ1, FooterPosY1, 100, 60);
                XRect ZatwierdzilVal = new XRect(130, FooterPosY1, 100, 60);
                XRect WydalVal = new XRect(230, FooterPosY1 + 30, 80, 30);
                XRect DataVal = new XRect(310, FooterPosY1 + 30, 80, 30);
                XRect OdebralVal = new XRect(390, FooterPosY1 + 15, 170, 45);
                XRect DaneDoPrzelewu1 = new XRect(posXWZ1, FooterPosY1+85, 530, 45);
                XRect DaneDoPrzelewu2 = new XRect(posXWZ1, FooterPosY1 + 100, 530, 45);

                graphics.DrawRectangle(Print.pen, XBrushes.LightGray, Wystawil);
                graphics.DrawRectangle(Print.pen, XBrushes.LightGray, Zatwierdzil);
                graphics.DrawRectangle(Print.pen, XBrushes.LightGray, Ilosci);
                graphics.DrawRectangle(Print.pen, XBrushes.LightGray, Wydal);
                graphics.DrawRectangle(Print.pen, XBrushes.LightGray, Data);
                graphics.DrawRectangle(Print.pen, XBrushes.LightGray, Odebral);
                graphics.DrawRectangle(Print.pen, WystawilVal);
                graphics.DrawRectangle(Print.pen, ZatwierdzilVal);
                graphics.DrawRectangle(Print.pen, WydalVal);
                graphics.DrawRectangle(Print.pen, DataVal);
                graphics.DrawRectangle(Print.pen, OdebralVal);

                graphics.DrawString("Wystawił", fontProductsHeader, brush, Wystawil, XStringFormats.Center);
                graphics.DrawString("Zatwierdził", fontProductsHeader, brush, Zatwierdzil, XStringFormats.Center);
                graphics.DrawString("Wymienione ilości", fontProductsHeader, brush, Ilosci, XStringFormats.Center);
                graphics.DrawString("Wydał", fontProductsHeader, brush, Wydal, XStringFormats.Center);
                graphics.DrawString("Data", fontProductsHeader, brush, Data, XStringFormats.Center);
                graphics.DrawString("Odebrał", fontProductsHeader, brush, Odebral, XStringFormats.Center);
                graphics.DrawString(wystawil, fontProductsHeader, brush, WystawilVal, XStringFormats.Center);
                graphics.DrawString("Dane do przelewu: "+ lineour[0]+" "+lineour[1]+", "+lineour[2], fontBold, brush, DaneDoPrzelewu1, XStringFormats.TopLeft);
                graphics.DrawString(lineour[5] , fontBold, brush, DaneDoPrzelewu2, XStringFormats.TopLeft);

                XPen pen = XPens.LightGray.Clone();
                pen.DashStyle = XDashStyle.DashDot;
                graphics.DrawLine(pen, new XPoint(30, page.Height / 2), new XPoint(page.Width - 30.75, page.Height / 2));
            }
            
        } 
    }

}
