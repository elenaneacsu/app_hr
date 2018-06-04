using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace app_hr
{
    public partial class Form3 : Form
    {
        static string conString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Elena Neacsu\Desktop\app_hr\app_hr\HR.mdb";
        OleDbConnection con = new OleDbConnection(conString);
        OleDbCommand cmd;
        OleDbDataAdapter adapter;
        DataTable dt = new DataTable();

        public Form3()
        {
            InitializeComponent();
            listView1.FullRowSelect = true;
            listView1.View = View.Details;
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = listView1.SelectedItems[0].SubItems[0].Text;
            textBox2.Text = listView1.SelectedItems[0].SubItems[1].Text;
            if (listView1.SelectedItems[0].SubItems[2].Text == "True")
            {
                radioButton1.Checked=true;
            }
            else
            {
                radioButton2.Checked = true;
            }
        }

        private void write(string idJ, string denumire, string tip)
        {
            string[] array = { idJ, denumire, tip };
            using(StreamWriter writer = new StreamWriter(@"joburi.txt", true))
            {
                for(int i = 0; i < array.Length; i++)
                {
                    writer.Write(array[i] + " ");
                }
                writer.Write(DateTime.Now);
                writer.Write(Environment.NewLine);
            }
        }

        private void fill(string idJ, string denumire, string tip)
        {
            string[] row = { idJ, denumire, tip };
            ListViewItem item = new ListViewItem(row);
            listView1.Items.Add(item);
        }

        //vizualizare
        private void retrieve()
        {
            listView1.Items.Clear();

            string sql = "SELECT * FROM Joburi ";
            cmd = new OleDbCommand(sql, con);

            try
            {
                con.Open();
                adapter = new OleDbDataAdapter(cmd);
                adapter.Fill(dt);

                foreach(DataRow row in dt.Rows)
                {
                    fill(row[0].ToString(), row[1].ToString(), row[2].ToString());
                }
                dt.Rows.Clear();
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
            retrieve();
        }

        //insert
        private void add(int idJ, string denumire, bool tip)
        {
            string sql = "INSERT INTO Joburi VALUES(?, ?, ?)";
            cmd = new OleDbCommand(sql, con);
            cmd.Parameters.Add("idJ", OleDbType.Integer).Value = idJ;
            cmd.Parameters.Add("denumire", OleDbType.Char, 50).Value = denumire;
            cmd.Parameters.Add("tip", OleDbType.Boolean).Value = tip;

            try
            {
                con.Open();
                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Inserare efectuata!");
                } 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
                retrieve();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int idJ;
            try
            {
                idJ = Convert.ToInt32(textBox1.Text);
                string denumire = textBox2.Text;
                bool tip;
                if (radioButton1.Checked == true)
                {
                    tip = true;
                }
                else
                {
                    tip = false;
                }
                add(idJ, denumire, tip);
                write(idJ.ToString(), denumire, tip.ToString());
            } catch(Exception ex)
            {
                MessageBox.Show("Id-ul nu poate contine litere!");
                textBox1.Focus();
            }
           

            clearTb();

        }

        private void clearTb()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            radioButton1.Checked = false;
            radioButton2.Checked = false;
        }

        //update
        private void update(int idJ, string denumire, bool tip)
        {
            string sql = "UPDATE Departamente SET denumire='" + denumire + "', tip=" + tip + " WHERE idJ=" + idJ + "";
            cmd = new OleDbCommand(sql, con);

            try
            {
                con.Open();
                adapter = new OleDbDataAdapter(cmd);
                adapter.UpdateCommand = con.CreateCommand();
                adapter.UpdateCommand.CommandText = sql;

                if(adapter.UpdateCommand.ExecuteNonQuery() > 0)
                {
                    clearTb();
                    MessageBox.Show("Editare efectuata!");
                }
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
                retrieve();
            }
            write(idJ.ToString(), denumire, tip.ToString());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string selected = listView1.SelectedItems[0].SubItems[0].Text;
            int id = Convert.ToInt32(selected);
            string selected2 = listView1.SelectedItems[0].SubItems[2].Text;
            bool tipJ = Convert.ToBoolean(selected2);

            update(id, textBox2.Text, tipJ);
        }

        //delete
        private void delete(int idJ)
        {
            string sql = "DELETE FROM Joburi WHERE idJ=" + idJ + "";
            cmd = new OleDbCommand(sql, con);

            try
            {
                con.Open();
                if(MessageBox.Show("Sunteti sigur ca doriti stergerea?", "STERGERE", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        clearTb();
                        MessageBox.Show("Stergere efectuata cu succes!");
                    }
                }
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close();
                retrieve();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string selected = listView1.SelectedItems[0].SubItems[0].Text;
            int id = Convert.ToInt32(selected);

            delete(id);
        }

        private void radioButton1_Enter(object sender, EventArgs e)
        {
            radioButton1.Checked = true;
        }

        private void radioButton2_Enter(object sender, EventArgs e)
        {
            radioButton2.Checked = true;
        }
    }
}
