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
using WebSocketSharp;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Utilities;
using Newtonsoft.Json.Schema;
using Alchemy;
using Alchemy.Classes;
using System.Collections.Concurrent;
using System.Threading;

namespace Prj_Orientation_Notification.Classes
{        
    public class websocket
    {
        //globals variables - that i received from Vinicius's Applications
        public static int notificationModel = 0;
        public static int notificationLevel = 0;
        public static int privacyControl = 0;
        public static int privacyControlOn = 0;
        public static int notificationOn = 0;
        public static int privacyControlAuto=0;
        public static int privacyControlAutoOn=0;
        public static IPAddress ip = IPAddress.Parse("134.190.170.127");
           
        protected static ConcurrentDictionary<string, Connection> OnlineConnections = new ConcurrentDictionary<string, Connection>();
                       
        public static void Listening() {              
            try
            {
                //change ip to application server 
                var aServer = new WebSocketServer(9001, ip)
                {
                    OnReceive = OnReceive,
                    OnSend = OnSend,
                    OnConnected = OnConnect,
                    OnDisconnect = OnDisconnect,
                    TimeOut = new TimeSpan(0, 5, 0)
                };
                aServer.Start();

              Console.ForegroundColor = ConsoleColor.Yellow;
              Console.Title = "Alchemy WebSocket Server";
              Console.WriteLine("Running Alchemy WebSocket Server ...");
        
                
            }
            catch (Exception error)
            {
                Console.WriteLine("Error " + " " + error);
                throw;
            }

        }//end listening function

        public static void OnConnect(UserContext aContext)
        {
            Console.WriteLine("Client Connected From : " + aContext.ClientAddress.ToString());

            // Create a new Connection Object to save client context information
            var conn = new Connection { Context = aContext };

            // Add a connection Object to thread-safe collection
            OnlineConnections.TryAdd(aContext.ClientAddress.ToString(), conn);

        }
                
        public static void OnReceive(UserContext aContext)
        {
            try
            {
                Console.WriteLine("Data Received From [" + aContext.ClientAddress.ToString() + "] - " + aContext.DataFrame.ToString());
                
                JObject obj = new JObject();

                JToken token = aContext.DataFrame.ToJson();

                

                if (token!=null)
	            {
                      if ((int)token["notificationModel"] != null)
                        notificationModel = (int)token["notificationModel"];
                    if ((int)token["notificationLevel"] != null)
                        notificationLevel = (int)token["notificationLevel"];
                    if ((int)token["privacyControl"] != null)
                        privacyControl = (int)token["privacyControl"];
                    if ((int)token["privacyControlOn"]!= null)
                        privacyControlOn = (int)token["privacyControlOn"];
                    if ((int)token["notificationOn"] != null)
                        notificationOn = (int)token["notificationOn"];
                    if ((int)token["privacyControlAuto"] != null)
                        notificationOn = (int)token["privacyControlAuto"];
                    if ((int)token["privacyControlAutoOn"] != null)
                        notificationOn = (int)token["privacyControlAutoOn"];		 
	            }               

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }

        public static void OnSend(UserContext aContext)
        {
            Console.WriteLine("Data Sent To : " + aContext.ClientAddress.ToString());
            aContext.Send("teste");
        }
        
        public static void OnDisconnect(UserContext aContext)
        {
            Console.WriteLine("Client Disconnected : " + aContext.ClientAddress.ToString());

            // Remove the connection Object from the thread-safe collection
            Connection conn;
            OnlineConnections.TryRemove(aContext.ClientAddress.ToString(), out conn);

            // Dispose timer to stop sending messages to the client.
            conn.timer.Dispose();
        }
    }//end

        public class Connection
        {
            public System.Threading.Timer timer;
            public UserContext Context { get; set; }
            public Connection()
            {
                this.timer = new System.Threading.Timer(this.TimerCallback, null, 0, 1000);
            }

            private void TimerCallback(object state)
            {
                try
                {
                    // Sending Data to the Client
                    Context.Send("[" + Context.ClientAddress.ToString() + "] " + System.DateTime.Now.ToString());
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }//end class connection
}//end namespace
