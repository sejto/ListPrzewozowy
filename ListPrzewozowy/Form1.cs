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
        public static string FirstnrWZ;
        public static string FirstdataWZ;
        public static List<DaneFirmy> FirmLista = new List<DaneFirmy>();

        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
            CreateDGV();
            print pdf = new print();
            NewList();
            //TODO - dodać sprawdzanie czy baza istnieje i jest we właściwej wersji !=> inicjalizacja bazy
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            ToolTip ToolTip1 = new ToolTip();
            ToolTip1.SetToolTip(button2, "Zapisz dokument przed wydrukowaniem.");
    }

        void OnProcessExit(object sender, EventArgs e)
        {
            parametry("/Parametry/NrWZ/Wartosc", nrwz.ToString()); //zapamiętanie numeru ostatniej WZ-tki
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string appPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            Process.Start("explorer.exe", appPath);
        }  //pokaż folder z dokumentami pdf
        private void Button2_Click(object sender, EventArgs e) //button "print generuje PDF z tablelayout"
        {
            printCustomer();
            //   printSender();
        }
        private void button4_Click_1(object sender, EventArgs e)
        {
            if (button4.Text.Contains("poprawione"))
                UaktualnijList();
            else
                ZapiszList();
        } //Button Zapisz do bazy
        private void button3_Click(object sender, EventArgs e) //button "Wczytaj" - pokaz zapisane w bazie
        {
            PokazListy();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            Zapisz.DoLogu("asdasd");
        } //button5 test2
        private void RebuildSQL_btn_Click(object sender, EventArgs e)
        {
            Baza bazasql = new Baza();
            bazasql.ReinicjalizacjaBazy();
            NewList();
            MessageBox.Show("Baza została odbudowana.");
        } //Button reinicjujący baze
        private void New_btn_Click(object sender, EventArgs e)
        {
            NewList();
        } // Nowa lista
        private void Btn_szukajKTH_Click(object sender, EventArgs e)
        {
            SzukajKTH();
        }
        void CheckKeys(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                Btn_szukajKTH_Click(sender, (EventArgs)e);
            }
        }

        void NewList()
        {
            FirmLista.Clear();
            WczytajDaneDoDGV3();
            dateTimePicker1.Text = DateTime.Now.ToString("yyyy-MM-dd");
            Baza baza = new Baza();
            int a = Convert.ToInt32(baza.CzytajZBazy("SELECT COALESCE(MAX(nrwz), '0') FROM List"));
            WZtxt.Text = (a + 1).ToString();
            button4.Text = "Zapisz";
            ListNr_lbl.Text = "";
        }
        void SzukajKTH()
        {
            string sql;
            string nazwa = Txt_KTH.Text;
            if (NIPValidate(nazwa) != true)
            {
                sql = "select * from KontrahentNazwaView where nazwa like '%" + nazwa + "%'";
            }
            else
            {
                sql = "select * from KontrahentNIPView where nip ='" + nazwa + "'";
            }
            Baza BazaSQL = new Baza();
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = BazaSQL.Polacz(sql); ;
            dataGridView1.DataMember = "Kontrahenci";
            DataGridViewButtonColumn col = new DataGridViewButtonColumn
            {
                UseColumnTextForButtonValue = true,
                Text = "Wybierz",
                Name = "Wybor"
            };
            dataGridView1.Columns.Add(col);
            DataGridViewColumn columnNazwa = dataGridView1.Columns[1];
            DataGridViewColumn columnID = dataGridView1.Columns[0];
            columnNazwa.Width = 350;
            columnID.Width = 50;
        }
        void CreateDGV()
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
            dataGridView3.Columns[11].Width = 160;

            dataGridView3.Columns.Add("Column", "NrWZ");
            dataGridView3.Columns[12].Width = 45;

            var buttonCol = new DataGridViewButtonColumn();
            buttonCol.HeaderText = "Usun";
            buttonCol.Name = "Usun";
            buttonCol.Text = "Usun";
            buttonCol.UseColumnTextForButtonValue = true;
            dataGridView3.Columns.Add(buttonCol);
            dataGridView3.AllowUserToAddRows = false;
        }  //Utwórz nagłówki dgv listy z kontrahentami
        void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Wybor"].Index && e.RowIndex >= 0)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Wybierz")
                {
                    int rownumber = Convert.ToInt16(((DataGridView)sender).SelectedCells[0].RowIndex);
                    KontrID = dataGridView1[0, rownumber].Value.ToString();
                    data = dateTimePicker1.Text;
                    //nrwz =  zwiekszac w zaleznosci od firmlista
                    Kontrahent KontrahentForm = new Kontrahent();
                    KontrahentForm.OnRunMethod += new Kontrahent.methodHandler(WczytajDaneDoDGV3);
                    KontrahentForm.Show();
                }
                else if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString() == "Edytuj")
                {
                    string d = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
                    FirmLista.Clear();
                    PokazDok(d);
                }
            }

        } //Wyswietl Form_Kontrahent a następnie dodaj kontrahenta do zmiennej FirmLista oraz wyświetl ją w DGV
        void dataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int firmCount;
                if (e.ColumnIndex == dataGridView3.Columns["Usun"].Index && e.RowIndex >= 0)
                {
                    firmCount = FirmLista.Count;
                    FirmLista.RemoveAt(e.RowIndex);
               // MessageBox.Show(dataGridView3.Rows[e.RowIndex].Cells["nrwz"].Value.ToString());
                    WczytajDaneDoDGV3();
                }
            //---------------
            //string nrdok = 
            //string sql = "update list set aktywny=0 where id="+nrdok;  //usun dok=dokid
            //Baza BazaSQL = new Baza();
            //BazaSQL.ZapiszDoBazy(sql);
            //PokazDok(nrdok);
            //---------------

        } //Usuń wybranego kontrahenta z listy
        void WczytajDaneDoDGV3()
        {
           dataGridView3.Rows.Clear();
           dataGridView3.Refresh();
           int firmCount = FirmLista.Count;
                    for (int count = 0; count < firmCount; ++count)
                    {
                        DaneFirmy oFirma = FirmLista[count];
                        int Fuel = Convert.ToInt32(oFirma.Paliwo);
                        dataGridView3.Rows.Add(new object[] {oFirma.KontrNazwa, oFirma.KontrUlica + " " + oFirma.KontrNrDomu, oFirma.KontrMiasto + " " + oFirma.KontrKod,
                        oFirma.KontrNIP,oFirma.KontrTel,Fuel,oFirma.Ilosc.ToString(),oFirma.Cena,oFirma.FormPlat,oFirma.Termin,oFirma.Sent,oFirma.DostUlica + " " + oFirma.DostNr,oFirma.NrWZ,"Usuń" });
                    }
                }//Wyswietla dane z listy w DGV3
        void PokazDok(string nrdok)
        {
            FirmLista.Clear();
            button2.Enabled = false;
            button4.Text = "Zapisz poprawione";
            ListNr_lbl.Text = nrdok;
            string sql = "select * from WZView where dokid =" + nrdok;
            string keyname = "HKEY_CURRENT_USER\\MARKET\\ListPrzewozowy";
            rejestrIO rejestr = new rejestrIO();
            string klucz = rejestr.czytajklucz(keyname, "SQLconnect", true);
            using (SqlConnection connection = new SqlConnection(klucz))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                { FirmLista.Add(new DaneFirmy
                (reader["Data"].ToString(),
                Convert.ToInt32(reader["KontrID"]),
                reader["Nazwa"].ToString(),
                reader["Ulica"].ToString(),
                reader["NrDomu"].ToString(),
                reader["Kod"].ToString(),
                reader["Miasto"].ToString(),
                reader["Telefon"].ToString(),
                reader["NIP"].ToString(),
                Convert.ToInt32(reader["PaliwoID"]),
                Convert.ToInt32(reader["Ilosc"]),
                reader["Cena"].ToString(),
                reader["FormaPlat"].ToString(),
                reader["Termin"].ToString(),
                reader["Sent"].ToString(),
                reader["DostUlica"].ToString(),
                reader["DostNr"].ToString(),
                reader["DostMiasto"].ToString(),
                reader["DostKod"].ToString(),
                reader["DostPoczta"].ToString(),
                reader["DostKraj"].ToString(),
                reader["DostPlanRozp"].ToString(),
                reader["DostRozp"].ToString(),
                reader["DostPlanZak"].ToString(),
                reader["Uwagi"].ToString(),
                reader["nrwz"].ToString())); }
                connection.Close();
                AktualizujWczytanie(nrdok);
                WczytajDaneDoDGV3();
            }
        }
        void PokazListy()
        {
            string sql = "select * from ListyView";
            Baza bazaSQL = new Baza();
            dataGridView1.Columns.Clear();
            dataGridView1.DataSource = bazaSQL.Polacz(sql);
            dataGridView1.DataMember = "Kontrahenci";
            DataGridViewButtonColumn col = new DataGridViewButtonColumn
            {
                UseColumnTextForButtonValue = true,
                Text = "Edytuj",
                Name = "Wybor"
            };
            dataGridView1.Columns.Add(col);
        }
        void ClearDGV3()
        {
            while (dataGridView3.Rows.Count > 1)
            {
                dataGridView3.Rows.Clear();
            }

            while (dataGridView3.Columns.Count > 0)
            {
                dataGridView3.Columns.Clear();
            }
        }
        void AktualizujWczytanie(string nrdok)
        {
            PierwszaWZ(nrdok, out string FirstNRWZNew, out string FirstdataWZNewk);
            WZtxt.Text = FirstNRWZNew;
            dateTimePicker1.Text = FirstdataWZNewk;
        } //aktualizuje pole nrwz oraz date
        static void PierwszaWZ(string nrdok, out string FirstNRWZ, out string FirstdataWZ)
        {
            Baza baza = new Baza();
            FirstNRWZ = baza.CzytajZBazy("select top 1 nrwz from List where dokid=" + nrdok+ " and aktywny=1");
            FirstdataWZ = baza.CzytajZBazy("select top 1 data from List L inner join dok D on D.id = L.dokid where dokid=" + nrdok+ " and aktywny=1");
        }
        static string OstatniaWZ()
        {
            Baza baza = new Baza();
            string LastnrWZ = baza.CzytajZBazy("select top 1 nrwz from List L inner join dok D on D.id = L.dokid order by L.id desc");
            return LastnrWZ;
        }
        void ZapiszList()
            {
            int UserID = 1;
            int aktywnyDok = 1;
            data = dateTimePicker1.Value.Date.ToString("yyyy-MM-dd");
            string sql = "insert into dok (Data,userID)values('" + data + "'," + UserID + ")";  //uzytkownik id=
            Baza dok = new Baza();
            dok.ZapiszDoBazy(sql); //zapis do tabeli dok
            sql = "select COALESCE(MAX(id), '0') from dok";
            int nrdok = Convert.ToInt32(dok.CzytajZBazy(sql)); //ostatni numer dok
            sql = "SELECT COALESCE(MAX(nrwz), '0') FROM List";
            int WZnr = Convert.ToInt32(dok.CzytajZBazy(sql));
            WZnr = ++WZnr;
            int firmCount = FirmLista.Count;
            for (int count = 0; count < firmCount; ++count)
                {
                DaneFirmy oFirma = FirmLista[count];
                int Fuel = Convert.ToInt32(oFirma.Paliwo);
                MessageBox.Show(WZnr + "");
                sql = "insert into list (Dokid, KontrId, PaliwoID, Ilosc,Cena, FormaPlat,Termin, Sent,DostUlica, DostNr, DostMiasto, DostKod, DostPoczta, DostKraj, DostPlanRozp, DostRozp, DostPlanZak, NrWZ, Aktywny) " +
                       "values(" + nrdok + "," + oFirma.KontrahentID + "," + oFirma.Paliwo +"," + oFirma.Ilosc + ",'" + oFirma.Cena + "','" + oFirma.FormPlat + "','" + oFirma.Termin + "','" + oFirma.Sent + "','" + 
                        oFirma.DostUlica + "','" + oFirma.DostNr + "','" + oFirma.DostMiasto + "','" + oFirma.DostKod + "','" + oFirma.DostPoczta + "','" + oFirma.DostKraj +
                        "','" + oFirma.DostPlanRozp + "','" + oFirma.DostRozp + "','" + oFirma.DostPlanZak + "'," + WZnr + "," + aktywnyDok + ")";
                dok.ZapiszDoBazy(sql);
                WZnr = ++WZnr;
                }
            NewList();
            button2.Enabled = true;  //zezwolenie na drukowanie
            
        } 
        //TODO***************zapiszlist oraz UaktualnijList przerobić na wywoływanie procedur na serwerze SQL i jeżeli się da wykorzystać zatwierdzanie transakcji!!******************************
        void UaktualnijList()
        {
            int aktywnyDok = 1;
            int WZnr = Convert.ToInt32(WZtxt.Text);
            int Doknr = Convert.ToInt32(ListNr_lbl.Text);
            Baza dok = new Baza();
            string sql = "update list set aktywny=0,nrwz=0 where dokid=" + Doknr+" and Aktywny=1";
            dok.ZapiszDoBazy(sql);//wszystkie WZ z tego Listu jako nieaktywne
            int firmCount = FirmLista.Count;
            for (int count = 0; count < firmCount; ++count)
            {
                DaneFirmy oFirma = FirmLista[count];
                int Fuel = Convert.ToInt32(oFirma.Paliwo);
                MessageBox.Show(WZnr + "");
                sql = "insert into list (Dokid, KontrId, PaliwoID, Ilosc,Cena, FormaPlat,Termin, Sent,DostUlica, DostNr, DostMiasto, DostKod, DostPoczta, DostKraj, DostPlanRozp, DostRozp, DostPlanZak, NrWZ, Aktywny) " +
                   "values(" + Doknr + "," + oFirma.KontrahentID + "," + oFirma.Paliwo + "," + oFirma.Ilosc + ",'" + oFirma.Cena + "','" + oFirma.FormPlat + "','" + oFirma.Termin + "','" + oFirma.Sent + "','" +
                    oFirma.DostUlica + "','" + oFirma.DostNr + "','" + oFirma.DostMiasto + "','" + oFirma.DostKod + "','" + oFirma.DostPoczta + "','" + oFirma.DostKraj +
                    "','" + oFirma.DostPlanRozp + "','" + oFirma.DostRozp + "','" + oFirma.DostPlanZak + "'," + WZnr + "," + aktywnyDok + ")";
                dok.ZapiszDoBazy(sql); //dodaj na nowo wszystkie wz na nowo z FirmLista
                WZnr = ++WZnr;
            }
            NewList();
            button2.Enabled = true;
            PokazListy();
        }
        //TODO - przy zapisie zaktualizowanego Listu dopisac warunek sprawdzający, aby edycja wczesniejszych listów nie przekracza liczby wz w oryginalnym LisciePrzewozowym
        //bo się zdublują nr WZ. W teorii nie powinno się tego robić... Nie dotyczy ostatniego ListuPrzewozowego

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
        
        public void printCustomer()
                {
                    nrwz = Convert.ToInt32(WZtxt.Text);
                    data = dateTimePicker1.Text;
                    int UserId = 1; 
            
            //TODO ************************************  Pobranie Id użytkownika z DropListy - do stworzenia na później ***************************************
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
                        string DataDok = dateTimePicker1.Value.Date.ToString("yyyy-MM-dd");
            //            ZapiszDok(DataDok, UserId); //Nowy dokument w bazie
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
                
                        if (oFirma.Paliwo == 1) //ON
                            { litryON = litryON + oFirma.Ilosc; }
                        else if(oFirma.Paliwo == 2) //ONA
                            { litryONA = litryONA + oFirma.Ilosc; }
                        else
                            { litryOP = litryOP + oFirma.Ilosc; } //OP
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

          //           ZapiszList(oFirma.KontrahentID, oFirma.Paliwo, oFirma.Ilosc, oFirma.Cena, oFirma.FormPlat, oFirma.Termin, oFirma.Sent, oFirma.DostUlica, oFirma.DostNr, oFirma.DostMiasto, oFirma.DostKod,
           //                     oFirma.DostPoczta, oFirma.DostKraj, oFirma.DostPlanRozp, oFirma.DostRozp, oFirma.DostPlanZak, nrwz);
                     Baza CzytajSQL = new Baza();
                     var fuel = CzytajSQL.CzytajZBazy("select nazwa from paliwo where paliwoid=" + oFirma.Paliwo);
                     printWZ(oFirma.Ilosc.ToString(), fuel, oFirma.Cena, oFirma.FormPlat, termin, "", pierwszalinia, drugalinia, oFirma.KontrUlica
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
            string rok = dateTimePicker1.Value.Date.ToString("yyyy");
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
            pdf.DrawWZName(page, nrwz+"/"+rok, dataWZ,30);
            pdf.DrawWZBody(page,106, sentval);
            pdf.DrawWZFooter(page, parametry("/Parametry/Uzytkownik/Wartosc"),226); //Nazwisko wystawiającego w polu wystawil
            //-------część dolna WZ------------------
            pdf.DrawWZName(page, nrwz + "/"+rok, dataWZ, 450);
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
    /*    static void ZapiszDoLogu(string error)
        {
            string data = DateTime.Now.ToString();
            StackTrace stackTrace = new StackTrace();
            var nazwa=(stackTrace.GetFrame(1).GetMethod().Name);
            //File.WriteAllText(@"Logi.txt", data+": "+ error);
            File.AppendAllText(@"Logi.txt", data + ": " +nazwa+" "+ error + Environment.NewLine);
        }
        */

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
        public int Paliwo { get; private set; }
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


        public DaneFirmy(string nData, int nKontrahentID, string nKontrNazwa, string nKontrUlica, string nKontrNrDomu, string nKontrKod, string nKontrMiasto, string nKontrTel, string nKontrNIP, int nPaliwo,
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

    static class Zapisz
    {
        public static void DoLogu(string error)
        {
            string data = DateTime.Now.ToString();
            StackTrace stackTrace = new StackTrace();
            var nazwa = (stackTrace.GetFrame(1).GetMethod().Name);
            //File.WriteAllText(@"Logi.txt", data+": "+ error);
            File.AppendAllText(@"Logi.txt", data + ": " + nazwa + " " + error + Environment.NewLine);
        }
    }
}

