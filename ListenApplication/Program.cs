using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Utilities;
using Newtonsoft.Json.Schema;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;
using Fleck;
using System.Threading;
using Fleck.Handlers;
using System.Web.Script.Serialization;
using System.Runtime.Serialization.Json;
using System.Net;
using System.Net.Sockets;
using Prj_DBMongo;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Shared;
using Prj_GetDataDB;

//PROGRAM LISTENING USER APPLICATION
namespace ListenApplication
{
    public class Program
    {
       // public static new List<IWebSocketConnection> allSockets;
        public static string data = null;
        public static string[] sdata = null;
        public static bool status;
        //get bellow variables from Application
       // public static int notificationModel {get; set;}
        public static int privacyControl { get; set; }
        public static int notificationModel { get; set; }

            
        public static new List<IWebSocketConnection> allSockets;      
        public static WebSocketServer server;
               

        /*public static void OnDisconnect(object sender, EventArgs context)
        {
            Console.WriteLine("{0}: Disconnected", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
        }

        private static void OnConnect(object sender, EventArgs context)
        {
            Console.WriteLine("{0}: Connecting...", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
        }

        private static void OnConnected(object sender, EventArgs context)
        {
            Console.WriteLine("{0}: Connected", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            aClient.Send("Hey!");
        }

        private static void OnReceive(object sender, MessageReceivedEventArgs context)
        {
            Console.WriteLine("{0}: Message Received:\n{1}\n", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"), context.Message);
        }


        //as client - second option, when in prj_orientation_notification as server in port 9004
        public static void SendG4(BsonJavaScript values) {
            aClient = new WebSocket("ws://134.190.170.127:9004");
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

        public class HeatMap
        {
            public ObjectId Id { get; set; }
            public string type { get; set; }
            public int quant { get; set; }
            public double x { get; set; }
            public double y { get; set; }
            public int count { get; set; }
            public string userid1 { get; set; }
            public string userid2 { get; set; }
            public string timestamp { get; set; }
            public string address { get; set; }
        }

        public class RootObject
        {
            public List<HeatMap> itemHeatMap { get; set; }
        }

        public static void SearchHeatMap(string uid1, string uid2, string address)
        {

        //search and send to application
        /*string ud1 = "95";
         string ud2 = "96";
         string addss = "http://134.190.154.139:8081/index.html";*/

          //  var connectionString = "mongodb://134.190.170.127";//thamara

            var connectionString = "mongodb://" + Prj_GetDataDB.Properties.Settings.Default.ip;


         //   var connectionString = "mongodb://192.168.0.15";
            //var connectionString = "mongodb://134.190.154.139"; //hui
            var clientDB = new MongoClient(connectionString);
            var serverDB = clientDB.GetServer();
            var database = serverDB.GetDatabase("userApplication");
            var collection = database.GetCollection("heatmap");
                    
            var query =
                    collection.AsQueryable<HeatMap>()
                    .Where(e => e.userid1 == uid1)
                    .Where(e => e.userid2 == uid2)
                    .Where(e => e.address == address)
                    .Select(e => e);

            int quant = Convert.ToInt16(query.Count());
            var data = query.ToList();

            for (int i = 0; i < quant; i++)
            {
               // Console.WriteLine(item.quant + item.x + item.y);
                var objectToSerialize = new RootObject();

                objectToSerialize.itemHeatMap = new List<HeatMap>{               
                    new HeatMap { type ="set-heatmap", quant = quant,
                x = Convert.ToDouble(data[i].x), y = Convert.ToDouble(data[i].y), count = Convert.ToInt16(data[i].count), userid1 = Convert.ToString(data[i].userid1), userid2 = Convert.ToString(data[i].userid2), timestamp = Convert.ToString(data[i].timestamp), address = Convert.ToString(data[i].address) }
                };

             //    var valuee = new List<HeatMap>;
           /*    var  valuee = new HeatMap { type ="set-heatmap", quant = quant,
                x = Convert.ToDouble(data[i].x), y = Convert.ToDouble(data[i].y), count = Convert.ToInt16(data[i].count), userid1 = Convert.ToString(data[i].userid1), userid2 = Convert.ToString(data[i].userid2), timestamp = Convert.ToString(data[i].timestamp), address = Convert.ToString(data[i].address) }
                };*/

                var test = objectToSerialize.itemHeatMap[0];
           
                var jsonSerialiser = new JavaScriptSerializer();
                var json = jsonSerialiser.Serialize(test);
                              

               // var test = objectToSerialize.itemHeatMap[0].ToJson();
           /*     var json = new JavaScriptSerializer().Serialize(valuee);*/

                
                SendMessage(json);
                Console.WriteLine("Sending to User Application");

            }
            
        }
        
        public static void Main(string[] args)
        {
           // SearchHeatMap(null,null,null);
            FleckLog.Level = LogLevel.Debug;
            allSockets = new List<IWebSocketConnection>();
            // var server = new WebSocketServer("ws://134.190.154.139:9002"); //hui 
            //var server = new WebSocketServer("ws://134.190.170.127:9002"); //thamara

            var server = new WebSocketServer("ws://" + Prj_GetDataDB.Properties.Settings.Default.ip + ":9002");
            //var server = new WebSocketServer("ws://192.168.0.15:9002");
            //update color in console application
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Title = "Listen Application";
            Console.WriteLine("To close this, write 'EXIT' in console!");

            server.Start(socket =>
                {
                    socket.OnOpen = () =>
                        {
                            Console.WriteLine("Open!");
                            allSockets.Add(socket);
                            status = true;
                        };
                    socket.OnClose = () =>
                        {
                            Console.WriteLine("Close!");
                            allSockets.Remove(socket);
                            status = true;
                        };
                    socket.OnMessage = message =>
                        {
                            Console.WriteLine(message);

                            if (message != "Notification")
                            {
                                //to receive data
                                if (message != "Hey!")
                                {
                                    JObject o = JObject.Parse(message);

                                    //notification
                                    Console.WriteLine("type: " + o["type"]);
                                    Console.WriteLine("level: " + o["level"]);
                                    Console.WriteLine("client: " + o["client"]);
                                    Console.WriteLine("notificationModel: " + o["notificationModel"]);
                                    //privacy control
                                    Console.WriteLine("privacyControl: " + o["privacyControl"]);
                                    Console.WriteLine("status: " + o["status"]);
                                    if (o["notificationModel"] != null)
                                    {
                                        notificationModel = Convert.ToInt16(o["notificationModel"]);
                                    }
                                    if (o["privacyControl"] != null)
                                    {
                                        privacyControl = Convert.ToInt16(o["privacyControl"]);
                                    }
                                    if (o["collection"] != null)
                                    {
                                        //send to the database
                                        //name of the database is 'userApplication'
                                        Prj_DBMongo.Program.Insert(null, message, null, "userApplication", null);
                                    }
                                    if (o["type"] != null)
                                    {
                                        if (o["type"].ToString() == "get-heatmap")
                                        {
                                            string userid1 = o["userid1"].ToString();
                                            string userid2 = o["userid2"].ToString();
                                            string address = o["address"].ToString();
                                            SearchHeatMap(userid1, userid2, address);
                                        }
                                    }
                                   // SendMessage(message);
                                }
                                
                            }
                            SendMessage(message);
                        };
                });

            var input = Console.ReadLine();
          
            while (input != "exit")
            {               
                input = Console.ReadLine();
            }
        }

        public static void SendMessage(string message)
        {
            try
            {
                allSockets.ToList().ForEach(s => s.Send(message));
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                throw;
            }
        }       
    }

        
}
    

