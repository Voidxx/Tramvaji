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
    public partial class Notification : UserControl
    {
        public Notification(string naslov, string tekst, string datum, int id)
        {
            InitializeComponent();
            label1.Text = naslov;
            label2.Text = datum;
            richTextBox1.Text = tekst;
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            DeleteButton deleteButton1 = new DeleteButton(id);
            deleteButton1.Location = new Point(158, 3);
            this.Controls.Add(deleteButton1);        
            if(LoginInfo.UserType == 1)
            {
                deleteButton1.Visible = false;
            }
            else
            {
                deleteButton1.Visible = true;
            }
        }

        private void Notification_Load(object sender, EventArgs e)
        {
            
        }
    }
}
