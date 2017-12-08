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
     /*   public static string DostarczUlica;
        public static string DostarczNrdomu;
        public static string DostarczMiasto;
        public static string DostarczKod; */
        public static string nip;
        public static string telefon;
        public static string[] KontrahentDane = new string[21];

        public static List<DaneFirmy> FirmLista = new List<DaneFirmy>();
        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            AppDomain.CurrentDomain.ProcessExit += new EventHandler(OnProcessExit);
            Createtable();
            odbiorcy.Visible = false;
            string[] kontr;
            kontr = new string[5];
            print pdf = new print();
            WZtxt.Text=parametry("/Parametry/NrWZ/Wartosc");
            nrwz = Convert.ToInt32(WZtxt.Text);

            //----testy array-----------------------
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
            odbiorcy.Visible = true;
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
            
        } //Wyswietl Form_Kontrahent a następnie dodaj kontrahenta do listy i wyświetl TLP
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

   /*     private void DodajKontrahentList(int rownumber,string nazwa, string ulica, string nrdomu, string kod, string miasto, string nip, string telefon)
        {
            FirmLista.Add(new DaneFirmy(rownumber, nazwa, ulica, nrdomu));
        }
        */

        public void Createtable()
        {
              // TableLayoutPanel odbiorcy = new TableLayoutPanel();
            odbiorcy.Controls.Add(new Label() { Text = "Nazwa" }, 0, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Adres" }, 1, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Miasto" }, 2, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Nip" }, 3, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Telefon" }, 4, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Paliwo" }, 5, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Ilość(litry)" }, 6, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Cena" }, 7, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Forma płat." }, 8, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Termin" }, 9, 0); 
            odbiorcy.Controls.Add(new Label() { Text = "SENT" }, 10, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Adres dostawy" }, 11, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Usuń" }, 12, 0);

        }
        private void DodajKontrahenta(string nazwa, string ulica, string nrdomu, string kod, string miasto, string nip, string telefon)
        {
            Cursor.Current = Cursors.WaitCursor;
            RowStyle temp = odbiorcy.RowStyles[odbiorcy.RowCount - 1];
            odbiorcy.RowCount++;
            odbiorcy.AutoSize = true;
            odbiorcy.RowStyles.Add(new RowStyle(temp.SizeType, temp.Height));
            odbiorcy.Controls.Add(new Label() { Text = nazwa, AutoSize = true }, 0, odbiorcy.RowCount - 1);
            odbiorcy.Controls.Add(new Label() { Text = ulica, AutoSize = true }, 1, odbiorcy.RowCount - 1);
            odbiorcy.Controls.Add(new Label() { Text = nrdomu}, 2, odbiorcy.RowCount - 1);
            odbiorcy.Controls.Add(new Label() { Text = kod}, 3, odbiorcy.RowCount - 1);
            odbiorcy.Controls.Add(new Label() { Text = miasto, AutoSize = true }, 4, odbiorcy.RowCount - 1);
            odbiorcy.Controls.Add(new Label() { Text = nip }, 5, odbiorcy.RowCount - 1);
            odbiorcy.Controls.Add(new Label() { Text = telefon }, 6, odbiorcy.RowCount - 1);
            AddComboPaliwo(); //7
            odbiorcy.Controls.Add(new TextBox() { Dock = DockStyle.Fill }, 8, odbiorcy.RowCount - 1);//litry
            odbiorcy.Controls.Add(new TextBox() { Dock = DockStyle.Fill }, 9, odbiorcy.RowCount - 1);//cena
            AddComboPlatnosc(); //10
            odbiorcy.Controls.Add(new TextBox() { Dock = DockStyle.Fill }, 11, odbiorcy.RowCount - 1);//UWAGI
            odbiorcy.Controls.Add(new TextBox() { Dock = DockStyle.Fill}, 12, odbiorcy.RowCount - 1);//Uwagi

            Button button = new Button{Name = (odbiorcy.RowCount).ToString(),Text = "Usun"};
            button.Click += new EventHandler(usun_Click);
            odbiorcy.Controls.Add(button, 13, odbiorcy.RowCount - 1);
            AddComboSENT(); //14
        }

        void item_SelectedIndexChanged(object sender, EventArgs e)
        {
          //  MessageBox.Show(((ComboBox)sender).Text);
        }

        private void AddComboPaliwo()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(file);
            XmlNodeList nodeList = xmlDoc.SelectNodes("/Parametry/Paliwo/Wartosc");
            for (int comboIndex = 1; comboIndex < 2; comboIndex++)
            {
                ComboBox combo = new ComboBox();
                foreach (XmlNode _node in nodeList)
                {
                    String nodeVal = _node.InnerText.ToString();
                    combo.Items.Add(nodeVal.ToString());
                }
                //  combo.SelectedIndexChanged += new EventHandler(item_SelectedIndexChanged);
                combo.SelectedIndex = 0;
                odbiorcy.Controls.Add(combo, 7, odbiorcy.RowCount - 1);
            }
        } //pobiera towary z xml-a
        private void AddComboPlatnosc()
        {
            for (int comboIndex = 1; comboIndex < 2; comboIndex++)
            {
                ComboBox combo = new ComboBox();
                combo.Items.Add("Gotówka");
                combo.Items.Add("Przelew");
                combo.SelectedIndexChanged += new EventHandler(item_SelectedIndexChanged);
                combo.SelectedIndex = 0;
                odbiorcy.Controls.Add(combo, 10, odbiorcy.RowCount - 1);
            } //Dodaje combo z wyborem płatności

        }
        private void AddComboSENT()
        {
            for (int comboIndex = 1; comboIndex < 2; comboIndex++)
            {
                ComboBox combo = new ComboBox();
                combo.Items.Add("");
                combo.Items.Add("SENT my zamykamy");
                combo.Items.Add("SENT oni zamykają");
                combo.SelectedIndexChanged += new EventHandler(item_SelectedIndexChanged);
                combo.SelectedIndex = 0;
                odbiorcy.Controls.Add(combo, 14, odbiorcy.RowCount - 1);
            } //Dodaje combo z wyborem SENT

        }
        private void Button2_Click(object sender, EventArgs e) //button "print generuje PDF z tablelayout"
        {
            printCustomer();
         //   printSender();
        }
        private void button3_Click(object sender, EventArgs e) //button "Wczytaj" - pokaz zapisane w bazie
        {
            string sql;
            sql = "select data,nazwa, paliwo, ilosc, cena, termin, sent from OTD.dbo.kontrahent join list on list.kontrid = OTD.dbo.kontrahent.kontrid";
            
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

            odbiorcy.Visible = true;

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

        public void RemoveRow(TableLayoutPanel panel, int rowIndex)
        {
            if (panel.RowCount >1 && rowIndex <panel.RowCount)
            {
                panel.RowStyles.RemoveAt(rowIndex);
                for (int columnIndex = 0; columnIndex < panel.ColumnCount; columnIndex++)
                {
                    var control = panel.GetControlFromPosition(columnIndex, rowIndex);
                    panel.Controls.Remove(control);
                }
                for (int i = rowIndex + 1; i < panel.RowCount; i++)
                {
                    for (int columnIndex = 0; columnIndex < panel.ColumnCount; columnIndex++)
                    {
                        var control = panel.GetControlFromPosition(columnIndex, i);
                        panel.SetRow(control, i - 1);
                    }
                }
                panel.RowCount--;
            }
            else
            {
                MessageBox.Show("nie bangla");
            }
        }

        public void usun_Click(object sender, EventArgs e)
        {
            Control ctrlactive = this.ActiveControl;
            TableLayoutPanelCellPosition celpos = odbiorcy.GetCellPosition(ctrlactive);
            int rownum = celpos.Row;
            RemoveRow(odbiorcy, rownum);
        }
        public void printCustomer()
        {
            nrwz = Convert.ToInt32(WZtxt.Text);
            //MessageBox.Show(odbiorcy.GetControlFromPosition(7, 1).Text.ToString());
            data = dateTimePicker1.Text;
            string filename = "wykaz_" + data+".pdf";
            PdfDocument document = new PdfDocument(); 
            PdfPage page = document.AddPage(); 
            int heightRowCust = 85;
            int litryON = 0;
            int litryOP = 0;
            int numpage = 1;
            page = document.Pages[numpage-1]; //numpage-1, ponieważ w document numeracja pages jest od 0.
            string[] cust = new string[11]; //Dane kontrahenta z tablelayout
            print pdf = new print();
            pdf.DrawHeader(page, data);
            pdf.DrawFooters(page, numpage);
            for (int i = 1; i < odbiorcy.RowCount; i++)  //dla wszystkich odbiorców w TL
            {
                if (heightRowCust + 55 > page.Height - 30) //Jeżeli koniec strony
                {
                    page = document.AddPage();
                    numpage++;
                    page = document.Pages[numpage-1];
                    heightRowCust = 55;
                    pdf.DrawFooters(page, numpage);
                }
                for (int c = 0; c < cust.Length; c++)
                    { cust[c] = odbiorcy.GetControlFromPosition(c, i).Text.ToString(); };
                string nazwakontrahenta = cust[0];
                string ilosc = cust[6];
                string paliwo = odbiorcy.GetControlFromPosition(5, i).Text.ToString();
                string termin = cust[9];
              //  string uwagiN = cust[11];
                string pierwszalinia = "";
                string drugalinia = "";
                string trzecialinia = cust[1] + ", " + cust[2] ;
                string NIPlinia = cust[3];
                string tellinia = cust[4];
                string sentlinia = "";
                if (cust[10].Length > 1) { sentlinia = cust[10]; }
                string formaplat = cust[8];
                string cena = cust[7];

                if (String.IsNullOrEmpty(ilosc))
                { litryON = 0; MessageBox.Show("Nie podano litrów"); return; }
                else
                {
                    if (paliwo == "Olej napędowy")
                    { litryON = litryON + Convert.ToUInt16(ilosc); }
                    else
                    { litryOP = litryOP + Convert.ToUInt16(ilosc); }
                };
                pdf.DrawCustomer(page, heightRowCust, nazwakontrahenta, trzecialinia, NIPlinia, tellinia, "", ilosc, sentlinia+", "+cena+"-"+formaplat+","+termin);
                heightRowCust = heightRowCust + 70;
                //----------------drukowanie WZ dla każdego kontrahenta---------------

                StringBuilder completedWord = new StringBuilder();
                int znaki = nazwakontrahenta.Count();
                if (znaki > 35)
                {
                    completedWord.Append(nazwakontrahenta.Substring(0, 35));//Jeżeli za długa nazwa kontrahenta, to po 35 znaku podzielic na 2 linie
                    completedWord.AppendLine();
                    pierwszalinia = completedWord.ToString();
                    completedWord.Clear();
                    completedWord.Append(nazwakontrahenta.Substring(35, znaki - 35));
                    drugalinia = completedWord.ToString();
                }
                else
                    pierwszalinia = nazwakontrahenta;
                
                int f = 1;
                while (File.Exists(filename)) { filename = "wykaz_kierowca_" + data + "_"+f+".pdf";f++; }
                ZapiszDoBazy();
                printWZ(ilosc, paliwo,cena, formaplat,  termin, "" , pierwszalinia, drugalinia,trzecialinia,"NIP/PESEL:"+NIPlinia,"tel:"+tellinia);
                //--------------------------------------------------------------------
                //MessageBox.Show(formaplat + cena);
            }
            page = document.Pages[0];
            pdf.DrawBody(page, litryON, litryOP);
            document.Save(filename);
            Process.Start(filename);
        }
        public void printWZ(string ilosc, string paliwo,string cena,string formaplat, string termin, string uwagiN, string line1, string line2, string line3, string line4, string line5)
        {
           
            string filename = "WZ_"+nrwz+".pdf";
            //MessageBox.Show(odbiorcy.GetControlFromPosition(7, 1).Text.ToString());
            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            print pdf = new print();
            pdf.ilosc = ilosc;
            pdf.paliwo = paliwo;
            pdf.uwagi = formaplat + "," + termin;
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
            if (formaplat == "Przelew") { cena = ""; }
            pdf.cenapaliwa = cena;
            pdf.DrawWZName(page, nrwz+"/2017", dataWZ,30);
            pdf.DrawWZBody(page,106);
            pdf.DrawWZFooter(page, parametry("/Parametry/Uzytkownik/Wartosc"),226); //Nazwisko wystawiającego w polu wystawil
            //-------część dolna WZ------------------
            pdf.DrawWZName(page, nrwz + "/2017", dataWZ, 450);
            pdf.DrawWZBody(page, 526);
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
        } 

        public void WczytajDane()
        {
            //-----tutaj będzie dopisywanie danych z form2 do tablicy list z kontrahentami widocznymi w TLP, a nastepnie zapis tej tablicy do sql---------------
         //   FirmLista.Add(new DaneFirmy(odbiorcy.RowCount, KontrahentDane[1], KontrahentDane[2] + " " + KontrahentDane[3], KontrahentDane[6]));

            RowStyle temp = odbiorcy.RowStyles[odbiorcy.RowCount - 1];
            odbiorcy.RowCount++;
            odbiorcy.AutoSize = true;
            odbiorcy.RowStyles.Add(new RowStyle(temp.SizeType, temp.Height));
            odbiorcy.Controls.Add(new Label() { Text = KontrahentDane[1], AutoSize = true }, 0, odbiorcy.RowCount - 1);// Nazwa
            odbiorcy.Controls.Add(new Label() { Text = KontrahentDane[2]+" "+ KontrahentDane[3], AutoSize = true }, 1, odbiorcy.RowCount - 1); //adres - ulica+nrdomu
            odbiorcy.Controls.Add(new Label() { Text = KontrahentDane[4]+" "+ KontrahentDane[5], AutoSize = true }, 2, odbiorcy.RowCount - 1); //miasto - miasto+kod
            odbiorcy.Controls.Add(new Label() { Text = KontrahentDane[6] }, 3, odbiorcy.RowCount - 1); // NIP
            odbiorcy.Controls.Add(new Label() { Text = KontrahentDane[7]}, 4, odbiorcy.RowCount - 1); //telefon
            odbiorcy.Controls.Add(new Label() { Text = KontrahentDane[8], AutoSize = true }, 5, odbiorcy.RowCount - 1); //paliwo
            odbiorcy.Controls.Add(new Label() { Text = KontrahentDane[9] }, 6, odbiorcy.RowCount - 1); //ilosc
            odbiorcy.Controls.Add(new Label() { Text = KontrahentDane[10] }, 7, odbiorcy.RowCount - 1); //cena
            odbiorcy.Controls.Add(new Label() { Text = KontrahentDane[11] }, 8, odbiorcy.RowCount - 1); //forma płatności
            odbiorcy.Controls.Add(new Label() { Text = KontrahentDane[12] }, 9, odbiorcy.RowCount - 1); //termin
            odbiorcy.Controls.Add(new Label() { Text = KontrahentDane[13] }, 10, odbiorcy.RowCount - 1); //Sent info
            odbiorcy.Controls.Add(new Label() { Text = KontrahentDane[14]+" "+KontrahentDane[14], AutoSize = true }, 11, odbiorcy.RowCount - 1);//miejsce dostarczenia - ulica+nr domu

            Button button = new Button { Name = (odbiorcy.RowCount).ToString(), Text = "Usun" }; //button usuń
            button.Click += new EventHandler(usun_Click);
            odbiorcy.Controls.Add(button, 12, odbiorcy.RowCount - 1);


      /*      for (int i = 0; i < KontrahentDane.Length; i++)
            {
                //odbiorcy.Controls.Add(new Label() { Text = KontrahentDane[i], AutoSize = true }, i, odbiorcy.RowCount - 1);

            }
            */

            odbiorcy.Visible = true;
        }//Czyta dane z form2-Kontrahent

        private void test_btn_Click(object sender, EventArgs e)
        {
            //-----testy do DGV zamiast TLP--------------
            //dataGridView2.Rows.Add(new object[] { oFirma.KontrNazwa, oFirma.KontrUlica + " " + oFirma.KontrNrDomu, oFirma.KontrMiasto + " " + oFirma.KontrKod });
            //--------------------------------------------

            // odbiorcy.Controls.Clear();
            //odbiorcy.RowStyles.Clear();
            //  Createtable();
            // odbiorcy.RowCount = 1;

            dataGridView2.Rows.Clear();
            dataGridView2.Refresh();
            int firmCount = FirmLista.Count;
            for (int count = 0; count < firmCount; ++count)
            {
                DaneFirmy oFirma = FirmLista[count];

                dataGridView2.Rows.Add(new object[] { oFirma.KontrNazwa, oFirma.KontrUlica + " " + oFirma.KontrNrDomu, oFirma.KontrMiasto + " " + oFirma.KontrKod,
                oFirma.KontrNIP,oFirma.KontrTel,oFirma.Paliwo,oFirma.Ilosc.ToString(),oFirma.Cena,oFirma.FormPlat,oFirma.Termin,oFirma.Sent,oFirma.DostUlica + " " + oFirma.DostNr,"Usuń" });

/*
                RowStyle temp = odbiorcy.RowStyles[odbiorcy.RowCount - 1];
                odbiorcy.RowCount++;
                odbiorcy.AutoSize = true;
                odbiorcy.RowStyles.Add(new RowStyle(temp.SizeType, temp.Height));
                odbiorcy.Controls.Add(new Label() { Text = oFirma.KontrNazwa, AutoSize = true }, 0, odbiorcy.RowCount - 1);// Nazwa
                odbiorcy.Controls.Add(new Label() { Text = oFirma.KontrUlica + " " + oFirma.KontrNrDomu, AutoSize = true }, 1, odbiorcy.RowCount - 1); //adres - ulica+nrdomu
                odbiorcy.Controls.Add(new Label() { Text = oFirma.KontrMiasto + " " + oFirma.KontrKod, AutoSize = true }, 2, odbiorcy.RowCount - 1); //miasto - miasto+kod
                odbiorcy.Controls.Add(new Label() { Text = oFirma.KontrNIP }, 3, odbiorcy.RowCount - 1); // NIP
                odbiorcy.Controls.Add(new Label() { Text = oFirma.KontrTel }, 4, odbiorcy.RowCount - 1); //telefon
                odbiorcy.Controls.Add(new Label() { Text = oFirma.Paliwo, AutoSize = true }, 5, odbiorcy.RowCount - 1); //paliwo
                odbiorcy.Controls.Add(new Label() { Text = oFirma.Ilosc.ToString() }, 6, odbiorcy.RowCount - 1); //ilosc
                odbiorcy.Controls.Add(new Label() { Text = oFirma.Cena }, 7, odbiorcy.RowCount - 1); //cena
                odbiorcy.Controls.Add(new Label() { Text = oFirma.FormPlat }, 8, odbiorcy.RowCount - 1); //forma płatności
                odbiorcy.Controls.Add(new Label() { Text = oFirma.Termin }, 9, odbiorcy.RowCount - 1); //termin
                odbiorcy.Controls.Add(new Label() { Text = oFirma.Sent }, 10, odbiorcy.RowCount - 1); //Sent info
                odbiorcy.Controls.Add(new Label() { Text = oFirma.DostUlica + " " + oFirma.DostNr, AutoSize = true }, 11, odbiorcy.RowCount - 1);//miejsce dostarczenia - ulica+nr domu
                Button button = new Button { Name = (odbiorcy.RowCount).ToString(), Text = "Usun" }; //button usuń
                button.Click += new EventHandler(usun_Click);
                odbiorcy.Controls.Add(button, 12, odbiorcy.RowCount - 1);
                */
            }
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

