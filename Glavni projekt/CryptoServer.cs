using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using CefSharp;
using CefSharp.WinForms;
using System.Net;
using System.IO;
using System.Threading;
using MessagingToolkit.QRCode.Codec;
using Coinbase.Commerce;
using Coinbase.Commerce.Models;
using Newtonsoft.Json;


namespace Glavni_projekt
{
    public partial class CryptoServer : Form
    {
        
        int vr;
        int am;
        public ChromiumWebBrowser chromeBrowser;
        public MySql.Data.MySqlClient.MySqlConnection connect;
        konekcija konekt = new konekcija();
        string kod = RandomString(8);
        Bitmap bmp;
        Thread Serv;

        public CryptoServer(int vrsta)
        {
            InitializeComponent();
            vr = vrsta;
            if (vrsta == 1)
            {
                am = 2;
            }
            else if (vrsta == 2)
            {
                am = 4;
            }
            else
            {
                am = 6;
            }


            this.Shown += new System.EventHandler(this.CryptoServer_Shown);



        }



        public void InitializeChromium(Response<Charge> response)
        {
            if (!Cef.IsInitialized) // Check before init
            {
                CefSettings settings = new CefSettings();
                // Initialize cef with the provided settings
                Cef.Initialize(settings);
            }
            // Create a browser component
            chromeBrowser = new ChromiumWebBrowser(response.Data.HostedUrl);
            // Add it to the form and fill it to the form window.
            this.Controls.Add(chromeBrowser);
            chromeBrowser.Dock = DockStyle.Fill;




        }
        private static Random random = new Random();
        private static string RandomString(int length)
        {



            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());

        }

        public async Task CreateResponse()
        {
            var commerceApi = new CommerceApi("82e76978-bc34-4ebb-8065-16184f821fce");
            string customerId = LoginInfo.UserID.ToString();



            var charge = new CreateCharge
            {
                Name = "Karta za tramvaj",
                Description = "",
                PricingType = PricingType.FixedPrice,
                LocalPrice = new Money { Amount = am, Currency = "HRK" },
                Metadata =
         {
            {"customerId", customerId}
         },
            };

            var response = await commerceApi.CreateChargeAsync(charge);


            if (response.HasError())
            {

                Console.WriteLine(response.Error.Message);
                return;
            }

            
            InitializeChromium(response);
        }



        private void server()
        {


            Thread.Sleep(5000);
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add($"http://+:80/");

            listener.Start();
            Console.WriteLine("Listening...");

            HttpListenerContext context = listener.GetContext();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse resp = context.Response;

            string text;
            
            using (var reader = new StreamReader(request.InputStream,
                                     request.ContentEncoding))
            {
                text = reader.ReadToEnd();
            }


            string operationLocation = request.Headers.GetValues("X-Cc-Webhook-Signature").FirstOrDefault();

            if (WebhookHelper.IsValid("fb126610-6358-466b-96d8-fa4e6ae43cb9", operationLocation , text))
            {

                var webhook = JsonConvert.DeserializeObject<Webhook>(text);

                var chargeInfo = webhook.Event.DataAs<Charge>();

  
                

                if (webhook.Event.IsChargeFailed)
                {
                    
                    
                }

                //else if (webhook.Event.IsChargeConfirmed)
                // {
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

               
           // }
  














 

        }




        private async void CryptoServer_Load(object sender, EventArgs e)
        {


            await CreateResponse();

            


        }

        private void CryptoServer_Shown(object sender, EventArgs e)
        {

            Serv = new Thread(server);
            Serv.Start();
        }

        private void CryptoServer_FormClosing(object sender, FormClosingEventArgs e)
        {
           
        }
    }
}
