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
using MySql.Data.MySqlClient;
using CefSharp;
using CefSharp.WinForms;
using System.Net;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace Glavni_projekt
{
    public partial class Server : Form
    {
        public ChromiumWebBrowser chromeBrowser;
        int vr;
        public MySql.Data.MySqlClient.MySqlConnection connect;
        konekcija konekt = new konekcija();
        string kod = RandomString(8);
        Bitmap bmp;
        string nonce;
        Thread Serv;

        public static BraintreeGateway gateway = new BraintreeGateway
        {
            Environment = Braintree.Environment.SANDBOX,
            MerchantId = "x6w64q4zj6f8z2q7",
            PublicKey = "tg24kwjq3bsd7ckg",
            PrivateKey = "4c4cd238d0b7464eabda0926b36daab1"
        };


        public void InitializeChromium(string token)
        {
            if (!Cef.IsInitialized)
            {
                CefSettings settings = new CefSettings();
                Cef.Initialize(settings);
            }
            chromeBrowser = new ChromiumWebBrowser("http://localhost/PhpProject1/index.php?token=" + token);
            this.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;




        }


        public Server(int vrsta)
        {
            InitializeComponent();
            vr = vrsta;
            var clientToken = gateway.ClientToken.Generate(
new ClientTokenRequest
{

}
);
            
            InitializeChromium(clientToken);
            

        }

        public void server()
        {
            Thread.Sleep(3000);
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add($"http://localhost:8000/");

            listener.Start();
            Console.WriteLine("Listening...");

            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            string documentContents;
            using (Stream receiveStream = request.InputStream)
            {
                using (StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8))
                {
                    documentContents = readStream.ReadToEnd();
                }
            }

            nonce = documentContents;
            string[] noncesplit = nonce.Split('=');

            nonce = noncesplit[1];



            //
            var Trrequest = new TransactionRequest
            {
                Amount = 10.00M,
                PaymentMethodNonce = nonce,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            Result<Transaction> result = gateway.Transaction.Sale(Trrequest);
            

            if (result.IsSuccess())
            {
                
            string responseString = "<HTML><BODY> Payment successful! Returning to home page...</BODY></HTML>";
                
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
                Thread.Sleep(1000);

                konekt.Open();
                connect = konekt.Vrati();
                int korisnikid = LoginInfo.UserID;



                QRCodeEncoder encoder = new QRCodeEncoder();
                encoder.QRCodeScale = 8;
                bmp = encoder.Encode(kod);

               


                using (connect)
                {
                    try
                    {
                        string query = "INSERT INTO karta (korisnik_id, vrsta_karte_id, vrijeme_datum, status, QRcode, QRimage) VALUES (@id, @vrsta, CURRENT_TIMESTAMP(), 1, @string, @image);";

                        using (MySqlCommand cmd = new MySqlCommand(query, connect))
                        {

                            MemoryStream stream = new MemoryStream();
                            bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
                            Byte[] bytes = stream.ToArray();

                            cmd.Parameters.AddWithValue("@id", korisnikid);
                            cmd.Parameters.AddWithValue("@vrsta", vr);
                            cmd.Parameters.AddWithValue("@string", kod);
                            cmd.Parameters.AddWithValue("@image", bytes);
                            cmd.ExecuteNonQuery();
                        }

                    }
                    catch (MySqlException ex)
                    {
                        MessageBox.Show("Exception: " + ex.Message);
                    }
                }
                string path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop);

                bmp.Save(path + @"\qrkod" + kod + ".png");

                konekt.Close();


            //


          

            chromeBrowser.Delete();
            
            
            listener.Stop();
            
            
            listener.Close();

                if (InvokeRequired)
                {
                    this.Invoke(new MethodInvoker(delegate
                    {
                        Form glavna = new Glavna_stranica();
                        this.Hide();
                        this.Close();
                        Serv.Abort();
                        glavna.ShowDialog();
                        
                    }));
                }
                else
                {
                    Form glavna = new Glavna_stranica();
                    this.Hide();
                    this.Close();
                    Serv.Abort();
                    glavna.ShowDialog();
                    
                }
            }



        }

       
        private void Klijent_Load(object sender, EventArgs e)
        {





            Serv = new Thread(server);
            Serv.Start();
            
            







        }
        private static Random random = new Random();
        private static string RandomString(int length)
        {



            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());

        }



    }
}

 