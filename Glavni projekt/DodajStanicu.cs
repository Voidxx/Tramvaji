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
    public partial class DodajStanicu : Form
    {
        public MySql.Data.MySqlClient.MySqlConnection connect;
        konekcija konekt = new konekcija();
        public DodajStanicu()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string naziv = textBox1.Text;

            konekt.Open();
            connect = konekt.Vrati();

            using (connect)
            {
                try
                {
                    string query = "INSERT INTO stanica (naziv) VALUES (@naziv);";

                    using (MySqlCommand cmd = new MySqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@naziv", naziv);


                        cmd.ExecuteNonQuery();

                    }

                    MessageBox.Show("Uspješno postavljena stanica!");
                }
                catch (MySqlException)
                {
                    MessageBox.Show("Krivi podatci");
                }
            }
            konekt.Close();
        }
    }
}
