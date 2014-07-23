using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Threading;
using System.Xaml;
using System.Windows.Forms.Integration;
using ListenPortG4.Properties;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.Data.Sql;
using ListenPortG4;

namespace ListenPortG4
{
    public partial class listen : Form
    {
        public static string data = null;
        TcpListener server = null;
        Byte[] bytes = new Byte[256];
        TcpClient client = null;
        int aux = 0;

        public listen()
        {
            InitializeComponent();
        }

        private void btnSair_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnListen_Click(object sender, EventArgs e)
        {
            backgroundWorker1_DoWork(null,null);       
           /*
            ElementHost host = new ElementHost();
            host.Dock = DockStyle.Fill;    
            Data frm = new Data(localAddr, port);        
            host.Child = frm;
            panelData.Controls.Add(host);*/                                                              
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {                        
            Thread listen = new Thread(Listening);
            listen.Start();
        }
                      
        public void Listening() {
                       
            //if i press the button listen , i need close before the changes          
            //verify if button is enable or no
                if (cbListen.Checked == true)
                {                 
                    //connection string to database
                    string connectionString = ListenPortG4.Properties.Settings.Default.ConexaoSDF;
                    SqlCeConnection conn = null;
                    int aux = 1;

                /*for LOCAL TEST = IP LOCAL 127.0.0.1 | localhost , port 9000                   
                    IPAddress localAddr = IPAddress.Parse(txbIP.Text);                                         
                    server = new TcpListener(localAddr,Convert.ToInt32(txbPort.Text));
                    server.Start();*/

                //FOR TABLET 
                    IPAddress localAddr = IPAddress.Parse(txbIP.Text);
                    IPEndPoint ipend = new IPEndPoint(localAddr, Convert.ToInt32(txbPort.Text));
                    server = new TcpListener(ipend);
                    server.Start();                                                          

                    try
                    {              
                        /**/
                        //conn = new SqlCeConnection("Data Source=C:\\Users\\thamaraalves\\Documents\\Visual Studio 2012\\Projects\\ListenPortG4\\ListenPortG4\\g4Data.sdf");
                        conn = new SqlCeConnection(" Data Source=C:\\Users\\gemlab1\\Downloads\\ListenPortG4\\ListenPortG4\\ListenPortG4\\g4Data.sdf");
                        conn.Open();          
   
                            while (true)
                            {
                                if (txtData.InvokeRequired)
                                {
                                    txtData.Invoke(new MethodInvoker(delegate { txtData.Text += "Waiting for a connection..." + "\r\n"; }));
                                }

                                client = server.AcceptTcpClient();

                                if (txtData.InvokeRequired)
                                {
                                    txtData.Invoke(new MethodInvoker(delegate { txtData.Text += "Connected!" + "\r\n"; }));
                                }

                                // Get a stream object for reading and writing
                                NetworkStream stream = client.GetStream();

                                int i;
                                
                                // Loop to receive all the data sent by the client. 
                                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                                {
                                    // Translate data bytes to a ASCII string.
                                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                                    
                                    if (txtData.InvokeRequired)
                                    {
                                        txtData.Invoke(new MethodInvoker(delegate { txtData.Text += "Received {" + aux + "}" + data + "\r\n"; }));
                                       
                                    }

                                        //insert in DB
                                       string query = @"INSERT INTO tblDataG4(sData)                                                                         
                                                            VALUES(@sData)";
                                        SqlCeCommand cmd = new SqlCeCommand(query, conn);
                                      //  cmd.CommandText =                                                                      
                                                                            
                                        cmd.Parameters.AddWithValue("@sData", data);                                       
                                        cmd.ExecuteNonQuery();
                                    
                                        //quant of insertions in database
                                        if (lbData.InvokeRequired)
                                        {
                                            lbData.Invoke(new MethodInvoker(delegate { lbData.Text = "Insertions in Database:" + " " + aux.ToString(); }));
                                        }
                                        // Process the data sent by the client.
                                        data = data.ToUpper();
                                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                                        aux++;                              
                                }
                                                               
                                
                                }
                                // Shutdown and end connection
                                client.Close();
                         
                    }
                    catch (SocketException ei)
                    {
                        MessageBox.Show("SocketException: {0}" + ei);
                    }
                    catch (System.IO.IOException ex)
                    {
                        MessageBox.Show("IO Exception ERROR: {0}" + ex);
                    }
                    catch (System.Data.SqlServerCe.SqlCeException sql){
                        MessageBox.Show("ERROR SQL:" + sql);
                    }
                    catch(SqlException e)
                    {
                        MessageBox.Show(e.Message.ToString(), "Error Message");
                    }
                    finally
                    {
                        // Stop listening for new clients.
                        server.Stop();
                        conn.Close();
                    }
                    MessageBox.Show("\nHit enter to continue...");                        
                }
                else
                {
                    if (cbListen.InvokeRequired)
                    {
                        cbListen.Invoke(new MethodInvoker(delegate { cbListen.Checked = true; }));
                        client.Close();
                        server.Stop();                        
                    }
                    // Start listening for client requests.                    
                }                                                                              
        }

        private void btnClearMessage_Click(object sender, EventArgs e)
        {
            txtData.Clear();
        }

        private void cbListen_CheckedChanged(object sender, EventArgs e)
        {
            backgroundWorker1_DoWork(null, null);          
        }        

      /*  private void btnClearMessage_Click(object sender, EventArgs e)
        {
            txbMessageServer.Clear();
        }*/


        public void connectDB() {


        }

        private void listen_Load(object sender, EventArgs e)
        {
            //
        }


    }
}
