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
    public partial class TimeTableZaInfoTramvaj : UserControl
    {
        public TimeTableZaInfoTramvaj(string dolazak, string polazak, string stanica)
        {
            InitializeComponent();
            label4.Text = dolazak;
            label5.Text = polazak;
            label6.Text = stanica;
        }

        private void TimeTableZaInfoTramvaj_Load(object sender, EventArgs e)
        {

        }
    }
}
