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
    public partial class AddTimetableStanica : Form
    {
        string[] timetable;
        int i = 0;
        public MySql.Data.MySqlClient.MySqlConnection connect;
        konekcija konekt = new konekcija();
        TimeTableRowStanica Rows;
        int stanica;
        int j = 0;
        int z = 0;
        string vr;
        public AddTimetableStanica(int id, string vrsta)
        {
            InitializeComponent();
            stanica = id;
            vr = vrsta;
        }
        List<TimeTableRowStanica> RowList = new List<TimeTableRowStanica>();

        private void AddTimetableStanica_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            i = i + 1;
            Rows = new TimeTableRowStanica(vr);
            Rows.ID = i;
            flowLayoutPanel1.Controls.Add(Rows);
            RowList.Add(Rows);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            konekt.Open();
            connect = konekt.Vrati();
            timetable = new string[i * 2];

            foreach (TimeTableRowStanica Row in RowList)
            {
                z = j + 2;
                while (j < z)
                {
                    timetable[j] = Row.vrijeme;
                    timetable[j + 1] = Row.Stanica;

                    j = j + 2;
                }
            }

            if (vr == "dolazak")
            {
                using (connect)
                {
                    try
                    {
                        string query = "UPDATE piprojekt.stanica SET dolazak = @array WHERE stanica_id = @stanica;";

                        using (MySqlCommand cmd = new MySqlCommand(query, connect))
                        {
                            BinaryFormatter bf = new BinaryFormatter();
                            MemoryStream ms = new MemoryStream();
                            bf.Serialize(ms, timetable);
                            byte[] data = ms.GetBuffer();
                            cmd.Parameters.AddWithValue("@array", data);
                            cmd.Parameters.AddWithValue("@stanica", stanica);

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



            if (vr == "polazak")
            {
                using (connect)
                {
                    try
                    {
                        string query = "UPDATE piprojekt.stanica SET polazak = @array WHERE stanica_id = @stanica;";

                        using (MySqlCommand cmd = new MySqlCommand(query, connect))
                        {
                            BinaryFormatter bf = new BinaryFormatter();
                            MemoryStream ms = new MemoryStream();
                            bf.Serialize(ms, timetable);
                            byte[] data = ms.GetBuffer();
                            cmd.Parameters.AddWithValue("@array", data);
                            cmd.Parameters.AddWithValue("@stanica", stanica);

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
    }
}
