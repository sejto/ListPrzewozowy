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
    class print
    {
        private static readonly XPen _pen = new XPen(XColors.Black, 0.5);
        private static readonly XBrush _brush = XBrushes.Black;
        private static readonly XFont _fontNormal = new XFont("Arial", 9, XFontStyle.Regular, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static readonly XFont _fontWZ = new XFont("Arial", 11, XFontStyle.Regular, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static readonly XFont _fontBold = new XFont("Arial", 10, XFontStyle.Bold, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static readonly XFont _fontHeader = new XFont("Arial", 14, XFontStyle.Bold, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static readonly XFont _fontInvoiceType = new XFont("Arial", 12, XFontStyle.Bold, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static readonly XFont _fontDates = new XFont("Arial", 10, XFontStyle.Bold, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static readonly XFont _fontProductsHeader = new XFont("Arial", 8, XFontStyle.Bold, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static readonly XFont _fontWZproduct = new XFont("Arial", 11, XFontStyle.Bold, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static readonly XFont _fontSubtitle = new XFont("Arial", 6, XFontStyle.Regular, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static double posX = 30;
        private static double lenListName = 360;
        private static double lenListDischarge = 60;
        private static double posYBody=55;
        //------------WZ---------------------------
        private static double posXWZ1 = 30;
        private static double posYWZ1 ;
        private static double posXWZ2 = 30;
        private static double posYWZ2 = 30;
        private static double widthYWZ = 220;
        private static double heightWZ = 76;
//        private static double newline = 30;
        private static double NamePosX1 = 250;
        private static double BodyPosY1 = 106;
        private static double FooterPosY1 = 226;
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

                graphics.DrawRectangle(_pen, listHead);
                XFont fontNumber = _fontDates;

                graphics.DrawString("Wykaz odbiorców UN 1202 w sprzedaży obwoźnej", _fontBold, _brush, listShow, XStringFormats.Center);
                graphics.DrawString("Załącznik do listu przewozowego z dnia ", _fontNormal, _brush, listHeader, XStringFormats.Center);
                graphics.DrawString(SaleDate, _fontHeader, _brush, listDate, XStringFormats.Center);
            }
        }
        public void DrawBody(PdfPage page, int litryON, int litryOP)
        {
            using (XGraphics graphics = XGraphics.FromPdfPage(page))
            {
                XRect listName = new XRect(posX, posYBody, lenListName, 30);
                XRect listName1 = new XRect(posX, posYBody+5, lenListName, 10);
                XRect listName2 = new XRect(posX, posYBody+15, lenListName, 10); 
                XRect listAmountH = new XRect(posX+ lenListName, posYBody, 170, 15);
                XRect listAmountHON = new XRect(posX + lenListName, posYBody, 35, 15);
                XRect listAmountON = new XRect(posX + lenListName+20, posYBody, 85, 15);
                XRect listAmountOP = new XRect(posX + lenListName + 85, posYBody, 85, 15);

                XRect listOrdered = new XRect(posX + lenListName, posYBody + 15, 55, 15);
                XRect listDischarge = new XRect(posX + lenListName+55, posYBody+15, 58, 15);
                XRect listRest = new XRect(posX+lenListName+lenListDischarge+53, posYBody+15, 57, 15);

                graphics.DrawRectangle(_pen, XBrushes.LightGray, listName);
                graphics.DrawRectangle(_pen,  listAmountH);
                graphics.DrawRectangle(_pen, XBrushes.LightGray, listOrdered);
                graphics.DrawRectangle(_pen, XBrushes.LightGray, listDischarge);
                graphics.DrawRectangle(_pen, XBrushes.LightGray, listRest);

                graphics.DrawString("Nazwa i adres odbiorcy", _fontNormal, _brush, listName1, XStringFormats.Center);
                graphics.DrawString("(miejsce rozładunku)", _fontNormal, _brush, listName2, XStringFormats.Center);
                graphics.DrawString("Ilość", _fontBold, _brush, listAmountHON, XStringFormats.Center);
                graphics.DrawString("ON: "+litryON, _fontInvoiceType, _brush, listAmountON, XStringFormats.Center);
                graphics.DrawString("OP: " + litryOP, _fontInvoiceType, _brush, listAmountOP, XStringFormats.Center);
                graphics.DrawString("Zamówione", _fontNormal, _brush, listOrdered, XStringFormats.Center);
                graphics.DrawString("Rozładowane", _fontNormal, _brush, listDischarge, XStringFormats.Center);
                graphics.DrawString("Pozostało", _fontNormal, _brush, listRest, XStringFormats.Center);
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

                graphics.DrawRectangle(_pen, listCustomer);

                graphics.DrawRectangle(_pen, listCustomerOrdered);
                graphics.DrawRectangle(_pen, listCustomerDischarge);
                graphics.DrawRectangle(_pen, listCustomerRest);

                graphics.DrawString(line1, _fontProductsHeader, _brush, listCust1, XStringFormats.TopLeft  );
                graphics.DrawString(line2, _fontNormal, _brush, listCust2, XStringFormats.TopLeft);
                graphics.DrawString("Nip: "+line3, _fontNormal, _brush, listCust3, XStringFormats.TopLeft);
                graphics.DrawString("Tel: "+line4, _fontNormal, _brush, listCust4, XStringFormats.TopLeft);
                graphics.DrawString("UWAGI: "+line5, _fontBold, _brush, listCust5, XStringFormats.TopLeft);
                graphics.DrawString("Uwagi: " + line6, _fontNormal, _brush, listCust6, XStringFormats.TopLeft);
                graphics.DrawString(amount, _fontInvoiceType, _brush, listCustomerOrdered, XStringFormats.Center);

            }
        }
        public void DrawFooters(PdfPage page, int numpage)
        {
                //PdfPage numpage = page[1];

                using (XGraphics graphics = XGraphics.FromPdfPage(page))
                {
                    graphics.DrawLine(_pen, new XPoint(28, page.Height - 26), new XPoint(page.Width - 30.75, page.Height - 26));
                    XRect rectFooter = new XRect(28, page.Height - 25, page.Width - 59, 7);
                    XStringFormat formatNear = new XStringFormat();
                    formatNear.Alignment = XStringAlignment.Near;
                    XStringFormat formatFar = new XStringFormat();
                    formatFar.Alignment = XStringAlignment.Far;

                    graphics.DrawString("Strona "+ numpage, _fontSubtitle, _brush, rectFooter, formatFar);
                    graphics.DrawString("List przewozowy v1.0 @sejto.pl", _fontSubtitle, _brush, rectFooter, formatNear);
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

                graphics.DrawRectangle(_pen, listOur1);
                graphics.DrawRectangle(_pen, listCust);
                graphics.DrawRectangle(_pen, XBrushes.LightGray, listNum);
                graphics.DrawRectangle(_pen, listDate);
                XFont fontNumber = _fontDates;

                graphics.DrawString(lineour[0], _fontWZ, _brush, listOurTxt, XStringFormats.TopCenter);
                graphics.DrawString(lineour[1], _fontWZ, _brush, listOur2, XStringFormats.TopCenter);
                graphics.DrawString(lineour[2], _fontWZ, _brush, listOur3, XStringFormats.TopCenter);
                graphics.DrawString(lineour[3], _fontWZ, _brush, listOur4, XStringFormats.TopCenter);
                graphics.DrawString(lineour[4], _fontWZ, _brush, listOur5, XStringFormats.TopCenter);
                graphics.DrawString(lineour[5], _fontBold, _brush, listOur6, XStringFormats.TopCenter);
                
                graphics.DrawString(line1, _fontNormal, _brush, listCustTxt, XStringFormats.TopCenter);
                graphics.DrawString(line2, _fontNormal, _brush, listCust1, XStringFormats.TopCenter);
                graphics.DrawString(line3, _fontNormal, _brush, listCust2, XStringFormats.TopCenter);
                graphics.DrawString(line4, _fontNormal, _brush, listCust3, XStringFormats.TopCenter);
                graphics.DrawString(line5, _fontNormal, _brush, listCust4, XStringFormats.TopCenter);
                graphics.DrawString("Adres dostawy: "+uwagiN, _fontNormal, _brush, listCust5, XStringFormats.TopLeft);

                graphics.DrawString("WZ", _fontBold, _brush, listNum, XStringFormats.TopCenter);
                graphics.DrawString(nrWZ, _fontBold, _brush, listNumTxt, XStringFormats.TopCenter);
                graphics.DrawString(data, _fontDates, _brush, listDateTxt, XStringFormats.TopCenter);

            }
        }

        public void DrawWZBody(PdfPage page, double BodyPosY1)
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
                    graphics.DrawRectangle(_pen, tabelka[t]);
                    tabelka[t] = new XRect(posXWZ1 + 30, BodyPosY1 + newline, 150, 30);
                    graphics.DrawRectangle(_pen, tabelka[t]);
                    tabelka[t] = new XRect(posXWZ1 + 180, BodyPosY1 + newline, 80, 30);
                    graphics.DrawRectangle(_pen, tabelka[t]);
                    tabelka[t] = new XRect(posXWZ1 + 260, BodyPosY1 + newline, 45, 30);
                    graphics.DrawRectangle(_pen, tabelka[t]);
                    tabelka[t] = new XRect(posXWZ1 + 305, BodyPosY1 + newline, 75, 30);
                    graphics.DrawRectangle(_pen, tabelka[t]);
                    tabelka[t] = new XRect(posXWZ1 + 380, BodyPosY1 + newline, 70, 30);
                    graphics.DrawRectangle(_pen, tabelka[t]);
                    tabelka[t] = new XRect(posXWZ1 + 450, BodyPosY1 + newline, 80, 30);
                    graphics.DrawRectangle(_pen, tabelka[t]);
                    newline = newline + 30;
                }

                graphics.DrawRectangle(_pen, XBrushes.LightGray, Head);
                graphics.DrawRectangle(_pen, Lp);
                graphics.DrawRectangle(_pen, NazwaTow);
                graphics.DrawRectangle(_pen, Ilosc);
                graphics.DrawRectangle(_pen, Zadysp);
                graphics.DrawRectangle(_pen, Jm);
                graphics.DrawRectangle(_pen, Wydana);
                graphics.DrawRectangle(_pen, Cena);
                graphics.DrawRectangle(_pen, Wartosc);
                graphics.DrawRectangle(_pen, LpVal);
                graphics.DrawRectangle(_pen, NazwaTowVal);
                graphics.DrawRectangle(_pen, ZadyspVal);
                graphics.DrawRectangle(_pen, JmVal);
                graphics.DrawRectangle(_pen, WydanaVal);
                graphics.DrawRectangle(_pen, TerminVal);
                graphics.DrawRectangle(_pen, WartoscVal);


                graphics.DrawString("Lp", _fontProductsHeader, _brush, Lp, XStringFormats.Center);
                graphics.DrawString("Nazwa towaru", _fontProductsHeader, _brush, NazwaTow, XStringFormats.Center);
                graphics.DrawString("Ilość", _fontProductsHeader, _brush, Ilosc, XStringFormats.Center);
                graphics.DrawString("Zadysponowana", _fontProductsHeader, _brush, Zadysp, XStringFormats.Center);
                graphics.DrawString("j.m.", _fontProductsHeader, _brush, Jm, XStringFormats.Center);
                graphics.DrawString("Wydana", _fontProductsHeader, _brush, Wydana, XStringFormats.Center);
                graphics.DrawString("Cena", _fontProductsHeader, _brush, Cena, XStringFormats.Center);
                graphics.DrawString("Wartość", _fontProductsHeader, _brush, Wartosc, XStringFormats.Center);
                graphics.DrawString(cenapaliwa, _fontProductsHeader, _brush, CenaVal, XStringFormats.Center);
                graphics.DrawString(paliwo, _fontWZproduct, _brush, NazwaTowVal, XStringFormats.Center);
                graphics.DrawString("1", _fontWZproduct, _brush, LpVal, XStringFormats.Center);
                graphics.DrawString(ilosc, _fontWZproduct, _brush, ZadyspVal, XStringFormats.Center);
                graphics.DrawString("l", _fontWZproduct, _brush, JmVal, XStringFormats.Center);
                graphics.DrawString("Sent", _fontWZproduct, _brush, Sent, XStringFormats.Center);
                graphics.DrawString(uwagi, _fontProductsHeader, _brush, Uwagi, XStringFormats.Center);
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

                graphics.DrawRectangle(_pen, XBrushes.LightGray, Wystawil);
                graphics.DrawRectangle(_pen, XBrushes.LightGray, Zatwierdzil);
                graphics.DrawRectangle(_pen, XBrushes.LightGray, Ilosci);
                graphics.DrawRectangle(_pen, XBrushes.LightGray, Wydal);
                graphics.DrawRectangle(_pen, XBrushes.LightGray, Data);
                graphics.DrawRectangle(_pen, XBrushes.LightGray, Odebral);
                graphics.DrawRectangle(_pen, WystawilVal);
                graphics.DrawRectangle(_pen, ZatwierdzilVal);
                graphics.DrawRectangle(_pen, WydalVal);
                graphics.DrawRectangle(_pen, DataVal);
                graphics.DrawRectangle(_pen, OdebralVal);
             //   graphics.DrawRectangle(_pen, DaneDoPrzelewu1);

                graphics.DrawString("Wystawił", _fontProductsHeader, _brush, Wystawil, XStringFormats.Center);
                graphics.DrawString("Zatwierdził", _fontProductsHeader, _brush, Zatwierdzil, XStringFormats.Center);
                graphics.DrawString("Wymienione ilości", _fontProductsHeader, _brush, Ilosci, XStringFormats.Center);
                graphics.DrawString("Wydał", _fontProductsHeader, _brush, Wydal, XStringFormats.Center);
                graphics.DrawString("Data", _fontProductsHeader, _brush, Data, XStringFormats.Center);
                graphics.DrawString("Odebrał", _fontProductsHeader, _brush, Odebral, XStringFormats.Center);
                graphics.DrawString(wystawil, _fontProductsHeader, _brush, WystawilVal, XStringFormats.Center);
                graphics.DrawString("Dane do przelewu: "+ lineour[0]+" "+lineour[1]+", "+lineour[2], _fontBold, _brush, DaneDoPrzelewu1, XStringFormats.TopLeft);
                graphics.DrawString(lineour[5] , _fontBold, _brush, DaneDoPrzelewu2, XStringFormats.TopLeft);

                XPen pen = XPens.LightGray.Clone();
                pen.DashStyle = XDashStyle.DashDot;
                graphics.DrawLine(pen, new XPoint(30, page.Height / 2), new XPoint(page.Width - 30.75, page.Height / 2));
            }
            
        } //Dokończyć rysowanie stopki WZ
    }

}
