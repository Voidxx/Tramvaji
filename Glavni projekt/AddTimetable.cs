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
    public partial class AddTimetable : Form
    {
        string[] timetable;
        int i = 0;
        public MySql.Data.MySqlClient.MySqlConnection connect;
        konekcija konekt = new konekcija();
        TimeTableRow Rows;
        int tramvaj;
        int j = 0;
        int z = 0;

        public AddTimetable(int id)
        {
            InitializeComponent();
            tramvaj = id;
        }
        List<TimeTableRow> RowList = new List<TimeTableRow>();
        private void AddTimetable_Load(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            i = i + 1;
            Rows = new TimeTableRow();
            Rows.ID = i;
            flowLayoutPanel1.Controls.Add(Rows);
            RowList.Add(Rows);
            
           
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            konekt.Open();
            connect = konekt.Vrati();
            timetable = new string[i * 3];

            foreach (TimeTableRow Row in RowList)
            {
                z = j + 3;
                while (j < z)
                {
                    timetable[j] = Row.Dolazak;
                    timetable[j+1] = Row.Odlazak;
                    timetable[j+2] = Row.Stanica;

                    j = j+3;
                }
            }

            using (connect)
            {
                try
                {
                    string query = "UPDATE piprojekt.tramvaj SET Timetable = @array WHERE tramvaj_id = @tramvaj;";

                    using (MySqlCommand cmd = new MySqlCommand(query, connect))
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        MemoryStream ms = new MemoryStream();
                        bf.Serialize(ms, timetable);
                        byte[] data = ms.GetBuffer();
                        cmd.Parameters.AddWithValue("@array", data);
                        cmd.Parameters.AddWithValue("@tramvaj", tramvaj);

                        cmd.ExecuteNonQuery();

                    }

                }
                catch (MySqlException ex)
                {
                    MessageBox.Show("Exception: " + ex.Message);
                }


            }
            konekt.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Remove(Rows);
            RowList.Remove(Rows);
            i = i - 1;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.Close();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
