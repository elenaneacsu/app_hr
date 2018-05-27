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
    public partial class Form2 : Form
    {
        List<Persoana> listaPers = new List<Persoana>();
        static string conString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=C:\Users\Elena Neacsu\Desktop\app_hr\app_hr\HR.mdb";
        OleDbConnection con = new OleDbConnection(conString);
        OleDbCommand cmd;
        OleDbDataAdapter adapter;
        DataTable dt = new DataTable();

        public Form2()
        {
            InitializeComponent();

            listView1.View = View.Details;
            listView1.FullRowSelect = true;
        }

        //insert into db
        private void addDb(int idP, string nume, double salariu, string departament, int idJ, string denumireJ, bool tip)
        {
            string sql = "INSERT INTO Persoane VALUES(?, ?, ?, ?, ?, ?, ?)";
            cmd = new OleDbCommand(sql, con);
            cmd.Parameters.Add("idP", OleDbType.Integer).Value = idP ;
            cmd.Parameters.Add("nume", OleDbType.Char, 50).Value=nume;
            cmd.Parameters.Add("salariu", OleDbType.Double).Value=salariu;
            cmd.Parameters.Add("departament", OleDbType.Char, 20).Value=departament;
            cmd.Parameters.Add("idJ", OleDbType.Integer).Value=idJ;
            cmd.Parameters.Add("denumireJ", OleDbType.Char, 50).Value=denumireJ;
            cmd.Parameters.Add("tip", OleDbType.Boolean).Value=tip;

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
            }

        }


        //filling listview
        private void fill(string idP, string nume, string salariu, string departament, string idJ, string denumire, string tip) {
            String[] row = { idP, nume, salariu, departament, idJ, denumire, tip };
            ListViewItem item = new ListViewItem(row);
            listView1.Items.Add(item);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int idP, idJ;
            double salariu;
            try
            {
                idP = Convert.ToInt32(textBox1.Text);
                string nume = textBox2.Text;
                salariu = Convert.ToDouble(textBox3.Text);
                string dep = tbDep.Text;
                idJ = Convert.ToInt32(textBox4.Text);
                string denumireJ = textBox5.Text;
                bool tip = false;
                if (radioButton1.Checked)
                {
                    tip = true;
                }
                Persoana pers = new Persoana(idP, nume, salariu, dep, new Job(idJ, denumireJ, tip));
                listaPers.Add(pers);

                addDb(idP, nume, salariu, dep, idJ, denumireJ, tip);

                fill(textBox1.Text, textBox2.Text, textBox3.Text, tbDep.Text, textBox4.Text, textBox5.Text, tip.ToString());
            } catch(Exception ex)
            {
                if (string.IsNullOrEmpty(textBox1.Text) || textBox1.Text.Any(Char.IsLetter))
                {
                    MessageBox.Show("Id-ul nu poate contine litere!");
                    textBox1.Focus();
                    return;
                }
                if(string.IsNullOrEmpty(textBox3.Text) || textBox3.Text.Any(Char.IsLetter))
                {
                    MessageBox.Show("Salariul nu poate contine litere!");
                    textBox3.Focus();
                    return;
                }
                if(string.IsNullOrEmpty(textBox4.Text) || textBox4.Text.Any(Char.IsLetter))
                {
                    MessageBox.Show("Id-ul nu poate contine litere!");
                    textBox4.Focus();
                    return;
                }
                throw;
            }

            clearTB();
        }

        //delete LV
        private void delete()
        {
            if(MessageBox.Show("Sunteti sigur ca doriti stergerea?", "STERGERE", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
            {
                listView1.Items.RemoveAt(listView1.SelectedIndices[0]);
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                tbDep.Text = "";
                radioButton1.Checked = false;
                radioButton2.Checked = false;
            }
        }

        //delete from db
        private void delete(int id)
        {
            string sql = "DELETE FROM Persoane WHERE idP=" + id + "";
            cmd = new OleDbCommand(sql, con);

            try
            {
                con.Open();
                adapter = new OleDbDataAdapter(cmd);

                adapter.DeleteCommand = con.CreateCommand();
                adapter.DeleteCommand.CommandText = sql;

                if(MessageBox.Show("Sunteti sigur ca doriti stergerea?", "STERGERE", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                {
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        clearTB();
                        MessageBox.Show("Stergere efectuata cu succes");
                    }
                }
            } catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally{
                con.Close();
                retrieve();
            }
        }

        //stergere
        private void button3_Click(object sender, EventArgs e)
        {
           // delete();

            string selected = listView1.SelectedItems[0].SubItems[0].Text;
            int id = Convert.ToInt32(selected);

            delete(id);
        }

        //update LV
        private void update()
        {
            listView1.SelectedItems[0].SubItems[0].Text = textBox1.Text;
            listView1.SelectedItems[0].SubItems[1].Text = textBox2.Text;
            listView1.SelectedItems[0].SubItems[2].Text = textBox3.Text;
            listView1.SelectedItems[0].SubItems[3].Text = tbDep.Text;
            listView1.SelectedItems[0].SubItems[4].Text = textBox4.Text;
            listView1.SelectedItems[0].SubItems[5].Text = textBox5.Text;

            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            tbDep.Text = "";
            radioButton1.Checked = false;
            radioButton2.Checked = false;

        }

        //update DB
        private void update(int idP, string nume, double salariu, string departament, int idJ, string denumireJ, bool tip)
        {
            string sql = "UPDATE Persoane SET nume='" + nume + "',salariu=" + salariu + ",departament='" + departament + "',idJ=" + idJ + ",denumireJ='" + denumireJ + "',tip=" + tip + " WHERE idP=" + idP + "";
            cmd = new OleDbCommand(sql, con);

            try
            {
                con.Open();
                adapter = new OleDbDataAdapter(cmd);

                adapter.UpdateCommand = con.CreateCommand();
                adapter.UpdateCommand.CommandText = sql;

                if (adapter.UpdateCommand.ExecuteNonQuery() > 0)
                {
                    clearTB();
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

        //editare
        private void button2_Click(object sender, EventArgs e)
        {
          //  update();

            string selected = listView1.SelectedItems[0].SubItems[0].Text;
            int idPers = Convert.ToInt32(selected);
            string selected2 = listView1.SelectedItems[0].SubItems[6].Text;
            bool tipJ = Convert.ToBoolean(selected2);

            update(idPers, textBox2.Text, Convert.ToDouble(textBox3.Text), tbDep.Text, Convert.ToInt32(textBox4.Text), textBox5.Text, tipJ);
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            textBox1.Text = listView1.SelectedItems[0].SubItems[0].Text;
            textBox2.Text = listView1.SelectedItems[0].SubItems[1].Text;
            textBox3.Text = listView1.SelectedItems[0].SubItems[2].Text;
            tbDep.Text = listView1.SelectedItems[0].SubItems[3].Text;
            textBox4.Text = listView1.SelectedItems[0].SubItems[4].Text;
            textBox5.Text = listView1.SelectedItems[0].SubItems[5].Text;
            if (listView1.SelectedItems[0].SubItems[6].Text == "True")
            {
                radioButton1.Checked = true;
            }
            else
            {
                radioButton2.Checked = true;
            }
        }

        //retrieve from db
        private void retrieve()
        {
            listView1.Items.Clear();

            string sql = "SELECT * FROM Persoane ";
            cmd = new OleDbCommand(sql, con);

            try
            {
                con.Open();
                adapter = new OleDbDataAdapter(cmd);
                adapter.Fill(dt);

                foreach(DataRow row in dt.Rows)
                {
                    fill(row[0].ToString(), row[1].ToString(), row[2].ToString(), row[3].ToString(), row[4].ToString(), row[5].ToString(), row[6].ToString());
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

        //vizualizare
        private void button4_Click(object sender, EventArgs e)
        {
            retrieve();
        }

        private void clearTB()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            tbDep.Text = "";
            radioButton1.Checked = false;
            radioButton2.Checked = false;
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
