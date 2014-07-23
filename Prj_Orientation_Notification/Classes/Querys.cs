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

namespace Prj_Orientation_Notification.Classes
{
    public class Entity
    {
        public ObjectId Id { get; set; }

        public string Name { get; set; }
    }

    class Querys
    {
       // Find an Existing Document
       //In this example we will read back an Entity assuming we know the Id value:
           /* var query = Query<Entity>.EQ(e => e.Id, id);
            var entity = collection.FindOne(query);*/

        //update an existing document
       // Update an Existing Document
        //An alternative to Save is Update. The difference is that Save sends the entire document back to the server, but Update sends just the changes. For example:

       /*var query = Query<Entity>.EQ(e => e.Id, id);
        var update = Update<Entity>.Set(e => e.Name, "Harry"); // update modifiers
        collection.Update(query, update);*/


    }
}
