using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    class connect
    {
        public string[] mess ;
        public void sqlexecute(string connetionString, string sql, int numfield)
        {
            SqlConnection cnn;
            SqlDataReader dataReader;
            SqlCommand command;
            cnn = new SqlConnection(connetionString);   
            try
            {
                cnn.Open();
                //-------------------------------------------
                command = new SqlCommand(sql, cnn);
                dataReader = command.ExecuteReader();
                mess = new string[numfield];
                while (dataReader.Read())
                {
                    // MessageBox.Show(dataReader.GetValue(0) + " - " + dataReader.GetValue(1) + " - " + dataReader.GetValue(3));
                    //MessageBox.Show(dataReader.GetValue(3) + "hhh");
                    for (int num = 0; num < numfield; num++)
                    {
                        mess[num]= (string)dataReader.GetValue(num);
                    }
                }
                dataReader.Close();
                command.Dispose();
                //----------------------------------------
                cnn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can not open connection ! " + ex);
            }
        }
    }

}
