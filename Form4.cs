using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace app_hr
{
    public partial class Form4 : Form
    {
        static string conString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Elena Neacsu\Desktop\app_hr\app_hr\HR.mdb";
        OleDbConnection con = new OleDbConnection(conString);
        OleDbCommand cmd;
        OleDbDataReader reader;

        public Form4()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int myTrue = -1;
            string sql = "SELECT COUNT(tip) FROM Persoane WHERE tip="+myTrue+" ";
            cmd = new OleDbCommand(sql, con);
            try
            {
                con.Open();
                int ft = Convert.ToInt32(cmd.ExecuteScalar());
                chart1.Series["Full Time"].Points.AddXY("", ft);

                int myFalse = 0;
                string sql1 = "SELECT COUNT(tip) FROM Persoane WHERE tip="+myFalse+"";
                cmd = new OleDbCommand(sql1, con);
                int pt = Convert.ToInt32(cmd.ExecuteScalar());
                chart1.Series["Part Time"].Points.AddXY("", pt);
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
           

           

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string sql = "SELECT * FROM Persoane ";
            cmd = new OleDbCommand(sql, con);
            try
            {
                con.Open();
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    chart2.Series["Angajati"].Points.AddXY(reader["nume"].ToString(), reader["salariu"].ToString());
                }
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
            }
        }
    }
}
