using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;


namespace Glavni_projekt
{
    public partial class Glavna_stranica : Form
    {

        public MySql.Data.MySqlClient.MySqlConnection connect;
        konekcija konekt = new konekcija();
        TimeSpan ts;
        int sekunde;
   

        public Glavna_stranica()
        {
            InitializeComponent();
            flowLayoutPanel1.HorizontalScroll.Enabled = false;
            if (LoginInfo.UserType == 1)
            {
                button7.Visible = false;
                button8.Visible = false;
                button9.Visible = false;
            }
            else
            {
                button7.Visible = true;
                button8.Visible = true;
                button9.Visible = true;
            }
            GraphicsPath p = new GraphicsPath();
            p.AddEllipse(1, 1, button1.Width - 4, button1.Height - 4);
            button1.Region = new Region(p);
            button2.Region = new Region(p);
            button3.Region = new Region(p);
            RefreshNotifications();
            panel3.Anchor = (AnchorStyles.Bottom);
            button1.Anchor = (AnchorStyles.Left);
            button3.Anchor = (AnchorStyles.Right);
            label2.Anchor = (AnchorStyles.Bottom);
            flowLayoutPanel1.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top);
            flowLayoutPanel2.Anchor = (AnchorStyles.Bottom | AnchorStyles.Top | AnchorStyles.Left);
            flowLayoutPanel3.Anchor = (AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left);
            panel1.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right | AnchorStyles.Top);
            panel2.Anchor = (AnchorStyles.Top | AnchorStyles.Left);
            button9.Anchor = ( AnchorStyles.Right | AnchorStyles.Top);
            button8.Anchor = (AnchorStyles.Right | AnchorStyles.Top);



        }

        private void RefreshNotifications()
        {
            flowLayoutPanel1.Controls.Clear();
            konekt.Open();
            connect = konekt.Vrati();
            using (connect)
            {
                try
                {
                    string query2 = "SELECT * FROM piprojekt.obavijesti";

                    using (MySqlCommand cmd2 = new MySqlCommand(query2, connect))
                    {



                        using (MySqlDataReader reader2 = cmd2.ExecuteReader())
                        {


                            while (reader2.Read())
                            {
                                flowLayoutPanel1.Controls.Add(new Notification(reader2.GetString(1), reader2.GetString(2), reader2.GetString(3), reader2.GetInt32(0)));


                            }
                            reader2.Close();
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

        private void Glavna_stranica_Load(object sender, EventArgs e)
        {
            label7.Text = LoginInfo.User;

                label3.Text = "Trenutno nemate nijednu aktivnu kartu.";
                pictureBox1.BackColor = Color.Red;

                label4.Text = "";
                label5.Text = "";
                label6.Text = "";

            konekt.Open();
            connect = konekt.Vrati();
            using (connect)
            {
                try
                {
                    string query = "SELECT * FROM piprojekt.karta WHERE korisnik_id = @korisnik";

                    using (MySqlCommand cmd = new MySqlCommand(query, connect))
                    {

                        cmd.Parameters.AddWithValue("@korisnik", LoginInfo.UserID);


                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                        
                            while (reader.Read())
                            {

                                DateTime curTime = DateTime.Now;
                                DateTime karta = reader.GetDateTime(7);

                                    if (karta > curTime)
                                    {
                                        label3.Text = "Karta u upotrebi!";
                                        pictureBox1.BackColor = Color.Green;
                                        ts = (karta - curTime).StripMilliseconds();
                                        
                                        sekunde = Convert.ToInt32(ts.TotalSeconds);
                                        timer1.Enabled = true;


                                    }
                                
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

        private void button6_Click(object sender, EventArgs e)
        {
            Form pocetna = new pocetna();
            this.Hide();
            pocetna.ShowDialog();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form linije = new Linije();
            linije.ShowDialog();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form kupnja = new kupnja(15);
            this.Hide();
            kupnja.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form kupnja = new kupnja(30);
            this.Hide();
            kupnja.ShowDialog();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form kupnja = new kupnja(45);
            this.Hide();
            kupnja.ShowDialog();
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Form kontrolor = new Kontrolor();
            this.Hide();
            kontrolor.ShowDialog();
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if(sekunde > 0)
            {
                sekunde--;
                int stvarneMinute = sekunde / 60;
                int stvarneSekunde = sekunde - (stvarneMinute * 60);
                this.label4.Text = stvarneMinute.ToString();
                this.label5.Text = ":";
                this.label6.Text = stvarneSekunde.ToString();

            }
            else
            {
                this.timer1.Stop();
                MessageBox.Show("Vaša karta je istekla!");
                label3.Text = "Trenutno nemate nijednu aktivnu kartu.";
                pictureBox1.BackColor = Color.Red;

                label4.Text = "";
                label5.Text = "";
                label6.Text = "";
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form povijest = new Povijest();
            povijest.ShowDialog();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form add = new AddOrEditNotification();
            add.ShowDialog();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            RefreshNotifications();
        }


        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            string startupPath = Environment.CurrentDirectory;
            if (keyData == Keys.F1)
            {
                Help.ShowHelp(this, startupPath +"\\PIhelp\\help.chm", "content.htm");
                return true;
            }
            else return false;

        }

    }
    public static class TimeExstensions
    {
        public static TimeSpan StripMilliseconds(this TimeSpan time)
        {
            return new TimeSpan(time.Days, time.Hours, time.Minutes, time.Seconds);
        }
    }




}

