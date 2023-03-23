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
    public partial class InfoStanica : Form
    {

        public MySql.Data.MySqlClient.MySqlConnection connect;
        konekcija konekt = new konekcija();
        int identifikator;

        string informacije;
        public InfoStanica(string info)
        {
            InitializeComponent();
            informacije = info;
        }

        private void InfoStanica_Load(object sender, EventArgs e)
        {
            label2.Text = informacije;

            if(LoginInfo.UserType != 2)
            {
                button1.Visible = false;
                button2.Visible = false;
                button3.Visible = false;
            }

            konekt.Open();
            connect = konekt.Vrati();
            using (connect)
            {
                try
                {
                    string query = "SELECT * FROM piprojekt.stanica WHERE naziv = @ime";

                    using (MySqlCommand cmd = new MySqlCommand(query, connect))
                    {

                        cmd.Parameters.AddWithValue("@ime", informacije);


                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                int id = reader.GetInt32(0);
                                identifikator = id;

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
                    string query2 = "SELECT dolazak FROM piprojekt.stanica WHERE stanica_id = @id";

                    using (MySqlCommand cmd2 = new MySqlCommand(query2, connect))
                    {
                        cmd2.Parameters.AddWithValue("@id", identifikator);


                        using (MySqlDataReader reader2 = cmd2.ExecuteReader())
                        {


                            while (reader2.Read())
                            {
                                if ((reader2[0] != DBNull.Value)) { 
                                BinaryFormatter bf = new BinaryFormatter();
                                byte[] data = (byte[])reader2[0];


                                MemoryStream ms = new MemoryStream(data);
                                string[] ar2 = (string[])bf.Deserialize(ms);
                                for (int i = 0; i < ar2.Length; i = i + 2)
                                {

                                   tabControl1.TabPages["tabPage1"].Controls.Add(new TimeTableZaInfoStanica(ar2[i], ar2[i + 1]));
                                       
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






            konekt.Open();
            connect = konekt.Vrati();

            using (connect)
            {
                try
                {
                    string query3 = "SELECT polazak FROM piprojekt.stanica WHERE stanica_id = @id";

                    using (MySqlCommand cmd3 = new MySqlCommand(query3, connect))
                    {
                        cmd3.Parameters.AddWithValue("@id", identifikator);


                        using (MySqlDataReader reader3 = cmd3.ExecuteReader())
                        {


                            while (reader3.Read() && (reader3[0] != DBNull.Value))
                            {
                                BinaryFormatter bf2 = new BinaryFormatter();
                                byte[] data2 = (byte[])reader3[0];


                                MemoryStream ms2 = new MemoryStream(data2);
                                string[] ar3 = (string[])bf2.Deserialize(ms2);
                                for (int i = 0; i < ar3.Length; i = i + 2)
                                {

                                    tabPage2.Controls.Add(new TimeTableZaInfoStanica(ar3[i], ar3[i + 1]));

                                }


                            }
                            reader3.Close();
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








    

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form addtimetablePolazak = new AddTimetableStanica(identifikator, "dolazak");
            addtimetablePolazak.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form addtimetablePolazak = new AddTimetableStanica(identifikator, "polazak");
            addtimetablePolazak.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            konekt.Open();
            connect = konekt.Vrati();

            using (connect)
            {
                try
                {
                    string query = "DELETE FROM piprojekt.stanica WHERE stanica_id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@id", identifikator);
         



                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Uspješno obrisana stanica!");
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
