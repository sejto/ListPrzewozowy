using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using WindowsFormsApp1;

namespace ListPrzewozowy
{
    public partial class Kontrahent : Form
    {
        string file = "parametry.xml";
        public delegate void methodHandler();
        public methodHandler OnRunMethod;
        string nrWZ = Form1.nrwz.ToString();
        public string dataDok = Form1.data;
        public string KontrNazwa = "";
        public string KontrUlica = "";
        public string KontrNrDomu = "";
        public string KontrMiasto = "";
        public string KontrKod = "";
        public string KontrNip = "";
        public string KontrTelefon = "";

        public Kontrahent()
        {
            InitializeComponent();
            KontrIDlabel.Text = Form1.KontrID;
            //Odpytac w bazie sql OTD o pozostałe dane Kontrahenta na podstawie kontrID i wyjebac wszystkie zbędne zmienne----------------------
            //---------------------------
            string sql = "select Nazwa, Ulica, Nrdomu, Miasto, Kod, NIP, Telefon from OTD.dbo.kontrahent where kontrid="+Form1.KontrID;
            string keyname = "HKEY_CURRENT_USER\\MARKET\\serwerLokal";
            rejestrIO rejestr = new rejestrIO();
            string klucz = rejestr.czytajklucz(keyname, "SQLconnect", true); //parametry połączenia do bazy SQL zapisane w rejestrze
            SqlConnection connection = new SqlConnection(klucz); //skopiować gałąź na pozostałe kompy lub użyć KonfiguratorSQL
            SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
            DataSet ds = new DataSet();
            connection.Open();

            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                   // MessageBox.Show(reader.GetString(0)+", "+reader.GetString(1)+", "+reader.GetString(2));
                    KontrNazwa = reader.GetString(0);
                    KontrUlica = reader.GetString(1);
                    if (!reader.IsDBNull(2))
                    { KontrNrDomu = reader.GetString(2); }else
                    { KontrNrDomu = ""; }
                    KontrMiasto = reader.GetString(3);
                    KontrKod = reader.GetString(4);
                    KontrNip = reader.GetString(5);
                    KontrTelefon = reader.GetString(6);
                }
            }
            connection.Close();

            //---------------------------
            NazwaLabel.Text = KontrNazwa;
            MiastoLabel.Text = KontrKod+" "+KontrMiasto;
            UlicaLabel.Text = KontrUlica+" "+KontrNrDomu;
            NipLabel.Text = KontrNip;
                   
        }


        private void Kontrahent_Load(object sender, EventArgs e)
        {
            string keyname = "HKEY_CURRENT_USER\\MARKET\\ListPrzewozowy";
            rejestrIO rejestr = new rejestrIO();
            string klucz = rejestr.czytajklucz(keyname, "SQLconnect", true); //parametry połączenia do bazy SQL zapisane w rejestrze
            var conn = new SqlConnection(klucz);
            conn.Open();
            using (SqlCommand command = new SqlCommand("select nazwa from paliwo", conn))
            {
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    TowarBox.Items.Add(new Towar {Name = reader.GetValue(0).ToString()});
                }
            }
            conn.Close();
            TowarBox.SelectedIndex = 0;

            FormaPlatBox.Items.Add(new Platnosc { Name = "Gotówka", Value = "G" }); //Combobox z formami płatności
            FormaPlatBox.Items.Add(new Platnosc { Name = "Przelew", Value = "P" });
            FormaPlatBox.SelectedIndex = 0;

            SentBox.Items.Add(new Sent { Name = "", Value = "0" }); //Combobox z działaniami ws SENT, value praktycznie niepotrzebne - zwraca null
            SentBox.Items.Add(new Sent { Name = "My zamykamy SENT", Value = "1" });
            SentBox.Items.Add(new Sent { Name = "Oni zamykają SENT", Value = "1" });
            SentBox.SelectedIndex = 0;

        }

        private void DodajKthBtn_Click(object sender, EventArgs e)
        {
            string DataPlanRozp = dateTimePicker1.Value.Date.ToString("dd.MM.yyyy");
            string DataRozp= dateTimePicker2.Value.Date.ToString("dd.MM.yyyy");
            string DataPlanZak= dateTimePicker3.Value.Date.ToString("dd.MM.yyyy");
            int ilosc=0;
            if (String.IsNullOrEmpty(IloscBox.Text))
            { ilosc = 0; MessageBox.Show("Nie podano litrów"); return; }
            else
            { if (!Int32.TryParse(IloscBox.Text, out ilosc)) 
                { MessageBox.Show("Błędna ilość"); return; }
            else
           { ilosc = Convert.ToInt32(IloscBox.Text); }
            }
            Form1.FirmLista.Add(new DaneFirmy
                (Form1.data, Convert.ToInt32(Form1.KontrID),KontrNazwa,KontrUlica, KontrNrDomu, KontrKod, KontrMiasto,KontrTelefon,KontrNip, TowarBox.SelectedIndex+1, ilosc,
                CenaBox.Text,FormaPlatBox.Text,TerminBox.Text,SentBox.Text,UlicaBox.Text,NrDomuBox.Text,MiejscowoscBox.Text,KodBox.Text,MiejscowoscBox.Text,
                "PL",DataPlanRozp,DataRozp,DataPlanZak,UwagiBox.Text, nrWZ));
            //MessageBox.Show(TowarBox.SelectedIndex.ToString());
            Close();
           
            OnRunMethod();//wywołuje funkcję WczytajDane z form1 za pomocą delegata
        }  //przycisk Zapisz

        private void SentBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SentBox.Text.Length < 1)
            {
                foreach (Control ctrl in MiejsceGroup.Controls)
                { ctrl.Enabled = false; }
                {
                    foreach (Control ctrl in DataBox.Controls)
                    { ctrl.Enabled = false; }
                }
            }
            else
            {
                foreach (Control ctrl in MiejsceGroup.Controls)
                { ctrl.Enabled = true; }
                {
                    foreach (Control ctrl in DataBox.Controls)
                    { ctrl.Enabled = true; }
                }
                KrajBox.Text = "PL";
            }
        }
    }
    public class Towar
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public override string ToString() { return Name; }
    }
    public class Sent
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public override string ToString() { return Name; }
    }
    public class Platnosc
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public override string ToString() { return Name; }
    }
}
