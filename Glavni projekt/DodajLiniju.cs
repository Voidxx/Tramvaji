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
    public partial class DodajLiniju : Form
    {

        public MySql.Data.MySqlClient.MySqlConnection connect;
        konekcija konekt = new konekcija();
        public DodajLiniju()
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
            int polaziste = int.Parse(comboBox1.SelectedItem.ToString());
            int odrediste = int.Parse(comboBox2.SelectedItem.ToString());

            if (polaziste != odrediste)
            {
                konekt.Open();
                connect = konekt.Vrati();

                using (connect)
                {
                    try
                    {
                        string query = "INSERT INTO linija (polaziste, odrediste) VALUES (@polaziste, @odrediste);";

                        using (MySqlCommand cmd = new MySqlCommand(query, connect))
                        {
                            cmd.Parameters.AddWithValue("@polaziste", polaziste);
                            cmd.Parameters.AddWithValue("@odrediste", odrediste);



                            cmd.ExecuteNonQuery();
                        }

                        MessageBox.Show("Uspješno postavljena linija!");
                        this.Hide();
                        this.Close();
                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Exception1: " + ex.Message);
                    }
                }
                konekt.Close();
            }
            else
            {
                MessageBox.Show("Polaziste nemoze biti isto kao i odrediste!");
            }

        }

        private void DodajLiniju_Load(object sender, EventArgs e)
        {
          
            List<int> staniceValue = new List<int>();
            List<string> staniceDisplay = new List<string>();


            konekt.Open();
            connect = konekt.Vrati();
            try
            {
                string query = "SELECT stanica_id, naziv from stanica";
                using (MySqlCommand cmd = new MySqlCommand(query, connect))
                {
                    var dataAdapter = new MySqlDataAdapter(query, connect);
                    MySqlDataReader myReader;
                    myReader = cmd.ExecuteReader();
                    try
                    {
                        while (myReader.Read())
                        {
                            for (int i = 0; i < myReader.FieldCount; i=i+2)
                            {
                                staniceValue.Add(myReader.GetInt32(i));
                                
                            }
                            for (int i = 1; i < myReader.FieldCount; i = i + 2)
                            {
                                staniceDisplay.Add(myReader.GetString(i));

                            }
                        }
                    }
                    finally
                    {
                        myReader.Close();
                        konekt.Close();
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Exception6: " + ex.Message);
            }




            comboBox1.BindingContext = new BindingContext();
            comboBox1.DataSource = staniceValue;
            
            comboBox2.BindingContext = new BindingContext();
            comboBox2.DataSource = staniceValue;
            

        }
    }
}
