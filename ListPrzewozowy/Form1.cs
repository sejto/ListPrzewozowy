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

            // "where ko.znaczenie=76 and k.nazwa like '%" + nazwa + "%'";
            string keyname = "HKEY_CURRENT_USER\\MARKET\\serwerLokal";
            rejestrIO rejestr = new rejestrIO();
            string klucz = rejestr.czytajklucz(keyname, "SQLconnect", true);
            SqlConnection connection = new SqlConnection(klucz);
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
                DodajKontrahenta(nazwa, ulica, nrdomu, kod, miasto, nip, telefon, "", "");
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
            odbiorcy.Controls.Add(new Label() { Text = "Ilosc (litry)" }, 7, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Adres rozładunku/Uwagi", AutoSize = true }, 8, 0);
            odbiorcy.Controls.Add(new Label() { Text = "Usun" }, 9, 0);
        }


        private void button1_Click(object sender, EventArgs e)
        {
            DodajKontrahenta("Firma pierwsza sp. z o.o.", "Ciekawa", "14b", "00-456", "Ciechanów", "8440001235", "+48695213456", "", "");
        }
        private void DodajKontrahenta(string nazwa, string ulica, string nrdomu, string kod, string miasto, string nip, string telefon, string litry, string uwagi)
        {
            //get a reference to the previous existent 
            RowStyle temp = odbiorcy.RowStyles[odbiorcy.RowCount - 1];
            //increase panel rows count by one
            odbiorcy.RowCount++;
            //add a new RowStyle as a copy of the previous one
            //odbiorcy.RowStyles.Add(new RowStyle(temp.SizeType, temp.Height));
            odbiorcy.AutoSize = true;
            odbiorcy.RowStyles.Add(new RowStyle(temp.SizeType, temp.Height));
            odbiorcy.Controls.Add(new Label() { Text = nazwa, AutoSize = true }, 0, odbiorcy.RowCount - 1);
            odbiorcy.Controls.Add(new Label() { Text = ulica, AutoSize = true }, 1, odbiorcy.RowCount - 1);
            odbiorcy.Controls.Add(new Label() { Text = nrdomu, AutoSize = true }, 2, odbiorcy.RowCount - 1);
            odbiorcy.Controls.Add(new Label() { Text = kod, AutoSize = true }, 3, odbiorcy.RowCount - 1);
            odbiorcy.Controls.Add(new Label() { Text = miasto, AutoSize = true }, 4, odbiorcy.RowCount - 1);
            odbiorcy.Controls.Add(new Label() { Text = nip, AutoSize = true }, 5, odbiorcy.RowCount - 1);
            odbiorcy.Controls.Add(new Label() { Text = telefon, AutoSize = true }, 6, odbiorcy.RowCount - 1);
            odbiorcy.Controls.Add(new TextBox() { Dock = DockStyle.Fill }, 7, odbiorcy.RowCount - 1);
            odbiorcy.Controls.Add(new TextBox() { Dock = DockStyle.Fill, AutoSize = true }, 8, odbiorcy.RowCount - 1);
            //odbiorcy.Controls.Add(new CheckBox() { Dock = DockStyle.Fill, AutoSize = true }, 9, odbiorcy.RowCount - 1);
              odbiorcy.Controls.Add(new Button() { Text = "Usun", Name = "usun_btn",   Dock = DockStyle.Fill, AutoSize = true}, 9, odbiorcy.RowCount - 1 );
            
           /*  poniższe dziala na przycisk usun, ale sie pieprzy z indeksem kolumn. Z kolei powyższy button tworzy prawidlowo, ale nie reaguje na button...
            Button button = new Button();
            button.Name = (odbiorcy.RowCount).ToString();
            button.Text = "Usun";
            button.Click += new System.EventHandler(this.usun_Click);
            odbiorcy.Controls.Add(button);
            */

            //  odbiorcy.RowCount++;
            //   int i = odbiorcy.RowCount - 1;
            //MessageBox.Show(i.ToString());
            //  string c = odbiorcy.GetControlFromPosition(0, i).Text.ToString();
            //MessageBox.Show(c);
            //odbiorcy.MouseClick += new MouseEventHandler(usun_Click);
        }

        private void button2_Click(object sender, EventArgs e) //button "print"
        {
            // PdfCreator generuj = new PdfCreator();
            // generuj.Create();
            string filename = "print.pdf";
            PdfDocument document = new PdfDocument(); // stworzenie nowego dokumentu
            PdfPage page = document.AddPage(); // stworzenie nowej strony dokumentu
            print pdf = new print();
            string data = dateTimePicker1.Text;
            pdf.DrawHeader(page, data);

            pdf.DrawCustomer(page, 80, "Super Firma sp.z o.o.", "16-542 Cieciówka, Maławieś 14a", "8441234567", "606123789");
            pdf.DrawCustomer(page, 135, "Niezła Firma II sp.z o.o.", "26-442 Miech, Przegonowo 6", "8341234599", "606123780");

            pdf.DrawBody(page, "5900");
            pdf.DrawFooters(page);
            document.Save(filename);
            Process.Start(filename);

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

        public void printPDF()
        {
            Document document = new Document();
            document.Info.Author = "Rolf Baxter";
            document.Info.Keywords = "MigraDoc, Examples, C#";
            // Get the A4 page size
            Unit width, height;
            PageSetup.GetPageSize(PageFormat.A4, out width, out height);
            // Add a section to the document and configure it such that it will be in the centre
            // of the page
            Section section = document.AddSection();
            section.PageSetup.PageHeight = height;
            section.PageSetup.PageWidth = width + 30;
            section.PageSetup.LeftMargin = 20;
            section.PageSetup.RightMargin = 10;
            section.PageSetup.TopMargin = height / 2;
            // Create a table so that we can draw the horizontal lines
            Table table = new Table();
            table.Borders.Width = 1; // Default to show borders 1 pixel wide Column
            column = table.AddColumn(width);
            column.Format.Alignment = ParagraphAlignment.Center;
            double fontHeight = 36;
            MigraDoc.DocumentObjectModel.Font font = new MigraDoc.DocumentObjectModel.Font("Times New Roman", fontHeight);
            // Add a row with a single cell for the first line
            Row row = table.AddRow();
            Cell cell = row.Cells[0];
            cell.Format.Font.Color = Colors.Black;
            cell.Format.Font.ApplyFont(font);
            cell.Borders.Left.Visible = true;
            cell.Borders.Right.Visible = true;
            cell.Borders.Bottom.Visible = true;
            cell.AddParagraph("Hello, World!");
            // Add a row with a single cell for the second line
            row = table.AddRow();
            cell = row.Cells[0];
            cell.Format.Font.Color = Colors.Black;
            cell.Format.Alignment = ParagraphAlignment.Left;
            cell.Format.Font.ApplyFont(font);
            cell.Borders.Left.Visible = false;
            cell.Borders.Right.Visible = false;
            cell.Borders.Top.Visible = false;
            cell.AddParagraph("This is some long text that *will* auto-wrap when the edge of the page is reached");
            document.LastSection.Add(table);

            // Create a renderer
            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();
            // Associate the MigraDoc document with a renderer
            pdfRenderer.Document = document;
            // Layout and render document to PDF
            pdfRenderer.RenderDocument();
            // Save and show the document
            pdfRenderer.PdfDocument.Save
            ("TestDocument.pdf");
            Process.Start("TestDocument.pdf");
        }

        public void printPDF2()
        {
            string filename = "c:\\print.pdf";
            //nowy dokument
            PdfDocument document = new PdfDocument(); // stworzenie nowego dokumentu
            PdfPage page = document.AddPage(); // stworzenie nowej strony dokumentu
            XGraphics graphics = XGraphics.FromPdfPage(page); // stworzenie obiektu odpowiedzialnego za wygląd strony
            // prostokat
            XPen pen = new XPen(XColors.Black, 1); // obramowanie prostokąta
            XBrush brush = XBrushes.Red; // wypełnienie prostokąta
            XRect rect = new XRect(0, 0, 60, 20); // położenie i wymiary prostokąta (x, y, szerokość, wysokość)
            graphics.DrawRectangle(pen, brush, rect); // narysowanie prostokąta
            //dodanie tekstu
            XFont font = new XFont("Arial", 10, XFontStyle.Bold); // krój, rozmiar i styl czcionki
            graphics.DrawString("Poufne", font, XBrushes.Black, rect, XStringFormats.Center); // dodanie czarnego napisu w środku stworzonego wcześniej prostokąta
            //zapis pliku
            document.Save(filename);
            //----------------------

            Process.Start(filename); //podgląd w domyślnym programie
        }

        private void button3_Click(object sender, EventArgs e) //button "test"
        {
            for (int i = 1; i < odbiorcy.RowCount; i++)
            {
                //int i = odbiorcy.RowCount - 1;
                string c = odbiorcy.GetControlFromPosition(0, i).Text.ToString();
                string d = odbiorcy.GetControlFromPosition(1, i).Text.ToString();
                MessageBox.Show(c + "xxx:" + d);
            }
        }

        private void button4_Click(object sender, EventArgs e) //button "remove"
        {
           // MessageBox.Show(odbiorcy.RowCount.ToString());
            //odbiorcy.RowStyles.RemoveAt(1);
            int rem = Convert.ToUInt16(textBox1.Text);
            RemoveRow(odbiorcy, rem);
        }

        public void RemoveRow(TableLayoutPanel panel, int rowIndex)
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
        public void usun_Click(object sender, EventArgs e)
        {
            //          Point p = this.odbiorcy.PointToClient(Control.MousePosition);//get mouse position
            //            string a = p.X.ToString() + ":" + p.Y.ToString();//get mouse position
            //-----------------------------------------------------------------------------
            var button = sender as Button;
            int a = Convert.ToUInt16(button.Name)-1;
             MessageBox.Show("Numer do usuniecia "+a);
            RemoveRow(odbiorcy, a);
        }
    }
  
}

