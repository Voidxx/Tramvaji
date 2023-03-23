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
    public partial class TimeTableRowStanica : UserControl
    {

        string vr;
        public String vrijeme
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }

        }

        public String Stanica
        {
            get { return textBox2.Text; }
            set { textBox2.Text = value; }

        }

        public int ID
        {
            get; set;
        }

        public TimeTableRowStanica(string vrsta)
        {
            InitializeComponent();
            vr = vrsta;
        }

        private void TimeTableRowStanica_Load(object sender, EventArgs e)
        {
            label1.Text = vr;
        }
    }
}
