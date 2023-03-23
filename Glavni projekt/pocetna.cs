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
    public partial class pocetna : Form
    {
        public pocetna()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form register = new registracija();
            register.ShowDialog();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form prijava = new prijava();
            prijava.ShowDialog();
            this.Close();
        }

        private void pocetna_Load(object sender, EventArgs e)
        {
            groupBox1.Anchor = (AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left);
            button1.Anchor = (AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left);
            button2.Anchor = (AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Left);
        }
    }
}
