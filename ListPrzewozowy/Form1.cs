﻿using System;
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
        private Column column;
        public Form1()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            Createtable();
            odbiorcy.Visible = false;
            string[] kontr;
            kontr = new string[5];

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
            DataGridViewButtonColumn col = new DataGridViewButtonColumn();
            col.UseColumnTextForButtonValue = true;
            col.Text = "Wybierz";
            col.Name = "Wybor";
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
                //  MessageBox.Show("Button on row clicked" + e.RowIndex);

                //---------------------------------Zapamiętać---------------------------------------
                //string someString = dataGridView1[columnIndex,rowIndex].Value.ToString();
                //----------------------------------------------------------------------------------
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
        }

        public void Createtable()
        {
            //   TableLayoutPanel odbiorcy = new TableLayoutPanel();
            odbiorcy.Controls.Add(new Label() { Text = "Nazwa" }, 0, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Ulica" }, 1, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Nrdomu" }, 2, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Kod" }, 3, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Miasto" }, 4, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Nip" }, 5, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Telefon" }, 6, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Paliwo" }, 7, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Ilosc (litry)" }, 8, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Adres rozładunku/Uwagi", AutoSize = true }, 9, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Nasze uwagi" }, 10, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Usun" }, 11, 0);
        }
        private void DodajKontrahenta(string nazwa, string ulica, string nrdomu, string kod, string miasto, string nip, string telefon, string litry, string uwagi,Boolean sent)
        {
            RowStyle temp = odbiorcy.RowStyles[odbiorcy.RowCount - 1];
            odbiorcy.RowCount++;
            odbiorcy.AutoSize = true;
            odbiorcy.RowStyles.Add(new RowStyle(temp.SizeType, temp.Height));
            odbiorcy.Controls.Add(new Label() { Text = nazwa, AutoSize = true }, 0, odbiorcy.RowCount - 1);
            odbiorcy.Controls.Add(new Label() { Text = ulica, AutoSize = true }, 1, odbiorcy.RowCount - 1);
            odbiorcy.Controls.Add(new Label() { Text = nrdomu, AutoSize = true }, 2, odbiorcy.RowCount - 1);
            odbiorcy.Controls.Add(new Label() { Text = kod, AutoSize = true }, 3, odbiorcy.RowCount - 1);
            odbiorcy.Controls.Add(new Label() { Text = miasto, AutoSize = true }, 4, odbiorcy.RowCount - 1);
            odbiorcy.Controls.Add(new Label() { Text = nip, AutoSize = true }, 5, odbiorcy.RowCount - 1);
            odbiorcy.Controls.Add(new Label() { Text = telefon, AutoSize = true }, 6, odbiorcy.RowCount - 1);
            //odbiorcy.Controls.Add(new ComboBox() {Text="aa",  AutoSize = true }, 7, odbiorcy.RowCount - 1);//rodzaj paliwa

            AddNewComboBox();
            odbiorcy.Controls.Add(new TextBox() { Dock = DockStyle.Fill }, 8, odbiorcy.RowCount - 1);//litry
            odbiorcy.Controls.Add(new TextBox() { Dock = DockStyle.Fill, AutoSize = true }, 9, odbiorcy.RowCount - 1);//UWAGI
            if (sent==true) odbiorcy.Controls.Add(new TextBox() { Text = "SENT my zamykamy, ", Dock = DockStyle.Fill, AutoSize = true }, 10, odbiorcy.RowCount - 1);//Uwagi;
            else
            odbiorcy.Controls.Add(new TextBox() { Dock = DockStyle.Fill, AutoSize = true }, 10, odbiorcy.RowCount - 1);//Uwagi

            Button button = new Button();
            button.Name = (odbiorcy.RowCount).ToString();
            button.Text = "Usun";
            button.Click += new EventHandler(usun_Click);
            odbiorcy.Controls.Add(button, 11, odbiorcy.RowCount - 1);
        }

        void item_SelectedIndexChanged(object sender, EventArgs e)
        {
          //  MessageBox.Show(((ComboBox)sender).Text);
        }

        private void AddNewComboBox()
        {
            /*  for (int comboIndex = 1; comboIndex < 2; comboIndex++)
              {
                  ComboBox combo = new ComboBox();
                  combo.Items.Add("Olej napędowy");
                  combo.Items.Add("Olej opałowy");
                  combo.SelectedIndexChanged += new EventHandler(item_SelectedIndexChanged);
                  odbiorcy.Controls.Add(combo,7,odbiorcy.RowCount-1);
                  */
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("parametry.xml");
            XmlNodeList nodeList = xmlDoc.SelectNodes("/Parametry/Paliwo/Wartosc");
            for (int comboIndex = 1; comboIndex < 2; comboIndex++)
            {
                ComboBox combo = new ComboBox();
                foreach (XmlNode _node in nodeList)
                {
                    String nodeVal = _node.InnerText.ToString();
                    combo.Items.Add(nodeVal.ToString());
                }
                combo.SelectedIndexChanged += new EventHandler(item_SelectedIndexChanged);
                odbiorcy.Controls.Add(combo, 7, odbiorcy.RowCount - 1);
            }
        } //pobiera towary z xml-a

        private void button1_Click(object sender, EventArgs e)// Dodaje kontrahenta z ręki
        {
            DodajKontrahenta("Firma pierwsza sp. z o.o.", "Ciekawa", "14b", "00-456", "Ciechanów", "8440001235", "+48695213456", "2500", "brak uwag",true);
        }
        private void Button2_Click(object sender, EventArgs e) //button "print generuje PDF z tablelayout"
        {
            printCustomer();
         //   printSender();
        }
        private void button3_Click(object sender, EventArgs e) //button "test"
        {
            printWZ();
        }
        private void button4_Click(object sender, EventArgs e) //button "remove" na podstawie IDrow textboxa 
        {
            // MessageBox.Show(odbiorcy.RowCount.ToString());
            //odbiorcy.RowStyles.RemoveAt(1);
            int rem = Convert.ToUInt16(textBox1.Text);
            RemoveRow(odbiorcy, rem);
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

           // MessageBox.Show("RowCount=" +panel.RowCount+" rowIndex= "+rowIndex);
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
            /*
            var button = sender as Button;
            int a = Convert.ToUInt16(button.Name)-1;
             MessageBox.Show("Numer do usuniecia "+a);
            RemoveRow(odbiorcy, a);
            */
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
                for (int c = 0; c < 11; c++)
                { cust[c] = odbiorcy.GetControlFromPosition(c, i).Text.ToString(); }
                if ((String.IsNullOrEmpty(cust[8])))
                { litry = 0; MessageBox.Show("Nie podano litrów"); return; }
                else
                { litry = litry + Convert.ToUInt16(cust[8]); };
                pdf.DrawCustomer(page, heightRowCust, cust[0], cust[1] + ", " + cust[2] + ", " + cust[3] + ", " + cust[4], cust[5], cust[6], cust[9], cust[8], "");
                heightRowCust = heightRowCust + 70; 
            }
            page = document.Pages[0];
            pdf.DrawBody(page, litry);
            document.Save(filename);
            Process.Start(filename);
        }
        public void printSender()
        {
            string data = dateTimePicker1.Text;
            string filename = "wykaz_wewnetrzny_" + data+".pdf";
            PdfDocument document = new PdfDocument(); 
            PdfPage page = document.AddPage(); 
            int heightRowCust = 85;
            int litry = 0;
            int numpage = 1;
            page = document.Pages[numpage-1];
            string[] cust = new string[10];
            print pdf = new print();
            pdf.DrawHeader(page, data);
            pdf.DrawFooters(page, numpage);
            for (int i = 1; i < odbiorcy.RowCount; i++)
            {
                if (heightRowCust + 55 > page.Height - 30)
                {
                    page = document.AddPage();
                    numpage++;
                    page = document.Pages[numpage-1];
                    heightRowCust = 55;
                    pdf.DrawFooters(page, numpage);
                }
                for (int c = 0; c < 10; c++)
                { cust[c] = odbiorcy.GetControlFromPosition(c, i).Text.ToString(); }
                if ((String.IsNullOrEmpty(cust[7])))
                { litry = 0; MessageBox.Show("Nie podano litrów"); return; }
                else
                { litry = litry + Convert.ToUInt16(cust[7]); };
                pdf.DrawCustomer(page, heightRowCust, cust[0], cust[1] + ", " + cust[2] + ", " + cust[3] + ", " + cust[4], cust[5], cust[6], cust[8], cust[7], cust[9]);
                heightRowCust = heightRowCust + 70;
            }
            page = document.Pages[0];
            pdf.DrawBody(page, litry);
            document.Save(filename);
            Process.Start(filename);
        }
        public void printWZ()
        {
            string filename = "WZ.pdf";
            //MessageBox.Show(odbiorcy.GetControlFromPosition(7, 1).Text.ToString());
            //MessageBox.Show(parametry("/Parametry/Uzytkownik/Wartosc"));
            //select parwartosc from konfig where parnazwa='nazwa1' select parwartosc from konfig where parnazwa='ulica'

            PdfDocument document = new PdfDocument();
            PdfPage page = document.AddPage();
            print pdf = new print();
            pdf.DrawWZName(page, "659/2017");
            pdf.DrawWZBody(page,"asda","adad","adasd");

            document.Save(filename);
            Process.Start(filename);
        }

        public string parametry(string param)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("parametry.xml");
            XmlNode dane = xmlDoc.DocumentElement.SelectSingleNode(param);
            return dane.InnerText;
        }  //odczytuje gałąź konfiguracji z xml
    }

}

