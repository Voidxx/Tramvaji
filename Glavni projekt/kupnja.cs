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
using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using Braintree;
using System.Web;
using System.Threading;

namespace Glavni_projekt
{
    public partial class kupnja : Form
    {
        int vrsta;
  

        public kupnja(int vrijeme)
        {
            InitializeComponent();
            textBox1.Text = vrijeme.ToString();
           
            if(vrijeme == 15)
            {
                vrsta = 1;
                label4.Text = "2,00 kn";
            }
            else if (vrijeme == 30)
            {
                vrsta = 2;
                label4.Text = "4,00 kn";
            }
            else
            {
                vrsta = 3;
                label4.Text = "6,00 kn";
            }
        }

        private void kupnja_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form glavna  = new Glavna_stranica();
            this.Hide();
            glavna.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
            {
                Thread.Sleep(1000);
                Form klijent = new Server(vrsta);
                this.Hide();
                klijent.ShowDialog();
                this.Close();
            }
            if(radioButton2.Checked == true)
            {
                Form klijent2 = new CryptoServer(vrsta);
                this.Hide();
                klijent2.ShowDialog();
                this.Close();
            }



        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
