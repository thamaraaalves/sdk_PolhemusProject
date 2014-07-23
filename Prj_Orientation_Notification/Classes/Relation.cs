using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using ListenPortG4;
using WebSocketSharp;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.GridFS;
using MongoDB.Driver.Linq;
using Prj_Orientation_Notification.Classes;
using Prj_Orientation_Notification;
using ListenApplication;


namespace Prj_Orientation_Notification.Classes
{
    public class Map
    {
        public string type { get; set; }
        public string top { get; set; }
        public string left { get; set; }
        public string angle { get; set; }     
    }

    public class Not {
        public string type { get; set; }
        public double level { get; set; }     
    }
   
     public class PControl {
         public string type { get; set; }    //privacy-control-0       
        public double status { get; set; }   
    }

    public class RootObject
    {
        public List<Map> itemsMap { get; set; }
        public List<Not> itemsNot { get; set; }
        public List<PControl> itemsPControl { get; set; }
    }

    //calculate the directional relationship between vectors
    public class NotificationEventArgs
    {
        //create variables 
        private Vector3 p1 = new Vector3();  
        private Vector3 p2 = new Vector3();  

        private Quaternion q1 = new Quaternion();
        private Quaternion q2 = new Quaternion();

        private Vector3 lookDirection = new Vector3();
        private Angle angleab = new Angle(); //the angle from Presence A to Presence B. angle between a's looking direction and the ab 
        private Angle angle = new Angle(); //absolute angle between A and B
        private bool atowardsb = false;  //whether entity A is facing B
        private bool afromb = false;
        private bool parallel = false;
        private bool isClose = false;
        private bool isViewing = false;
        private double distance = 0;
      

        // The threshold angle for two direction vectors to be considered within field of view
        public static Angle ViewThreshold = new Angle(60.0, AngleUnit.Degrees);
        //The threshold angle for two direction vectors to be considered relatively parallel.
        public static Angle ParallelThreshold = new Angle(30.0, AngleUnit.Degrees);
        public double UpperDistanceThreshold = 200; //the threshold for upper limit of notification
        public double MiddleDistanceThreshold = 150;  //the threshold for middle level of distance notification
        public double LowerDistanceThreshold = 90;  //the threshold for middle level of distance notification
        

        public NotificationEventArgs(Vector3 p1, Vector3 p2, Quaternion q1, Quaternion q2)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.q1 = q1;
            this.q2 = q2;
            Vector3 directAB = (p2 - p1).Normalized;
            lookDirection = Quaternion.RotatedVector(p1, q1);
            distance = Vector3.Distance(p1,p2);
            angleab = directAB.AnglePS(lookDirection);
            angle = lookDirection.AnglePS(Quaternion.RotatedVector(p2, q2));
            atowardsb = angleab <= ParallelThreshold; 
            afromb = angleab >= Angle.Angle180Degrees - ParallelThreshold;
            parallel = angle <= ParallelThreshold || angle >= Angle.Angle180Degrees - ParallelThreshold;
            isClose = distance <= UpperDistanceThreshold;
            isViewing = angleab <= ViewThreshold;
            
        }

        #region Properties
        /// <summary>
        /// True if Presence A is facing toward Presence B.
        /// </summary>
        [Description("True if Presence A is facing toward Presence B.")]
        public bool ATowardsB
        {
            get
            {
                return atowardsb;
            }
        }

        /// True if Presence A is facing toward Presence B.
        /// </summary>
        [Description("True if Presence A is facing toward Presence B.")]
        public bool AFromB
        {
            get
            {
                return afromb;
            }
        }


        /// <summary>
        /// Returns the angle from Presence A to Presence B.
        /// </summary>
        [Description("Returns the angle from Presence A to Presence B.")]
        public Angle AngleAtoB
        {
            get
            {
                return angleab;
            }
        }

        /// <summary>
        /// Return the absolute angle between Presence A and B.
        /// </summary>
        [Description("Return the absolute angle between Presence A and B.")]
        public Angle Angle
        {
            get
            {
                return angle;
            }
        }

        /// <summary>
        /// True if Presence A and B are facing parallel to one another.
        /// </summary>
        [Description("True if Presence A and B are facing parallel to one another.")]
        public bool Parallel
        {
            get
            {
                return parallel;
            }
        }


        /// <summary>
        /// returns the distance between two points.
        /// </summary>
        [Description("True if Presence A and B are facing parallel to one another.")]
        public double Distance
        {
            get
            {
                return distance;
            }
        }

        /// <summary>
        /// True if the intruder cross the distance threshold for the tablet.
        /// </summary>
        [Description("True if the intruder is close enough to the tablet.")]
        public bool IsClose
        {
            get
            {
                return isClose;
            }
        }

        /// <summary>
        /// True if the intruder's looking angle cross the threshold relative to the tablet.
        /// </summary>
        [Description("True if the intruder is looking at the tablet.")]
        public bool IsViewing
        {
            get
            {
                return isViewing;
            }
        }


        #endregion

        #region Method

        //takes an notification type and call different notification methods
  /*      public void Notify(int notitype) {
            double left;
            double top;
            double angle;

            if (notitype == 0) //simple notification
                Noti_simple();
            else if (notitype == 1) //coarse notification
                Noti_coarse();
            else if (notitype == 2)
                Noti_fine();
            else if (notitype == 3)
                Noti_map();
        }*/


        //N1: Notification simple (on vs off). When the passerby comes close to the tablet and cross the threshold, the notification will be on.
        public void Noti_simple() {
            if (distance >= UpperDistanceThreshold){
                Console.WriteLine("Your notification is OFF now");

                //create message json format - 0 is off
                var objectToSerialize = new RootObject();
                objectToSerialize.itemsNot = new List<Not> 
                {
                    new Not {type="notification-1", level=0},                           
                };     

                //call function in class websocket
                //and send just the parameter            

                Prj_Orientation_Notification.Program.SendMessage(objectToSerialize.itemsNot[0].ToJson());
            }
               
            else if (distance < UpperDistanceThreshold){
                Console.WriteLine("Your notification is ON now"); //send message to vinicius 1 is on

                var objectToSerialize = new RootObject();
                objectToSerialize.itemsNot = new List<Not>{               
                    new Not {type="notification-1", level=1},};


                Prj_Orientation_Notification.Program.SendMessage(objectToSerialize.itemsNot[0].ToJson());
            }           
         
        }

        //N2: Notification coarse. When the passerby comes close to the tablet, green; when the passerby come close enough, but not looking at the tablet, yellow; look at screen, red.
        //or notification-2 and 3
        public void Noti_coarse()
        {
            if(distance>=UpperDistanceThreshold){
               Console.WriteLine("Your notification is OFF now"); //type notification-2 level 0
               var objectToSerialize = new RootObject();
               objectToSerialize.itemsNot = new List<Not>{               
                    new Not {type="notification-2", level=0},};
               Prj_Orientation_Notification.Program.SendMessage(objectToSerialize.itemsNot[0].ToJson());
            }
            else if(distance<UpperDistanceThreshold && distance>MiddleDistanceThreshold){
                Console.WriteLine("GREEN notification!"); //type notification-2 level 1
                var objectToSerialize = new RootObject();
                objectToSerialize.itemsNot = new List<Not>{               
                    new Not {type="notification-2", level=1},};
                Prj_Orientation_Notification.Program.SendMessage(objectToSerialize.itemsNot[0].ToJson());
            }
            else if(distance<=MiddleDistanceThreshold && angleab>=ViewThreshold){
               Console.WriteLine("YELLOW notification!!"); //type notification-2 level 2
               var objectToSerialize = new RootObject();
               objectToSerialize.itemsNot = new List<Not>{               
                    new Not {type="notification-2", level=2},};
               Prj_Orientation_Notification.Program.SendMessage(objectToSerialize.itemsNot[0].ToJson());
            }
            else if (distance <= MiddleDistanceThreshold && angleab < ViewThreshold) { 
               Console.WriteLine("RED notification!!!"); //type notification-2 level 3
               var objectToSerialize = new RootObject();
               objectToSerialize.itemsNot = new List<Not>{               
                    new Not {type="notification-2", level=3},};
               Prj_Orientation_Notification.Program.SendMessage(objectToSerialize.itemsNot[0].ToJson());
            }
        }

        //N3: Notification fine. As the passerby comes closer to the tablet, the notification becomes less transparent.
        public double Noti_fine() {

            const double OpacityA = -0.00556; //linear function argument for opacity
            const double OpacityB = 1.111;
            const double OpacityUpper = 1.0; //can see the notification best
            const double OpacityLower = 0;//can't see the notification

            double opacity = OpacityUpper; //initialization

            if (distance > UpperDistanceThreshold)
                return OpacityLower;
            else if (distance < LowerDistanceThreshold)
                return OpacityUpper;
            else
            {
                opacity = OpacityA * distance + OpacityB;
                opacity = Math.Round(opacity, 2);

                //send to app vinicius
                //notification-5 and level==opacity Not item
                Console.WriteLine("The opacity value for the current notification is: " + opacity);
                var objectToSerialize = new RootObject();
                int opacityI = Convert.ToInt16(opacity);

                objectToSerialize.itemsNot = new List<Not>{               
                    new Not {type="notification-5", level=opacity},};
                //send to application
                //var message = objectToSerialize.itemsNot.ToJson();

                 Prj_Orientation_Notification.Program.SendMessage(objectToSerialize.itemsNot[0].ToJson());
                
                return opacity;
            }
        }

        //N4: Notification map. When the passerby comes close enough, the radar map show the passerby’s position/looking direction relative to the tablet.
        public void Noti_map() {             
            const double topOffset = 37.5;
            const double leftOffset = 37.5;
            const double radius = 75;
             double top = 0;
             double left =0; 
             double angle =0;
            //calculate x, y coordinates for the map
            Vector3 RelativePoint = p1 - p2; //get the new coordinates of the passerby, tablet as the center
                      
            left = RelativePoint.X * radius / UpperDistanceThreshold + leftOffset;
            top = RelativePoint.Y * radius / UpperDistanceThreshold + topOffset;
            Console.WriteLine("The vector for relative point is: " + RelativePoint);
            Console.WriteLine("The adjusted left coordinate is " + left);
            Console.WriteLine("The adjusted top coordinate is " + top);
                        
            //calculate the looking angle for the map -------------------------------------------
            angle = 0;
            double radians = Math.Atan2(lookDirection.Y, lookDirection.X);
            angle = 180 - radians * (180 / Math.PI);
         
            Console.WriteLine("The radian is " + radians);
            Console.WriteLine("The degree is " + angle);

          //  int topI = Convert.ToInt16(top);
          //  int leftI = Convert.ToInt16(left);
          //  int angleI = Convert.ToInt16(angle);
            
            //create message in json format           
            //type notification-4
            var objectToSerialize = new RootObject();
            objectToSerialize.itemsMap = new List<Map>{
                new Map { type="notification-4", top = top.ToString(), left = left.ToString(), angle = angle.ToString() },};

            Prj_Orientation_Notification.Program.SendMessage(objectToSerialize.itemsMap[0].ToJson());
        }


        //PC model 1: Privacy control- Automatic. When the passerby comes close to the tablet, the contents on the tablet automatically change.
        public void PrivacyControl_auto() {             
            //call the appropriate privacy control mechanisms
        }

        //PC model 2: Privacy control- manual. Only when the tablet user take actions will the content on tablet changes
        public void PrivacyControl_manual()
        {  }        
        
        //PC2 Grayscale. As the passerby comes closer, the content change to grayscale
        public void PrivacyControl_grayscale() 
        { 
            const double GrayScaleA = -0.00556; //linear function argument for grayscale
            const double GrayScaleB = 1.111;
            const double GrayScaleUpper = 1.0;
            const double GrayScaleLower = 0;

            double gray = GrayScaleUpper; //initialization

            if (distance > UpperDistanceThreshold)
                gray = GrayScaleLower;
            else if (distance < LowerDistanceThreshold)
                gray = GrayScaleUpper;
            else
            {
                gray = GrayScaleA * distance + GrayScaleB;
                gray = Math.Round(gray, 2);

                //send to server app
                //type:privacyControl-2
                              
             }

        //    int grayI = Convert.ToInt16(gray);

            Console.WriteLine("The grayscale value for the current privacy control is: " + gray);
            var objectToSerialize = new RootObject();
            objectToSerialize.itemsPControl = new List<PControl>{               
                    new PControl {type="privacy-control-2", status=gray},};
            Prj_Orientation_Notification.Program.SendMessage(objectToSerialize.itemsPControl[0].ToJson());
        }
        
        //PC3 Brightness. As the passerby comes closer, the brightness of the content is turned down
        public void PrivacyControl_brightness()
        {
            const double BrightA = 0.005; //linear function argument for brightness
            const double BrightB = 0;
            const double BrightUpper = 1.0; //all the content is seen
            const double BrightLower = 0.1; //the user can barely see anything

            double bright = BrightUpper; //initialization

            if (distance > UpperDistanceThreshold){
                bright = BrightUpper;
            }
                
            else if (distance < LowerDistanceThreshold){
                bright = BrightLower;
            }
               
            else
            {
                bright = BrightA * distance + BrightB;
                bright = Math.Round(bright, 2);             
            }

           // int brightI = Convert.ToInt16(bright);

            Console.WriteLine("The brightness value for the current privacy control is: " + bright);
            var objectToSerialize = new RootObject();
            objectToSerialize.itemsPControl = new List<PControl>{               
                    new PControl {type="privacy-control-3", status=bright},};
            Prj_Orientation_Notification.Program.SendMessage(objectToSerialize.itemsPControl[0].ToJson());
 
        }

        //PC3 Selective hiding. As the passerby comes closer, hide all the infographs, images, anonymize name, address?????        
        public void PrivacyControl_Hidden()
        {
            if (distance >= UpperDistanceThreshold)
            {
                Console.WriteLine("Your notification is OFF now");
                //create message json format - 0 is off
                var objectToSerialize = new RootObject();
                objectToSerialize.itemsPControl = new List<PControl> 
                {
                    new PControl {type="privacy-control-1", status=0},};
                Prj_Orientation_Notification.Program.SendMessage(objectToSerialize.itemsPControl[0].ToJson());
            }

            else if (distance < UpperDistanceThreshold)
            {
                Console.WriteLine("Your notification is ON now"); //send message to vinicius 1 is on
                //create message json format - 0 is off              

                var objectToSerialize = new RootObject();
                objectToSerialize.itemsPControl = new List<PControl> 
                {
                    new PControl {type="privacy-control-1", status=1},};  
                Prj_Orientation_Notification.Program.SendMessage(objectToSerialize.itemsPControl[0].ToJson());
            }
        }

        //lantern
        public void PrivacyControl_Lantern()
        {
            if (distance >= UpperDistanceThreshold)
            {
                Console.WriteLine("Your notification is OFF now");
                //create message json format - 0 is off
                var objectToSerialize = new RootObject();
                objectToSerialize.itemsPControl = new List<PControl> 
                {
                    new PControl {type="privacy-control-4", status=0},};
                Prj_Orientation_Notification.Program.SendMessage(objectToSerialize.itemsPControl[0].ToJson());
            }

            else if (distance < UpperDistanceThreshold)
            {
                Console.WriteLine("Your notification is ON now"); //send message to vinicius 1 is on
                //create message json format - 0 is off
                var objectToSerialize = new RootObject();
                objectToSerialize.itemsPControl = new List<PControl> 
                {
                    new PControl {type="privacy-control-4", status=1},};
                Prj_Orientation_Notification.Program.SendMessage(objectToSerialize.itemsPControl[0].ToJson());
            }
        }       

        //PC4 Selective showing. User of the tablet can choose to see part of the screen
        #endregion
    }


   


}
