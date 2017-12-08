using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1;

namespace Konfigurator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Text = "Parametry połączenia do bazy SQL";
            wczytaj();
        }
        const string keyname = "HKEY_CURRENT_USER\\MARKET\\serwerLokal";
        const string keynameremote = "HKEY_CURRENT_USER\\MARKET\\serwerRemote";
        const string keyList = "HKEY_CURRENT_USER\\MARKET\\ListPrzewozowy";

        public void wczytaj()
        {
            rejestrIO rejestr = new rejestrIO();

            userBox.Text = rejestr.czytajklucz(keynameremote, "user", false);
            passBox.Text = rejestr.czytajklucz(keynameremote, "pass", true);
            connBox.Text = rejestr.czytajklucz(keyname, "SQLconnect", true);
            connBoxremote.Text = rejestr.czytajklucz(keynameremote, "SQLconnect", true);
            connBoxList.Text = rejestr.czytajklucz(keyList, "SQLconnect", true);
        }



        private void Zapisz_Click(object sender, EventArgs e)
        {
            string userremote = userBox.Text;
            string passremote = passBox.Text;
            string conn = connBox.Text;
            string connremote = connBoxremote.Text;
            string connList = connBoxList.Text;

            rejestrIO rejestr = new rejestrIO();
            //if (string.IsNullOrWhitespace(userremote))
            if (!string.IsNullOrEmpty(userremote))
            {
                rejestr.zapiszklucz(keynameremote, "user", userremote, false);
            }
            if (!string.IsNullOrEmpty(passremote))
            {
                rejestr.zapiszklucz(keynameremote, "pass", passremote, true);
            }
            if (!string.IsNullOrEmpty(connremote))
            {
                rejestr.zapiszklucz(keynameremote, "SQLconnect", connremote, true);
            }
            if (!string.IsNullOrEmpty(conn))
            {
                rejestr.zapiszklucz(keyname, "SQLconnect", conn, true);
            }
            if (!string.IsNullOrEmpty(connList))
            {
                rejestr.zapiszklucz(keyList, "SQLconnect", connList, true);
            }
            Close();
        }
    }
}
