using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace Glavni_projekt
{
    public partial class Povijest : Form
    {
        public MySql.Data.MySqlClient.MySqlConnection connect;
        konekcija konekt = new konekcija();
        string min;
        string datum;
        string ID; string ime; string prezime;
        byte[] a;


        public Povijest()
        {
            InitializeComponent();
        }



        private void Povijest_Load(object sender, EventArgs e)
        {
            

            konekt.Open();
            connect = konekt.Vrati();
            int id;
            
            using (connect)
            {
                try
                {
                    string query = "SELECT a.karta_id AS ID, a.vrijeme_datum AS Vrijeme_kupnje, a.Vrijedi_do AS Istek, CONCAT(b.vrijeme, 'min') AS vrijeme_trajanja_karte, a.status FROM piprojekt.karta a JOIN piprojekt.vrsta_karte b ON a.vrsta_karte_id = b.vrsta_karte_id WHERE korisnik_id = @korisnik";
                    using (MySqlCommand cmd = new MySqlCommand(query, connect))
                    {
                        id = LoginInfo.UserID;
                        cmd.Parameters.AddWithValue("@korisnik", id);
                        var dataAdapter = new MySqlDataAdapter(query, connect);
                        dataAdapter.SelectCommand = cmd;
                        var commandBuilder = new MySqlCommandBuilder(dataAdapter);
                        var ds = new DataSet();
                        dataAdapter.Fill(ds);
                        dataGridView1.DataSource = ds.Tables[0];
                        this.dataGridView1.Columns["status"].Visible = false;
                        DateTime now = DateTime.Now;
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            int currStatus = Convert.ToInt32(row.Cells["status"].Value);
                            DateTime karta = Convert.ToDateTime(row.Cells["Istek"].Value);
                            if(currStatus == 1)
                            {
                                row.DefaultCellStyle.BackColor = Color.CadetBlue;
                            }
                            else if (currStatus == 0 && karta > now)
                            {
                                row.DefaultCellStyle.BackColor = Color.Green;
                            }
                            else if(currStatus == 0)
                            {
                                row.DefaultCellStyle.BackColor = Color.Red;
                            }

                        }
                        DataGridViewButtonColumn col = new DataGridViewButtonColumn();
                        
                        col.Text = "ADD";
                        col.Name = "Račun";
                        dataGridView1.Columns.Add(col);

                        




                    }
                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Exception: " + ex.Message);
                }
            }
            konekt.Close();
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();
        }




        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Račun")
            {
                int col = this.dataGridView1.CurrentCell.ColumnIndex;
                int row = this.dataGridView1.CurrentCell.RowIndex;

                ID = dataGridView1.Rows[row].Cells[0].Value.ToString();
                datum = dataGridView1.Rows[row].Cells[1].Value.ToString();
                min = dataGridView1.Rows[row].Cells[3].Value.ToString();

                konekt.Open();
                connect = konekt.Vrati();
                using (connect)
                {
                    try
                    {
                        string query = "SELECT ime, prezime FROM piprojekt.korisnici WHERE korime = @korisnik";

                        using (MySqlCommand cmd = new MySqlCommand(query, connect))
                        {
                            string korisnik = LoginInfo.User;
                            cmd.Parameters.AddWithValue("@korisnik", korisnik);


                            using (MySqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {

                                   ime = reader.GetString(0);
                                   prezime = reader.GetString(1);

                                }
                                reader.Close();
                            }

                        }

                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Exception: " + ex.Message);
                    }



                    try
                    {
                        string query2 = "SELECT QRimage FROM piprojekt.karta WHERE karta_id = @id";

                        using (MySqlCommand cmd2 = new MySqlCommand(query2, connect))
                        {
                            
                            cmd2.Parameters.AddWithValue("@id", ID);


                           a = (byte[])cmd2.ExecuteScalar();

                        }

                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Exception: " + ex.Message);
                    }


                    Form racun = new Racun(min, datum, ID, ime, prezime, a);
                    racun.ShowDialog();
                }
                konekt.Close();
            }



            }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
    }

