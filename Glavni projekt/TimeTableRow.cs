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
    public partial class TimeTableRow : UserControl
    {
        public String Dolazak
        {
            get { return textBox1.Text; }
            set { textBox1.Text = value; }
           
        }

        public String Odlazak
        {
            get { return textBox2.Text; }
            set { textBox2.Text = value; }

        }

        public String Stanica
        {
            get { return textBox3.Text; }
            set { textBox3.Text = value; }

        }

        public int ID
        {
            get; set;
        }
        
        public TimeTableRow()
        {
            InitializeComponent();
        }

        private void TimeTableRow_Load(object sender, EventArgs e)
        {

        }
    }
}
