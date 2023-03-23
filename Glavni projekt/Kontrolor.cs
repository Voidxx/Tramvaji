using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;
using System.Drawing.Imaging;
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using MySql.Data.MySqlClient;
using System.Threading;

namespace Glavni_projekt
{
    public partial class Kontrolor : Form
    {
        public MySql.Data.MySqlClient.MySqlConnection connect;
        konekcija konekt = new konekcija();
        public Kontrolor()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form glavna = new Glavna_stranica();
            this.Hide();
            glavna.ShowDialog();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Multiselect = false;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = Image.FromFile(ofd.FileName);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            QRCodeDecoder decoder = new QRCodeDecoder();
            string dekodirano = decoder.Decode(new QRCodeBitmapImage(pictureBox1.Image as Bitmap));
            konekt.Open();
            connect = konekt.Vrati();
            int min;
            using (connect)
            {
                try
                {
                    string query = "SELECT * FROM piprojekt.karta WHERE QRcode = @kod";



                    using (MySqlCommand cmd = new MySqlCommand(query, connect))
                    {

                        cmd.Parameters.AddWithValue("@kod", dekodirano);
                        

                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {


                                string QR = reader.GetString(5);
                                int vrstaKarte = reader.GetInt32(2);
                                if(vrstaKarte == 1)
                                {
                                    min = 15;
                                }
                                else if(vrstaKarte == 2)
                                {
                                    min = 30;
                                }
                                else
                                {
                                    min = 45;
                                }

                                if (QR == dekodirano)
                                {
                                    reader.Close();

                                    DateTime currentTime = DateTime.Now;
                                    DateTime vrijediDo = currentTime.AddMinutes(min);
                                    string formatForMySql = vrijediDo.ToString("yyyy-MM-dd HH:mm:ss");

                                    string query2 = "UPDATE piprojekt.karta SET status = 0, Vrijedi_do = @vrijeme WHERE QRcode = @code";

                                    using (MySqlCommand cmd2 = new MySqlCommand(query2, connect))
                                    {
                                        cmd2.Parameters.AddWithValue("@vrijeme", formatForMySql);
                                        cmd2.Parameters.AddWithValue("@code", dekodirano);
                                        cmd2.ExecuteNonQuery();
                                        MessageBox.Show("Karta pronadena!");
                                        Thread.Sleep(2000);

                                        Form glavna = new Glavna_stranica();
                                        this.Hide();
                                        glavna.ShowDialog();
                                        this.Close();
                                    }



                                }
                                else
                                {
                                    MessageBox.Show("Karta error.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Karta nije pronadena.");
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

        }
    }
}
