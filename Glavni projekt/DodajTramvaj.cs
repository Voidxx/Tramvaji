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
    public partial class DodajTramvaj : Form
    {
        public MySql.Data.MySqlClient.MySqlConnection connect;
        konekcija konekt = new konekcija();

        public DodajTramvaj()
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
            string serijska = textBox2.Text;
            string max_brzina = textBox3.Text;
            string max_putnici = textBox4.Text;
            konekt.Open();
            connect = konekt.Vrati();

            using (connect)
            {
                try
                {
                    string query = "INSERT INTO tramvaj (naziv, serijska_oznaka, max_brzina, max_broj_putnika) VALUES (@naziv, @serijska, @brzina, @putnici);";

                    using (MySqlCommand cmd = new MySqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@naziv", naziv);
                        cmd.Parameters.AddWithValue("@serijska", serijska);
                        cmd.Parameters.AddWithValue("@brzina", max_brzina);
                        cmd.Parameters.AddWithValue("@putnici", max_putnici);

                        cmd.ExecuteNonQuery();

                    }

                    MessageBox.Show("Uspješno postavljen tramvaj!");
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
