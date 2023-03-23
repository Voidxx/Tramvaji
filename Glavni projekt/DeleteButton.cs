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
    public partial class DeleteButton : UserControl
    {
        public MySql.Data.MySqlClient.MySqlConnection connect;
        konekcija konekt = new konekcija();
        readonly int id;
        public DeleteButton()
        {
            InitializeComponent();
            
        }

        public DeleteButton(int broj)
        {
            InitializeComponent();
            id = broj;

        }


        private void button1_Click(object sender, EventArgs e)
        {
            konekt.Open();
            connect = konekt.Vrati();


            using (connect)
            {
                try
                {
                    string query = "DELETE FROM piprojekt.obavijesti WHERE obavijesti_id = @id";

                    using (MySqlCommand cmd = new MySqlCommand(query, connect))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();

                    }
                   
                }
                catch (MySqlException)
                {

                }
            }
            konekt.Close();
        }
    }
}
