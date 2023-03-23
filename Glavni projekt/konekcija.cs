using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;
using System.Windows.Forms;

namespace Glavni_projekt
{
    class konekcija
    {
        
        
        string myConnectionString;
        MySql.Data.MySqlClient.MySqlConnection conn;

        public konekcija()
        {
            myConnectionString = "server=127.0.0.1;uid=root;" +
        "pwd=ghosts;database=piprojekt;";
            conn = new MySql.Data.MySqlClient.MySqlConnection(myConnectionString);

        }



        public void Open()
        {
            try
            {
                
                conn.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public MySqlConnection Vrati()
        {
            return conn;
        }

        public void Close()
        {
            conn.Close();
        }
       
    }
}
