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
    public partial class prijava : Form
    {
        public MySql.Data.MySqlClient.MySqlConnection connect;
        konekcija konekt = new konekcija();

        public prijava()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form pocetna = new pocetna();
            pocetna.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string korisnik= textBox1.Text;
            string lozinka = textBox2.Text;
            string hashirana = Hash(lozinka);
            konekt.Open();
            connect = konekt.Vrati();
            using (connect)
            {
                try
                {
                    string query = "SELECT korisnik_id, korime, lozinkaSHA, vrsta_korisnika_id FROM piprojekt.korisnici WHERE korime = @korisnik";

                    using (MySqlCommand cmd = new MySqlCommand(query, connect))
                    {
                        
                        cmd.Parameters.AddWithValue("@korisnik", korisnik);


                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                int id = reader.GetInt32(0);
                                string username = reader.GetString(1);
                                string password = reader.GetString(2);
                                int type = reader.GetInt32(3);

                                if (password == hashirana)
                                {
                                    LoginInfo.User = username;
                                    LoginInfo.UserID = id;
                                    LoginInfo.UserType = type;
                                    this.Hide();
                                    Form glavna = new Glavna_stranica();
                                    glavna.ShowDialog();
                                    this.Close();
                                    
                                   
                                }
                                else
                                {
                                    MessageBox.Show("Login Error", "Incorrect username or password.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Login Error", "Incorrect username or password.");
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


        public string Hash(string lozinka)
        {

            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new System.Text.StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(lozinka));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            return hash.ToString();

        }

        private void prijava_Load(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = true;
            groupBox1.Anchor = (AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left);
            button1.Anchor = (AnchorStyles.Left);
            button2.Anchor = (AnchorStyles.Right);
            label3.Anchor = (AnchorStyles.Right | AnchorStyles.Left);
            label4.Anchor = (AnchorStyles.Right | AnchorStyles.Left);
            textBox1.Anchor = (AnchorStyles.Right | AnchorStyles.Left);
            textBox2.Anchor = (AnchorStyles.Right | AnchorStyles.Left);
        }

 
    }
}
