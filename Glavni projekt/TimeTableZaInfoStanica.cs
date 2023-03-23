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
    public partial class TimeTableZaInfoStanica : UserControl
    {
       
        public TimeTableZaInfoStanica(string vrijeme, string tramvaj)
        {
            InitializeComponent();
            label5.Text = vrijeme;
            label6.Text = tramvaj;
            
        }

        private void TimeTableZaInfoStanica_Load(object sender, EventArgs e)
        {
            

        }
    }
}
