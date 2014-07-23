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

namespace Prj_Orientation_Notification.Classes
{
    public class mongodb
    {
        public static void insert(BsonJavaScript entity, string Tcollection) 
        {
            //Beginning DATABASE MONGODB---------------------------------------------------------------------------------
            //mongo database
            var nameDB = "test17";
            //Get a Reference to the Client Object
            var connectionString = "mongodb://localhost";
            var clientDB = new MongoClient(connectionString);
            //Get a Reference to a Server Object
            var serverDB = clientDB.GetServer();
            //Get a Reference to a Database Object
            var database = serverDB.GetDatabase(nameDB);
            // "entities" is the name of the collection
            var collection = database.GetCollection<Entity>(Tcollection);
            collection.Insert(entity);           
        }

        //methods for sample 
        public static void InsertDataSensors(Entity entity, string collection){      
            

        }

        
        
    }
}
