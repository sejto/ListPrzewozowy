using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListPrzewozowy
{
    class SentAwaria
    {
        private static readonly XPen _pen = new XPen(XColors.Black, 0.5);
        private static readonly XBrush _brush = XBrushes.Black;
        private static readonly XFont _fontNormal = new XFont("Arial", 9, XFontStyle.Regular, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static readonly XFont _fontBold = new XFont("Arial", 10, XFontStyle.Bold, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static readonly XFont _fontHeader = new XFont("Arial", 12, XFontStyle.Bold, new XPdfFontOptions(PdfFontEncoding.Unicode));
        private static readonly XFont _fontSmall = new XFont("Arial", 8, XFontStyle.Regular, new XPdfFontOptions(PdfFontEncoding.Unicode));

        private static double posX = 25;
        private static double posY = 100;
      //  private static double currPosX;
        private static double currPosY;
        private static double lenCol1 = 360;
        private static double lenCol2 = 180;
        private static double lenCol1a = 180;
        private static double lenCol1b = 180;
        private static double lenColHalf = 270;
        private static double height1 = 28;
        private static double height2 = 40;
        private static double height3 = 43;
        private static double posXCol2 = posX + lenCol1;
        private static double posXCol1b = posX + lenCol1a;
        private int lp = 2;

        public string OwnNumber = "";
        public string SenderName = "";
        public string SenderNIP = "";
        public string SenderStreet = "";
        public string SenderNr = "";
        public string SenderTown = "";
        public string SenderCode = "";
        public string SenderPost = "";
        public string SenderCountry = "";

        public string RecipientName = "";
        public string RecipientNIP = "";
        public string RecipientStreet = "";
        public string RecipientNr = "";
        public string RecipientTown = "";
        public string RecipientCode = "";
        public string RecipientPost = "";
        public string RecipientCountry = "";

        public string CarrierName = "";
        public string CarrierNIP = "";
        public string CarrierStreet = "";
        public string CarrierNr = "";
        public string CarrierTown = "";
        public string CarrierCode = "";
        public string CarrierPost = "";
        public string CarrierCountry = "";

        public string GoodsName = "";
        public string GoodsCN = "";
        public string GoodsAmount = "";
        public string GoodsUnit = "";

        public string CarriagePlaned = "";
        public string CarriageStart = "";
        public string CarriagePlanedEnd = "";

        public string LoadingStreet = "";
        public string LoadingNr = "";
        public string LoadingTwon = "";
        public string LoadingCode = "";
        public string LoadingPost = "";
        public string LoadingCountry = "";

        public string TransportNr = "";
        public string TransportPermit = "";
        public string TransportNrDoc = "";

        public void DrawPage1(PdfPage page)
        {
            using (XGraphics graphics = XGraphics.FromPdfPage(page))
            {
                XRect lineHead = new XRect(170, posY - 30, 250, 15);
                XRect lineOwnNr = new XRect(posX, posY, lenCol1, height1);
                XRect lineRefNr = new XRect(posX + lenCol1, posY, lenCol2, height1);
                currPosY = posY + height1;
                graphics.DrawRectangle(_pen, lineOwnNr);
                graphics.DrawRectangle(_pen, lineRefNr);
                graphics.DrawString("DOKUMENT ZASTĘPUJĄCY ZGŁOSZENIE", _fontHeader, _brush, lineHead, XStringFormats.Center);

                graphics.DrawString(" 1A. Numer własny dokumentu:", _fontNormal, _brush, lineOwnNr, XStringFormats.TopLeft);
                graphics.DrawString(OwnNumber, _fontBold, _brush, lineOwnNr, XStringFormats.Center);
                graphics.DrawString(" 1B. Numer referencyjny:", _fontNormal, _brush, lineRefNr, XStringFormats.TopLeft);

                for (int i = 0; i < 3; i++)
                {
                    //----------------modul_x3---------------------------------------------------------------------------
                    //currPosY = posY + height1;
                    XRect lineHeadGrey = new XRect(posX, currPosY, lenCol1, height2);
                    XRect lineSISC = new XRect(posXCol2, currPosY, lenCol2, height2);
                    currPosY = currPosY + height2;
                    XRect lineName = new XRect(posX, currPosY, lenCol1, height3);
                    XRect lineNIP = new XRect(posXCol2, currPosY, lenCol2, height3);
                    currPosY = currPosY + height3;
                    XRect lineStreet = new XRect(posX, currPosY, lenCol1a, height1);
                    XRect lineHouse = new XRect(posXCol1b, currPosY, lenCol1b, height1);
                    XRect lineTown = new XRect(posXCol2, currPosY, lenCol2, height1);
                    currPosY = currPosY + height1;
                    XRect lineCode = new XRect(posX, currPosY, lenCol1a, height1);
                    XRect linePost = new XRect(posXCol1b, currPosY, lenCol1b, height1);
                    XRect lineCountry = new XRect(posXCol2, currPosY, lenCol2, height1);

                    graphics.DrawRectangle(_pen, XBrushes.LightGray, lineHeadGrey);
                    graphics.DrawRectangle(_pen, lineSISC);
                    graphics.DrawRectangle(_pen, lineName);
                    graphics.DrawRectangle(_pen, lineNIP);
                    graphics.DrawRectangle(_pen, lineStreet);
                    graphics.DrawRectangle(_pen, lineHouse);
                    graphics.DrawRectangle(_pen, lineTown);
                    graphics.DrawRectangle(_pen, lineCode);
                    graphics.DrawRectangle(_pen, linePost);
                    graphics.DrawRectangle(_pen, lineCountry);

                    if (i == 0)
                    {
                        graphics.DrawString(" I. DANE PODMIOTU WYSYŁAJĄCEGO/NADAWCY TOWARU:", _fontNormal, _brush, lineHeadGrey, XStringFormats.TopLeft);
                        graphics.DrawString(" " + lp + ". Id SISC:", _fontNormal, _brush, lineSISC, XStringFormats.TopLeft);
                        lp++;
                    }
                    else if (i == 1)
                    {
                        graphics.DrawString(" II. DANE PODMIOTU ODBIERAJĄCEGO/ODBIORCY TOWARU:", _fontNormal, _brush, lineHeadGrey, XStringFormats.TopLeft);
                        graphics.DrawString(" " + lp + ". Id SISC:", _fontNormal, _brush, lineSISC, XStringFormats.TopLeft);
                        lp++;
                    }
                    else if (i == 2)
                    {
                        graphics.DrawString(" III. DANE PRZEWOŹNIKA:", _fontNormal, _brush, lineHeadGrey, XStringFormats.TopLeft);
                        graphics.DrawString(" " + lp + ". Id SISC:", _fontNormal, _brush, lineSISC, XStringFormats.TopLeft);
                        lp++;

                    }
                    graphics.DrawString(" " + lp + ". Imię i nazwisko albo nazwa:", _fontNormal, _brush, lineName, XStringFormats.TopLeft);
                    lp++;
                    graphics.DrawString(" " + lp + ". NIP/nr innego identyfikatora:", _fontNormal, _brush, lineNIP, XStringFormats.TopLeft);
                    lp++;
                    graphics.DrawString(" " + lp + ". Ulica:", _fontNormal, _brush, lineStreet, XStringFormats.TopLeft);
                    lp++;
                    graphics.DrawString(" " + lp + ". Nr budynku/nr lokalu:", _fontNormal, _brush, lineHouse, XStringFormats.TopLeft);
                    lp++;
                    graphics.DrawString(" " + lp + ". Miejscowość:", _fontNormal, _brush, lineTown, XStringFormats.TopLeft);
                    lp++;
                    graphics.DrawString(" " + lp + ". Kod pocztowy:", _fontNormal, _brush, lineCode, XStringFormats.TopLeft);
                    lp++;
                    graphics.DrawString(" " + lp + ". Poczta:", _fontNormal, _brush, linePost, XStringFormats.TopLeft);
                    lp++;
                    graphics.DrawString(" " + lp + ". Kraj:", _fontNormal, _brush, lineCountry, XStringFormats.TopLeft);
                    lp++;

                    currPosY = currPosY + height1;
                    //------------modul_x3_END----------------------------------------------------------------------------
                }
                //-----------------------TOWAR--------------------------------------
                XRect lineGoodsHead = new XRect(posX, currPosY, lenCol1 + lenCol2, height1);
                currPosY = currPosY + height1;
                XRect lineGoodsName = new XRect(posX, currPosY, lenCol1a, height1);
                XRect lineGoodsCN = new XRect(posXCol1b, currPosY, lenCol1b, height1);
                XRect lineGoodsPKWIU = new XRect(posXCol2, currPosY, lenCol2, height1);
                currPosY = currPosY + height1;
                XRect lineGoodsAmount = new XRect(posX, currPosY, lenCol1a, height1);
                XRect lineAmountGross = new XRect(posXCol1b, currPosY, lenCol1b, height1);
                XRect lineGoodsCapacity = new XRect(posXCol2, currPosY, lenCol2, height1);

                graphics.DrawRectangle(_pen, XBrushes.LightGray, lineGoodsHead);
                graphics.DrawRectangle(_pen, lineGoodsName);
                graphics.DrawRectangle(_pen, lineGoodsCN);
                graphics.DrawRectangle(_pen, lineGoodsPKWIU);
                graphics.DrawRectangle(_pen, lineGoodsAmount);
                graphics.DrawRectangle(_pen, lineAmountGross);
                graphics.DrawRectangle(_pen, lineGoodsCapacity);

                graphics.DrawString(" IV. DANE DOTYCZĄCE TOWARU:", _fontNormal, _brush, lineGoodsHead, XStringFormats.TopLeft);
                graphics.DrawString(" " + lp + ". Rodzaj towaru", _fontNormal, _brush, lineGoodsName, XStringFormats.TopLeft);
                lp++;
                graphics.DrawString(" " + lp + ". Pozycja CN:", _fontNormal, _brush, lineGoodsCN, XStringFormats.TopLeft);
                lp++;
                graphics.DrawString(" " + lp + ". Podkategoria PKWiU:", _fontNormal, _brush, lineGoodsPKWIU, XStringFormats.TopLeft);
                lp++;
                graphics.DrawString(" " + lp + ". Ilość towaru:", _fontNormal, _brush, lineGoodsAmount, XStringFormats.TopLeft);
                lp++;
                graphics.DrawString(" " + lp + ". Masa brutto towaru:", _fontNormal, _brush, lineAmountGross, XStringFormats.TopLeft);
                lp++;
                graphics.DrawString(" " + lp + ". Objętość towaru:", _fontNormal, _brush, lineGoodsCapacity, XStringFormats.TopLeft);
                currPosY = currPosY + height1;
                //--------------------------DATY-----------------------------------------
                XRect lineDateHead = new XRect(posX, currPosY, lenCol1 + lenCol2, height1);
                currPosY = currPosY + height1;
                XRect lineDatePlaned = new XRect(posX, currPosY, lenCol1a, height1);
                XRect lineDateStart = new XRect(posXCol1b, currPosY, lenCol1b, height1);
                XRect lineDateEnd = new XRect(posXCol2, currPosY, lenCol2, height1);

                graphics.DrawRectangle(_pen, XBrushes.LightGray, lineDateHead);
                graphics.DrawRectangle(_pen, lineDatePlaned);
                graphics.DrawRectangle(_pen, lineDateStart);
                graphics.DrawRectangle(_pen, lineDateEnd);

                graphics.DrawString(" V. DATY ROZPOCZĘCIA I ZAKOŃCZENIA PRZEWOZU NA TERYTORIUM RP:", _fontNormal, _brush, lineDateHead, XStringFormats.TopLeft);
                lp++;
                graphics.DrawString(" " + lp + ". Planowana data rozpoczęcia przewozu:", _fontNormal, _brush, lineDatePlaned, XStringFormats.TopLeft);
                lp++;
                graphics.DrawString(" " + lp + ". Data rozpoczęcia przewozu:", _fontNormal, _brush, lineDateStart, XStringFormats.TopLeft);
                lp++;
                graphics.DrawString(" " + lp + ". Planowana data zakończenia przewozu:", _fontNormal, _brush, lineDateEnd, XStringFormats.TopLeft);
                currPosY = currPosY + height1;


            }
        }

        public void DrawPage2(PdfPage page)
        {
            using (XGraphics graphics = XGraphics.FromPdfPage(page))
            {
                currPosY = posY;
                for (int i = 0; i < 2; i++)
                {
                    //--------------------modul_x2-----------------------------------------------------------------
                    XRect lineLoadHead = new XRect(posX, currPosY, lenCol1 + lenCol2, height1);
                    graphics.DrawRectangle(_pen, XBrushes.LightGray, lineLoadHead);

                    if (i == 0)
                    {
                        graphics.DrawString(" VI. MIEJSCE ZAŁADUNKU TOWARU:", _fontNormal, _brush, lineLoadHead, XStringFormats.TopLeft);
                    }else if (i==1)
                    {
                        graphics.DrawString(" VII. MIEJSCE DOSTARCZENIA TOWARU:", _fontNormal, _brush, lineLoadHead, XStringFormats.TopLeft);
                    }

                    currPosY = currPosY + height1;
                    XRect lineLoadStreet = new XRect(posX, currPosY, lenCol1a, height1);
                    XRect lineLoadHouse = new XRect(posXCol1b, currPosY, lenCol1b, height1);
                    XRect lineLoadTown = new XRect(posXCol2, currPosY, lenCol2, height1);
                    currPosY = currPosY + height1;
                    XRect lineLoadCode = new XRect(posX, currPosY, lenCol1a, height1);
                    XRect lineLoadPost = new XRect(posXCol1b, currPosY, lenCol1b, height1);
                    XRect lineLoadCountry = new XRect(posXCol2, currPosY, lenCol2, height1);
                    

                    graphics.DrawRectangle(_pen, lineLoadStreet);
                    graphics.DrawRectangle(_pen, lineLoadHouse);
                    graphics.DrawRectangle(_pen, lineLoadTown);
                    graphics.DrawRectangle(_pen, lineLoadCode);
                    graphics.DrawRectangle(_pen, lineLoadPost);
                    graphics.DrawRectangle(_pen, lineLoadCountry);

                    lp++;
                    graphics.DrawString(" " + lp + ". Ulica:", _fontNormal, _brush, lineLoadStreet, XStringFormats.TopLeft);
                    lp++;
                    graphics.DrawString(" " + lp + ". Nr budynku/nr lokalu:", _fontNormal, _brush, lineLoadHouse, XStringFormats.TopLeft);
                    lp++;
                    graphics.DrawString(" " + lp + ". Miejscowość:", _fontNormal, _brush, lineLoadTown, XStringFormats.TopLeft);
                    lp++;
                    graphics.DrawString(" " + lp + ". Kod pocztowy:", _fontNormal, _brush, lineLoadCode, XStringFormats.TopLeft);
                    lp++;
                    graphics.DrawString(" " + lp + ". Poczta:", _fontNormal, _brush, lineLoadPost, XStringFormats.TopLeft);
                    lp++;
                    graphics.DrawString(" " + lp + ". Kraj:", _fontNormal, _brush, lineLoadCountry, XStringFormats.TopLeft);
                    currPosY = currPosY + height1;
                    //---------------------modul_x2_END------------------------------------------------------------------------------
                }

                XRect lineRPStart = new XRect(posX, currPosY, lenColHalf, height1);
                XRect lineRPEnd = new XRect(posX + lenColHalf, currPosY, lenColHalf, height1);
                currPosY = currPosY + height1;
                XRect lineRPnr1 = new XRect(posX, currPosY, lenColHalf, height2*2);
                XRect lineRPnr2 = new XRect(posX + lenColHalf, currPosY, lenColHalf, height2*2);
                currPosY = currPosY + (height2*2);

                graphics.DrawRectangle(_pen, XBrushes.LightGray, lineRPStart);
                graphics.DrawRectangle(_pen, XBrushes.LightGray, lineRPEnd);
                graphics.DrawRectangle(_pen, lineRPnr1);
                graphics.DrawRectangle(_pen, lineRPnr2);


                graphics.DrawString(" VIII. MIEJSCE ROZPOCZĘCIA PRZEWOZU NA TERYTORIUM RP:", _fontSmall, _brush, lineRPStart, XStringFormats.TopLeft);
                graphics.DrawString(" IX. MIEJSCE ZAKOŃCZENIA PRZEWOZU NA TERYTORIUM RP:", _fontSmall, _brush, lineRPEnd, XStringFormats.TopLeft);
                lp++;
                graphics.DrawString(" " + lp + ". Miejscowość/Przejście graniczne/Nr drogi:", _fontNormal, _brush, lineRPnr1, XStringFormats.TopLeft);
                lp++;
                graphics.DrawString(" " + lp + ". Miejscowość/Przejście graniczne/Nr drogi:", _fontNormal, _brush, lineRPnr2, XStringFormats.TopLeft);
                //---------------------------Środek transportu--------------------------------------------------------------
                XRect lineTruckHead = new XRect(posX, currPosY, lenCol1 + lenCol2, height1);
                currPosY = currPosY + height1;
                XRect lineTruckNr = new XRect(posX, currPosY, lenCol1a, height2);
                XRect lineTruckPerm = new XRect(posXCol1b, currPosY, lenCol1a, height2);
                XRect lineTruckDoc = new XRect(posXCol2, currPosY, lenCol1b, height2);
                currPosY = currPosY + height2;

                graphics.DrawRectangle(_pen, XBrushes.LightGray, lineTruckHead);
                graphics.DrawRectangle(_pen, lineTruckNr);
                graphics.DrawRectangle(_pen, lineTruckPerm);
                graphics.DrawRectangle(_pen, lineTruckDoc);

                lp++;
                graphics.DrawString(" X. ŚRODEK TRANSPORTU:", _fontNormal, _brush, lineTruckHead, XStringFormats.TopLeft);
                graphics.DrawString(" " + lp + ". Nr rejestracyjne:", _fontNormal, _brush, lineTruckNr, XStringFormats.TopLeft);
                lp++;
                graphics.DrawString(" " + lp + ". Nr zezwolenia drogowego:", _fontNormal, _brush, lineTruckPerm, XStringFormats.TopLeft);
                lp++;
                graphics.DrawString(" " + lp + ". Nr dokumentu przewozowego:", _fontNormal, _brush, lineTruckDoc, XStringFormats.TopLeft);
                //-------------------------------Odbiór towaru---------------------------------------------------------------
                XRect lineReceiveHead = new XRect(posX, currPosY, lenCol1 + lenCol2, height1);
                currPosY = currPosY + height1;
                XRect lineReceiveData = new XRect(posX, currPosY, lenCol1a, height1*2);
                XRect lineReceiveComments = new XRect(posX+lenCol1a, currPosY, lenCol1a*2, height1*2);
                currPosY = currPosY + (height1*2);

                graphics.DrawRectangle(_pen, XBrushes.LightGray, lineReceiveHead);
                graphics.DrawRectangle(_pen, lineReceiveData);
                graphics.DrawRectangle(_pen, lineReceiveComments);

                lp++;
                graphics.DrawString(" XI. INFORMACJA O ODBIORZE TOWARU:", _fontNormal, _brush, lineReceiveHead, XStringFormats.TopLeft);
                graphics.DrawString(" " + lp + ". Data odbioru towaru:", _fontNormal, _brush, lineReceiveData, XStringFormats.TopLeft);
                lp++;
                graphics.DrawString(" " + lp + ". Uwagi:", _fontNormal, _brush, lineReceiveComments, XStringFormats.TopLeft);
                //--------------------------------Oswiadczenie-----------------------------------------------------------------
                XRect lineStatementHead = new XRect(posX, currPosY, lenCol1 + lenCol2, height1);
                currPosY = currPosY + height1;
                XRect lineStatement = new XRect(posX, currPosY, lenCol1+lenCol2, height2*2);
                currPosY = currPosY + 15;
                XRect lineStatement1 = new XRect(posX, currPosY, lenCol1 + lenCol2, 15);
                currPosY = currPosY + 15;
                XRect lineStatement2 = new XRect(posX, currPosY, lenCol1 + lenCol2, 15);
                currPosY = currPosY + 15;
                XRect lineStatement3 = new XRect(posX, currPosY, lenCol1 + lenCol2, 15);
                currPosY = currPosY + 20;
                XRect lineStatement4 = new XRect(posX, currPosY, lenCol1 + lenCol2, 15);

                graphics.DrawRectangle(_pen, XBrushes.LightGray, lineStatementHead);
                graphics.DrawRectangle(_pen, lineStatement);

                lp++;
                graphics.DrawString(" XII. OŚWIADCZENIE:", _fontNormal, _brush, lineStatementHead, XStringFormats.TopLeft);
                graphics.DrawString(" " + lp + ". Oświadczam, że posiadam upoważnienie do złożenia/uzupełnienia/aktualizacji zgłoszenia * w imieniu i na rzecz:", _fontNormal, _brush, lineStatement, XStringFormats.TopLeft);
                graphics.DrawString(" [ ] podmiotu wysyłającego", _fontNormal, _brush, lineStatement1, XStringFormats.TopLeft);
                graphics.DrawString(" [ ] podmiotu odbierającego", _fontNormal, _brush, lineStatement2, XStringFormats.TopLeft);
                graphics.DrawString(" [ ] przewoźnika", _fontNormal, _brush, lineStatement3, XStringFormats.TopLeft);
                graphics.DrawString(" [ Dane oświadczającego ]", _fontNormal, _brush, lineStatement4, XStringFormats.TopLeft);

            }
        }
    }
}
