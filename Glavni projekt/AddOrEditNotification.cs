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
 
    public partial class AddOrEditNotification : Form
    {
        public MySql.Data.MySqlClient.MySqlConnection connect;
        konekcija konekt = new konekcija();
        public AddOrEditNotification()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            konekt.Open();
            connect = konekt.Vrati();


            using (connect)
            {
                try
                {
                    string query = "INSERT INTO piprojekt.obavijesti (naslov, tekst, datum_objave) VALUES (@naslov, @tekst, CURRENT_TIMESTAMP())";

                    using (MySqlCommand cmd = new MySqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@naslov", textBox1.Text);
                        cmd.Parameters.AddWithValue("@tekst", richTextBox1.Text);
                        cmd.ExecuteNonQuery();

                    }
                    this.Hide();
                    this.Close();
                }
                catch (MySqlException)
                {

                }
            }
            konekt.Close();
        }
    }
}
