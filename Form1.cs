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

namespace app_hr
{
    public partial class Form1 : Form
    {
        static string conString = @"Provider = Microsoft.Jet.OLEDB.4.0; Data Source = C:\Users\Elena Neacsu\Desktop\app_hr\app_hr\HR.mdb";
        OleDbConnection con = new OleDbConnection(conString);
        OleDbCommand cmd;
        OleDbDataAdapter adapter;
        DataTable dt = new DataTable();

        public Form1()
        {
            InitializeComponent();
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
        }

        private void adaugareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }

        private void adaugareToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3();
            form3.Show();
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = listView1.SelectedItems[0].SubItems[0].Text;
            textBox2.Text = listView1.SelectedItems[0].SubItems[1].Text;
        }

        private void fill(string idDep, string denumire)
        {
            string[] row = { idDep, denumire };
            ListViewItem item = new ListViewItem(row);
            listView1.Items.Add(item);
        }

        //insert into db
        private void addDb(int idDep, string denumire)
        {
            string sql = "INSERT INTO Departamente VALUES(?,?)";
            cmd = new OleDbCommand(sql,con);
            cmd.Parameters.Add("idDep", OleDbType.Integer).Value = idDep;
            cmd.Parameters.Add("denumire", OleDbType.Char, 30).Value = denumire;

            try
            {
                con.Open();
                if (cmd.ExecuteNonQuery() > 0)
                {
                    MessageBox.Show("Inserare efectuata");
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

        private void button1_Click(object sender, EventArgs e)
        {
            int idDep;
            try
            {
                idDep = Convert.ToInt32(textBox1.Text);
                string denumire = textBox2.Text;
                addDb(idDep, denumire);
            } catch(Exception ex)
            {
                MessageBox.Show("Id-ul nu poate contine litere!");
                textBox1.Focus();
            }

            clearTb();
        }

        //vizualizare
        private void retrieve()
        {
            listView1.Items.Clear();

            string sql = "SELECT * FROM Departamente ";
            cmd = new OleDbCommand(sql, con);

            try
            {
                con.Open();
                adapter = new OleDbDataAdapter(cmd);
                adapter.Fill(dt);

                foreach (DataRow row in dt.Rows)
                {
                    fill(row[0].ToString(), row[1].ToString());
                }
                dt.Rows.Clear();
            }
            catch (Exception ex)
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

        //update
        private void update(int idDep, string denumire)
        {
            string sql = "UPDATE Departamente SET denumire='" + denumire + "' WHERE idDep=" + idDep + "";
            cmd = new OleDbCommand(sql, con);

            try
            {
                con.Open();
                adapter = new OleDbDataAdapter(cmd);

                adapter.UpdateCommand = con.CreateCommand();
                adapter.UpdateCommand.CommandText = sql;

                if (adapter.UpdateCommand.ExecuteNonQuery() > 0)
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
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string selected = listView1.SelectedItems[0].SubItems[0].Text;
            int id = Convert.ToInt32(selected);
            update(id, textBox2.Text);
        }

        //delete
        private void delete(int idDep)
        {
            string sql = "DELETE FROM Departamente WHERE idDep=" + idDep+"";
            cmd = new OleDbCommand(sql, con);

            try
            {
                con.Open();
                adapter = new OleDbDataAdapter(cmd);

                adapter.DeleteCommand = con.CreateCommand();
                adapter.DeleteCommand.CommandText = sql;

                if(MessageBox.Show("Sunteti sigur ca doriti stergerea?", "STERGERE", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if(cmd.ExecuteNonQuery() > 0)
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


        private void clearTb()
        {
            textBox1.Text = "";
            textBox2.Text = "";
        }

        
    }
}
