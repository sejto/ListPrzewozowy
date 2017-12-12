using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;
using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Diagnostics;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Drawing.Layout;
using System.Xml;
using System.IO;
using System.Collections;

namespace ListPrzewozowy
{

    public partial class Form1 : Form
    {
        string file = "parametry.xml";
        public static int nrwz;
        public static string data;
        public static string nazwa;
        public static string KontrID;
        public static string ulica;
        public static string nrdomu;
        public static string kod;
        public static string miasto;
        public static string nip;
        public static string telefon;
        public static string[] KontrahentDane = new string[21];

        public static List<DaneFirmy> FirmLista = new List<DaneFirmy>();
        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
            CreateDGV();
            string[] kontr;
            kontr = new string[5];
            print pdf = new print();
            WZtxt.Text=parametry("/Parametry/NrWZ/Wartosc");
            nrwz = Convert.ToInt32(WZtxt.Text);
        }

        void OnProcessExit(object sender, EventArgs e)
        {
            parametry("/Parametry/NrWZ/Wartosc", nrwz.ToString()); //zapamiętanie numeru ostatniej WZ-tki
        }

        private void Btn_szukajKTH_Click(object sender, EventArgs e)
        {
            string sql;
            string nazwa = Txt_KTH.Text;
            if (NIPValidate(nazwa) != true)
            {
                sql = "select k.kontrid as ID, k.Nazwa, Ulica, Nrdomu, kod, miasto, Nip, Telefon, " +
"(Select top 1  case when tekst like '%pel%' then 'Pełnomocnictwo' else 'Brak' end as Pelnomocnictwo From KontrOpis ko where KO.KontrId = K.KontrId and Znaczenie = 76) as Pelnomocnictwo," +
"(Select top 1  case when tekst like '%osw%' then 'Oswiadczenie' else 'Brak' end as Oswiadczenie From KontrOpis ko where KO.KontrId = K.KontrId and Znaczenie = 76) as Oswiadczenie " +
"from kontrahent k where k.nazwa like '%" + nazwa + "%'";
            }
            else
            {
                sql = "select k.kontrid as ID,k.Nazwa, Ulica, Nrdomu, kod, miasto, Nip, Telefon, " +
"(Select top 1  case when tekst like '%pel%' then 'Pełnomocnictwo' else 'Brak' end as Pelnomocnictwo From KontrOpis ko where KO.KontrId = K.KontrId and Znaczenie = 76) as Pelnomocnictwo," +
"(Select top 1  case when tekst like '%osw%' then 'Oswiadczenie' else 'Brak' end as Oswiadczenie From KontrOpis ko where KO.KontrId = K.KontrId and Znaczenie = 76) as Oswiadczenie " +
"from kontrahent k where k.nip ='" + nazwa + "'";
            }
            string keyname = "HKEY_CURRENT_USER\\MARKET\\serwerLokal";
            rejestrIO rejestr = new rejestrIO();
            string klucz = rejestr.czytajklucz(keyname, "SQLconnect", true); //parametry połączenia do bazy SQL zapisane w rejestrze
            SqlConnection connection = new SqlConnection(klucz); //skopiować gałąź na pozostałe kompy, ta sama co w ZielonyKoszyk
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();
            connection.Open();
            dataadapter.Fill(ds, "Kontrahenci");
            connection.Close();
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "Kontrahenci";
            DataGridViewButtonColumn col = new DataGridViewButtonColumn
            {
                UseColumnTextForButtonValue = true,
                Text = "Wybierz",
                Name = "Wybor"
            };
            dataGridView1.Columns.Add(col);
            //odbiorcy.Visible = true;
            DataGridViewColumn columnNazwa = dataGridView1.Columns[1];
            DataGridViewColumn columnID = dataGridView1.Columns[0];
            columnNazwa.Width = 350;
            columnID.Width = 50;
        }

        private void CheckKeys(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                Btn_szukajKTH_Click(sender, (EventArgs)e);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Wybor"].Index && e.RowIndex >= 0)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Wybierz")
                {
                    int rownumber = Convert.ToInt16(((DataGridView)sender).SelectedCells[0].RowIndex);
                    KontrID = dataGridView1[0, rownumber].Value.ToString();
                 /*   nazwa = dataGridView1[1, rownumber].Value.ToString();
                    ulica = dataGridView1[2, rownumber].Value.ToString();
                    nrdomu = dataGridView1[3, rownumber].Value.ToString();
                    kod = dataGridView1[4, rownumber].Value.ToString();
                    miasto = dataGridView1[5, rownumber].Value.ToString();
                    nip = dataGridView1[6, rownumber].Value.ToString();
                    telefon = dataGridView1[7, rownumber].Value.ToString();
                    string pelnomocnictwo = dataGridView1[8, rownumber].Value.ToString();
                    */
                    data = dateTimePicker1.Text;

                    Kontrahent KontrahentForm = new Kontrahent();
                    KontrahentForm.OnRunMethod += new Kontrahent.methodHandler(WczytajDane);
                    KontrahentForm.Show();
                    //  DodajKontrahenta(nazwa, ulica, nrdomu, kod, miasto, nip, telefon);
                    //  DodajKontrahentList(rownumber,nazwa, ulica, nrdomu, kod, miasto, nip, telefon);
                }
                else if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Edytuj")
                {
                    MessageBox.Show("Tu będzie wczytywanie listu z bazy SQL.");
                }
            }

        } //Wyswietl Form_Kontrahent a następnie dodaj kontrahenta do zmiennej FirmLista oraz wyświetl ją w DGV
        private void ZapiszDoBazy()
        {
            data = dateTimePicker1.Value.Date.ToString("dd.MM.yyyy");
            string sql = "insert into list (Data, KontrId, Paliwo, Ilosc,Cena, FormaPlat,Termin, Sent,DostUlica, DostNr, DostMiasto, DostKod, DostPoczta, DostKraj, DostPlanRozp, DostRozp, DostPlanZak, NrWZ) " +
                "values('"+data+"'," + KontrID + ", '"+KontrahentDane[8]+"',"+KontrahentDane[9]+ ",'" + KontrahentDane[10] + "','" + KontrahentDane[11] + "'," + KontrahentDane[12] +
                ",'" + KontrahentDane[13] + "','" + KontrahentDane[14] + "','" + KontrahentDane[15] + "','" + KontrahentDane[16] + "','" + KontrahentDane[17] + "','" + KontrahentDane[16] + "','PL', " +
                "'" + KontrahentDane[18] + "','" + KontrahentDane[19] + "','" + KontrahentDane[20] + "', '" + nrwz+"')";
            MessageBox.Show(sql);
            string keyname = "HKEY_CURRENT_USER\\MARKET\\ListPrzewozowy";
            rejestrIO rejestr = new rejestrIO();
            string klucz = rejestr.czytajklucz(keyname, "SQLconnect", true); //parametry połączenia do bazy SQL zapisane w rejestrze
            using (var conn = new SqlConnection(klucz))
            using (var cmd = conn.CreateCommand())
            {
                conn.Open();
                cmd.CommandText = sql;
                var result = cmd.ExecuteNonQuery();
                conn.Close();
            }
        }
        public void CreateDGV()
                {
                    dataGridView3.CellClick += dataGridView3_CellClick;
                    dataGridView3.RowHeadersVisible = false;
                    dataGridView3.Columns.Add("Column", "Nazwa");
                    dataGridView3.Columns[0].Width = 180;
                    dataGridView3.Columns.Add("Column", "Ulica");
                    dataGridView3.Columns[1].Width = 120;
                    dataGridView3.Columns.Add("Column", "Miasto");
                    dataGridView3.Columns[2].Width = 100;
                    dataGridView3.Columns.Add("Column", "Nip");
                    dataGridView3.Columns[3].Width = 80;
                    dataGridView3.Columns.Add("Column", "Telefon");
                    dataGridView3.Columns[4].Width = 80;
                    dataGridView3.Columns.Add("Column", "Paliwo");
                    dataGridView3.Columns.Add("Column", "Ilość");
                    dataGridView3.Columns[6].Width = 60;
                    dataGridView3.Columns.Add("Column", "Cena");
                    dataGridView3.Columns[7].Width = 60;
                    dataGridView3.Columns.Add("Column", "Forma płat");
                    dataGridView3.Columns.Add("Column", "Termin");
                    dataGridView3.Columns.Add("Column", "Sent");
                    dataGridView3.Columns.Add("Column", "Miejsce dostarcz");
                    dataGridView3.Columns[11].Width = 200;

                    var buttonCol = new DataGridViewButtonColumn();
                    buttonCol.HeaderText = "Usun";
                    buttonCol.Name = "Usun";
                    buttonCol.Text = "Usun";
                    buttonCol.UseColumnTextForButtonValue = true;
                    dataGridView3.Columns.Add(buttonCol);
                    dataGridView3.AllowUserToAddRows = false;
                }  //Utwórz nagłówki dgv listy z kontrahentami
        void item_SelectedIndexChanged(object sender, EventArgs e)
        {
          //  MessageBox.Show(((ComboBox)sender).Text);
        }
        public void WczytajDane()
                {
                    dataGridView3.Rows.Clear();
                    dataGridView3.Refresh();
                    int firmCount = FirmLista.Count;
                    for (int count = 0; count < firmCount; ++count)
                    {
                        DaneFirmy oFirma = FirmLista[count];
                        dataGridView3.Rows.Add(new object[] { oFirma.KontrNazwa, oFirma.KontrUlica + " " + oFirma.KontrNrDomu, oFirma.KontrMiasto + " " + oFirma.KontrKod,
                        oFirma.KontrNIP,oFirma.KontrTel,oFirma.Paliwo,oFirma.Ilosc.ToString(),oFirma.Cena,oFirma.FormPlat,oFirma.Termin,oFirma.Sent,oFirma.DostUlica + " " + oFirma.DostNr,"Usuń" });
                    }
                }//Wyswietla dane z listy w DGV3
        private void Button2_Click(object sender, EventArgs e) //button "print generuje PDF z tablelayout"
        {
            printCustomer();
         //   printSender();
        }
        private void button3_Click(object sender, EventArgs e) //button "Wczytaj" - pokaz zapisane w bazie
        {
            string sql;
            /* sql = "select d.ID as Nr_listu,d.Data, count(K.Nazwa)as Kontrahentów,p.nazwa as Paliwo, sum(l.Ilosc) as Litrów from dok D "+
                 "inner join List L on L.dokID = D.ID inner join OTD.dbo.kontrahent K on k.kontrid = L.kontrID inner join paliwo P on p.paliwoID = L.paliwoID "+
                 "group by d.ID, d.Data,p.nazwa"; */
            sql = "select * from (select d.ID as Nr_listu,d.Data, count(K.Nazwa) as Kontrahentów," +
                "(select sum(ilosc) from list where paliwoid = 1) as Olej_napedowy," +
                "(select sum(ilosc) from list where paliwoid = 2) as Olej_arktyczny," +
                "(select sum(ilosc) from list where paliwoid = 3) as Olej_opałowy," +
                "U.Nazwa as Wystawił from dok D inner join List L on L.dokID = D.ID " +
                "inner join OTD.dbo.kontrahent K on k.kontrid = L.kontrID inner join paliwo P on p.paliwoID = L.paliwoID " +
                "inner join Uzytkownik U on D.userId = U.id group by d.ID, d.Data,u.nazwa)k";
            
            string keyname = "HKEY_CURRENT_USER\\MARKET\\ListPrzewozowy";
            rejestrIO rejestr = new rejestrIO();
            string klucz = rejestr.czytajklucz(keyname, "SQLconnect", true); //parametry połączenia do bazy SQL zapisane w rejestrze
            SqlConnection connection = new SqlConnection(klucz); //skopiować gałąź na pozostałe kompy, ta sama co w ZielonyKoszyk
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();
            connection.Open();
            dataadapter.Fill(ds, "Kontrahenci");
            connection.Close();
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = ds;
            dataGridView1.DataMember = "Kontrahenci";
            DataGridViewButtonColumn col = new DataGridViewButtonColumn
            {
                UseColumnTextForButtonValue = true,
                Text = "Edytuj",
                Name = "Wybor"
            };
            dataGridView1.Columns.Add(col);

            //odbiorcy.Visible = true;

        }

        static public bool NIPValidate(string NIPValidate)
        {
            const byte lenght = 10;

            ulong nip = ulong.MinValue;
            byte[] digits;
            byte[] weights = new byte[] { 6, 5, 7, 2, 3, 4, 5, 6, 7 };

            if (NIPValidate.Length.Equals(lenght).Equals(false)) return false;

            if (ulong.TryParse(NIPValidate, out nip).Equals(false)) return false;
            else
            {
                string sNIP = NIPValidate.ToString();
                digits = new byte[lenght];

                for (int i = 0; i < lenght; i++)
                {
                    if (byte.TryParse(sNIP[i].ToString(), out digits[i]).Equals(false)) return false;
                }

                int checksum = 0;

                for (int i = 0; i < lenght - 1; i++)
                {
                    checksum += digits[i] * weights[i];
                }

                return (checksum % 11 % 10).Equals(digits[digits.Length - 1]);
            }

        }
        private void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
                    {
                    int firmCount;
                    if (e.ColumnIndex == dataGridView3.Columns["Usun"].Index && e.RowIndex >= 0)
                        firmCount = FirmLista.Count;
                        FirmLista.RemoveAt(e.RowIndex);
                    WczytajDane();
                } //Usuń wybranego kontrahenta z listy
        public void printCustomer()
                {
                    nrwz = Convert.ToInt32(WZtxt.Text);
                    data = dateTimePicker1.Text;
                    string filename = "wykaz_" + data + ".pdf";
                    string pierwszalinia="";
                    string drugalinia="";
                    string termin;
                    Boolean sentval;
                    PdfDocument document = new PdfDocument();
                    PdfPage page = document.AddPage();
                    int heightRowCust = 85;
                    int litryON = 0;
                    int litryONA = 0;
                    int litryOP = 0;
                    int numpage = 1;
                    page = document.Pages[numpage - 1];
                    print pdf = new print();
                    pdf.DrawHeader(page, data);
                    pdf.DrawFooters(page, numpage);
                    int firmCount = FirmLista.Count;
                    for (int count = 0; count < firmCount; ++count)  //dla wszystkich odbiorców w zmiennej
                    {
                        DaneFirmy oFirma = FirmLista[count];
                        if (heightRowCust + 55 > page.Height - 30) //Jeżeli koniec strony
                        {
                            page = document.AddPage();
                            numpage++;
                            page = document.Pages[numpage - 1];
                            heightRowCust = 55;
                            pdf.DrawFooters(page, numpage);
                        }
                
                        if (oFirma.Paliwo == "Olej napędowy")
                            { litryON = litryON + oFirma.Ilosc; }
                        else if(oFirma.Paliwo == "Olej napędowy arktyczny")
                            { litryONA = litryONA + oFirma.Ilosc; }
                        else
                            { litryOP = litryOP + oFirma.Ilosc; }
                        if (oFirma.FormPlat == "Gotówka")
                            termin = oFirma.Termin;
                        else
                            termin = oFirma.Termin + " dni";

                        pdf.DrawCustomer(page, heightRowCust, oFirma.KontrNazwa, oFirma.KontrUlica+" "+oFirma.KontrNrDomu, oFirma.KontrNIP, oFirma.KontrTel, "", 
                            oFirma.Ilosc.ToString(), oFirma.Sent+ ", " + oFirma.Cena + " " + oFirma.FormPlat + "," + termin );
                        heightRowCust = heightRowCust + 70;
                        StringBuilder completedWord = new StringBuilder();
                        int znaki = oFirma.KontrNazwa.Count();
                        if (znaki > 35)
                        {
                            completedWord.Append(oFirma.KontrNazwa.Substring(0, 35));//Jeżeli za długa nazwa kontrahenta, to po 35 znaku podzielic na 2 linie
                            completedWord.AppendLine();
                           pierwszalinia = completedWord.ToString();
                            completedWord.Clear();
                            completedWord.Append(oFirma.KontrNazwa.Substring(35, znaki - 35));
                           drugalinia = completedWord.ToString();
                        }
                        else
                            pierwszalinia = oFirma.KontrNazwa;

                        int f = 1;
                        while (File.Exists(filename)) { filename = "wykaz_kierowca_" + data + "_" + f + ".pdf"; f++; }
                        if (oFirma.Sent.Length != 0)
                            sentval = true; else sentval = false;


                        printWZ(oFirma.Ilosc.ToString(), oFirma.Paliwo, oFirma.Cena, oFirma.FormPlat, termin, "", pierwszalinia, drugalinia, oFirma.KontrUlica
                            +" "+oFirma.KontrNrDomu+", "+oFirma.KontrMiasto, "NIP/PESEL:" + oFirma.KontrNIP, "tel:" + oFirma.KontrTel,sentval);
                    }
                    page = document.Pages[0];
                    pdf.DrawBody(page, litryON, litryONA, litryOP);
                    document.Save(filename);
                    Process.Start(filename);
                } //drukuj list przewozowy
        public void printWZ(string ilosc, string paliwo,string cena,string formaplatWZ, string termin, string uwagiN, string line1, string line2, string line3, string line4, string line5, Boolean sentval)
        {
           
            string filename = "WZ_"+nrwz+".pdf";
            //MessageBox.Show(odbiorcy.GetControlFromPosition(7, 1).Text.ToString());
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            print pdf = new print();
            pdf.ilosc = ilosc;
            pdf.paliwo = paliwo;
            pdf.uwagi = formaplatWZ + " " + termin;
            pdf.uwagiN = uwagiN;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(file);
            XmlNodeList nodeList = xmlDoc.SelectNodes("/Parametry/Firma/Wartosc");
            int l = 0;
            foreach (XmlNode _node in nodeList)
                {
                pdf.lineour[l] = _node.InnerText.ToString(); //Kolejne linie nazwy naszej firmy z xml (6 linii w ramce w pdf)
                l = l+1;
            }
            string dataWZ = dateTimePicker1.Value.Date.ToString("dd.MM.yyyy");
            pdf.line1 = line1;
            pdf.line2 = line2;
            pdf.line3 = line3;
            pdf.line4 = line4;
            pdf.line5 = line5;
         //   if (formaplatWZ == "Przelew") { cena = ""; }
            pdf.cenapaliwa = cena+" zł";
            pdf.DrawWZName(page, nrwz+"/2017", dataWZ,30);
            pdf.DrawWZBody(page,106, sentval);
            pdf.DrawWZFooter(page, parametry("/Parametry/Uzytkownik/Wartosc"),226); //Nazwisko wystawiającego w polu wystawil
            //-------część dolna WZ------------------
            pdf.DrawWZName(page, nrwz + "/2017", dataWZ, 450);
            pdf.DrawWZBody(page, 526,sentval);
            pdf.DrawWZFooter(page, parametry("/Parametry/Uzytkownik/Wartosc"), 646); //Nazwisko wystawiającego w polu wystawil
            //---------------------------------------
            document.Save(filename);
            Process.Start(filename);
            nrwz = ++nrwz ; //następny numer WZ
//            WZtxt.Text = num.ToString(); //aktualizacja textboxa
            //parametry("/Parametry/NrWZ/Wartosc", num.ToString()); //zapamiętanie numeru ostatniej WZ-tki
        }

        public string parametry(string param)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("parametry.xml");
            XmlNode dane = xmlDoc.DocumentElement.SelectSingleNode(param);
            return dane.InnerText;
        }  //odczytuje gałąź konfiguracji z xml
        public void parametry(string param, string val)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(file);
            xmlDoc.SelectSingleNode(param).InnerText = val;
            xmlDoc.Save(file);
        }  //zapisuje konfigurację do xml

        public void SENT100()
        {
            TraderAddress SenderAddress = new TraderAddress();
            SenderAddress.City = "Wypierdek Mamuci";
            TraderInfo Trader = new TraderInfo();
            Trader.TraderIdentityNumber = "84422233388";

            ZapiszSENT("/ns2:SENT_100/ns2:GoodsSender/TraderInfo/TraderIdentityNumber", "345353535");
        }
        public void ZapiszSENT(string param, string val)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(file);
            xmlDoc.SelectSingleNode(param).InnerText = val;
            xmlDoc.Save(file);
        }  //zapisuje SENT

        private void button1_Click(object sender, EventArgs e)
        {
            string appPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            Process.Start("explorer.exe", appPath);
        }  //pokaż folder z dokumentami pdf

        private void drawSENTawaria()
        {
            
            string filename = "SENT_awaria_WZ_" + nrwz + ".pdf";
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            SentAwaria awaria = new SentAwaria();
            awaria.OwnNumber = "12312313/2017";

            awaria.DrawPage1(page);
            page = document.AddPage();
            awaria.DrawPage2(page);
            //MessageBox.Show("Height: "+page.Height+", Width: "+page.Width);

            document.Save(filename);
            Process.Start(filename);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            
            
            //DodajKontrahenta(SentKontrahent.Dane[0], SentKontrahent.Dane[1], SentKontrahent.Dane[2], SentKontrahent.Dane[3], "", "", "");
            //odbiorcy.Visible = true;
            //MessageBox.Show(nazwa);
           // WczytajDane();
        }  //test

        private void button5_Click(object sender, EventArgs e)
        {
           // WczytajDane();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void New_btn_Click(object sender, EventArgs e)
        {
            FirmLista.Clear();
            WczytajDane();
        }
    }
    public class DaneFirmy
    {
        public string Data { get; private set; }
        public int KontrahentID { get; private set; }
        public string KontrNazwa { get; private set; }
        public string KontrUlica { get; private set; }
        public string KontrNrDomu { get; private set; }
        public string KontrKod { get; private set; }
        public string KontrMiasto { get; private set; }
        public string KontrTel { get; private set; }
        public string Paliwo { get; private set; }
        public int Ilosc { get; internal set; }
        public string Cena { get; private set; }
        public string FormPlat { get; private set; }
        public string Termin { get; private set; }
        public string Sent { get; private set; }
        public string DostUlica { get; private set; }
        public string DostNr { get; private set; }
        public string DostMiasto { get; private set; }
        public string DostKod { get; private set; }
        public string DostPoczta { get; private set; }
        public string DostKraj { get; private set; }
        public string DostPlanRozp { get; private set; }
        public string DostRozp { get; private set; }
        public string DostPlanZak { get; private set; }
        public string Uwagi { get; private set; }
        public string KontrNIP { get; private set; }
        public string NrWZ { get; private set; }


        public DaneFirmy(string nData, int nKontrahentID, string nKontrNazwa, string nKontrUlica, string nKontrNrDomu, string nKontrKod, string nKontrMiasto, string nKontrTel, string nKontrNIP, string nPaliwo,
            int nIlosc, string nCena, string nFormPlat, string nTermin, string nSent, string nDostUlica, string nDostNr, string nDostMiasto, string nDostKod, string nDostPoczta, string nDostKraj, 
            string nDostPlanRozp, string nDostRozp, string nDostPlanZak, string nUwagi, string nNrWZ  )
        {
            Data = nData;
            KontrahentID = nKontrahentID;
            KontrNazwa = nKontrNazwa;
            KontrUlica = nKontrUlica;
            KontrNrDomu = nKontrNrDomu;
            KontrKod = nKontrKod;
            KontrMiasto = nKontrMiasto;
            KontrTel = nKontrTel;
            KontrNIP = nKontrNIP;
            Paliwo = nPaliwo;
            Ilosc = nIlosc;
            Cena = nCena;
            FormPlat = nFormPlat;
            Termin = nTermin;
            Sent = nSent;
            DostUlica = nDostUlica;
            DostNr = nDostNr;
            DostMiasto = nDostMiasto;
            DostKod = nDostKod;
            DostPoczta = nDostPoczta;
            DostKraj = nDostKraj;
            DostPlanRozp = nDostPlanRozp;
            DostRozp = nDostRozp;
            DostPlanZak = nDostPlanZak;
            Uwagi = nUwagi;
            NrWZ = nNrWZ;
        }
    }

   /* public static class SentKontrahent
    {
        private static Dictionary<int, string> _messages;
        static SentKontrahent()
        {
            _messages = new Dictionary<int, string>();
        }

        public static Dictionary<int, string> Dane
        {
            get { return _messages; }
        }
    }

    */

    

}

