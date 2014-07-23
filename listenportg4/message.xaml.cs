using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Sockets;
using System.Net;
using System.Threading;


namespace ListenPortG4
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class Data : UserControl
    {
        IPAddress nIP;
        //nIP encapsulate
        
        public IPAddress NIP
        {
            get { return nIP; }
            set { nIP = value; }
        }

        Int32 nPort;
        //nPort encapsulate
        public Int32 NPort
        {
            get { return nPort; }
            set { nPort = value; }
        }

        public Data(IPAddress ip, Int32 port)
        {
            nIP = ip;
            nPort = port;
            InitializeComponent();
            showData();
        }

        //this method
        public void showData() {
           
           //verify in server

            int aux = 0;
            TcpListener server = null;

            try
            {
                // Set the TcpListener on port 9000.      
                // TcpListener server = new TcpListener(port);
                server = new TcpListener(nIP, nPort);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[256];
                String data = null;

                // Enter the listening loop. 
                while (true)
                {
                    txbData.Text  = "Waiting for a connection... ";

                    // Perform a blocking call to accept requests. 
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                   txbData.Text  = "Connected!";

                    data = null;
                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client. 
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        txbData.Text  = "Received {1} : {0}" + data + aux;

                        // Process the data sent by the client.
                        data = data.ToUpper();
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                        aux++;
                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                MessageBox.Show("SocketException: {0}" + e);
            }
            catch (System.IO.IOException ex)
            {
                MessageBox.Show("IO Exception ERROR: {0}" + ex);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }


            Console.WriteLine("\nHit enter to continue...");



            
        }
    }
}
