using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Glavni_projekt
{
    public partial class Linije : Form
    {
        public MySql.Data.MySqlClient.MySqlConnection connect;
        konekcija konekt = new konekcija();
        int linija_id;
        public Linije()
        {
            InitializeComponent();
            if(LoginInfo.UserType != 2)
            {
                button3.Visible = false;
                button4.Visible = false;
                button2.Visible = false;
                button5.Visible = false;
                button7.Visible = false;
                

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != -1 && e.ColumnIndex != 0)
            {
                if (dataGridView1.CurrentCell != null && dataGridView1.CurrentCell.Value != null)
                {
                    if (e.ColumnIndex == 3)
                    {
                        Form informacije = new info(dataGridView1.CurrentCell.Value.ToString());
                        informacije.ShowDialog();
                    }
                    else if (e.ColumnIndex == 2 || e.ColumnIndex == 1)
                    {
                        Form stanica = new InfoStanica(dataGridView1.CurrentCell.Value.ToString());
                        stanica.ShowDialog();
                    }
                }
            }
            if (e.ColumnIndex == 0)
            {
                if (dataGridView1.CurrentCell != null && dataGridView1.CurrentCell.Value != null)
                {
                    button7.Enabled = true;
                    linija_id = int.Parse(dataGridView1.CurrentCell.Value.ToString());
                }
            }
            else
            {
                button7.Enabled = false;
            }


        }

        private void Linije_Load(object sender, EventArgs e)
        {
            button7.Enabled = false;
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);

            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dataGridView1.RowHeadersVisible = false;

            konekt.Open();
            connect = konekt.Vrati();
            try
            {
                string query = "SELECT l.linija_id AS Redni_broj, s1.naziv AS Polazište, s2.naziv AS Odredište, t.naziv AS Tramvaj FROM piprojekt.linija l, piprojekt.tramvaj t, piprojekt.vozi v, piprojekt.stanica s1, piprojekt.stanica s2 WHERE l.linija_id = v.linija_id AND t.tramvaj_id = v.tramvaj_id AND s1.stanica_id = l.polaziste AND s2.stanica_id = l.odrediste";
                using (MySqlCommand cmd = new MySqlCommand(query, connect))
                {
                    var dataAdapter = new MySqlDataAdapter(query, connect);
                    var commandBuilder = new MySqlCommandBuilder(dataAdapter);
                    var ds = new DataSet();
                    dataAdapter.Fill(ds);
                    dataGridView1.DataSource = ds.Tables[0];
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Exception: " + ex.Message);
            }

            konekt.Close();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {



        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Form dodajLiniju = new DodajLiniju();
            dodajLiniju.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form dodajTramvaj = new DodajTramvaj();
            dodajTramvaj.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form dodajStanicu = new DodajStanicu();
            dodajStanicu.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form aktiviraj = new AktivacijaLinija();
            aktiviraj.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            konekt.Open();
            connect = konekt.Vrati();

            using (connect)
            {
                try
                {
                    string query = "DELETE FROM piprojekt.linija WHERE linija_id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@id", linija_id);




                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Uspješno obrisana linija!");
                    this.Hide();
                    this.Close();
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Exception: " + ex.Message);
                }
            }
            konekt.Close();
        }

        private void Linije_KeyDown(object sender, KeyEventArgs e)
        {

        }



    }



}
