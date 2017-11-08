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
        private static readonly XFont _fontBold = new XFont("Arial", 9, XFontStyle.Bold, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static readonly XFont _fontHeader = new XFont("Arial", 14, XFontStyle.Bold, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static readonly XFont _fontInvoiceType = new XFont("Arial", 12, XFontStyle.Bold, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static readonly XFont _fontDates = new XFont("Arial", 10, XFontStyle.Bold, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static readonly XFont _fontProductsHeader = new XFont("Arial", 8, XFontStyle.Bold, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static readonly XFont _fontSubtitle = new XFont("Arial", 6, XFontStyle.Regular, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static double posX = 30;
        private static double lenListName = 360;
        private static double lenListDischarge = 75;
        private static double posYCustomer=80;
        private static double lineCustomer = 5;
        private static double lineCustN = 10;

        private string CustomerId = "1";
        private string Id = "1";
        private string Number = "WZ/2017/546";
        
        private string amount = "1500";

        public void DrawHeader(PdfPage page, string SaleDate)
        {
            using (XGraphics graphics = XGraphics.FromPdfPage(page))
            {
                
                XRect listHeader = new XRect(posX, 15, 530, 30);
                XRect listShow = new XRect(posX, 35, 530, 10);

                graphics.DrawRectangle(_pen, listHeader);
                XFont fontNumber = _fontDates;

                graphics.DrawString("Wykaz odbiorców UN 1202 w sprzedaży obwoźnej", _fontBold, _brush, listShow, XStringFormats.Center);
                graphics.DrawString("Załącznik do listu przewozowego z dnia " +SaleDate, _fontNormal, _brush, listHeader, XStringFormats.Center);
            }
        }

        public void DrawBody(PdfPage page, string litry)
        {
            using (XGraphics graphics = XGraphics.FromPdfPage(page))
            {
                XRect listName = new XRect(posX, 50, lenListName, 30);
                XRect listName1 = new XRect(posX, 55, lenListName, 10);
                XRect listName2 = new XRect(posX, 65, lenListName, 10);
                XRect listAmount = new XRect(posX+ lenListName, 50, 170, 15);
                XRect listDischarge = new XRect(posX + lenListName, 65, lenListDischarge, 15);
                XRect listRest = new XRect(posX+lenListName+lenListDischarge, 65, 95, 15);

                graphics.DrawRectangle(_pen, XBrushes.LightGray, listName);
                graphics.DrawRectangle(_pen,  listAmount);
                graphics.DrawRectangle(_pen, XBrushes.LightGray, listDischarge);
                graphics.DrawRectangle(_pen, XBrushes.LightGray, listRest);

                graphics.DrawString("Nazwa i adres odbiorcy ", _fontNormal, _brush, listName1, XStringFormats.Center);
                graphics.DrawString("(miejsce rozładunku)", _fontNormal, _brush, listName2, XStringFormats.Center);
                graphics.DrawString("Ilość (litry): "+litry, _fontBold, _brush, listAmount, XStringFormats.Center);
                graphics.DrawString("Rozładowane ", _fontNormal, _brush, listDischarge, XStringFormats.Center);
                graphics.DrawString("Pozostało w cysternie ", _fontNormal, _brush, listRest, XStringFormats.Center);
            }

        }

        public void DrawCustomer(PdfPage page, double posYCustomer, string line1, string line2, string line3, string line4)
        {
            using (XGraphics graphics = XGraphics.FromPdfPage(page))
            {
                XRect listCustomer = new XRect(posX, posYCustomer, lenListName, 55);
                XRect listCust1 = new XRect(posX, posYCustomer+5, lenListName, 5);
                XRect listCust2 = new XRect(posX, posYCustomer+15, lenListName, 5);
                XRect listCust3 = new XRect(posX, posYCustomer+25, lenListName, 5);
                XRect listCust4 = new XRect(posX, posYCustomer+35, lenListName, 5);
                XRect listCust5 = new XRect(posX, posYCustomer+45, lenListName, 5);
                XRect listCustomerAmount = new XRect(posX+ lenListName, posYCustomer, lenListDischarge, 55);
                XRect listCustomerRest = new XRect(posX + lenListName + lenListDischarge, posYCustomer, 95, 55);

                graphics.DrawRectangle(_pen, listCustomer);
                graphics.DrawRectangle(_pen, listCustomerAmount);
                graphics.DrawRectangle(_pen, listCustomerRest);


                graphics.DrawString(line1, _fontNormal, _brush, listCust1, XStringFormats.Center);
                graphics.DrawString(line2, _fontNormal, _brush, listCust2, XStringFormats.Center);
                graphics.DrawString(line3, _fontNormal, _brush, listCust3, XStringFormats.Center);
                graphics.DrawString("Tel: "+line4, _fontNormal, _brush, listCust4, XStringFormats.Center);
                graphics.DrawString("UWAGI: "+"5 linia", _fontNormal, _brush, listCust5, XStringFormats.Center);
                graphics.DrawString(amount, _fontNormal, _brush, listCustomerAmount, XStringFormats.Center);

            }
        }

        public void DrawFooters(PdfPage page)
        {
             //   PdfPage page = document.Pages[1];

                using (XGraphics graphics = XGraphics.FromPdfPage(page))
                {
                    graphics.DrawLine(_pen, new XPoint(28, page.Height - 26), new XPoint(page.Width - 30.75, page.Height - 26));

                    XRect rectFooter = new XRect(28, page.Height - 25, page.Width - 59, 7);

                    XStringFormat formatNear = new XStringFormat();
                    formatNear.Alignment = XStringAlignment.Near;
                    XStringFormat formatFar = new XStringFormat();
                    formatFar.Alignment = XStringAlignment.Far;

                    graphics.DrawString("List przewozowy v1.0", _fontSubtitle, _brush, rectFooter, formatNear);
                }
        }

    }
    public class PdfCreator
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


}
