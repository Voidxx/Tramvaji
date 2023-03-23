using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Drawing;
using System.Drawing.Imaging;


namespace Glavni_projekt
{
    public partial class Racun : Form
    {
        public Racun(string min, string datum, string ID, string ime, string prezime, byte[] a)
        {
            InitializeComponent();
            label8.Text = min;
            label9.Text = datum;
            label10.Text = ID;
            label11.Text = ime + " " + prezime;
            MemoryStream stream = new MemoryStream(a);
            
            
            stream.Position = 0;



            
            Bitmap bitmap = new Bitmap(stream);

            pictureBox1.Image = bitmap;

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void Racun_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var frm = Form.ActiveForm;
            using (var bmp = GetFormImageWithoutBorders(frm))
            {
                string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);
                MemoryStream mem = new MemoryStream();
                bmp.Save(mem, System.Drawing.Imaging.ImageFormat.Jpeg);
                

                PdfDocument doc = new PdfDocument();
                doc.Pages.Add(new PdfPage());
                XGraphics xgr = XGraphics.FromPdfPage(doc.Pages[0]);
                XImage img = XImage.FromStream(mem);

                

                xgr.DrawImage(img, 0, 0);
                doc.Save(path + "\\document.pdf");
                doc.Close();
                mem.Dispose();
                linkLabel1.Visible = true;
            }

           



        }

        private Bitmap GetFormImageWithoutBorders(Form frm)
        {
            
            using (Bitmap whole_form = GetControlImage(frm))
            {
                
                Point origin = frm.PointToScreen(new Point(0, 0));
                int dx = origin.X - frm.Left;
                int dy = origin.Y - frm.Top;

               
                int wid = frm.ClientSize.Width;
                int hgt = frm.ClientSize.Height;
                Bitmap bm = new Bitmap(wid, hgt);
                using (Graphics gr = Graphics.FromImage(bm))
                {
                    gr.DrawImage(whole_form, 0, 0,
                        new Rectangle(dx, dy, wid, hgt),
                        GraphicsUnit.Pixel);
                }
                return bm;
            }
        }

        private Bitmap GetControlImage(Control ctl)
        {
            linkLabel1.Visible = false;
            Bitmap bm = new Bitmap(ctl.Width, ctl.Height);
            ctl.DrawToBitmap(bm,
                new Rectangle(0, 0, ctl.Width, ctl.Height));
            return bm;
        }
    }
}
