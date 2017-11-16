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

namespace ListPrzewozowy
{
    public partial class Form1 : Form
    {
        string file = "parametry.xml";
        int num ;
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
            num = Convert.ToInt32(WZtxt.Text);
        }
        void OnProcessExit(object sender, EventArgs e)
        {
          //  parametryZapisz("/Parametry/NrWZ/Wartosc", num.ToString());
        }

        private void Btn_szukajKTH_Click(object sender, EventArgs e)
        {
            string sql;
            string nazwa = Txt_KTH.Text;
            if (NIPValidate(nazwa) != true)
            {
                sql = "select k.Nazwa, Ulica, Nrdomu, kod, miasto, Nip, Telefon, " +
"(Select top 1  case when tekst like '%pel%' then 'Pełnomocnictwo' else 'Brak' end as Pelnomocnictwo From KontrOpis ko where KO.KontrId = K.KontrId and Znaczenie = 76) as Pelnomocnictwo," +
"(Select top 1  case when tekst like '%osw%' then 'Oswiadczenie' else 'Brak' end as Oswiadczenie From KontrOpis ko where KO.KontrId = K.KontrId and Znaczenie = 76) as Oswiadczenie " +
"from kontrahent k where k.nazwa like '%" + nazwa + "%'";
            }
            else
            {
                sql = "select k.Nazwa, Ulica, Nrdomu, kod, miasto, Nip, Telefon, " +
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
            DataGridViewColumn column = dataGridView1.Columns[0];
            column.Width = 350;
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
                int rownumber = Convert.ToInt16(((DataGridView)sender).SelectedCells[0].RowIndex);
                string nazwa = dataGridView1[0, rownumber].Value.ToString();
                string ulica = dataGridView1[1, rownumber].Value.ToString();
                string nrdomu = dataGridView1[2, rownumber].Value.ToString();
                string kod = dataGridView1[3, rownumber].Value.ToString();
                string miasto = dataGridView1[4, rownumber].Value.ToString();
                string nip = dataGridView1[5, rownumber].Value.ToString();
                string telefon = dataGridView1[6, rownumber].Value.ToString();
                string pelnomocnictwo = dataGridView1[7, rownumber].Value.ToString();
                Boolean sent=false;
                if (pelnomocnictwo == "Pełnomocnictwo") sent = true;
                DodajKontrahenta(nazwa, ulica, nrdomu, kod, miasto, nip, telefon, "", "",sent);
            }
        } //Dodaj kontrahenta do TLP

        public void Createtable()
        {
              // TableLayoutPanel odbiorcy = new TableLayoutPanel();
            odbiorcy.Controls.Add(new Label() { Text = "Nazwa" }, 0, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Ulica" }, 1, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Nrdomu" }, 2, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Kod" }, 3, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Miasto" }, 4, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Nip" }, 5, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Telefon" }, 6, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Paliwo" }, 7, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Ilosc (litry)" }, 8, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Cena" }, 9, 0); 
            odbiorcy.Controls.Add(new Label() { Text = "Forma płat." }, 10, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Adres dostawy", AutoSize = true }, 11, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Termin" }, 12, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Usun" }, 13, 0);
        }
        private void DodajKontrahenta(string nazwa, string ulica, string nrdomu, string kod, string miasto, string nip, string telefon, string litry, string uwagi,Boolean sent)
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
            odbiorcy.Controls.Add(new TextBox() { Dock = DockStyle.Fill }, 9, odbiorcy.RowCount - 1);//cena !!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            AddComboPlatnosc(); //10
            odbiorcy.Controls.Add(new TextBox() { Dock = DockStyle.Fill }, 11, odbiorcy.RowCount - 1);//UWAGI
            odbiorcy.Controls.Add(new TextBox() { Dock = DockStyle.Fill}, 12, odbiorcy.RowCount - 1);//Uwagi

            Button button = new Button{Name = (odbiorcy.RowCount).ToString(),Text = "Usun"};
            button.Click += new EventHandler(usun_Click);
            odbiorcy.Controls.Add(button, 13, odbiorcy.RowCount - 1);
            if (sent == true)
            {
                odbiorcy.Controls.Add(new Label() { Text = "SENT my zamykamy"}, 14, odbiorcy.RowCount - 1);
            }
            else
            {
                odbiorcy.Controls.Add(new Label() { Text = "." }, 14, odbiorcy.RowCount - 1);
            }
            Cursor.Current = Cursors.Default;
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
                odbiorcy.Controls.Add(combo, 10, odbiorcy.RowCount - 1);
            } //Dodaje combo z wyborem płatności

        }
        private void Button2_Click(object sender, EventArgs e) //button "print generuje PDF z tablelayout"
        {
            printCustomer();
         //   printSender();
        }
        private void button3_Click(object sender, EventArgs e) //button "test"
        {
            //parametryZapisz("/Parametry/NrWZ/Wartosc","30");
            printWZ("3000","olej napedowy","4,35","przelew","3 dni","dostawa:w polu","line1","line2","line3","line4","line5");
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
            //MessageBox.Show(odbiorcy.GetControlFromPosition(7, 1).Text.ToString());
            string data = dateTimePicker1.Text;
            string filename = "wykaz_kierowca_" + data+".pdf";
            PdfDocument document = new PdfDocument(); 
            PdfPage page = document.AddPage(); 
            int heightRowCust = 85;
            int litry = 0;
            int numpage = 1;
            page = document.Pages[numpage-1]; //numpage-1, ponieważ w document numeracja pages jest od 0.
            string[] cust = new string[15]; //Dane kontrahenta z tablelayout
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
                for (int c = 0; c < 15; c++)
                    { cust[c] = odbiorcy.GetControlFromPosition(c, i).Text.ToString(); };
                string nazwakontrahenta = cust[0];
                string ilosc = cust[8];
                string paliwo = odbiorcy.GetControlFromPosition(7, i).Text.ToString();
                string termin = cust[12];
                string uwagiN = cust[11];
                string pierwszalinia = "";
                string drugalinia = "";
                string trzecialinia = cust[1] + ", " + cust[2] + ", " + cust[3] + " " + cust[4];
                string NIPlinia = cust[5];
                string tellinia = cust[6];
                string sentlinia = cust[14];
                string formaplat = cust[10];
                string cena = cust[9];


                if (String.IsNullOrEmpty(ilosc))
                { litry = 0; MessageBox.Show("Nie podano litrów"); return; }
                else
                { litry = litry + Convert.ToUInt16(ilosc); };
                pdf.DrawCustomer(page, heightRowCust, nazwakontrahenta, trzecialinia, NIPlinia, tellinia, uwagiN, ilosc, sentlinia+", "+cena+"-"+formaplat+","+termin);
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
                printWZ(ilosc, paliwo,cena, formaplat,  termin, uwagiN , pierwszalinia, drugalinia,trzecialinia,"Nip:"+NIPlinia,"tel:"+tellinia);
                //--------------------------------------------------------------------
                //MessageBox.Show(formaplat + cena);
            }
            page = document.Pages[0];
            pdf.DrawBody(page, litry);
            document.Save(filename);
            Process.Start(filename);
        }
        public void printWZ(string ilosc, string paliwo,string cena,string formaplat, string termin, string uwagiN, string line1, string line2, string line3, string line4, string line5)
        {
            num = Convert.ToInt32(WZtxt.Text);
            string filename = "WZ_"+num+".pdf";
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
            pdf.cenapaliwa = cena;
            pdf.DrawWZName(page, num+"/2017", dataWZ);
            pdf.DrawWZBody(page);
            pdf.DrawWZFooter(page, parametry("/Parametry/Uzytkownik/Wartosc")); //Nazwisko wystawiającego w polu wystawil
            document.Save(filename);
            Process.Start(filename);
            num = ++num ; //następny numer WZ
            WZtxt.Text = num.ToString(); //aktualizacja textboxa
            parametry("/Parametry/NrWZ/Wartosc", num.ToString()); //zapamiętanie numeru ostatniej WZ-tki
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
        }
    }

}

