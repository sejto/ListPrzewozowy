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
        private static double lenListDischarge = 75;
        private static double posYBody=55;
        private static double lineCustomer = 5;
        private static double lineCustN = 10;
        //------------WZ---------------------------
        private string Number = "WZ/2017/546";
        private static double posXWZ1 = 30;
        private static double posYWZ1 = 30;
        private static double posXWZ2 = 30;
        private static double posYWZ2 = 30;
        private static double widthYWZ = 220;
        private static double heightWZ = 76;
        private static double newline = 30;

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
        public void DrawBody(PdfPage page, int litry)
        {
            using (XGraphics graphics = XGraphics.FromPdfPage(page))
            {
                XRect listName = new XRect(posX, posYBody, lenListName, 30);
                XRect listName1 = new XRect(posX, posYBody+5, lenListName, 10);
                XRect listName2 = new XRect(posX, posYBody+15, lenListName, 10); 
                XRect listAmountH = new XRect(posX+ lenListName, posYBody, 170, 15);
                XRect listAmount = new XRect(posX + lenListName+40, posYBody, 170, 15);
                XRect listDischarge = new XRect(posX + lenListName, posYBody+15, lenListDischarge, 15);
                XRect listRest = new XRect(posX+lenListName+lenListDischarge, posYBody+15, 95, 15);

                graphics.DrawRectangle(_pen, XBrushes.LightGray, listName);
                graphics.DrawRectangle(_pen,  listAmountH);
                graphics.DrawRectangle(_pen, XBrushes.LightGray, listDischarge);
                graphics.DrawRectangle(_pen, XBrushes.LightGray, listRest);

                graphics.DrawString("Nazwa i adres odbiorcy ", _fontNormal, _brush, listName1, XStringFormats.Center);
                graphics.DrawString("(miejsce rozładunku)", _fontNormal, _brush, listName2, XStringFormats.Center);
                graphics.DrawString("Ilość (litry): ", _fontBold, _brush, listAmountH, XStringFormats.Center);
                graphics.DrawString(""+litry, _fontInvoiceType, _brush, listAmount, XStringFormats.Center);
                graphics.DrawString("Rozładowane ", _fontNormal, _brush, listDischarge, XStringFormats.Center);
                graphics.DrawString("Pozostało w cysternie ", _fontNormal, _brush, listRest, XStringFormats.Center);
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
                XRect linesent = new XRect(posX + 10, posYCustomer + 55, lenListName, 5); //ustawić współrzedne

                XRect listCustomerAmount = new XRect(posX+ lenListName, posYCustomer, lenListDischarge, 70);
                XRect listCustomerRest = new XRect(posX + lenListName + lenListDischarge, posYCustomer, 95, 70);

                graphics.DrawRectangle(_pen, listCustomer);
                graphics.DrawRectangle(_pen, listCustomerAmount);
                graphics.DrawRectangle(_pen, listCustomerRest);

                graphics.DrawString(line1, _fontNormal, _brush, listCust1, XStringFormats.TopLeft  );
                graphics.DrawString(line2, _fontNormal, _brush, listCust2, XStringFormats.TopLeft);
                graphics.DrawString("Nip: "+line3, _fontNormal, _brush, listCust3, XStringFormats.TopLeft);
                graphics.DrawString("Tel: "+line4, _fontNormal, _brush, listCust4, XStringFormats.TopLeft);
                graphics.DrawString("UWAGI: "+line5, _fontBold, _brush, listCust5, XStringFormats.TopLeft);
                graphics.DrawString("Uwagi: " + line6, _fontNormal, _brush, listCust6, XStringFormats.TopLeft);
                graphics.DrawString(amount, _fontInvoiceType, _brush, listCustomerAmount, XStringFormats.Center);

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
                    graphics.DrawString("List przewozowy v1.0", _fontSubtitle, _brush, rectFooter, formatNear);
                }
        }

        public void DrawWZName(PdfPage page, string nrWZ)  //, string [] ourname, string[] custname
        {
            using (XGraphics graphics = XGraphics.FromPdfPage(page))
            {

                XRect listOur = new XRect(posXWZ1, posYWZ1, widthYWZ, heightWZ);
                XRect listOurTxt = new XRect(posXWZ1, posYWZ1+5, widthYWZ, heightWZ);
                XRect listCust = new XRect(posXWZ1+widthYWZ, posYWZ1, 230, heightWZ);
                XRect listCustTxt = new XRect(posXWZ1 + widthYWZ, posYWZ1+5, 230, heightWZ);
                XRect listNum = new XRect(posXWZ1 + widthYWZ+230, posYWZ1, 80, heightWZ/2);
                XRect listNumTxt = new XRect(posXWZ1 + widthYWZ + 230, posYWZ1+15, 80, heightWZ);
                XRect listDate = new XRect(posXWZ1 + widthYWZ + 230, posYWZ1+38, 80, heightWZ/2);
                XRect listDateTxt = new XRect(posXWZ1 + widthYWZ + 240, posYWZ1 + 50, 60, heightWZ/2);
                XRect listOur1 = new XRect(posXWZ1, posYWZ1 + 15, widthYWZ, 5);
                XRect listOur2 = new XRect(posXWZ1, posYWZ1 + 25, widthYWZ, 5);
                XRect listOur3 = new XRect(posXWZ1, posYWZ1 + 35, widthYWZ, 5);
                XRect listOur4 = new XRect(posXWZ1, posYWZ1 + 45, widthYWZ, 5);
                XRect listOur5 = new XRect(posXWZ1, posYWZ1 + 60, widthYWZ, 5);
                XRect listCust1 = new XRect(posXWZ1 + widthYWZ, posYWZ1 + 15, widthYWZ, 5);
                XRect listCust2 = new XRect(posXWZ1 + widthYWZ, posYWZ1 + 25, widthYWZ, 5);
                XRect listCust3 = new XRect(posXWZ1 + widthYWZ, posYWZ1 + 35, widthYWZ, 5);
                XRect listCust4 = new XRect(posXWZ1 + widthYWZ, posYWZ1 + 45, widthYWZ, 5);
                XRect listCust5 = new XRect(posXWZ1 + widthYWZ+5, posYWZ1 + 60, widthYWZ, 5);

                graphics.DrawRectangle(_pen, listOur);
                graphics.DrawRectangle(_pen, listCust);
                graphics.DrawRectangle(_pen, XBrushes.LightGray, listNum);
                graphics.DrawRectangle(_pen, listDate);
                XFont fontNumber = _fontDates;

                graphics.DrawString("Oil Transfer Development", _fontWZ, _brush, listOurTxt, XStringFormats.TopCenter);
                graphics.DrawString("Stacja Paliw sp. z o.o.", _fontWZ, _brush, listOur1, XStringFormats.TopCenter);
                graphics.DrawString("Pułaskiego 107, 16-400 Suwałki", _fontWZ, _brush, listOur2, XStringFormats.TopCenter);
                graphics.DrawString("tel: 87 567 13 61", _fontWZ, _brush, listOur3, XStringFormats.TopCenter);
                graphics.DrawString("NIP: 844 235 55 66", _fontWZ, _brush, listOur4, XStringFormats.TopCenter);
                graphics.DrawString("PKO BP 87 1020 1332 0000 1702 1011 0536", _fontBold, _brush, listOur5, XStringFormats.TopCenter);

                graphics.DrawString("Nazwa firmy", _fontWZ, _brush, listCustTxt, XStringFormats.TopCenter);
                graphics.DrawString("linia druga", _fontWZ, _brush, listCust1, XStringFormats.TopCenter);
                graphics.DrawString("linia trzecia blablabla", _fontWZ, _brush, listCust2, XStringFormats.TopCenter);
                graphics.DrawString("linia czwarta blablabla", _fontWZ, _brush, listCust3, XStringFormats.TopCenter);
                graphics.DrawString("linia piata blablabla", _fontWZ, _brush, listCust4, XStringFormats.TopCenter);
                graphics.DrawString("Adres dostawy:  blablabla", _fontWZ, _brush, listCust5, XStringFormats.TopLeft);

                graphics.DrawString("WZ", _fontBold, _brush, listNum, XStringFormats.TopCenter);
                graphics.DrawString("659/2017", _fontBold, _brush, listNumTxt, XStringFormats.TopCenter);
                graphics.DrawString("10.11.2017", _fontDates, _brush, listDateTxt, XStringFormats.TopCenter);

            }
        }

        public void DrawWZBody(PdfPage page, string paliwo, string ilosc, string uwagi)
        {
            using (XGraphics graphics = XGraphics.FromPdfPage(page))
            {
                XRect[] tabelka;
                tabelka = new XRect[7];
                XRect Head = new XRect(posXWZ1, posYWZ1+ heightWZ, widthYWZ+ widthYWZ+90, 30);
                XRect listName1 = new XRect(posXWZ1, posYWZ1+ heightWZ + 5, lenListName, 10);
                XRect Lp = new XRect(posXWZ1, posYWZ1 + heightWZ, newline, 30);
                XRect NazwaTow = new XRect(posXWZ1 + 30, posYWZ1 + heightWZ, 150, 30);
                XRect Ilosc = new XRect(posXWZ1 + 180, posYWZ1 + heightWZ, 200, 15);
                XRect Zadysp = new XRect(posXWZ1 + 180, posYWZ1 + heightWZ+15, 80, 15);
                XRect Jm = new XRect(posXWZ1 + 260, posYWZ1 + heightWZ + 15, 45, 15);
                XRect Wydana = new XRect(posXWZ1 + 305, posYWZ1 + heightWZ + 15, 75, 15);
                XRect Cena = new XRect(posXWZ1 + 380, posYWZ1 + heightWZ, 70, 30);
                XRect Wartosc = new XRect(posXWZ1 + 450, posYWZ1 + heightWZ, 80, 30);
                XRect Uwagi = new XRect(posXWZ1 + 380, posYWZ1 + heightWZ+newline*3, 80, 30);
                XRect Sent = new XRect(posXWZ1 + 10, posYWZ1 + heightWZ+newline*3, 80, 30);

                XRect LpVal = new XRect(posXWZ1, posYWZ1 + heightWZ+ newline, 30, 30);
                XRect NazwaTowVal = new XRect(posXWZ1+30, posYWZ1 + heightWZ+ newline, 150, 30);
                XRect ZadyspVal = new XRect(posXWZ1 + 180, posYWZ1 + heightWZ + newline, 80, 30);
                XRect JmVal = new XRect(posXWZ1 + 260, posYWZ1 + heightWZ + newline, 45, 30);
                XRect WydanaVal = new XRect(posXWZ1 + 305, posYWZ1 + heightWZ + newline, 75, 30);
                XRect CenaVal = new XRect(posXWZ1 + 380, posYWZ1 + heightWZ+ newline, 70, 30);
                XRect WartoscVal = new XRect(posXWZ1 + 450, posYWZ1 + heightWZ+ newline, 80, 30);
                newline = newline + newline;
                for (int i=0; i<2;i++)
                    {
                    tabelka[i] = new XRect(posXWZ1, posYWZ1 + heightWZ + newline, 30, 30);
                    graphics.DrawRectangle(_pen, tabelka[i]);
                    tabelka[i] = new XRect(posXWZ1 + 30, posYWZ1 + heightWZ + newline, 150, 30);
                    graphics.DrawRectangle(_pen, tabelka[i]);
                    tabelka[i] = new XRect(posXWZ1 + 180, posYWZ1 + heightWZ + newline, 80, 30);
                    graphics.DrawRectangle(_pen, tabelka[i]);
                    tabelka[i] = new XRect(posXWZ1 + 260, posYWZ1 + heightWZ + newline, 45, 30);
                    graphics.DrawRectangle(_pen, tabelka[i]);
                    tabelka[i] = new XRect(posXWZ1 + 305, posYWZ1 + heightWZ + newline, 75, 30);
                    graphics.DrawRectangle(_pen, tabelka[i]);
                    tabelka[i] = new XRect(posXWZ1 + 380, posYWZ1 + heightWZ + newline, 70, 30);
                    graphics.DrawRectangle(_pen, tabelka[i]);
                    tabelka[i] = new XRect(posXWZ1 + 450, posYWZ1 + heightWZ + newline, 80, 30);
                    graphics.DrawRectangle(_pen, tabelka[i]);
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
                graphics.DrawRectangle(_pen, CenaVal);
                graphics.DrawRectangle(_pen, WartoscVal);


                graphics.DrawString("Lp", _fontProductsHeader, _brush, Lp, XStringFormats.Center);
                graphics.DrawString("Nazwa towaru", _fontProductsHeader, _brush, NazwaTow, XStringFormats.Center);
                graphics.DrawString("Ilosc", _fontProductsHeader, _brush, Ilosc, XStringFormats.Center);
                graphics.DrawString("Zadysponowana", _fontProductsHeader, _brush, Zadysp, XStringFormats.Center);
                graphics.DrawString("j.m.", _fontProductsHeader, _brush, Jm, XStringFormats.Center);
                graphics.DrawString("Wydana", _fontProductsHeader, _brush, Wydana, XStringFormats.Center);
                graphics.DrawString("Cena", _fontProductsHeader, _brush, Cena, XStringFormats.Center);
                graphics.DrawString("Olej napędowy", _fontWZproduct, _brush, NazwaTowVal, XStringFormats.Center);
                graphics.DrawString("1", _fontWZproduct, _brush, LpVal, XStringFormats.Center);
                graphics.DrawString("4000", _fontWZproduct, _brush, ZadyspVal, XStringFormats.Center);
                graphics.DrawString("l", _fontWZproduct, _brush, JmVal, XStringFormats.Center);
                graphics.DrawString("Sent", _fontWZproduct, _brush, Sent, XStringFormats.Center);
                graphics.DrawString("Przelew 2 dni", _fontProductsHeader, _brush, Uwagi, XStringFormats.Center);
            }
        } //Drukuje zawartość WZ

        public void DrawWZFooter(PdfPage page)
        {
            using (XGraphics graphics = XGraphics.FromPdfPage(page))
            {
                XRect Head = new XRect(posXWZ1, posYWZ1 + heightWZ, widthYWZ + widthYWZ + 90, 30);

                graphics.DrawRectangle(_pen, XBrushes.LightGray, Head);

                graphics.DrawString("Wystawił", _fontProductsHeader, _brush, Head, XStringFormats.Center);
            }
        } //Dokończyć rysowanie stopki WZ
    }
   /* public class PdfCreator
    {
        private PdfDocument document;
        private static readonly XPen _pen = new XPen(XColors.Black, 0.5);
        private static readonly XBrush _brush = XBrushes.Black;
        private static readonly XFont _fontNormal = new XFont("Arial", 9, XFontStyle.Regular, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static readonly XFont _fontBold = new XFont("Arial", 9, XFontStyle.Bold, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static readonly XFont _fontHeader = new XFont("Arial", 14, XFontStyle.Bold, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static readonly XFont _fontInvoiceType = new XFont("Arial", 12, XFontStyle.Bold, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static readonly XFont _fontDates = new XFont("Arial", 10, XFontStyle.Bold, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static readonly XFont _fontProductsHeader = new XFont("Arial", 8, XFontStyle.Bold, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static readonly XFont _fontSubtitle = new XFont("Arial", 6, XFontStyle.Regular, new XPdfFontOptions(PdfFontEncoding.Unicode));

    }
    */

}
