using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Glavni_projekt
{
    public partial class registracija : Form
    {
        public MySql.Data.MySqlClient.MySqlConnection connect ;
        konekcija konekt = new konekcija();
        public registracija()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ValidateText())
            {
                string ime = textBox1.Text;
                string prezime = textBox2.Text;
                string email = maskedTextBox1.Text;
                string korime = textBox3.Text;
                string lozinka = textBox5.Text;
                string lozinkaSHA = Hash(lozinka);
                konekt.Open();
                connect = konekt.Vrati();

                using (connect)
                {
                    try
                    {
                        string query = "INSERT INTO korisnici (ime, prezime, email, korime, status, lozinkaSHA, vrsta_korisnika_id) VALUES (@ime, @prezime, @email, @korime, @status, @SHA, @vrsta);";

                        using (MySqlCommand cmd = new MySqlCommand(query, connect))
                        {
                            cmd.Parameters.AddWithValue("@ime", ime);
                            cmd.Parameters.AddWithValue("@prezime", prezime);
                            cmd.Parameters.AddWithValue("@email", email);
                            cmd.Parameters.AddWithValue("@korime", korime);
                            cmd.Parameters.AddWithValue("@status", 0);
                            cmd.Parameters.AddWithValue("@SHA", lozinkaSHA);
                            cmd.Parameters.AddWithValue("@vrsta", 1);
                            cmd.ExecuteNonQuery();

                        }
                        Form prijava = new prijava();
                        this.Hide();
                        prijava.ShowDialog();
                        this.Close();
                    }
                    catch (MySqlException)
                    {

                    }
                }
                konekt.Close();
            }
            
        }
        private void registracija_Load(object sender, EventArgs e)
        {
            button1.Anchor = (AnchorStyles.Right);
            button2.Anchor = (AnchorStyles.Left);
            groupBox1.Anchor = (AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left);
            panel1.Anchor = (AnchorStyles.Right | AnchorStyles.Left);
            label1.Anchor = (AnchorStyles.Right | AnchorStyles.Left);
            label2.Anchor = (AnchorStyles.Right | AnchorStyles.Left);
            label3.Anchor = (AnchorStyles.Right | AnchorStyles.Left);
            label4.Anchor = (AnchorStyles.Right | AnchorStyles.Left);
            label5.Anchor = (AnchorStyles.Right | AnchorStyles.Left);
            label6.Anchor = (AnchorStyles.Right | AnchorStyles.Left);
            textBox1.Anchor = (AnchorStyles.Right | AnchorStyles.Left);
            textBox2.Anchor = (AnchorStyles.Right | AnchorStyles.Left);
            textBox3.Anchor = (AnchorStyles.Right | AnchorStyles.Left);
            maskedTextBox1.Anchor = (AnchorStyles.Right | AnchorStyles.Left);
            textBox5.Anchor = (AnchorStyles.Right | AnchorStyles.Left);
            textBox6.Anchor = (AnchorStyles.Right | AnchorStyles.Left);


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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form pocetna = new pocetna();
            pocetna.ShowDialog();
            this.Close();
        }

        System.Text.RegularExpressions.Regex expr = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z][\w\.-]{2,28}[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");
        private bool ValidateText()
        {

            if (string.IsNullOrEmpty(textBox1.Text))
            {

                textBox1.Focus();
                errorProvider1.SetError(textBox1, "Molim Vas unesite Vaše ime.");
                return false;
            }
            else if (string.IsNullOrEmpty(textBox2.Text))
            {

                textBox2.Focus();
                errorProvider1.SetError(textBox2, "Molim Vas unesite Vaše prezime.");
                return false;
            }
            else if (string.IsNullOrEmpty(maskedTextBox1.Text))
            {
                maskedTextBox1.Focus();
                errorProvider1.SetError(maskedTextBox1, "Molim Vas unesite Vaš email.");
                return false;
                

            }
            else if (!expr.IsMatch(maskedTextBox1.Text))
            {
                maskedTextBox1.Focus();
                errorProvider1.SetError(maskedTextBox1, "Molim Vas unesite pravilan email.");
                return false;
            }
            else if (string.IsNullOrEmpty(textBox3.Text))
            {

                textBox3.Focus();
                errorProvider1.SetError(textBox3, "Molim Vas unesite Vaše korisničko ime.");
                return false;
            }
            else if (string.IsNullOrEmpty(textBox5.Text))
            {

                textBox5.Focus();
                errorProvider1.SetError(textBox5, "Molim Vas unesite Vašu lozinku.");
                return false;
            }
            else if (string.IsNullOrEmpty(textBox6.Text))
            {

                textBox6.Focus();
                errorProvider1.SetError(textBox6, "Molim Vas unesite ponovljenu lozinku.");
                return false;
            }
            else if (textBox5.Text != textBox6.Text)
            {

                textBox6.Focus();
                errorProvider1.SetError(textBox6, "Lozinke nisu iste.");
                return false;
            }

            else return true;

        }

        bool IsValidEmail(string email)
        {
            try
            {
                var mail = new System.Net.Mail.MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
