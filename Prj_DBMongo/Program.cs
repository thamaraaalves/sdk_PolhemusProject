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
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Utilities;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Shared;
using Prj_GetDataDB;

//PROJETO DATA BASE - INSERTION
namespace Prj_DBMongo
{
    public class Program
    {
        //CHANGE THE IP IF CHANGE COMPUTER
        //public static string connectionString = "mongodb://134.190.154.139"; //hui application
        public static string connectionString = "mongodb://" + Prj_GetDataDB.Properties.Settings.Default.ip;
        //public static string clientDB = new MongoClient(connectionString);  
        //public static string clientDB = new MongoClient();

        public static MongoClient clientDB = new MongoClient(connectionString);
        public static int cont = 0;

        public class RootObject
        {
            public List<HeatMap> itemHeatMap { get; set; }
        }
        
        public class S1
        {
            //public string id { get; set; }        
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
            //public string id { get; set; }         
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
            //public string id { get; set; }         
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
          // public string id { get; set; }       
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


        //heatmap
        public class HeatMap {
            public ObjectId Id { get; set; }            
            public double x { get; set; }
            public double y { get; set; }
            public int count { get; set; }
            public string userid1 { get; set; }
            public string userid2 { get; set; }
            public string timestamp { get; set; }
            public string address { get; set; }        
        }

        //controls
        public class Controls {
            public ObjectId Id { get; set; }     
            public string control { get; set; }
            public string action { get; set; }
            public string value { get; set; }
            public string userid1 { get; set; }
            public string userid2 { get; set; }
            public string timestamp { get; set; }
            public string address { get; set; }                
        }

        //notifications
        public class Notifications {
            public ObjectId Id { get; set; }     
            public string notification { get; set; }
            public string action { get; set; }
            public string level { get; set; }
            public string userid1 { get; set; }
            public string userid2 { get; set; }
            public string timestamp { get; set; }
            public string address { get; set; }     
        }

        //privacycontrol
        public class PrivacyControl
        {
            public ObjectId Id { get; set; }
            public string privacycontrol { get; set; }
            public string action { get; set; }
            public string status { get; set; }
            public string userid1 { get; set; }
            public string userid2 { get; set; }
            public string timestamp { get; set; }
            public string address { get; set; }
        }

        //game
        public class Game {
            public ObjectId Id { get; set; }
            public string userid1 { get; set; }
            public string userid1cards { get; set; }
            public string userid1timespent { get; set; }
            public string userid2 { get; set; }
            public string userid2selectedcards { get; set; }
            public string userid2rightcards { get; set; }
            public string address { get; set; }  
        }
    
        
        public static void Main(string[] args)
        {
            //connection();          
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Number of insertions: " + cont);
                       
            Console.WriteLine("Mongo database connected...");
            // Console.ReadLine();
            //var input = Console.ReadLine();
        }

        /*public static void SearchHeatMap(string uid1, string uid2, string address)
        {
            //search and send to application

            var connectionString = "mongodb://134.190.170.127";//thamara
            var clientDB = new MongoClient(connectionString);
            var serverDB = clientDB.GetServer();

            var database = serverDB.GetDatabase("userApplication");
            var collection = database.GetCollection("heatmap");

            var query = Query.And(
                Query.EQ("userid1", uid1),
                Query.EQ("userid1", uid2),
                Query.EQ("address", address)
            );

            MongoCursor<BsonDocument> dbResult = collection.Find(query);

            foreach (var items in dbResult)
            {
                var objectToSerialize = new RootObject();
                objectToSerialize.itemHeatMap = new List<HeatMap>{               
                    new HeatMap {
                x = Convert.ToDouble(items["x"]), y = Convert.ToDouble(items["y"]), count = Convert.ToInt16(items["count"]), userid1 = Convert.ToString(items["userid1"]), userid2 = Convert.ToString(items["userd2"]), timestamp = Convert.ToString(items["timestamp"]), address = Convert.ToString(items["address"]) }
                };
            }            
            //send (objectToSerialize.itemsPControl[0].ToJson());
            

        }*/
           
        public static void Insert(string sensors, string data, string collec, string nameDB, string typeSensor) {
            try
            {
                //connection again
            //    connection();
                if (data != null)
                {
                    JToken o = JObject.Parse(data);
                    //var entity="";
                    if (o["collection"] != null)
                    {
                        // var collections = o["collection"].ToString();
                        var typeOfCollection = o["collection"].ToString();

                        if (typeOfCollection == "heatmap")
                        {
                            var entity = new HeatMap { x = Convert.ToDouble(o["x"]), y = Convert.ToDouble(o["y"]), count = Convert.ToInt16(o["count"]), userid1 = Convert.ToString(o["userid1"]), userid2 = Convert.ToString(o["userid2"]), timestamp = Convert.ToString(o["timestamp"]), address = Convert.ToString(o["address"]) };
                            var serverDB = clientDB.GetServer();
                            //Get a Reference to a Database Object
                            var database = serverDB.GetDatabase(nameDB);
                            // "entities" is the name of the collection
                            var collection = database.GetCollection<HeatMap>(typeOfCollection);
                            collection.Insert(entity);                          
                        }
                        if (typeOfCollection == "controls")
                        {
                            var entity = new Controls { control = Convert.ToString(o["control"]), action = Convert.ToString(o["action"]), value = Convert.ToString(o["value"]), userid1 = Convert.ToString(o["userid1"]), userid2 = Convert.ToString(o["userid2"]), timestamp = Convert.ToString(o["timestamp"]), address = Convert.ToString(o["address"]) };
                            var serverDB = clientDB.GetServer();
                            //Get a Reference to a Database Object
                            var database = serverDB.GetDatabase(nameDB);

                            // "entities" is the name of the collection
                            var collection = database.GetCollection<Controls>(typeOfCollection);
                            collection.Insert(entity);                            
                        }
                        if (typeOfCollection == "notification")
                        {
                            var entity = new Notifications { notification = Convert.ToString(o["notification"]), action = Convert.ToString(o["action"]), level = Convert.ToString(o["level"]), userid1 = Convert.ToString(o["userid1"]), userid2 = Convert.ToString(o["userid2"]), timestamp = Convert.ToString(o["timestamp"]), address = Convert.ToString(o["address"]) };
                            var serverDB = clientDB.GetServer();
                            //Get a Reference to a Database Object
                            var database = serverDB.GetDatabase(nameDB);

                            // "entities" is the name of the collection
                            var collection = database.GetCollection<Notifications>(typeOfCollection);
                            collection.Insert(entity);                           
                        }
                        if (typeOfCollection == "privacycontrol")
                        {
                            var entity = new PrivacyControl { privacycontrol = Convert.ToString(o["privacycontrol"]), action = Convert.ToString(o["action"]), status = Convert.ToString(o["status"]), userid1 = Convert.ToString(o["userid1"]), userid2 = Convert.ToString(o["userid2"]), timestamp = Convert.ToString(o["timestamp"]), address = Convert.ToString(o["address"]) };
                            var serverDB = clientDB.GetServer();
                            //Get a Reference to a Database Object
                            var database = serverDB.GetDatabase(nameDB);

                            // "entities" is the name of the collection
                            var collection = database.GetCollection<PrivacyControl>(typeOfCollection);
                            collection.Insert(entity);
                        }

                        if (typeOfCollection == "game")
                        {
                            var entity = new Game { userid1 = Convert.ToString(o["userid1"]), userid1cards = Convert.ToString(o["userid1cards"]), userid1timespent = Convert.ToString(o["userid1timespent"]), userid2 = Convert.ToString(o["userid2"]), userid2selectedcards = Convert.ToString(o["userid2selectedcards"]), userid2rightcards = Convert.ToString(o["userid2rightcards"]), address = Convert.ToString(o["address"]) };
                            var serverDB = clientDB.GetServer();
                            //Get a Reference to a Database Object
                            var database = serverDB.GetDatabase(nameDB);
                            // "entities" is the name of the collection
                            var collection = database.GetCollection<Game>(typeOfCollection);
                            collection.Insert(entity);                           
                        }
                    }
                }

                if (sensors != null)
                {
                    if (typeSensor == "S1")
                    {
                        JToken o = JObject.Parse(sensors);
                        var serverDB = clientDB.GetServer();
                        //Get a Reference to a Database Object
                        var database = serverDB.GetDatabase(nameDB);
                        var entity = new S1 { cont = Convert.ToString(o["cont"]), date = Convert.ToString(o["date"]), time = Convert.ToString(o["time"]), p1 = Convert.ToDouble(o["p1"]), p2 = Convert.ToDouble(o["p2"]), p3 = Convert.ToDouble(o["p3"]), x = Convert.ToDouble(o["x"]), y = Convert.ToDouble(o["y"]), z = Convert.ToDouble(o["z"]), q0 = Convert.ToDouble(o["q0"]), q1 = Convert.ToDouble(o["q1"]), q2 = Convert.ToDouble(o["q2"]), q3 = Convert.ToDouble(o["q3"]) };
                        // "entities" is the name of the collection
                        var collection = database.GetCollection<S1>(collec);
                        collection.Insert(entity);                                           
                    }

                    if (typeSensor == "S2")
                    {
                        JToken o = JObject.Parse(sensors);
                        var serverDB = clientDB.GetServer();
                        //Get a Reference to a Database Object
                        var database = serverDB.GetDatabase(nameDB);
                        var entity = new S2 { cont = Convert.ToString(o["cont"]), date = Convert.ToString(o["date"]), time = Convert.ToString(o["time"]), p1 = Convert.ToDouble(o["p1"]), p2 = Convert.ToDouble(o["p2"]), p3 = Convert.ToDouble(o["p3"]), x = Convert.ToDouble(o["x"]), y = Convert.ToDouble(o["y"]), z = Convert.ToDouble(o["z"]), q0 = Convert.ToDouble(o["q0"]), q1 = Convert.ToDouble(o["q1"]), q2 = Convert.ToDouble(o["q2"]), q3 = Convert.ToDouble(o["q3"]) };
                        // "entities" is the name of the collection
                        var collection = database.GetCollection<S2>(collec);
                        collection.Insert(entity);  
                    }

                    if (typeSensor == "S3")
                    {
                        JToken o = JObject.Parse(sensors);
                        var serverDB = clientDB.GetServer();
                        //Get a Reference to a Database Object
                        var database = serverDB.GetDatabase(nameDB);
                        var entity = new S3 { cont = Convert.ToString(o["cont"]), date = Convert.ToString(o["date"]), time = Convert.ToString(o["time"]), p1 = Convert.ToDouble(o["p1"]), p2 = Convert.ToDouble(o["p2"]), p3 = Convert.ToDouble(o["p3"]), x = Convert.ToDouble(o["x"]), y = Convert.ToDouble(o["y"]), z = Convert.ToDouble(o["z"]), q0 = Convert.ToDouble(o["q0"]), q1 = Convert.ToDouble(o["q1"]), q2 = Convert.ToDouble(o["q2"]), q3 = Convert.ToDouble(o["q3"]) };
                        // "entities" is the name of the collection
                        var collection = database.GetCollection<S3>(collec);
                        collection.Insert(entity);                       
                    }

                    if (typeSensor == "S4")
                    {
                        JToken o = JObject.Parse(sensors);
                        var serverDB = clientDB.GetServer();
                        //Get a Reference to a Database Object
                        var database = serverDB.GetDatabase(nameDB);
                        var entity = new S4 { cont = Convert.ToString(o["cont"]), date = Convert.ToString(o["date"]), time = Convert.ToString(o["time"]), p1 = Convert.ToDouble(o["p1"]), p2 = Convert.ToDouble(o["p2"]), p3 = Convert.ToDouble(o["p3"]), x = Convert.ToDouble(o["x"]), y = Convert.ToDouble(o["y"]), z = Convert.ToDouble(o["z"]), q0 = Convert.ToDouble(o["q0"]), q1 = Convert.ToDouble(o["q1"]), q2 = Convert.ToDouble(o["q2"]), q3 = Convert.ToDouble(o["q3"]) };
                        // "entities" is the name of the collection
                        var collection = database.GetCollection<S4>(collec);
                        collection.Insert(entity);                       
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Error in insertion at database: " + error);               
            }                                                              
        }
    }
}
