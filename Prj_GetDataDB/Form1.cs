using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MongoDB.Driver.Linq;
using MongoDB.Bson;
//using Prj_DBMongo;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.IO;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using System.Configuration;
using System.IO;
using MongoDB.Driver.Operations;
using MongoDB.Driver.Builders;

namespace Prj_GetDataDB
{
    public partial class Form1 : Form
    {
       // public static string connectionString = "mongodb://134.190.154.139";
        //public static string clientDB = new MongoClient(connectionString);  
        //public static string clientDB = new MongoClient();

   //     public static MongoClient clientDB = new MongoClient(connectionString);
        public static int cont = 0;

        public class S1
        {
            //public string id { get; set; }                 
            public int id { get; set; }
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

        public Form1()
        {
            InitializeComponent();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void connection()
        {
        }

        private void copyAlltoClipboard()
        {
            dtvSensor1.SelectAll();
            DataObject dataObj = dtvSensor1.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }

        private void CreateCells() {
            try
            {
              
            }
            catch (Exception)
            {
                
                throw;
            }
        }


        private void FindApplicationData(string type, int N) {
            try
            {
                //get values from Mongo dataBase
               // var connectionString = "mongodb://134.190.154.139"; //hui
              //  var connectionString = "mongodb://134.190.170.127"; //thamara
                var connectionString = "mongodb://" + Prj_GetDataDB.Properties.Settings.Default.ip;

                var clientDB = new MongoClient(connectionString);
                var serverDB = clientDB.GetServer();
                             

                if (type=="all-data")
                {
                    //get collections
                    var database = serverDB.GetDatabase("userApplication");
                    var collection1 = database.GetCollection("heatmap");
                    var collection2 = database.GetCollection("controls");
                    var collection3 = database.GetCollection("notification");
                    var collection4 = database.GetCollection("privacycontrol");
                    var collection5 = database.GetCollection("game");
                    
                    //int size = (int)collection1.Count();
                    var cursor1 = collection1.FindAll();
                    var cursor2 = collection2.FindAll();
                    var cursor3 = collection3.FindAll();
                    var cursor4 = collection4.FindAll();
                    var cursor5 = collection5.FindAll();
                    
                    //I CAN USE FOR EVERY SEARCH
                    //datagridviewPolhemus.DataSource = cursor.ToList();                
                    int aux1 = 0;
                    int aux2 = 0;
                    int aux3 = 0;
                    int aux4 = 0;
                    int aux5 = 0;
                    
                    //fill heatmap
                    foreach (var items in cursor1)
                    {
                        aux1++;
                        //datagridviewPolhemus.Rows.Add(item["p1"].ToString());
                        DataGridViewRow dt = new DataGridViewRow();
                        dt.CreateCells(dtvHeatMap);
                        dt.Cells[0].Value = aux1.ToString();
                        dt.Cells[1].Value = items["x"].ToString();
                        dt.Cells[2].Value = items["y"].ToString();
                        dt.Cells[3].Value = items["count"].ToString();
                       
                        dt.Cells[4].Value = items["userid1"].ToString();
                        dt.Cells[5].Value = items["userid2"].ToString();
                      /*  }
                       else
                        {
                            dt.Cells[4].Value = "";
                            dt.Cells[5].Value = "";
                        }*/
                      
                        dt.Cells[6].Value = items["timestamp"].ToString();
                        dt.Cells[7].Value = items["address"].ToString();
                        dtvHeatMap.Rows.Add(dt);                     
                        Application.DoEvents();
                        // datagridviewPolhemus.Rows[aux].Cells["p1"].Value = item["p1"].ToString();                   
                    }

                    //fill controls
                    foreach (var items in cursor2)
                    {
                        aux2++;
                        //datagridviewPolhemus.Rows.Add(item["p1"].ToString());
                        DataGridViewRow dt = new DataGridViewRow();
                        dt.CreateCells(dtvControls);
                        dt.Cells[0].Value = aux2.ToString();
                        dt.Cells[1].Value = items["control"].ToString();
                        dt.Cells[2].Value = items["action"].ToString();
                        dt.Cells[3].Value = items["value"].ToString();
                        /* if (items["userid1"] != null && items["userid2"] != null)
                        {*/
                        dt.Cells[4].Value = items["userid1"].ToString();
                        dt.Cells[5].Value = items["userid2"].ToString();
                        /* }
                        else
                        {
                            dt.Cells[4].Value = "";
                            dt.Cells[5].Value = "";
                        }*/
                        dt.Cells[6].Value = items["timestamp"].ToString();
                        dt.Cells[7].Value = items["address"].ToString();
                        dtvControls.Rows.Add(dt);
                        Application.DoEvents();
                        // datagridviewPolhemus.Rows[aux].Cells["p1"].Value = item["p1"].ToString();                   
                    }


                    //fill notification
                    foreach (var items in cursor3)
                    {
                        aux3++;
                        //datagridviewPolhemus.Rows.Add(item["p1"].ToString());
                        DataGridViewRow dt = new DataGridViewRow();
                        dt.CreateCells(dtvNotification);
                        dt.Cells[0].Value = aux3.ToString();
                        dt.Cells[1].Value = items["notification"].ToString();
                        dt.Cells[2].Value = items["action"].ToString();
                        dt.Cells[3].Value = items["level"].ToString();
                      /*  if (items["userid1"] != null && items["userid2"] != null)
                        {*/
                        dt.Cells[4].Value = items["userid1"].ToString();
                        dt.Cells[5].Value = items["userid2"].ToString();
                      /*  }
                        else
                        {
                            dt.Cells[4].Value = "";
                            dt.Cells[5].Value = "";
                        }*/
                     
                        dt.Cells[6].Value = items["timestamp"].ToString();
                        dt.Cells[7].Value = items["address"].ToString();
                        dtvNotification.Rows.Add(dt);
                        Application.DoEvents();
                        // datagridviewPolhemus.Rows[aux].Cells["p1"].Value = item["p1"].ToString();                   
                    }

                    //fill privacycontrol
                    foreach (var items in cursor4)
                    {
                        aux4++;
                        //datagridviewPolhemus.Rows.Add(item["p1"].ToString());
                        DataGridViewRow dt = new DataGridViewRow();
                        dt.CreateCells(dtvPrivacyControl);
                        dt.Cells[0].Value = aux4.ToString();
                        dt.Cells[1].Value = items["privacycontrol"].ToString();
                        dt.Cells[2].Value = items["action"].ToString();
                        dt.Cells[3].Value = items["status"].ToString();
                       
                        dt.Cells[4].Value = items["userid1"].ToString();
                        dt.Cells[5].Value = items["userid2"].ToString();
                       
                        dt.Cells[6].Value = items["timestamp"].ToString();
                        dt.Cells[7].Value = items["address"].ToString();
                        dtvPrivacyControl.Rows.Add(dt);
                        Application.DoEvents();
                        // datagridviewPolhemus.Rows[aux].Cells["p1"].Value = item["p1"].ToString();                   
                    }

                    //fill game
                    foreach (var items in cursor5)
                    {
                        aux5++;
                        //datagridviewPolhemus.Rows.Add(item["p1"].ToString());
                        DataGridViewRow dt = new DataGridViewRow();
                        dt.CreateCells(dtvGame);
                        dt.Cells[0].Value = aux5.ToString();
                   
                        dt.Cells[1].Value = items["userid1"].ToString();
                        dt.Cells[2].Value = items["userid1cards"].ToString();
                        dt.Cells[3].Value = items["userid1timespent"].ToString();
                        dt.Cells[4].Value = items["userid2"].ToString();
                        dt.Cells[5].Value = items["userid2selectedcards"].ToString();
                        dt.Cells[6].Value = items["userid2rightcards"].ToString();
                       
                        dt.Cells[7].Value = items["address"].ToString();
                        dtvGame.Rows.Add(dt);
                        Application.DoEvents();
                        // datagridviewPolhemus.Rows[aux].Cells["p1"].Value = item["p1"].ToString();                   
                    }




                }

                if (type == "x-frames")
                {
                    //get collections
                    var database = serverDB.GetDatabase("userApplication");
                    var collection1 = database.GetCollection("heatmap");
                    var collection2 = database.GetCollection("controls");
                    var collection3 = database.GetCollection("notification");
                    var collection4 = database.GetCollection("privacycontrol");
                    var collection5 = database.GetCollection("game");

                    int size1 = (int)collection1.Count();
                    int size2 = (int)collection2.Count();
                    int size3 = (int)collection3.Count();
                    int size4 = (int)collection4.Count();
                    int size5 = (int)collection4.Count();

                    var cursor1 = collection1.FindAll().Skip(size1 - N);
                    var cursor2 = collection2.FindAll().Skip(size2 - N);
                    var cursor3 = collection3.FindAll().Skip(size3 - N);
                    var cursor4 = collection4.FindAll().Skip(size4 - N);
                    var cursor5 = collection4.FindAll().Skip(size5 - N);
                                        
                    //I CAN USE FOR EVERY SEARCH
                    //datagridviewPolhemus.DataSource = cursor.ToList();                
                    int aux1 = 0;
                    int aux2 = 0;
                    int aux3 = 0;
                    int aux4 = 0;
                    int aux5 = 0;
                    
                    //fill heatmap
                    foreach (var items in cursor1)
                    {
                        aux1++;
                        //datagridviewPolhemus.Rows.Add(item["p1"].ToString());
                        DataGridViewRow dt = new DataGridViewRow();
                        dt.CreateCells(dtvHeatMap);
                        dt.Cells[0].Value = aux1.ToString();
                        dt.Cells[1].Value = items["x"].ToString();
                        dt.Cells[2].Value = items["y"].ToString();
                        dt.Cells[3].Value = items["count"].ToString();

                    //    dt.Cells[4].Value = "";
                    //    dt.Cells[5].Value = "";

                      dt.Cells[4].Value = items["userid1"].ToString();
                      dt.Cells[5].Value = items["userid2"].ToString();
                        dt.Cells[6].Value = items["timestamp"].ToString();
                        dt.Cells[7].Value = items["address"].ToString();
                        dtvHeatMap.Rows.Add(dt);
                        Application.DoEvents();
                        // datagridviewPolhemus.Rows[aux].Cells["p1"].Value = item["p1"].ToString();                   
                    }

                    //fill controls
                    foreach (var items in cursor2)
                    {
                        aux2++;
                        //datagridviewPolhemus.Rows.Add(item["p1"].ToString());
                        DataGridViewRow dt = new DataGridViewRow();
                        dt.CreateCells(dtvControls);
                        dt.Cells[0].Value = aux2.ToString();
                        dt.Cells[1].Value = items["control"].ToString();
                        dt.Cells[2].Value = items["action"].ToString();
                        dt.Cells[3].Value = items["value"].ToString();

                        //just for test
                       // dt.Cells[4].Value = "";
                      //  dt.Cells[5].Value = "";

                       dt.Cells[4].Value = items["userid1"].ToString();
                       dt.Cells[5].Value = items["userid2"].ToString();
                        dt.Cells[6].Value = items["timestamp"].ToString();
                        dt.Cells[7].Value = items["address"].ToString();
                        dtvControls.Rows.Add(dt);
                        Application.DoEvents();
                        // datagridviewPolhemus.Rows[aux].Cells["p1"].Value = item["p1"].ToString();                   
                    }

                    //fill notification
                    foreach (var items in cursor3)
                    {
                        aux3++;
                        //datagridviewPolhemus.Rows.Add(item["p1"].ToString());
                        DataGridViewRow dt = new DataGridViewRow();
                        dt.CreateCells(dtvNotification);
                        dt.Cells[0].Value = aux3.ToString();
                        dt.Cells[1].Value = items["notification"].ToString();
                        dt.Cells[2].Value = items["action"].ToString();
                        dt.Cells[3].Value = items["level"].ToString();

                        //just for test
                     //   dt.Cells[4].Value = "";
                    //    dt.Cells[5].Value = "";

                       dt.Cells[4].Value = items["userid1"].ToString();
                       dt.Cells[5].Value = items["userid2"].ToString();
                        dt.Cells[6].Value = items["timestamp"].ToString();
                        dt.Cells[7].Value = items["address"].ToString();
                        dtvNotification.Rows.Add(dt);
                        Application.DoEvents();
                        // datagridviewPolhemus.Rows[aux].Cells["p1"].Value = item["p1"].ToString();                   
                    }

                    //fill privacycontrol
                    foreach (var items in cursor4)
                    {
                        aux4++;
                        //datagridviewPolhemus.Rows.Add(item["p1"].ToString());
                        DataGridViewRow dt = new DataGridViewRow();
                        dt.CreateCells(dtvPrivacyControl);
                        dt.Cells[0].Value = aux4.ToString();
                        dt.Cells[1].Value = items["privacycontrol"].ToString();
                        dt.Cells[2].Value = items["action"].ToString();
                        dt.Cells[3].Value = items["status"].ToString();

                        //this is just before vinicius send me new version
                       // dt.Cells[4].Value = "";
                       // dt.Cells[5].Value = "";

                       dt.Cells[4].Value = items["userid1"].ToString();
                       dt.Cells[5].Value = items["userid2"].ToString();
                        dt.Cells[6].Value = items["timestamp"].ToString();
                        dt.Cells[7].Value = items["address"].ToString();
                        dtvPrivacyControl.Rows.Add(dt);
                        Application.DoEvents();
                        // datagridviewPolhemus.Rows[aux].Cells["p1"].Value = item["p1"].ToString();                   
                    }

                    //fill game
                    foreach (var items in cursor5)
                    {
                        aux5++;
                        //datagridviewPolhemus.Rows.Add(item["p1"].ToString());
                        DataGridViewRow dt = new DataGridViewRow();
                        dt.CreateCells(dtvGame);
                        dt.Cells[0].Value = aux5.ToString();
                      /*  dt.Cells[1].Value = "";
                        dt.Cells[2].Value = "";
                        dt.Cells[3].Value = "";
                        dt.Cells[4].Value = "";
                        dt.Cells[5].Value = "";
                        dt.Cells[6].Value = "";*/

                       dt.Cells[1].Value = items["userid1"].ToString();
                       dt.Cells[2].Value = items["userid1cards"].ToString();
                       dt.Cells[3].Value = items["userid1timespent"].ToString();
                       dt.Cells[4].Value = items["userid2"].ToString();
                       dt.Cells[5].Value = items["userid2selectedcards"].ToString();
                       dt.Cells[6].Value = items["userid2rightcards"].ToString();
                        dt.Cells[7].Value = items["address"].ToString();
                        dtvGame.Rows.Add(dt);
                        Application.DoEvents();
                        // datagridviewPolhemus.Rows[aux].Cells["p1"].Value = item["p1"].ToString();                   
                    }

                    
                }



            }
            catch (Exception error)
            {
                MessageBox.Show("Error: " + error);
            }
            
        }

        private void FindPolhemusData(string type, int N, DateTime date)
        {
            try
            {
                //Get a Reference to the Client Object
               // var connectionString = "mongodb://134.190.154.139"; //hui
                var connectionString = "mongodb://" + Prj_GetDataDB.Properties.Settings.Default.ip;

                var clientDB = new MongoClient(connectionString);
                var serverDB = clientDB.GetServer();

                if (type == "all-data")
                {
                    var database = serverDB.GetDatabase("dataPolhemus");
                    var collection1 = database.GetCollection("sensor1");
                    var collection2 = database.GetCollection("sensor2");
                    var collection3 = database.GetCollection("sensor3");
                    var collection4 = database.GetCollection("sensor4");
                  
                    
                    //int size = (int)collection1.Count();
                    var cursor1 = collection1.FindAll();
                    var cursor2 = collection2.FindAll();
                    var cursor3 = collection3.FindAll();
                    var cursor4 = collection4.FindAll();

                    //I CAN USE FOR EVERY SEARCH
                    //datagridviewPolhemus.DataSource = cursor.ToList();                
                    int aux1 = 0;
                    int aux2 = 0;
                    int aux3 = 0;
                    int aux4 = 0;
                    //  var data = cursor.ToList();
                    // int count = data.Count;


                    //fill sensor1
                    foreach (var items in cursor1)
                    {
                        aux1++;
                        //datagridviewPolhemus.Rows.Add(item["p1"].ToString());
                        DataGridViewRow dt = new DataGridViewRow();
                        dt.CreateCells(dtvSensor1);
                        dt.Cells[0].Value = aux1.ToString();
                        dt.Cells[1].Value = items["date"].ToString();
                        dt.Cells[2].Value = items["time"].ToString();
                        dt.Cells[3].Value = items["p1"].ToString();
                        dt.Cells[4].Value = items["p2"].ToString();
                        dt.Cells[5].Value = items["p3"].ToString();
                        dt.Cells[6].Value = items["x"].ToString();
                        dt.Cells[7].Value = items["y"].ToString();
                        dt.Cells[8].Value = items["z"].ToString();
                        dt.Cells[9].Value = items["q0"].ToString();
                        dt.Cells[10].Value = items["q1"].ToString();
                        dt.Cells[11].Value = items["q2"].ToString();
                        dt.Cells[12].Value = items["q3"].ToString();
                        dtvSensor1.Rows.Add(dt);
                        //current++;
                        //pbPolhemusData.Value = current / count;
                        Application.DoEvents();
                        // datagridviewPolhemus.Rows[aux].Cells["p1"].Value = item["p1"].ToString();                   
                    }


                    //fill sensor2
                    foreach (var items in cursor2)
                    {
                        aux2++;
                        //datagridviewPolhemus.Rows.Add(item["p1"].ToString());
                        DataGridViewRow dt = new DataGridViewRow();
                        dt.CreateCells(dtvSensor2);
                        dt.Cells[0].Value = aux2.ToString();
                        dt.Cells[1].Value = items["date"].ToString();
                        dt.Cells[2].Value = items["time"].ToString();
                        dt.Cells[3].Value = items["p1"].ToString();
                        dt.Cells[4].Value = items["p2"].ToString();
                        dt.Cells[5].Value = items["p3"].ToString();
                        dt.Cells[6].Value = items["x"].ToString();
                        dt.Cells[7].Value = items["y"].ToString();
                        dt.Cells[8].Value = items["z"].ToString();
                        dt.Cells[9].Value = items["q0"].ToString();
                        dt.Cells[10].Value = items["q1"].ToString();
                        dt.Cells[11].Value = items["q2"].ToString();
                        dt.Cells[12].Value = items["q3"].ToString();
                        dtvSensor2.Rows.Add(dt);
                        //current++;
                        //pbPolhemusData.Value = current / count;
                        Application.DoEvents();
                        // datagridviewPolhemus.Rows[aux].Cells["p1"].Value = item["p1"].ToString();                   
                    }



                    //fill sensor3
                    foreach (var items in cursor3)
                    {
                        aux3++;
                        //datagridviewPolhemus.Rows.Add(item["p1"].ToString());
                        DataGridViewRow dt = new DataGridViewRow();
                        dt.CreateCells(dtvSensor3);
                        dt.Cells[0].Value = aux3.ToString();
                        dt.Cells[1].Value = items["date"].ToString();
                        dt.Cells[2].Value = items["time"].ToString();
                        dt.Cells[3].Value = items["p1"].ToString();
                        dt.Cells[4].Value = items["p2"].ToString();
                        dt.Cells[5].Value = items["p3"].ToString();
                        dt.Cells[6].Value = items["x"].ToString();
                        dt.Cells[7].Value = items["y"].ToString();
                        dt.Cells[8].Value = items["z"].ToString();
                        dt.Cells[9].Value = items["q0"].ToString();
                        dt.Cells[10].Value = items["q1"].ToString();
                        dt.Cells[11].Value = items["q2"].ToString();
                        dt.Cells[12].Value = items["q3"].ToString();
                        dtvSensor3.Rows.Add(dt);
                        //current++;
                        //pbPolhemusData.Value = current / count;
                        Application.DoEvents();
                        // datagridviewPolhemus.Rows[aux].Cells["p1"].Value = item["p1"].ToString();                   
                    }                    

                    //fill sensor4
                    foreach (var items in cursor4)
                    {
                        aux4++;
                        //datagridviewPolhemus.Rows.Add(item["p1"].ToString());
                        DataGridViewRow dt = new DataGridViewRow();
                        dt.CreateCells(dtvSensor4);
                        dt.Cells[0].Value = aux4.ToString();
                        dt.Cells[1].Value = items["date"].ToString();
                        dt.Cells[2].Value = items["time"].ToString();
                        dt.Cells[3].Value = items["p1"].ToString();
                        dt.Cells[4].Value = items["p2"].ToString();
                        dt.Cells[5].Value = items["p3"].ToString();
                        dt.Cells[6].Value = items["x"].ToString();
                        dt.Cells[7].Value = items["y"].ToString();
                        dt.Cells[8].Value = items["z"].ToString();
                        dt.Cells[9].Value = items["q0"].ToString();
                        dt.Cells[10].Value = items["q1"].ToString();
                        dt.Cells[11].Value = items["q2"].ToString();
                        dt.Cells[12].Value = items["q3"].ToString();
                        dtvSensor4.Rows.Add(dt);
                      
                        Application.DoEvents();
                        // datagridviewPolhemus.Rows[aux].Cells["p1"].Value = item["p1"].ToString();                   
                    }
                    

                }
                if (type == "x-frames")
                {
                    var database = serverDB.GetDatabase("dataPolhemus");
                    var collection1 = database.GetCollection("sensor1");
                    var collection2 = database.GetCollection("sensor2");
                    var collection3 = database.GetCollection("sensor3");
                    var collection4 = database.GetCollection("sensor4");

                    int size1 = (int)collection1.Count();
                    int size2 = (int)collection2.Count();
                    int size3 = (int)collection3.Count();
                    int size4 = (int)collection4.Count();

                    var cursor1 = collection1.FindAll().Skip(size1 - N);
                    var cursor2 = collection2.FindAll().Skip(size2 - N);
                    var cursor3 = collection3.FindAll().Skip(size3 - N);
                    var cursor4 = collection4.FindAll().Skip(size4 - N);

                    //I CAN USE FOR EVERY SEARCH
                    //datagridviewPolhemus.DataSource = cursor.ToList();                
                    int aux1 = 0;
                    int aux2 = 0;
                    int aux3 = 0;
                    int aux4 = 0;
                    //  var data = cursor.ToList();
                    //  int count = data.Count;

                    //fill sensor1
                    foreach (var items in cursor1)
                    {
                        aux1++;
                        //datagridviewPolhemus.Rows.Add(item["p1"].ToString());
                        DataGridViewRow dt = new DataGridViewRow();
                        dt.CreateCells(dtvSensor1);
                        dt.Cells[0].Value = aux1.ToString();
                        dt.Cells[1].Value = items["date"].ToString();
                        dt.Cells[2].Value = items["time"].ToString();
                        dt.Cells[3].Value = items["p1"].ToString();
                        dt.Cells[4].Value = items["p2"].ToString();
                        dt.Cells[5].Value = items["p3"].ToString();
                        dt.Cells[6].Value = items["x"].ToString();
                        dt.Cells[7].Value = items["y"].ToString();
                        dt.Cells[8].Value = items["z"].ToString();
                        dt.Cells[9].Value = items["q0"].ToString();
                        dt.Cells[10].Value = items["q1"].ToString();
                        dt.Cells[11].Value = items["q2"].ToString();
                        dt.Cells[12].Value = items["q3"].ToString();
                        dtvSensor1.Rows.Add(dt);
                        //current++;
                        //pbPolhemusData.Value = current / count;
                        Application.DoEvents();
                        // datagridviewPolhemus.Rows[aux].Cells["p1"].Value = item["p1"].ToString();                   
                    }


                    //fill sensor2
                    foreach (var items in cursor2)
                    {
                        aux2++;
                        //datagridviewPolhemus.Rows.Add(item["p1"].ToString());
                        DataGridViewRow dt = new DataGridViewRow();
                        dt.CreateCells(dtvSensor2);
                        dt.Cells[0].Value = aux2.ToString();
                        dt.Cells[1].Value = items["date"].ToString();
                        dt.Cells[2].Value = items["time"].ToString();
                        dt.Cells[3].Value = items["p1"].ToString();
                        dt.Cells[4].Value = items["p2"].ToString();
                        dt.Cells[5].Value = items["p3"].ToString();
                        dt.Cells[6].Value = items["x"].ToString();
                        dt.Cells[7].Value = items["y"].ToString();
                        dt.Cells[8].Value = items["z"].ToString();
                        dt.Cells[9].Value = items["q0"].ToString();
                        dt.Cells[10].Value = items["q1"].ToString();
                        dt.Cells[11].Value = items["q2"].ToString();
                        dt.Cells[12].Value = items["q3"].ToString();
                        dtvSensor2.Rows.Add(dt);
                        //current++;
                        //pbPolhemusData.Value = current / count;
                        Application.DoEvents();
                        // datagridviewPolhemus.Rows[aux].Cells["p1"].Value = item["p1"].ToString();                   
                    }



                    //fill sensor3
                    foreach (var items in cursor3)
                    {
                        aux3++;
                        //datagridviewPolhemus.Rows.Add(item["p1"].ToString());
                        DataGridViewRow dt = new DataGridViewRow();
                        dt.CreateCells(dtvSensor3);
                        dt.Cells[0].Value = aux3.ToString();
                        dt.Cells[1].Value = items["date"].ToString();
                        dt.Cells[2].Value = items["time"].ToString();
                        dt.Cells[3].Value = items["p1"].ToString();
                        dt.Cells[4].Value = items["p2"].ToString();
                        dt.Cells[5].Value = items["p3"].ToString();
                        dt.Cells[6].Value = items["x"].ToString();
                        dt.Cells[7].Value = items["y"].ToString();
                        dt.Cells[8].Value = items["z"].ToString();
                        dt.Cells[9].Value = items["q0"].ToString();
                        dt.Cells[10].Value = items["q1"].ToString();
                        dt.Cells[11].Value = items["q2"].ToString();
                        dt.Cells[12].Value = items["q3"].ToString();
                        dtvSensor3.Rows.Add(dt);
                        //current++;
                        //pbPolhemusData.Value = current / count;
                        Application.DoEvents();
                        // datagridviewPolhemus.Rows[aux].Cells["p1"].Value = item["p1"].ToString();                   
                    }

                    //fill sensor4
                    foreach (var items in cursor4)
                    {
                        aux4++;
                        //datagridviewPolhemus.Rows.Add(item["p1"].ToString());
                        DataGridViewRow dt = new DataGridViewRow();
                        dt.CreateCells(dtvSensor4);
                        dt.Cells[0].Value = aux4.ToString();
                        dt.Cells[1].Value = items["date"].ToString();
                        dt.Cells[2].Value = items["time"].ToString();
                        dt.Cells[3].Value = items["p1"].ToString();
                        dt.Cells[4].Value = items["p2"].ToString();
                        dt.Cells[5].Value = items["p3"].ToString();
                        dt.Cells[6].Value = items["x"].ToString();
                        dt.Cells[7].Value = items["y"].ToString();
                        dt.Cells[8].Value = items["z"].ToString();
                        dt.Cells[9].Value = items["q0"].ToString();
                        dt.Cells[10].Value = items["q1"].ToString();
                        dt.Cells[11].Value = items["q2"].ToString();
                        dt.Cells[12].Value = items["q3"].ToString();
                        dtvSensor4.Rows.Add(dt);

                        Application.DoEvents();
                        // datagridviewPolhemus.Rows[aux].Cells["p1"].Value = item["p1"].ToString();                   
                    }
                }

                if (type == "datetime")
                {
                    string datetime = date.Month.ToString() + "/" + date.Day.ToString() + "/" + date.Year.ToString();
                    var database = serverDB.GetDatabase("dataPolhemus");

                    var collection1 = database.GetCollection("sensor1");
                    var collection2 = database.GetCollection("sensor2");
                    var collection3 = database.GetCollection("sensor3");
                    var collection4 = database.GetCollection("sensor4");
                                        
                    
                    //int size = (int)collection.Count();
                    var cursor1 = collection1.Find(Query.EQ("date", datetime));
                    var cursor2 = collection2.Find(Query.EQ("date", datetime));
                    var cursor3 = collection3.Find(Query.EQ("date", datetime));
                    var cursor4 = collection4.Find(Query.EQ("date", datetime));


                    //I CAN USE FOR EVERY SEARCH
                    //datagridviewPolhemus.DataSource = cursor.ToList();                
                    int aux1 = 0;
                    int aux2 = 0;
                    int aux3 = 0;
                    int aux4 = 0;
                    //    var data = cursor.ToList();
                    //    int count = data.Count;

                    //fill sensor1
                    foreach (var items in cursor1)
                    {
                        aux1++;
                        //datagridviewPolhemus.Rows.Add(item["p1"].ToString());
                        DataGridViewRow dt = new DataGridViewRow();
                        dt.CreateCells(dtvSensor1);
                        dt.Cells[0].Value = aux1.ToString();
                        dt.Cells[1].Value = items["date"].ToString();
                        dt.Cells[2].Value = items["time"].ToString();
                        dt.Cells[3].Value = items["p1"].ToString();
                        dt.Cells[4].Value = items["p2"].ToString();
                        dt.Cells[5].Value = items["p3"].ToString();
                        dt.Cells[6].Value = items["x"].ToString();
                        dt.Cells[7].Value = items["y"].ToString();
                        dt.Cells[8].Value = items["z"].ToString();
                        dt.Cells[9].Value = items["q0"].ToString();
                        dt.Cells[10].Value = items["q1"].ToString();
                        dt.Cells[11].Value = items["q2"].ToString();
                        dt.Cells[12].Value = items["q3"].ToString();
                        dtvSensor1.Rows.Add(dt);
                        //current++;
                        //pbPolhemusData.Value = current / count;
                        Application.DoEvents();
                        // datagridviewPolhemus.Rows[aux].Cells["p1"].Value = item["p1"].ToString();                   
                    }

                  
                    //fill sensor2
                    foreach (var items in cursor2)
                    {
                        aux2++;
                        //datagridviewPolhemus.Rows.Add(item["p1"].ToString());
                        DataGridViewRow dt = new DataGridViewRow();
                        dt.CreateCells(dtvSensor2);
                        dt.Cells[0].Value = aux2.ToString();
                        dt.Cells[1].Value = items["date"].ToString();
                        dt.Cells[2].Value = items["time"].ToString();
                        dt.Cells[3].Value = items["p1"].ToString();
                        dt.Cells[4].Value = items["p2"].ToString();
                        dt.Cells[5].Value = items["p3"].ToString();
                        dt.Cells[6].Value = items["x"].ToString();
                        dt.Cells[7].Value = items["y"].ToString();
                        dt.Cells[8].Value = items["z"].ToString();
                        dt.Cells[9].Value = items["q0"].ToString();
                        dt.Cells[10].Value = items["q1"].ToString();
                        dt.Cells[11].Value = items["q2"].ToString();
                        dt.Cells[12].Value = items["q3"].ToString();
                        dtvSensor2.Rows.Add(dt);
                        //current++;
                        //pbPolhemusData.Value = current / count;
                        Application.DoEvents();
                        // datagridviewPolhemus.Rows[aux].Cells["p1"].Value = item["p1"].ToString();                   
                    }

                  

                    //fill sensor3
                    foreach (var items in cursor3)
                    {
                        aux3++;
                        //datagridviewPolhemus.Rows.Add(item["p1"].ToString());
                        DataGridViewRow dt = new DataGridViewRow();
                        dt.CreateCells(dtvSensor3);
                        dt.Cells[0].Value = aux3.ToString();
                        dt.Cells[1].Value = items["date"].ToString();
                        dt.Cells[2].Value = items["time"].ToString();
                        dt.Cells[3].Value = items["p1"].ToString();
                        dt.Cells[4].Value = items["p2"].ToString();
                        dt.Cells[5].Value = items["p3"].ToString();
                        dt.Cells[6].Value = items["x"].ToString();
                        dt.Cells[7].Value = items["y"].ToString();
                        dt.Cells[8].Value = items["z"].ToString();
                        dt.Cells[9].Value = items["q0"].ToString();
                        dt.Cells[10].Value = items["q1"].ToString();
                        dt.Cells[11].Value = items["q2"].ToString();
                        dt.Cells[12].Value = items["q3"].ToString();
                        dtvSensor3.Rows.Add(dt);
                        //current++;
                        //pbPolhemusData.Value = current / count;
                        Application.DoEvents();
                        // datagridviewPolhemus.Rows[aux].Cells["p1"].Value = item["p1"].ToString();                   
                    }
                                       
                    //fill sensor4
                    foreach (var items in cursor4)
                    {
                        aux4++;
                        //datagridviewPolhemus.Rows.Add(item["p1"].ToString());
                        DataGridViewRow dt = new DataGridViewRow();
                        dt.CreateCells(dtvSensor4);
                        dt.Cells[0].Value = aux4.ToString();
                        dt.Cells[1].Value = items["date"].ToString();
                        dt.Cells[2].Value = items["time"].ToString();
                        dt.Cells[3].Value = items["p1"].ToString();
                        dt.Cells[4].Value = items["p2"].ToString();
                        dt.Cells[5].Value = items["p3"].ToString();
                        dt.Cells[6].Value = items["x"].ToString();
                        dt.Cells[7].Value = items["y"].ToString();
                        dt.Cells[8].Value = items["z"].ToString();
                        dt.Cells[9].Value = items["q0"].ToString();
                        dt.Cells[10].Value = items["q1"].ToString();
                        dt.Cells[11].Value = items["q2"].ToString();
                        dt.Cells[12].Value = items["q3"].ToString();
                        dtvSensor4.Rows.Add(dt);

                        Application.DoEvents();
                        // datagridviewPolhemus.Rows[aux].Cells["p1"].Value = item["p1"].ToString();                   
                    }
                }


                if (type=="dateXframes")
                {
                     string datetime = date.Month.ToString() + "/" + date.Day.ToString() + "/" + date.Year.ToString();
                    var database = serverDB.GetDatabase("DB_Polhemus");

                    var collection1 = database.GetCollection("sensor1");
                    var collection2 = database.GetCollection("sensor2");
                    var collection3 = database.GetCollection("sensor3");
                    var collection4 = database.GetCollection("sensor4");

                    int size1 = (int)collection1.Count();
                    int size2 = (int)collection2.Count();
                    int size3 = (int)collection3.Count();
                    int size4 = (int)collection4.Count();
                                   
                    //int size = (int)collection.Count();
                    var cursor1 = collection1.Find(Query.EQ("date", datetime)).Skip(size1 - N);
                    var cursor2 = collection2.Find(Query.EQ("date", datetime)).Skip(size2 - N);
                    var cursor3 = collection3.Find(Query.EQ("date", datetime)).Skip(size3 - N);
                    var cursor4 = collection4.Find(Query.EQ("date", datetime)).Skip(size4 - N);


                    //I CAN USE FOR EVERY SEARCH
                    //datagridviewPolhemus.DataSource = cursor.ToList();                
                    int aux1 = 0;
                    int aux2 = 0;
                    int aux3 = 0;
                    int aux4 = 0;


                    //fill sensor1
                    foreach (var items in cursor1)
                    {
                        aux1++;
                        //datagridviewPolhemus.Rows.Add(item["p1"].ToString());
                        DataGridViewRow dt = new DataGridViewRow();
                        dt.CreateCells(dtvSensor1);
                        dt.Cells[0].Value = aux1.ToString();
                        dt.Cells[1].Value = items["date"].ToString();
                        dt.Cells[2].Value = items["time"].ToString();
                        dt.Cells[3].Value = items["p1"].ToString();
                        dt.Cells[4].Value = items["p2"].ToString();
                        dt.Cells[5].Value = items["p3"].ToString();
                        dt.Cells[6].Value = items["x"].ToString();
                        dt.Cells[7].Value = items["y"].ToString();
                        dt.Cells[8].Value = items["z"].ToString();
                        dt.Cells[9].Value = items["q0"].ToString();
                        dt.Cells[10].Value = items["q1"].ToString();
                        dt.Cells[11].Value = items["q2"].ToString();
                        dt.Cells[12].Value = items["q3"].ToString();
                        dtvSensor1.Rows.Add(dt);
                        //current++;
                        //pbPolhemusData.Value = current / count;
                        Application.DoEvents();
                        // datagridviewPolhemus.Rows[aux].Cells["p1"].Value = item["p1"].ToString();                   
                    }


                    //fill sensor2
                    foreach (var items in cursor2)
                    {
                        aux2++;
                        //datagridviewPolhemus.Rows.Add(item["p1"].ToString());
                        DataGridViewRow dt = new DataGridViewRow();
                        dt.CreateCells(dtvSensor2);
                        dt.Cells[0].Value = aux2.ToString();
                        dt.Cells[1].Value = items["date"].ToString();
                        dt.Cells[2].Value = items["time"].ToString();
                        dt.Cells[3].Value = items["p1"].ToString();
                        dt.Cells[4].Value = items["p2"].ToString();
                        dt.Cells[5].Value = items["p3"].ToString();
                        dt.Cells[6].Value = items["x"].ToString();
                        dt.Cells[7].Value = items["y"].ToString();
                        dt.Cells[8].Value = items["z"].ToString();
                        dt.Cells[9].Value = items["q0"].ToString();
                        dt.Cells[10].Value = items["q1"].ToString();
                        dt.Cells[11].Value = items["q2"].ToString();
                        dt.Cells[12].Value = items["q3"].ToString();
                        dtvSensor2.Rows.Add(dt);
                        //current++;
                        //pbPolhemusData.Value = current / count;
                        Application.DoEvents();
                        // datagridviewPolhemus.Rows[aux].Cells["p1"].Value = item["p1"].ToString();                   
                    }



                    //fill sensor3
                    foreach (var items in cursor3)
                    {
                        aux3++;
                        //datagridviewPolhemus.Rows.Add(item["p1"].ToString());
                        DataGridViewRow dt = new DataGridViewRow();
                        dt.CreateCells(dtvSensor3);
                        dt.Cells[0].Value = aux3.ToString();
                        dt.Cells[1].Value = items["date"].ToString();
                        dt.Cells[2].Value = items["time"].ToString();
                        dt.Cells[3].Value = items["p1"].ToString();
                        dt.Cells[4].Value = items["p2"].ToString();
                        dt.Cells[5].Value = items["p3"].ToString();
                        dt.Cells[6].Value = items["x"].ToString();
                        dt.Cells[7].Value = items["y"].ToString();
                        dt.Cells[8].Value = items["z"].ToString();
                        dt.Cells[9].Value = items["q0"].ToString();
                        dt.Cells[10].Value = items["q1"].ToString();
                        dt.Cells[11].Value = items["q2"].ToString();
                        dt.Cells[12].Value = items["q3"].ToString();
                        dtvSensor3.Rows.Add(dt);
                        //current++;
                        //pbPolhemusData.Value = current / count;
                        Application.DoEvents();
                        // datagridviewPolhemus.Rows[aux].Cells["p1"].Value = item["p1"].ToString();                   
                    }

                    //fill sensor4
                    foreach (var items in cursor4)
                    {
                        aux4++;
                        //datagridviewPolhemus.Rows.Add(item["p1"].ToString());
                        DataGridViewRow dt = new DataGridViewRow();
                        dt.CreateCells(dtvSensor4);
                        dt.Cells[0].Value = aux4.ToString();
                        dt.Cells[1].Value = items["date"].ToString();
                        dt.Cells[2].Value = items["time"].ToString();
                        dt.Cells[3].Value = items["p1"].ToString();
                        dt.Cells[4].Value = items["p2"].ToString();
                        dt.Cells[5].Value = items["p3"].ToString();
                        dt.Cells[6].Value = items["x"].ToString();
                        dt.Cells[7].Value = items["y"].ToString();
                        dt.Cells[8].Value = items["z"].ToString();
                        dt.Cells[9].Value = items["q0"].ToString();
                        dt.Cells[10].Value = items["q1"].ToString();
                        dt.Cells[11].Value = items["q2"].ToString();
                        dt.Cells[12].Value = items["q3"].ToString();
                        dtvSensor4.Rows.Add(dt);

                        Application.DoEvents();
                        // datagridviewPolhemus.Rows[aux].Cells["p1"].Value = item["p1"].ToString();                   
                    }
                    
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void cleanDgvPolhemus(){

            dtvSensor1.Rows.Clear();
            dtvSensor2.Rows.Clear();
            dtvSensor3.Rows.Clear();
            dtvSensor4.Rows.Clear();
        
        }

        private void cleanUserApplication() {
            dtvGame.Rows.Clear();
            dtvNotification.Rows.Clear();
            dtvHeatMap.Rows.Clear();
            dtvPrivacyControl.Rows.Clear();
            dtvControls.Rows.Clear();
        }

        private void btnReadPolhemusData_Click(object sender, EventArgs e)
        {
            try
            {
                if (rbAllData.Checked == true)
                {
                   //dtvSensor1.DataSource = "";   
                    cleanDgvPolhemus();

                    
                    FindPolhemusData("all-data", 0, DateTime.Now);
                }

                if (rbXFrames.Checked == true)
                {
                   // dtvSensor1.DataSource = "";
                    cleanDgvPolhemus();
                    FindPolhemusData("x-frames", Convert.ToInt16(txbX.Text), DateTime.Now);
                }

                if (rbDate.Checked == true)
                {
                    //clear datagridview
                    cleanDgvPolhemus();
                    FindPolhemusData("datetime", 0, dtpDataTime.Value);
                }

                if (rbDateXframes.Checked ==true)
                {
                    // dtvSensor1.DataSource = "";
                    cleanDgvPolhemus();
                    FindPolhemusData("dateXframes", Convert.ToInt16(txbX.Text), dtpDataTime.Value);
                }


                //MessageBox.Show(list);
                //Query to get data from database in c://data                
            }
            catch (Exception error)
            {
                MessageBox.Show("Error: " + error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dtvPolhemusData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void datagridviewPolhemus_DataSourceChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void datagridviewPolhemus_DataMemberChanged(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {


                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Excel Documents (*.xls)|*.xls";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    // ToCsV(dataGridView1, @"c:\temp\export.xls");   ???????
                    ToCsV(dtvSensor1, sfd.FileName);
                    MessageBox.Show("Successfully Created " + sfd.FileName);
                }

            }
            catch (Exception error)
            {
                MessageBox.Show("Error: " + error);
            }
        }

        private void ToCsV(DataGridView dGV, string filename)
        {
            try
            {
                string stOutput = "";
                // Export titles:
                string sHeaders = "";

                for (int j = 0; j < dGV.Columns.Count; j++)
                    sHeaders = sHeaders.ToString() + Convert.ToString(dGV.Columns[j].HeaderText) + "\t";
                stOutput += sHeaders + "\n";
                // Export data.
                for (int i = 0; i < dGV.RowCount - 1; i++)
                {
                    string stLine = "";
                    for (int j = 0; j < dGV.Rows[i].Cells.Count; j++)
                        stLine = stLine.ToString() + "'" + Convert.ToString(dGV.Rows[i].Cells[j].Value) + "\t";
                    stOutput += stLine + "\n";
                }
                Encoding utf16 = Encoding.GetEncoding(1254);
                byte[] output = utf16.GetBytes(stOutput);
                FileStream fs = new FileStream(filename, FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);
                bw.Write(output, 0, output.Length); //write the encoded file
                bw.Flush();
                bw.Close();
                fs.Close();

            }
            catch (Exception error)
            {
                MessageBox.Show("Error meanwhile convert to excel: " + error);
            }

        }

        private void rbXFrames_CheckedChanged(object sender, EventArgs e)
        {
            if (rbXFrames.Checked == true)
            {
                txbX.Enabled = true;
                dtpDataTime.Enabled = false;
            }
        }

        private void rbAllData_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAllData.Checked == true && rbXFrames.Checked == false && rbDate.Checked == false)
            {
                txbX.Enabled = false;
                dtpDataTime.Enabled = false;
            }
        }

        private void rbDate_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDate.Checked == true)
            {
                dtpDataTime.Enabled = true;
                txbX.Enabled = false;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Excel Documents (*.xls)|*.xls";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    // ToCsV(dataGridView1, @"c:\temp\export.xls");   ???????
                    ToCsV(dtvHeatMap, sfd.FileName);
                    MessageBox.Show("Successfully Created " + sfd.FileName);
                }

            }
            catch (Exception error)
            {
                MessageBox.Show("Error: " + error);
            }
            

        }

        private void btnExportControls_Click(object sender, EventArgs e)
        {
            try
            {


                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Excel Documents (*.xls)|*.xls";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    // ToCsV(dataGridView1, @"c:\temp\export.xls");   ???????
                    ToCsV(dtvControls, sfd.FileName);
                    MessageBox.Show("Successfully Created " + sfd.FileName);
                }

            }
            catch (Exception error)
            {
                MessageBox.Show("Error: " + error);
            }
        }

        private void btnExportNotification_Click(object sender, EventArgs e)
        {
            try
            {


                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Excel Documents (*.xls)|*.xls";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    // ToCsV(dataGridView1, @"c:\temp\export.xls");   ???????
                    ToCsV(dtvNotification, sfd.FileName);
                    MessageBox.Show("Successfully Created " + sfd.FileName);
                }

            }
            catch (Exception error)
            {
                MessageBox.Show("Error: " + error);
            }
        }

        private void btnExportPrivacyControl_Click(object sender, EventArgs e)
        {
            try
            {


                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Excel Documents (*.xls)|*.xls";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    // ToCsV(dataGridView1, @"c:\temp\export.xls");   ???????
                    ToCsV(dtvPrivacyControl, sfd.FileName);
                    MessageBox.Show("Successfully Created " + sfd.FileName);
                }

            }
            catch (Exception error)
            {
                MessageBox.Show("Error: " + error);
            }
        }

        private void btnExportGame_Click(object sender, EventArgs e)
        {
            try
            {


                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Excel Documents (*.xls)|*.xls";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    // ToCsV(dataGridView1, @"c:\temp\export.xls");   ???????
                    ToCsV(dtvGame, sfd.FileName);
                    MessageBox.Show("Successfully Created " + sfd.FileName);
                }

            }
            catch (Exception error)
            {
                MessageBox.Show("Error: " + error);
            }
        }

        private void btnReadApp_Click(object sender, EventArgs e)
        {
            try
            {
               
                    if (rbXFramesApp.Checked==true)
                    {
                        cleanUserApplication();
                        FindApplicationData("x-frames", Convert.ToInt16(txbXApp.Text));
                    }
                    if (rbAllDataApp.Checked==true)
                    {
                        cleanUserApplication();
                        FindApplicationData("all-data", 0);
                    }
               

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void rbAllDataApp_CheckedChanged(object sender, EventArgs e)
        {
            if (rbAllDataApp.Checked = true && rbXFramesApp.Checked == false)
            {
                txbXApp.Enabled = false;
            }
            else
                txbXApp.Enabled = true;
        }

        private void rbXFramesApp_CheckedChanged(object sender, EventArgs e)
        {
            if (rbXFramesApp.Checked == true && rbAllDataApp.Checked == false)
            {
                txbXApp.Enabled = true;
            }
            else
                txbXApp.Enabled = false;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDateXframes.Checked == true)
            {
                txbX.Enabled = true;
                dtpDataTime.Enabled = true;
            }
            else
            {
                txbX.Enabled = false;
                dtpDataTime.Enabled = false;
            }
        }

        private void btnDeleteDB_Click(object sender, EventArgs e)
        {
            try
            {
                //dataPolhemus
                DialogResult result = MessageBox.Show("Do you really want to delete Polhemus Data?", "Warning",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    var connectionString = "mongodb://" + Prj_GetDataDB.Properties.Settings.Default.ip;
                    var clientDB = new MongoClient(connectionString);
                    var serverDB = clientDB.GetServer();
                    var database = serverDB.GetDatabase("dataPolhemus");

                    database.Drop();
                    cleanDgvPolhemus();
                }
                else if (result == DialogResult.No)
                {
                    //code for No
                }
                else if (result == DialogResult.Cancel)
                {
                    //code for Cancel
                }              
               


            }
            catch (Exception error)
            {
                MessageBox.Show("Error" + error);
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            try
            {


                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Excel Documents (*.xls)|*.xls";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    // ToCsV(dataGridView1, @"c:\temp\export.xls");   ???????
                    ToCsV(dtvSensor2, sfd.FileName);
                    MessageBox.Show("Successfully Created " + sfd.FileName);
                }

            }
            catch (Exception error)
            {
                MessageBox.Show("Error: " + error);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {


                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Excel Documents (*.xls)|*.xls";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    // ToCsV(dataGridView1, @"c:\temp\export.xls");   ???????
                    ToCsV(dtvSensor3, sfd.FileName);
                    MessageBox.Show("Successfully Created " + sfd.FileName);
                }

            }
            catch (Exception error)
            {
                MessageBox.Show("Error: " + error);
            }
        }

        private void button6_Click_1(object sender, EventArgs e)
        {
            try
            {


                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Excel Documents (*.xls)|*.xls";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    // ToCsV(dataGridView1, @"c:\temp\export.xls");   ???????
                    ToCsV(dtvSensor4, sfd.FileName);
                    MessageBox.Show("Successfully Created " + sfd.FileName);
                }

            }
            catch (Exception error)
            {
                MessageBox.Show("Error: " + error);
            }
        }

        private void btnDeleteDataApp_Click(object sender, EventArgs e)
        {
            try
            {
                //dataPolhemus
                DialogResult result = MessageBox.Show("Do you really want to delete Polhemus Data?", "Warning",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    var connectionString = "mongodb://" + Prj_GetDataDB.Properties.Settings.Default.ip;
                    var clientDB = new MongoClient(connectionString);
                    var serverDB = clientDB.GetServer();
                    var database = serverDB.GetDatabase("userApplication");

                    database.Drop();
                    cleanUserApplication();
                }
                else if (result == DialogResult.No)
                {
                    //code for No
                }
                else if (result == DialogResult.Cancel)
                {
                    //code for Cancel
                }



            }
            catch (Exception error)
            {
                MessageBox.Show("Error" + error);
            }
        }
    }
}
