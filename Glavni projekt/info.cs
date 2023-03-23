using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Glavni_projekt
{
    public partial class info : Form
    {
        public MySql.Data.MySqlClient.MySqlConnection connect;
        konekcija konekt = new konekcija();
        string informacije;
        int tramvaj;


        public info(string info)
        {
            InitializeComponent();
            informacije = info;
        }

        private void info_Load(object sender, EventArgs e)
        {
            if(LoginInfo.UserType != 2)
            {
                button1.Visible = false;
                button2.Visible = false;
            }


            konekt.Open();
            connect = konekt.Vrati();
            using (connect)
            {
                try
                {
                    string query = "SELECT * FROM piprojekt.tramvaj WHERE naziv = @ime";

                    using (MySqlCommand cmd = new MySqlCommand(query, connect))
                    {
                        
                        cmd.Parameters.AddWithValue("@ime", informacije);


                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                int id = reader.GetInt32(0);
                                tramvaj = id;
                                string ime = reader.GetString(1);
                                string serijska = reader.GetString(2);
                                string brzina = reader.GetString(3);
                                string putnici = reader.GetString(4);
                                label2.Text = ime;
                                label3.Text = serijska;
                                label4.Text = brzina;
                                label6.Text = putnici;
                            }

                            reader.Close();
                        }




                    }



                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Exception: " + ex.Message);
                }

                
            }

            konekt.Close();

            konekt.Open();
            connect = konekt.Vrati();

            using (connect)
            {
                try
                {
                    string query2 = "SELECT Timetable FROM piprojekt.tramvaj WHERE tramvaj_id = @tramvaj";

                    using (MySqlCommand cmd2 = new MySqlCommand(query2, connect))
                    {
                        cmd2.Parameters.AddWithValue("@tramvaj", tramvaj);


                        using (MySqlDataReader reader2 = cmd2.ExecuteReader())
                        {
                            

                            while (reader2.Read())
                            {
                                BinaryFormatter bf = new BinaryFormatter();
                                if (reader2[0] != DBNull.Value)
                                {
                                    byte[] data = (byte[])reader2[0];



                                    MemoryStream ms = new MemoryStream(data);
                                    string[] ar2 = (string[])bf.Deserialize(ms);
                                    for (int i = 0; i < ar2.Length; i = i + 3)
                                    {

                                        flowLayoutPanel1.Controls.Add(new TimeTableZaInfoTramvaj(ar2[i], ar2[i + 1], ar2[i + 2]));

                                    }
                                }

                            }
                            reader2.Close();
                        }
                    }
                }

                catch (MySqlException ex)
                {
                    MessageBox.Show("Exception: " + ex.Message);
                }

            }



            konekt.Close();
        }
        

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form addtimetable = new AddTimetable(tramvaj);
            addtimetable.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            konekt.Open();
            connect = konekt.Vrati();

            using (connect)
            {
                try
                {
                    string query = "DELETE FROM piprojekt.tramvaj WHERE tramvaj_id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@id", tramvaj);




                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Uspješno obrisan tramvaj!"); 
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
    }
}
