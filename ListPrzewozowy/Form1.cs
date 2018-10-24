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
using System.Xml.Linq;
using System.Text.RegularExpressions;
using OpenPop.Pop3;
using OpenPop.Mime;
using OpenPop.Mime.Header;
using System.Xml.XPath;
using System.Globalization;
using static ListPrzewozowy.Json;

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
        public static string Imie;
        public static string Nazwisko;
        public static string LatSent;
        public static string LongSent;
        public static string kodTeryt;
        public static List<DaneFirmy> FirmLista = new List<DaneFirmy>();
        public static string wersja = "20181024";

        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            pictureBox1.Visible = false;
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
            CreateDGV();
            Print pdf = new Print();
            NewList();
            WyswietlUser();
            WyswietlWojewodztwa();
            //var d = "20.08.2018";

            // MessageBox.Show(result.ToString());

            //TODO - dodać sprawdzanie czy baza istnieje i jest we właściwej wersji !=> inicjalizacja bazy

            SqlConnection connection = new SqlConnection(PobierzConnString());
            var NazwaBazy = connection.Database;

            Text = "List Przewozowy, " + "BazaSQL: " + NazwaBazy + ", wersja: v1." + wersja;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            // ToolTip ToolTip1 = new ToolTip();
            // ToolTip1.SetToolTip(button2, "Zapisz dokument przed wydrukowaniem.");
        }

        void OnProcessExit(object sender, EventArgs e)
        {
            //Parametry("/Parametry/NrWZ/Wartosc", nrwz.ToString()); //zapamiętanie numeru ostatniej WZ-tki
        }

        private void Print_btn_Click(object sender, EventArgs e)
        {
            string appPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            Process.Start("explorer.exe", appPath + @"\pdf\");
        }  //pokaż folder z dokumentami pdf
        private void Button2_Click(object sender, EventArgs e) //button "print" - generuje PDF
        {
            DrukujListPrzewozowy();
            //   printSender();
        }
        private void Zapisz_btn_Click_1(object sender, EventArgs e)
        {
            if (button4.Text.Contains("poprawione"))
                UaktualnijList();
            else
                ZapiszList();
        } //Button Zapisz do bazy
        private void Wczytaj_btn_Click(object sender, EventArgs e) //button "Wczytaj" - pokaz zapisane w bazie
        {
            PokazListy();
        }
        private void Button5_Click(object sender, EventArgs e)
        {
            /*            SENT dok = new SENT();
                        dok.FileName100 = AppDomain.CurrentDomain.BaseDirectory + @"\xml\SENT100test.xml";
                        dok.SENT100();
              */

        } //button5 test2
        private void wsp_btn_Click(object sender, EventArgs e)
        {
            PobierzWspolrzedne();
        }

        private void WyswietlWojewodztwa()
        {
            wojew_box.DisplayMember = "Text";
            wojew_box.ValueMember = "Value";

            var items = new[] {
                new { Text = "", Value = ""},
                new { Text = "DOLNOŚLĄSKIE", Value = "02" },
                new { Text = "KUJAWSKO-POMORSKIE", Value = "04" },
                new { Text = "LUBELSKIE", Value = "06" },
                new { Text = "LUBUSKIE", Value = "08" },
                new { Text = "ŁÓDZKIE", Value = "10" },
                new { Text = "MAŁOPOLSKIE", Value = "12" },
                new { Text = "MAZOWIECKIE", Value = "14" },
                new { Text = "OPOLSKIE", Value = "16" },
                new { Text = "PODKARPACKIE", Value = "18" },
                new { Text = "PODLASKIE", Value = "20" },
                new { Text = "POMORSKIE", Value = "22" },
                new { Text = "ŚLĄSKIE", Value = "24" },
                new { Text = "ŚWIĘTOKRZYSKIE", Value = "26" },
                new { Text = "WARMIŃSKO-MAZURSKIE", Value = "28" },
                new { Text = "WIELKOPOLSKIE", Value = "30" },
                new { Text = "ZACHODNIOPOMORSKIE", Value = "32" }
                               };
            wojew_box.DataSource = items;
           // wojew_box.SelectedIndex = 0;
        }

        private void SelectedWojewodztwo(object sender, EventArgs e)
        {
            // MessageBox.Show(wojew_box.SelectedValue.ToString());
            kodTeryt = wojew_box.SelectedValue.ToString();

        }

        private void PobierzWspolrzedne()
        {

            var suff = "&format=json&addressdetails=1&limit=0";
            var pref = @"https://geocode.pllab.itl.waw.pl/search?street=";
            string zalUlica = ZalUlica_txt.Text;
            string ZalNr = ZalNr_txt.Text;
            string ZalMiasto = ZalMiasto_txt.Text;
            var url = pref + ZalNr + " " + zalUlica + "&city=" + ZalMiasto + "&country=PL" + suff;
            Zapisz.DoLogu(url);
            Json json = new Json();
            if (json.Parsuj(url).display_name == null)
            {
                return;
            }
            byte[] bytes = Encoding.Default.GetBytes(json.Parsuj(url).display_name);
            string disp_name = Encoding.UTF8.GetString(bytes);
            string message = "Pobrane współrzędne: " + json.Parsuj(url).latitude + ", " + json.Parsuj(url).longitude + Environment.NewLine + "wskazują na: " + disp_name;
            DialogResult dialogResult = MessageBox.Show(message, "Czy dane są poprawne?", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                LatSent = json.Parsuj(url).latitude;
                LongSent = json.Parsuj(url).longitude;
            }
            else
            {
                ZalUlica_txt.Text = null;
                ZalNr_txt.Text = null;
                ZalMiasto_txt.Text = null;
            }
        }
        private void czytaj_email_Click(object sender, EventArgs e)
        {
            // CzytajEmail();
        }
        private void New_btn_Click(object sender, EventArgs e)
        {
            NewList();
        } // Nowa lista
        private void Btn_szukajKTH_Click(object sender, EventArgs e)
        {
            SzukajKTH();
        }
        private void xmlBtn_Click(object sender, EventArgs e)
        {
            string appPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
            Process.Start("explorer.exe", appPath + @"\xml\");
        } //Otworz xml
        void CheckKeys(object sender, KeyPressEventArgs e)
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
            //WZtxt.Text = (a + 1).ToString();
            nrWZ_lbl.Text = (a + 1).ToString();
            button4.Text = "Zapisz";
            button4.Enabled = true;  //Zapisz aktywny
            button2.Enabled = false; //drukuj nieaktywny
            ListNr_lbl.Text = ".";
            dataGridView1.Columns.Clear();
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
            dataGridView3.CellClick += DataGridView3_CellClick;
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

            var buttonCol = new DataGridViewButtonColumn
            {
                HeaderText = "Usun",
                Name = "Usun",
                Text = "Usun",
                UseColumnTextForButtonValue = true
            };
            dataGridView3.Columns.Add(buttonCol);
            dataGridView3.AllowUserToAddRows = false;
        }  //Utwórz nagłówki dgv listy z kontrahentami
        void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
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
                    button4.Enabled = false;
                }
            }

        } //Wyswietl Form_Kontrahent a następnie dodaj kontrahenta do zmiennej FirmLista oraz wyświetl ją w DGV
        void DataGridView3_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //  SprawdzCzyZmiany();
            int firmCount;
            if (e.ColumnIndex == dataGridView3.Columns["Usun"].Index && e.RowIndex >= 0)
            {
                firmCount = FirmLista.Count;
                FirmLista.RemoveAt(e.RowIndex);
                button2.Enabled = false;
                button4.Text = "Zapisz poprawione";
                button4.Enabled = true;
                WczytajDaneDoDGV3();
            }


        } //Usuń wybranego kontrahenta z listy
        void WczytajDaneDoDGV3()
        {
            SprawdzCzyZmiany();
            dataGridView3.Rows.Clear();
            dataGridView3.Refresh();
            int firmCount = FirmLista.Count;
            for (int count = 0; count < firmCount; ++count)
            {
                DaneFirmy oFirma = FirmLista[count];
                int Fuel = Convert.ToInt32(oFirma.Paliwo);
                dataGridView3.Rows.Add(new object[] {oFirma.KontrNazwa, oFirma.KontrUlica + " " + oFirma.KontrNrDomu, oFirma.KontrMiasto + " " + oFirma.KontrKod,
                        oFirma.KontrNIP,oFirma.KontrTel,Fuel,oFirma.Ilosc.ToString(),oFirma.Cena,oFirma.FormPlat,oFirma.Termin,oFirma.Sent,oFirma.DostUlica + " " + oFirma.DostNr,oFirma.NrWZ,"Usuń" });
                var a = oFirma.ZaladMiasto;
                ZalUlica_txt.Text = oFirma.ZaladUlica;
                ZalNr_txt.Text = oFirma.ZaladNr;
                ZalKod_txt.Text = oFirma.ZaladKod;
                ZalMiasto_txt.Text = oFirma.ZaladMiasto;
            }
            dataGridView3.ReadOnly = true;

        }//Wyswietla dane z listy w DGV3
        void SprawdzCzyZmiany()
        {
            if (ListNr_lbl.Text != ".")
            {
                int Doknr = Convert.ToInt32(ListNr_lbl.Text);
                string sql = "select count(nrwz) from List where aktywny = 1 and dokid = " + Doknr;
                Baza bazasql = new Baza();
                int Iledokbaza = Convert.ToInt32(bazasql.CzytajZBazy(sql));
                int IleDok = FirmLista.Count;
                if (Iledokbaza != IleDok)
                {
                    button4.Enabled = true;
                    button4.Text = "Zapisz poprawione";
                    button2.Enabled = false;
                }
            }
        }
        string PobierzConnString()
        {
            string keyname = "HKEY_CURRENT_USER\\MARKET\\ListPrzewozowy";
            RejestrIO rejestr = new RejestrIO();
            string connstring = rejestr.CzytajKlucz(keyname, "SQLconnect", true);
            return connstring;
        }
        void PokazDok(string nrdok)
        {
            FirmLista.Clear();
            ListNr_lbl.Text = nrdok;
            string sql = "select * from WZView where dokid =" + nrdok;
            string klucz = PobierzConnString();
            using (SqlConnection connection = new SqlConnection(klucz))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    FirmLista.Add(new DaneFirmy
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
                  reader["nrwz"].ToString(),
                  reader["ZalUlica"].ToString(),
                  reader["ZalNr"].ToString(),
                  reader["ZalMiasto"].ToString(),
                  reader["ZalKod"].ToString(),
                  reader["KodTeryt"].ToString(),
                  reader["LatSent"].ToString(),
                  reader["LongSent"].ToString()
                  ));
                }
                connection.Close();
                AktualizujWczytanie(nrdok);
                WczytajDaneDoDGV3();
               
            }
        }
        void PokazListy()
        {
            string sql = "select * from ListyView order by dokid";
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
            button2.Enabled = true;
            dataGridView1.ReadOnly = true;
            this.dataGridView1.FirstDisplayedScrollingRowIndex = this.dataGridView1.Rows.Count - 1;
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
            //WZtxt.Text = FirstNRWZNew;
            nrWZ_lbl.Text = FirstNRWZNew;
            dateTimePicker1.Text = FirstdataWZNewk;
        } //aktualizuje pole nrwz oraz date
        static void PierwszaWZ(string nrdok, out string FirstNRWZ, out string FirstdataWZ)
        {
            Baza baza = new Baza();
            FirstNRWZ = baza.CzytajZBazy("select top 1 nrwz from List where dokid=" + nrdok + " and aktywny=1");
            FirstdataWZ = baza.CzytajZBazy("select top 1 data from List L inner join dok D on D.id = L.dokid where dokid=" + nrdok + " and aktywny=1");
        }
        static string OstatniaWZ()
        {
            Baza baza = new Baza();
            string LastnrWZ = baza.CzytajZBazy("SELECT COALESCE(MAX(nrwz), '0') FROM List where aktywny=1");
            return LastnrWZ;
        }
        void WyswietlUser()
        {
            string klucz = PobierzConnString();
            using (SqlConnection conn = new SqlConnection(klucz))
            {
                string query = "select Nazwa, ID, Imie, Nazwisko from Uzytkownik";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                conn.Open();
                DataSet ds = new DataSet();
                da.Fill(ds, "User");
                UserBox.DisplayMember = "Nazwa";
                UserBox.ValueMember = "ID";
                UserBox.DataSource = ds.Tables["User"];
            }
            UserBox.DropDownStyle = ComboBoxStyle.DropDownList;
        }
        private void Userbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView view = UserBox.SelectedItem as DataRowView;
            string name = view["Nazwa"].ToString();
            int id = Convert.ToInt32(view["Id"]);
            Imie = view["Imie"].ToString();
            Nazwisko = view["Nazwisko"].ToString();
        }
        int PobierzIDUsera()
        {
            DataRowView view = UserBox.SelectedItem as DataRowView;
            int id = Convert.ToInt32(view["Id"]);
            return id;
        }
        string PobierzNazweUsera()
        {
            DataRowView view = UserBox.SelectedItem as DataRowView;
            string name = view["Nazwa"].ToString();
            return name;
        }
        bool CzySent()
        {
            int firmCount = FirmLista.Count;
            for (int count = 0; count < firmCount; ++count)
            {
                DaneFirmy oFirma = FirmLista[count];
                if (oFirma.Sent != "")
                {
                    return true;
                }
            }
            return false;
        }
        void ZapiszList()
        {
            if (CzySent() && ZalUlica_txt.Text == "")
            {
                MessageBox.Show("Nie wypelniono pól z adresem załadunku! " + ZalUlica_txt.Text, "SENT");
                return;
            }
            int UserID = PobierzIDUsera();
            int aktywnyDok = 1;
            data = dateTimePicker1.Value.Date.ToString("yyyy-MM-dd");
            string sql = "insert into dok (Data,userID)values('" + data + "'," + UserID + ")";
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
                sql = "insert into list (Dokid, KontrId, PaliwoID, Ilosc,Cena, FormaPlat,Termin, Sent," +
                    "DostUlica, DostNr, DostMiasto, DostKod, DostPoczta, DostKraj, DostPlanRozp, DostRozp, DostPlanZak, Uwagi, NrWZ, Aktywny,ZalUlica,ZalNr, ZalMiasto, ZalKod, KodTeryt, LatSent, LongSent) " +
                       "values(" + nrdok + "," + oFirma.KontrahentID + "," + oFirma.Paliwo + "," + oFirma.Ilosc + ",'" + oFirma.Cena + "','" + oFirma.FormPlat + "','" + oFirma.Termin + "','" + oFirma.Sent + "','" +
                        oFirma.DostUlica + "','" + oFirma.DostNr + "','" + oFirma.DostMiasto + "','" + oFirma.DostKod + "','" + oFirma.DostPoczta + "','" + oFirma.DostKraj +
                        "','" + oFirma.DostPlanRozp + "','" + oFirma.DostRozp + "','" + oFirma.DostPlanZak + "','" + oFirma.Uwagi + "'," + WZnr + "," + aktywnyDok +
                        ",'" + ZalUlica_txt.Text + "','" + ZalNr_txt.Text + "','" + ZalMiasto_txt.Text + "','" + ZalKod_txt.Text + "','"+kodTeryt+"','"+LatSent+"','"+LongSent+"')";
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
            int UserID = PobierzIDUsera();
            if (CzySent() && ZalUlica_txt.Text == "")
            {
                MessageBox.Show("Nie wypelniono pól z adresem załadunku! " + ZalUlica_txt.Text, "SENT");
                return;
            }
            //int WZnr = Convert.ToInt32(WZtxt.Text);
            int WZnr = Convert.ToInt32(nrWZ_lbl.Text);
            int Doknr = Convert.ToInt32(ListNr_lbl.Text);
            data = dateTimePicker1.Value.Date.ToString("yyyy-MM-dd");
            Baza dok = new Baza();
            string sql = "update dok set userid=" + UserID + ",data= '" + data + "' where id=" + Doknr;
            dok.ZapiszDoBazy(sql); //Uaktualnij uzytkownika i datę w dok
            sql = "update list set aktywny=0,nrwz=0 where dokid=" + Doknr + " and Aktywny=1";
            dok.ZapiszDoBazy(sql);//wszystkie WZ z tego Listu jako nieaktywne
            int firmCount = FirmLista.Count;
            for (int count = 0; count < firmCount; ++count)
            {
                DaneFirmy oFirma = FirmLista[count];
                int Fuel = Convert.ToInt32(oFirma.Paliwo);
                sql = "insert into list (Dokid, KontrId, PaliwoID, Ilosc,Cena, FormaPlat,Termin, Sent," +
                    "DostUlica, DostNr, DostMiasto, DostKod, DostPoczta, DostKraj, DostPlanRozp, DostRozp, DostPlanZak, Uwagi, NrWZ, Aktywny,ZalUlica,ZalNr, ZalMiasto, ZalKod, KodTeryt, LatSent, LongSent) " +
                       "values(" + Doknr + "," + oFirma.KontrahentID + "," + oFirma.Paliwo + "," + oFirma.Ilosc + ",'" + oFirma.Cena + "','" + oFirma.FormPlat + "','" + oFirma.Termin + "','" + oFirma.Sent + "','" +
                        oFirma.DostUlica + "','" + oFirma.DostNr + "','" + oFirma.DostMiasto + "','" + oFirma.DostKod + "','" + oFirma.DostPoczta + "','" + oFirma.DostKraj +
                        "','" + oFirma.DostPlanRozp + "','" + oFirma.DostRozp + "','" + oFirma.DostPlanZak + "','" + oFirma.Uwagi + "'," + WZnr + "," + aktywnyDok +
                         ",'" + ZalUlica_txt.Text + "','" + ZalNr_txt.Text + "','" + ZalMiasto_txt.Text + "','" + ZalKod_txt.Text + "','" + kodTeryt + "','" + LatSent + "','" + LongSent + "')";
                dok.ZapiszDoBazy(sql); //dodaj wszystkie wz na nowo z FirmLista
                WZnr = ++WZnr;
            }

            //PrzenumerujWZ(WZnr-1);
            PrzenumerujWZ();
            NewList();
            button2.Enabled = true;
            PokazListy();
        }

        void PrzenumerujWZ()
        {
            int WZOffset = Convert.ToInt32(Parametry("/Parametry/NrWZ/Wartosc"));  //ostatnia wystawiona WZ
            Baza dok = new Baza();
            string sql = "UPDATE list SET nrwz = (rowNumber +" + WZOffset + ") FROM list INNER JOIN (SELECT ID, row_number() OVER (ORDER BY dokID) as rowNumber " +
                "FROM list where aktywny = 1) drRowNumbers ON drRowNumbers.ID = list.ID";
            dok.ZapiszDoBazy(sql);
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
        void CzytajEmail()
        {
            string POPserwer = Parametry("/Parametry/POPserwer/Wartosc");
            int POPport = Convert.ToInt32(Parametry("/Parametry/POPport/Wartosc"));
            string POPemail = Parametry("/Parametry/POPemail/Wartosc");
            string POPhaslo = Parametry("/Parametry/POPhaslo/Wartosc");
            OdbierzXML(POPserwer, POPport, true, POPemail, POPhaslo);
            //OdbierzXML("poczta.o2.pl", 995, true, "test_vir2@o2.pl", "Test1234");
            //MessageBox.Show(POPserwer+ POPport+ POPemail+ POPhaslo);
        }

        void OdbierzXML(string hostname, int port, bool useSsl, string username, string password)
        {
            pictureBox1.Visible = true;
            Application.DoEvents();
            string folder = AppDomain.CurrentDomain.BaseDirectory + @"\xml\";
            using (Pop3Client client = new Pop3Client())
            {
                client.Connect(hostname, port, useSsl);
                client.Authenticate(username, password);

                List<string> uids = client.GetMessageUids();
                Baza dok = new Baza();
                int count = 0;
                for (int i = 0; i < uids.Count; i++)
                {
                    string sql = "SELECT UUID FROM PocztaSent where UUID='" + uids[i] + "'";
                    string UUID = dok.CzytajZBazy(sql);
                    if (UUID != uids[i])
                    {
                        OpenPop.Mime.Message message = client.GetMessage(i + 1);
                        foreach (MessagePart attachment in message.FindAllAttachments())
                        {
                            if (Path.GetExtension(attachment.FileName) == ".xml")
                            {

                                File.WriteAllBytes(folder + attachment.FileName, attachment.Body);
                                MessageHeader header = client.GetMessageHeaders(i + 1);
                                string subject = header.Subject;
                                sql = "insert into PocztaSent (UUID,Subject) values ('" + uids[i] + "','" + subject + "')";
                                dok.ZapiszDoBazy(sql);
                                ZapiszKluczeSent(folder + attachment.FileName);

                                count = ++count;
                                // MessageBox.Show("Zakonczono odbieranie " + attachment.FileName + ",uuid: " + uids[i]);
                            }
                        }
                    }
                }
                client.Disconnect();
                pictureBox1.Visible = false;
                MessageBox.Show("Zakończono sprawdzanie poczty. Odebrano " + count + " wiadomości");
            }
        }
        void ZapiszKluczeSent(string file)
        {
            var xml = XDocument.Load(file);
            var nodes = xml.Descendants();
            var SentNumber = nodes.First(d => d.Name.ToString().EndsWith("SentNumber")).Value;
            var senderKey = nodes.First(d => d.Name.ToString().EndsWith("SenderKey")).Value;
            var recipientKey = nodes.First(d => d.Name.ToString().EndsWith("RecipientKey")).Value;
            var carrierKey = nodes.First(d => d.Name.ToString().EndsWith("CarrierKey")).Value;
            MessageBox.Show(senderKey + " " + recipientKey + " " + carrierKey, SentNumber);
            if (SentNumber.Length > 0)
            {
                Baza dok = new Baza();
                string sql = "update List set SentNumber='" + SentNumber + "',SenderKey='" + senderKey + "',RecipientKey='" + recipientKey + "',CarrierKey='" + carrierKey + "'";
                dok.ZapiszDoBazy(sql);
                //========WyslijSent110(); //Aktualizuj dane przewoźnika

            }
        }

        void DrukujListPrzewozowy()
        {
            nrwz = Convert.ToInt32(nrWZ_lbl.Text);
            data = dateTimePicker1.Text;
            string rok = dateTimePicker1.Value.Date.ToString("yyyy");
            string filename = AppDomain.CurrentDomain.BaseDirectory + @"\pdf\wykaz_kierowca_" + data + ".pdf";
            string termin;
            Boolean sentval;
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            int heightRowCust = 85;
            int litryON = 0;
            int litryONA = 0;
            int litryOP = 0;
            int numpage = 1;
            int nr = 0;
            page = document.Pages[numpage - 1];
            Print pdf = new Print();
            pdf.DrawHeader(page, data);
            pdf.DrawFooters(page, numpage);
            string DataDok = dateTimePicker1.Value.Date.ToString("yyyy-MM-dd");
            int firmCount = FirmLista.Count;
            for (int count = 0; count < firmCount; ++count)  //dla wszystkich odbiorców w zmiennej
            {
                nr = nrwz + count;
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
                else if (oFirma.Paliwo == 2) //ONA
                { litryONA = litryONA + oFirma.Ilosc; }
                else if (oFirma.Paliwo == 3) //OP
                { litryOP = litryOP + oFirma.Ilosc; }

                if (oFirma.FormPlat == "Gotówka")
                    termin = oFirma.Termin;
                else
                    termin = oFirma.Termin + " dni";

                pdf.DrawCustomer(page, heightRowCust, oFirma.KontrNazwa, oFirma.KontrUlica + " " + oFirma.KontrNrDomu + ", " + oFirma.KontrKod + " " + oFirma.KontrMiasto, oFirma.KontrNIP, oFirma.KontrTel,
                    oFirma.DostUlica + " " + oFirma.DostNr + "," + oFirma.DostMiasto + "," + oFirma.Uwagi,
                    oFirma.Ilosc.ToString(), oFirma.Sent + ", " + oFirma.Cena + " " + oFirma.FormPlat + "," + termin);
                heightRowCust = heightRowCust + 70;
                StringBuilder completedWord = new StringBuilder();
                int znaki = oFirma.KontrNazwa.Count();
                string pierwszalinia = "";
                string drugalinia = "";
                if (znaki > 35)
                {
                    List<string> lines = oFirma.KontrNazwa.SplitOn(35);
                    pierwszalinia = lines[0];
                    drugalinia = lines[1];
                }
                else
                    pierwszalinia = oFirma.KontrNazwa;
                int f = 1;
                while (File.Exists(filename))
                { filename = AppDomain.CurrentDomain.BaseDirectory + @"\pdf\wykaz_kierowca_" + data + "_" + f + ".pdf"; f++; }
                if (oFirma.Sent.Length != 0)
                    sentval = true;
                else sentval = false;
                Baza CzytajSQL = new Baza();
                var fuel = CzytajSQL.CzytajZBazy("select nazwa from paliwo where paliwoid=" + oFirma.Paliwo);
                DrukujWZ(nrwz + count, oFirma.Ilosc.ToString(), fuel, oFirma.Cena, oFirma.FormPlat, termin, oFirma.DostUlica + " " + oFirma.DostNr + "," + oFirma.DostMiasto, pierwszalinia, drugalinia, oFirma.KontrUlica
                        + " " + oFirma.KontrNrDomu + ", " + oFirma.KontrKod + " " + oFirma.KontrMiasto, "NIP/PESEL:" + oFirma.KontrNIP, "tel:" + oFirma.KontrTel, sentval);
                //=========================================
                if (sentval)
                {
                    nr = nrwz + count;
                    SENT dok = new SENT
                    {
                        FileName100 = AppDomain.CurrentDomain.BaseDirectory + @"\xml\SENT100_WZ" + nr + "_" + rok + ".xml",
                        SenderName = Parametry("/Parametry/SenderName/Wartosc"),
                        SenderNIP = Parametry("/Parametry/SenderNIP/Wartosc"),
                        SenderStreet = Parametry("/Parametry/SenderStreet/Wartosc"),
                        SenderHouseNumber = Parametry("/Parametry/SenderHouseNumber/Wartosc")
                    };
                    if (Parametry("/Parametry/SenderFlatNumber/Wartosc") != string.Empty)
                    {
                        dok.SenderFlatNumber = "Brak";
                    }
                    else
                    {
                        dok.SenderFlatNumber = Parametry("/Parametry/SenderFlatNumber/Wartosc");
                    }
                    dok.SenderCity = Parametry("/Parametry/SenderCity/Wartosc");
                    dok.SenderPostalCode = Parametry("/Parametry/SenderPostalCode/Wartosc");

                    dok.RecipientName = oFirma.KontrNazwa;
                    dok.RecipientNIP = oFirma.KontrNIP;
                    dok.RecipientStreet = oFirma.KontrUlica;
                    dok.RecipientHouseNumber = oFirma.KontrNrDomu;
                    dok.RecipientCity = oFirma.KontrMiasto;
                    dok.RecipientPostalCode = oFirma.KontrKod;

                    dok.CodeTERC = CzytajSQL.CzytajZBazy("select KodTeryt from WZView where nrWZ=" + nr); 
                    dok.Latitude = CzytajSQL.CzytajZBazy("select LatSent from WZView where nrWZ=" + nr);
                    dok.Longitude = CzytajSQL.CzytajZBazy("select LongSent from WZView where nrWZ=" + nr);


                    dok.LoadingStreet = CzytajSQL.CzytajZBazy("select ZalUlica from WZView where nrWZ=" + nr);
                    dok.LoadingHouseNumber = CzytajSQL.CzytajZBazy("select Zalnr from WZView where nrWZ=" + nr);
                    dok.LoadingCity = CzytajSQL.CzytajZBazy("select Zalmiasto from WZView where nrWZ=" + nr);
                    dok.LoadingPostalCode = CzytajSQL.CzytajZBazy("select Zalkod from WZView where nrWZ=" + nr);
                    string ZalplanStarttemp = CzytajSQL.CzytajZBazy("select DostPlanRozp from WZView where nrWZ=" + nr);
                    DateTime DataPlanStart = DateTime.ParseExact(ZalplanStarttemp, "dd.MM.yyyy", CultureInfo.InvariantCulture);
                    string ZalplanStart = DataPlanStart.ToString("yyyy-MM-dd");
                    dok.LoadingPlanStart = ZalplanStart + "+02:00";

                    dok.GoodsName = fuel;
                    dok.AmountOfGoods = oFirma.Ilosc.ToString();

                    dok.DocumentId = "WZ" + nr + "/" + rok;
                    dok.Comments = "Sprzedaż obwoźna";
                    dok.email1 = Parametry("/Parametry/POPemail/Wartosc");
                    dok.FirstName = Imie;
                    dok.LastName = Nazwisko;

                    dok.SENT100();
                }
                //=====================================
                if (awariaCHK.Checked && sentval == true) DrawSENTawaria("WZ" + nr + "/" + rok);
                //***************cos nie dokladnie sprawdza warunek sentval-----sprawdzić
            }
            page = document.Pages[0];
            pdf.DrawBody(page, litryON, litryONA, litryOP);
            document.Save(filename);
            Process.Start(filename);

        } //drukuj list przewozowy

        void DrukujWZ(int nrwz, string ilosc, string paliwo, string cena, string formaplatWZ, string termin, string uwagiN, string line1, string line2, string line3, string line4, string line5, Boolean sentval)
        {

            string filename = AppDomain.CurrentDomain.BaseDirectory + @"\pdf\WZ_" + nrwz + ".pdf";
            string rok = dateTimePicker1.Value.Date.ToString("yyyy");
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            Print pdf = new Print
            {
                ilosc = ilosc,
                paliwo = paliwo,
                uwagi = formaplatWZ + " " + termin,
                uwagiN = uwagiN
            };
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(file);
            XmlNodeList nodeList = xmlDoc.SelectNodes("/Parametry/Firma/Wartosc");
            int l = 0;
            foreach (XmlNode _node in nodeList)
            {
                pdf.lineour[l] = _node.InnerText.ToString(); //Kolejne linie nazwy naszej firmy z xml (6 linii w ramce w pdf)
                l = l + 1;
            }
            string dataWZ = dateTimePicker1.Value.Date.ToString("dd.MM.yyyy");
            pdf.line1 = line1;
            pdf.line2 = line2;
            pdf.line3 = line3;
            pdf.line4 = line4;
            pdf.line5 = line5;
            pdf.cenapaliwa = cena + " zł";
            pdf.DrawWZName(page, nrwz + "/" + rok, dataWZ, 30);
            pdf.DrawWZBody(page, 106, sentval);
            pdf.DrawWZFooter(page, PobierzNazweUsera(), 226); //Nazwisko wystawiającego w polu wystawil
            //-------część dolna WZ------------------
            pdf.DrawWZName(page, nrwz + "/" + rok, dataWZ, 450);
            pdf.DrawWZBody(page, 526, sentval);
            pdf.DrawWZFooter(page, PobierzNazweUsera(), 646); //Nazwisko wystawiającego w polu wystawil
            //---------------------------------------
            document.Save(filename);
            Process.Start(filename);
            nrwz = ++nrwz; //następny numer WZ
        }

        string Parametry(string param)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("parametry.xml");
            XmlNode dane = xmlDoc.DocumentElement.SelectSingleNode(param);
            return dane.InnerText;
        }  //odczytuje gałąź konfiguracji z xml
        void Parametry(string param, string val)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(file);
            xmlDoc.SelectSingleNode(param).InnerText = val;
            xmlDoc.Save(file);
        }  //zapisuje konfigurację do xml

        #region
        void ZapiszSENT(string param, string val)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(file);
            xmlDoc.SelectSingleNode(param).InnerText = val;
            xmlDoc.Save(file);
        }  //zapisuje SENT
        #endregion
        private void DrawSENTawaria(string _OwnNumber)
        {

            string filename = "SENT_awaria_WZ_" + nrwz + ".pdf";
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            SentAwaria awaria = new SentAwaria
            {
                OwnNumber = _OwnNumber
            };

            awaria.DrawPage1(page);
            page = document.AddPage();
            awaria.DrawPage2(page);
            document.Save(filename);
            Process.Start(filename);
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

        public string ZaladUlica { get; private set; }
        public string ZaladNr { get; private set; }
        public string ZaladMiasto { get; private set; }
        public string ZaladKod { get; private set; }

        public string DostPlanRozp { get; private set; }
        public string DostRozp { get; private set; }
        public string DostPlanZak { get; private set; }
        public string Uwagi { get; private set; }
        public string KontrNIP { get; private set; }
        public string NrWZ { get; private set; }
        public string KodTeryt { get; private set; }
        public string LatSent { get; private set; }
        public string LongSent { get; private set; }


        public DaneFirmy(string nData, int nKontrahentID, string nKontrNazwa, string nKontrUlica, string nKontrNrDomu, string nKontrKod, string nKontrMiasto, string nKontrTel, string nKontrNIP, int nPaliwo,
            int nIlosc, string nCena, string nFormPlat, string nTermin, string nSent, string nDostUlica, string nDostNr, string nDostMiasto, string nDostKod, string nDostPoczta, string nDostKraj,
            string nDostPlanRozp, string nDostRozp, string nDostPlanZak, string nUwagi, string nNrWZ, string nZaladUlica, string nZaladNr, string nZaladMiasto, string nZaladKod, string nKodTeryt, string nLatSent, string nLongSent)
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
            ZaladUlica = nZaladUlica;
            ZaladNr = nZaladNr;
            ZaladMiasto = nZaladMiasto;
            ZaladKod = nZaladKod;
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
    public static class StringExtensions
    {
        public static List<string> SplitOn(this string initial, int MaxCharacters)
        {
            List<string> lines = new List<string>();

            if (string.IsNullOrEmpty(initial) == false)
            {
                string targetGroup = "Line";
                string pattern = string.Format(@"(?<{0}>.{{1,{1}}})(?:\W|$)", targetGroup, MaxCharacters);

                lines = Regex.Matches(initial, pattern, RegexOptions.Multiline | RegexOptions.CultureInvariant)
                             .OfType<Match>()
                             .Select(mt => mt.Groups[targetGroup].Value)
                             .ToList();
            }
            return lines;
        }
    }

}

