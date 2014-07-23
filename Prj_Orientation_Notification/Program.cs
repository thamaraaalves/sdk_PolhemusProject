using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;
using ListenPortG4;
using Prj_Orientation_Notification.Classes;
//using Fleck;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Utilities;
using Newtonsoft.Json.Schema;
//using Alchemy;
//using Alchemy.Classes;
using WebSocket4Net; //to use as 
using Prj_DBMongo;
using Prj_GetDataDB;


namespace Prj_Orientation_Notification
{
    public class Program
    {
       // public static new List<IWebSocketConnection> allSockets;
       // public static WebSocketServer server;

        public static int AI = 0; //for database 
        public static string data = null;
        public static string[] sdata = null;
        public static string[] array = new string[40]; //11 * 4 = four sensors        
        public static double[] sensor1 = null;
        public static double[] sensor2 = null;
        public static int notificationModel;
        public static int privacyControl;
        public static string cs1 = "";
        public static string cs2 = "";
        public static string cs3 = "";
        public static string cs4 = "";

        public static int intruder = 0;
        public static int user1 = 0;
        public static int user2 = 0;
        public static int tablet = 0;
        
        public static string oldMessage = null;

        //beggining variables ----------------------------------------------------------------------------------------
        public static double[] s1 = new double[10];  //sensor 1 - 'intruder'
        public static int ss1 = 0;
        public static double[] s2 = new double[10]; // sensor 2 - 'tablet'
        public static int ss2 = 0;
        public static double[] s3 = new double[10]; // sensor 3 - 'user 1'
        public static int ss3 = 0;
        public static double[] s4 = new double[10]; // sensor 4 -  ''
        public static int ss4 = 0;


        //client to listen 9002
        public static WebSocket aClient;
        public static WebSocket aServer;

        //SENSOR 1 FOR DATABASE           
        public class S1
        {
            public string id { get; set; }
            public string cont { get; set; }
            public string date { get; set; }
            public string time { get; set; }
            public double p1 { get; set; }
            public double p2 { get; set; }
            public double p3 { get; set; }
            public double x { get; set; }
            public double y { get; set; }
            public double z { get; set; }
            public double q0 { get; set; }
            public double q1 { get; set; }
            public double q2 { get; set; }
            public double q3 { get; set; }
        }

        //SENSOR 2 FOR DATABASE
        public class S2
        {
            public string id { get; set; }
            public string cont { get; set; }
            public string date { get; set; }
            public string time { get; set; }
            public double p1 { get; set; }
            public double p2 { get; set; }
            public double p3 { get; set; }
            public double x { get; set; }
            public double y { get; set; }
            public double z { get; set; }
            public double q0 { get; set; }
            public double q1 { get; set; }
            public double q2 { get; set; }
            public double q3 { get; set; }

        }

        //SENSOR 3 - FOR DATABASE
        public class S3
        {
            public string id { get; set; }
            public string cont { get; set; }
            public string date { get; set; }
            public string time { get; set; }
            public double p1 { get; set; }
            public double p2 { get; set; }
            public double p3 { get; set; }
            public double x { get; set; }
            public double y { get; set; }
            public double z { get; set; }
            public double q0 { get; set; }
            public double q1 { get; set; }
            public double q2 { get; set; }
            public double q3 { get; set; }

        }

        public class S4
        {
            public string id { get; set; }
            public string cont { get; set; }
            public string date { get; set; }
            public string time { get; set; }
            public double p1 { get; set; }
            public double p2 { get; set; }
            public double p3 { get; set; }
            public double x { get; set; }
            public double y { get; set; }
            public double z { get; set; }
            public double q0 { get; set; }
            public double q1 { get; set; }
            public double q2 { get; set; }
            public double q3 { get; set; }

        }

        public class RootObject
        {
            public List<S1> itemsS1 { get; set; }
            public List<S2> itemsS2 { get; set; }
            public List<S3> itemsS3 { get; set; }
            public List<S4> itemsS4 { get; set; }

        }

        public static void OnDisconnect(object sender, EventArgs context)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("{0}: Disconnected", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
        }

        public static void OnDisconnectPolhemus(object sender, EventArgs context)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("{0}: Disconnected", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
        }

        private static void OnConnected(object sender, EventArgs context)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("{0}: Connected", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
        }

        private static void OnConnectedPolhemus(object sender, EventArgs context)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("{0}: Connected", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
        }

        public static void SendMessage(string message)
        {
            if (aClient.State == WebSocketState.Closed)
            {
                Connected();
            }
            if (oldMessage != message)
            {
                aClient.Send(message);
                oldMessage = message;
            }
        }

        private static void OnReceive(object sender, MessageReceivedEventArgs context)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("{0}: Message Received:\n{1}\n", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), context.Message);

            // var message = context.Message;
            if (context.Message != "Notification")
            {
                //client
              /*  var  = context.Message
                
                if ()
                {
                    
                }*/
               
                    JObject o = JObject.Parse(context.Message);

                    Console.WriteLine("type: " + o["type"]);
                    Console.WriteLine("level: " + o["level"]);
                    Console.WriteLine("privacyControl: " + o["privacyControl"]);
                    // if (o["type"].ToString() == "")
                    //{
                    //just for test
                    Console.WriteLine("client: " + o["client"]);
                    Console.WriteLine("notificationModel: " + o["notificationModel"]);

                    if (o["notificationModel"] != null)
                    {
                        notificationModel = Convert.ToInt16(o["notificationModel"]);
                    }
                    if (o["privacyControl"] != null)
                    {
                        privacyControl = Convert.ToInt16(o["privacyControl"]);
                    }
               
            }
        }

        //the second as client in port 9002
        /*public static void GetV8aluesAsClient() {
            aClient = new WebSocket("ws://134.190.170.127:9002");
            aClient.Opened += OnConnected;
            aClient.Closed += OnDisconnect;
            aClient.MessageReceived += OnReceive;
            OnConnect(null, null);
            aClient.Open();
            while (true)
            {
                Thread.Sleep(50);
            }        
        }*/

        public static void Connected()
        {
            //aClient = new WebSocket("ws://134.190.154.139:9002"); // hui computer
         //  aClient = new WebSocket("ws://134.190.170.127:9002"); //thamara computer
            string connection = "ws://" + Prj_GetDataDB.Properties.Settings.Default.ip + ":9002";
            aClient = new WebSocket(connection);
          //  aClient = new WebSocket("ws://192.168.0.15:9002");
            aClient.Opened += OnConnected;
            //  aClient.Closed += OnDisconnect;
            aClient.MessageReceived += OnReceive;
            OnConnected(null, null);
            aClient.Open();
        }

        public static void Main(string[] args)
        {



            TcpListener server = null;
            Connected(); //connected listen 9002
            // beginning TCP LISTENING ------------------------------------------------------------------------------------          
            Int32 port = 9000;
            IPAddress localAddr = IPAddress.Parse(Prj_GetDataDB.Properties.Settings.Default.ip); //thamara
                                  
            server = new TcpListener(localAddr, port);

            // Start listening for client requests.
            server.Start();

            // Buffer for reading data
            Byte[] bytes = new Byte[365];
            String data = null;
             int aux = 1;
             
             // string portSend = "9001";
             //ip local
            // string ipAdress = "134.190.154.139"; //hui computer
         
             //ip dal 
             //string ipAdress = "134.190.170.127";       
                                   
           
            try
            {
                while (true)
                {
                  
                    Console.WriteLine("Calculate Application - listen port 9000 and send to 9002 ");
                    Console.Title = "Listening G4 Data - Calculate data";
                    Console.WriteLine("Waiting for a connection..." + "\r\n");
                    // Perform a blocking call to accept requests. 
                    // You could also user server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    //read sensor 1
                    Console.WriteLine("Write the combination for sensor 1: ");
                    cs1 = Console.ReadLine();

                    //read sensor 2
                    Console.WriteLine("Write the combination for sensor 2: ");
                    cs2 = Console.ReadLine();

                    //read sensor 3
                    Console.WriteLine("Write the combination for sensor 3: ");
                    cs3 = Console.ReadLine();

                    //read sensor 4
                    Console.WriteLine("Write the combination for sensor 4: ");
                    cs4 = Console.ReadLine();              
                    
                    var read = "";
                    //who is who
                    Console.WriteLine("Which sensor is the tablet?");
                    read = Console.ReadLine();
                    if (read!="")
                    {
                        tablet = Convert.ToInt16(read);
                    }
                    
                    Console.WriteLine("Which sensor is the intruder?");
                    read = Console.ReadLine();
                    if (read != "")
                    {
                        intruder = Convert.ToInt16(read);
                    }
                    Console.WriteLine("Which sensor is the user 1?");
                    read = Console.ReadLine();
                    if (read != "")
                    {
                        user1 = Convert.ToInt16(read);
                    }                   
                    Console.WriteLine("Which sensor is the user 2?");
                    read = Console.ReadLine();
                    if (read != "")
                    {
                        user2 = Convert.ToInt16(read);
                    }

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int t;

                    // Loop to receive all the data sent by the client. 
                    while ((t = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {                     
                        // Send back a response.
                        // stream.Write(msg, 0, msg.Length);
                        // Console.WriteLine("Sent: {0}", data);                                                                    

                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, t);
                        Console.WriteLine(data.Length + " " + "bytes read");
                       // Console.WriteLine(data);
                        // Process the data sent by the client.
                        data = data.ToUpper();

                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                        
                        sdata = data.Split(' ');

                        for (int i = 0; i < sdata.Length; i++)
                        {
                            Console.WriteLine(sdata[i]);
                        }

                        string[] newArray = new string[44];
                        int cont = 0;
                        //remove the '/'
                        string text = "";
                        char c = ' ';
                   //     int quant = 0;

                        //remove the ""
                        for (int i = 0; i < sdata.Length; i++)
                        {
                            if (sdata[i]!="" && cont<44)
                            {
                                newArray[cont] = sdata[i];
                                cont++;                                    
                            }
                        }

                        //remove the character '|'
                        for (int i = 0; i < newArray.Length; i++)
                        {                            
                                text = newArray[i];

                                if (text!=null)
                                {
                                    int last = text.Length - 1;

                                    c = text[text.Length - 1];

                                    char compare = '|';

                                    if (c == compare)
                                    {
                                        text = text.Remove(text.Length - 1);
                                        newArray[i] = text;
                                    }                           
                                }                                
                        }

                        //before verify if the string is correct
                        //sensor1
                        if ((newArray[0] == "0" && newArray[1] == "1" && newArray[3] == "0X0000") || (newArray[0] == "1" && newArray[1] == "0" && newArray[3] == "0X0000") || (newArray[0] == "0" && newArray[1] == "0" && newArray[3] == "0X0000") || (newArray[0] == "1" && newArray[1] == "1" && newArray[3] == "0X0000"))
                        {                           
                            //created sensors s1, s2, s3, s4        
                            //newArray[0] == "0" && newArray[1] == "0") || (newArray[11] == "0" && newArray[12] == "0") || (newArray[0] == "22" && newArray[1] == "23") || (newArray[33] == "0" && newArray[34] == "0")
                            
                            //verify the size of array   

                            if (cs1!="")
                            {
                                char cs1_1 = cs1[0];
                                char cs1_2 = cs1[1];


                                if ((newArray[0] == cs1_1.ToString() && newArray[1] == cs1_2.ToString()))
                                {
                                    for (int i = 0; i < 11; i++)
                                    {
                                        if (newArray[i] != "0X0000" && ss1 < 11)
                                        {
                                            s1[ss1] = Convert.ToDouble(newArray[i]);
                                            ss1++;
                                        }
                                    }

                                }                     
                            }

                            if (cs2!="")
                            {
                                char cs2_1 = cs2[0];
                                char cs2_2 = cs2[1];


                                //sensor2
                                //(newArray[0] == "0" && newArray[1] == "1") || (newArray[11] == "0" && newArray[12] == "1") || (newArray[22] == "0" && newArray[23] == "1") || (newArray[33] == "0" && newArray[34] == "1")
                                if ((newArray[11] == cs2_1.ToString() && newArray[12] == cs2_2.ToString()))
                                {
                                    for (int i = 11; i < 22; i++)
                                    {
                                        if (newArray[i] != "0X0000" && ss2 < 11)
                                        {
                                            s2[ss2] = Convert.ToDouble(newArray[i]);
                                            ss2++;
                                        }
                                    }
                                }
                            }

                            if (cs3!="")
                            {
                                char cs3_1 = cs3[0];
                                char cs3_2 = cs3[1];


                                //sensor3
                                //(newArray[0] == "1" && newArray[1] == "0") || (newArray[11] == "1" && newArray[12] == "0") || (newArray[22] == "1" && newArray[23] == "0") || (newArray[33] == "1" && newArray[34] == "0")
                                if ((newArray[22] == cs3_1.ToString() && newArray[23] == cs3_2.ToString()))
                                {
                                    for (int i = 22; i < 33; i++)
                                    {
                                        if (newArray[i] != "0X0000" && ss3 < 11)
                                        {
                                            s3[ss3] = Convert.ToDouble(newArray[i]);
                                            ss3++;
                                        }
                                    }
                                }
                            }

                            if (cs4!="")
                            {
                                char cs4_1 = cs4[0];
                                char cs4_2 = cs4[1];


                                //sensor
                                //(newArray[0] == "1" && newArray[1] == "1") || (newArray[11] == "1" && newArray[12] == "1") || (newArray[22] == "1" && newArray[23] == "1") || (newArray[33] == "1" && newArray[34] == "1")
                                if ((newArray[33] == cs4_1.ToString() && newArray[34] == cs4_2.ToString()))
                                {
                                    for (int i = 33; i < 44; i++)
                                    {
                                        if (newArray[i] != "0X0000" && ss4 < 11)
                                        {
                                            s4[ss4] = Convert.ToDouble(newArray[i]);
                                            ss4++;
                                        }
                                    }
                                }               
                            }                          
                                                  
                    
                                //To restart array
                                //  quant = 0;

                                //aux for sensors 1,2,3, and 4
                                ss1 = 0;
                                ss2 = 0;
                                ss3 = 0;
                                ss4 = 0;
                                cont = 0;
                                data = null;

                                //TO CALCULATE
                                //calculate  data , hui's algorithm -----------------------------------------------------------------------------

                            /*    bool hasDatas1 = true;
                                for (int i = 0; i < s1.Length; i++)
                                {
                                    if ( s1[i]==0)
                                    {
                                        hasDatas1 = false;
                                    }                                   
                                }

                                bool hasDatas2 = true;
                                for (int i = 0; i < s2.Length; i++)
                                {
                                    if (s2[i] == 0)
                                    {
                                        hasDatas2 = false;
                                    }
                                }

                                bool hasDatas3 = true;
                                for (int i = 0; i < s3.Length; i++)
                                {
                                    if (s3[i] == 0)
                                    {
                                        hasDatas3 = false;
                                    }
                                }

                                bool hasDatas4 = true;
                                for (int i = 0; i < s4.Length; i++)
                                {
                                    if (s4[i] == 0)
                                    {
                                        hasDatas4 = false;
                                    }
                                }*/


                              //  if (hasDatas1==true)
                               // {
                                    Console.WriteLine(s1[0] + " " + s1[1] + " " + s1[2] + " " + s1[3] + " " + s1[4] + " " + s1[5] + " " + s1[6] + " " + s1[7] + " " + s1[8] + " " + s1[9]);
                              //  }
                              //  if (hasDatas2==true)
                              //  {
                                    Console.WriteLine(s2[0] + " " + s2[1] + " " + s2[2] + " " + s2[3] + " " + s2[4] + " " + +s2[5] + " " + s2[6] + " " + s2[7] + " " + s2[8] + " " + s2[9]);
                              //  }

                             //   if (hasDatas3==true)
                             //   {
                                    Console.WriteLine(s3[0] + " " + s3[1] + " " + s3[2] + " " + s3[3] + " " + s3[4] + " " + s3[5] + " " + s3[6] + " " + s3[7] + " " + s3[8] + " " + s3[9]);
                              //  }
                             //   if (hasDatas4==true)
                             //   {
                                    Console.WriteLine(s4[0] + " " + s4[1] + " " + s4[2] + " " + s4[3] + " " + s4[4] + " " + s4[5] + " " + s4[6] + " " + s4[7] + " " + s4[8] + " " + s4[9]);
                            //    }
                               
                              

                                Vector3 p1 = new Vector3(s1[3], s1[4], s1[5]);
                                Vector3 p2 = new Vector3(s2[3], s2[4], s2[5]);
                                Vector3 p3 = new Vector3(s3[3], s3[4], s3[5]);
                                Vector3 p4 = new Vector3(s4[3], s4[4], s4[5]);

                                Quaternion q1 = new Quaternion(s1[6], s1[7], s1[8], s1[9]);
                                Quaternion q2 = new Quaternion(s2[6], s2[7], s2[8], s2[9]);
                                Quaternion q3 = new Quaternion(s3[6], s3[7], s3[8], s3[9]);
                                Quaternion q4 = new Quaternion(s4[6], s4[7], s4[8], s4[9]);
                                //END 

                                //Console.WriteLine("Rotated point:" + " " + Quaternion.Rotatedpoint(p1, q1));
                                Console.WriteLine("Rotated vector:" + " " + Quaternion.RotatedVector(p1, q1));

                               //

                                NotificationEventArgs passer_tablet = new NotificationEventArgs(p1, p2, q1, q2);
                                //NotificationEventArgs user_tablet = ...;;
                                NotificationEventArgs tablet_user = new NotificationEventArgs(p2, p3, q2, q3);
                                NotificationEventArgs tablet_user2 = new NotificationEventArgs(p2, p4, q2, q4);
                                NotificationEventArgs user_user = new NotificationEventArgs(p3, p4, q3, q4);

                                Console.WriteLine("The angle between the tablet and intruder is:");
                                Console.WriteLine("Degree:" + " " + passer_tablet.AngleAtoB.Degrees);
                                Console.WriteLine("The distance between the tablet and intruder is:");
                                Console.WriteLine("Distance:" + " " + passer_tablet.Distance);

                                //receive messages from vinicius (notification is on or off, which type, privacy control is on or off, auto or manual, type)
                                //get notificationModel and privacyControl                        
                                //ListenApplication.Program.NotificationModel                      

                                //notificationModel                     
                                if (notificationModel == 1)
                                    passer_tablet.Noti_simple();
                                if (notificationModel == 2)
                                    passer_tablet.Noti_coarse();
                                if (notificationModel == 3)
                                    passer_tablet.Noti_coarse();
                                if (notificationModel == 4)
                                    passer_tablet.Noti_map();
                                if (notificationModel == 5)
                                    passer_tablet.Noti_fine();

                                //privacyControl
                                if (privacyControl == 1)
                                    passer_tablet.PrivacyControl_Hidden();
                                if (privacyControl == 2)
                                    passer_tablet.PrivacyControl_grayscale();
                                if (privacyControl == 3)
                                    passer_tablet.PrivacyControl_brightness();
                                if (privacyControl == 4)
                                    passer_tablet.PrivacyControl_Lantern();

                                //TO Save data in mongo database ------------------------------------------------------------------------------------------
                                //separated data for database
                                var objectToSerializeS1 = new RootObject();
                                var objectToSerializeS2 = new RootObject();
                                var objectToSerializeS3 = new RootObject();
                                var objectToSerializeS4 = new RootObject();

                                string date = DateTime.Now.Month.ToString() + "/" + DateTime.Now.Day.ToString() + "/" + DateTime.Now.Year.ToString();
                                string time = DateTime.Now.TimeOfDay.ToString();

                                //this will be the id
                                string id = AI.ToString();
                                objectToSerializeS1.itemsS1 = new List<S1> { new S1 { cont = aux.ToString(), date = date, time = time, p1 = s1[0], p2 = s1[1], p3 = s1[2], x = s1[3], y = s1[4], z = s1[5], q0 = s1[6], q1 = s1[7], q2 = s1[8], q3 = s1[9] } };
                                objectToSerializeS2.itemsS2 = new List<S2> { new S2 { cont = aux.ToString(), date = date, time = time, p1 = s2[0], p2 = s2[1], p3 = s2[2], x = s2[3], y = s2[4], z = s2[5], q0 = s2[6], q1 = s2[7], q2 = s2[8], q3 = s2[9] } };
                                objectToSerializeS3.itemsS3 = new List<S3> { new S3 { cont = aux.ToString(), date = date, time = time, p1 = s3[0], p2 = s3[1], p3 = s3[2], x = s3[3], y = s3[4], z = s3[5], q0 = s3[6], q1 = s3[7], q2 = s3[8], q3 = s3[9] } };
                                objectToSerializeS4.itemsS4 = new List<S4> { new S4 { cont = aux.ToString(), date = date, time = time, p1 = s4[0], p2 = s4[1], p3 = s4[2], x = s4[3], y = s4[4], z = s4[5], q0 = s4[6], q1 = s4[7], q2 = s4[8], q3 = s4[9] } };
                                AI++;// this is for id

                                //CHANGE PARAMETER 'sensor1, 2, 3, and 4' to tablet, user, intruder..... everything

                                //the parameters is (1= polhemus data, 2=application data, 3=name of collection , 4=name of database, and 5=type of sensor)
                                //if (hasDatas1==true)
                              //  {
                                    Prj_DBMongo.Program.Insert(objectToSerializeS1.itemsS1[0].ToJson(), null, "sensor1", "dataPolhemus", "S1");
                               // }

                             //   if (hasDatas2==true)
                             //   {
                                    //save sensor 2
                                    Prj_DBMongo.Program.Insert(objectToSerializeS2.itemsS2[0].ToJson(), null, "sensor2", "dataPolhemus", "S2");
                             //   }
                            //    if (hasDatas3==true)
                            //    {
                                    //save sensor 3
                                    Prj_DBMongo.Program.Insert(objectToSerializeS3.itemsS3[0].ToJson(), null, "sensor3", "dataPolhemus", "S3");
                            //    }
                            //    if (hasDatas4==true)
                            //    {
                                    //save sensor 4
                                    Prj_DBMongo.Program.Insert(objectToSerializeS4.itemsS4[0].ToJson(), null, "sensor4", "dataPolhemus", "S4");
                           //     }                              
                                                                                           
                                // end SAVE DATA ---------------------------------------------------------------------------------------------

                                //save sensor 1            
                                //quant of insertions in database
                                //  Console.WriteLine("Number of frames:" + " " + aux.ToString());
                                aux++;
                           
                           
                            
                        }                      
                      //  data = null;
                    }
                    // Shutdown and end connection
                    client.Close();
                }             
                  
            }
            catch (SocketException ei)
            {
                Console.WriteLine("SocketException: {0}" + ei);
            }
            catch (System.IO.IOException ex)
            {
                Console.WriteLine("IO Exception ERROR: {0}" + ex);
            }
            finally
            {
                // Stop listening for new clients.
                //   server.Stop();
            }
            //    Console.WriteLine("\nHit enter to continue...");           
        }

        public static void OnReceiveListenSensors(object sender, MessageReceivedEventArgs context)
        {
            //Translate data bytes to a ASCII string.
            //   data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
            //BELLOW - JUST FOR TEST
            //Console.WriteLine("Received {" + aux + "}" + data + "\r\n");

            //aux to separated string 


        }
    }
}
