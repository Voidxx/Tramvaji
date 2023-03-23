using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Glavni_projekt
{
    public partial class AktivacijaLinija : Form
    {
        public MySql.Data.MySqlClient.MySqlConnection connect;
        konekcija konekt = new konekcija();
        int tram_id;
        int linija_id;
        public AktivacijaLinija()
        {
            InitializeComponent();

        }

        private void AktivacijaLinija_Load(object sender, EventArgs e)
        {
           
                button2.Enabled = false;
                button3.Enabled = false;
            
            List<string> tramvaji = new List<string>();
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
                string query = "SELECT l.linija_id, s1.naziv as polaziste, s2.naziv as odrediste FROM piprojekt.linija l, piprojekt.stanica s1, piprojekt.stanica s2 WHERE l.polaziste = s1.stanica_id AND l.odrediste = s2.stanica_id";
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



            konekt.Open();
            connect = konekt.Vrati();
            try
            {
                int data;
       
                string query = "SELECT linija_id from vozi";
                using (MySqlCommand cmd = new MySqlCommand(query, connect))
                {

                    MySqlDataReader myReader;
                    myReader = cmd.ExecuteReader();
                    try
                    {
                        while (myReader.Read())
                        {

                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                if (row.Displayed == false)
                                {
                                    data = -1;
                                }
                                else
                                {
                                    data = (int)row.Cells[0].Value;
                                }
                                for (int i = 0; i < myReader.FieldCount; i++)
                                    {
                                

                                    if (myReader.GetInt32(i) == data)
                                    {
                                        row.DefaultCellStyle.BackColor = Color.Green;
                                    }
                                    
                                   

                            }
                            }
                        }
                    }
                    finally
                    {
                        myReader.Close();

                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Exception5: " + ex.Message);
            }
            konekt.Close();








            konekt.Open();
            connect = konekt.Vrati();
            try
            {
                string query = "SELECT naziv from tramvaj";
                using (MySqlCommand cmd = new MySqlCommand(query, connect))
                {

                    MySqlDataReader myReader;
                    myReader = cmd.ExecuteReader();
                    try
                    {
                        while (myReader.Read())
                        {
                            for (int i = 0; i < myReader.FieldCount; i++)
                            {
                                tramvaji.Add(myReader.GetString(i));
                            }
                        }
                    }
                    finally
                    {
                        myReader.Close();

                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Exception5: " + ex.Message);
            }
            konekt.Close();

            





            comboBox1.DataSource = tramvaji;
            }

        private void button2_Click(object sender, EventArgs e)
        {
            string tramvaj = comboBox1.SelectedItem.ToString();
            

            konekt.Open();
            connect = konekt.Vrati();
            try
            {
                string query = "SELECT tramvaj_id from tramvaj WHERE naziv = @naziv";
                using (MySqlCommand cmd = new MySqlCommand(query, connect))
                {
                    cmd.Parameters.AddWithValue("@naziv", tramvaj);
                    MySqlDataReader myReader;
                    myReader = cmd.ExecuteReader();
                    try
                    {
                        while (myReader.Read())
                        {
                            tram_id = myReader.GetInt32(0);
                        }
                    }
                    finally
                    {
                        myReader.Close();

                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Exception5: " + ex.Message);
            }
            konekt.Close();


            if (tram_id != 0) {
                konekt.Open();
                connect = konekt.Vrati();

                using (connect)
                {
                    try
                    {
                        string query = "INSERT INTO vozi (tramvaj_id, linija_id) VALUES (@tramvaj, @linija);";

                        using (MySqlCommand cmd = new MySqlCommand(query, connect))
                        {
                            cmd.Parameters.AddWithValue("@tramvaj", tram_id);
                            cmd.Parameters.AddWithValue("@linija", linija_id);


                            cmd.ExecuteNonQuery();

                        }

                        MessageBox.Show("Uspješno aktivirana linija!");
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Exception4: " + ex.Message);
                    }
                }
                konekt.Close();
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentCell != null && dataGridView1.CurrentCell.Value != null)
            {
                if (e.ColumnIndex == 0 && dataGridView1.CurrentRow.DefaultCellStyle.BackColor != Color.Green)
                {
                    linija_id = int.Parse(dataGridView1.CurrentCell.Value.ToString());
                    button2.Enabled = true;
                    


                
                }
                else
                {
                    button2.Enabled = false;
                    
                }
                if (e.ColumnIndex == 0 && dataGridView1.CurrentRow.DefaultCellStyle.BackColor == Color.Green)
                {
                    linija_id = int.Parse(dataGridView1.CurrentCell.Value.ToString());
                    button3.Enabled = true;




                }
                else
                {
                    button3.Enabled = false;

                }

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            konekt.Open();
            connect = konekt.Vrati();

            using (connect)
            {
                try
                {
                    string query = "DELETE FROM vozi WHERE linija_id = @linija";

                    using (MySqlCommand cmd = new MySqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@linija", linija_id);


                        cmd.ExecuteNonQuery();

                    }

                    MessageBox.Show("Uspješno deaktivirana linija!");
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Exception4: " + ex.Message);
                }
            }
            konekt.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();
        }
    }
    }

