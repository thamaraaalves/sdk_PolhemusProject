using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using ListenPortG4;
//using Prj_Distance.Quaternion;

//old code

namespace Prj_Distance
{
    class Program
    {
       
        static void Main(string[] args)
        {            
            try
            {
                // test code
                double[] s1 = new double[11] {0,0,0,0, 100.0, 0, 0, 0.7071068, 0, 0.7071068, 0 };  //sensor 1--passerby
                double[] s2 = new double[11] {0,0,0,0, 200, 0, -100, 0, 0, 0, 0 }; // sensor 2 --tablet
                double[] s3 = new double[11] {0,0,0,0, 0, 0, 141.4, 1.0, 0, 0, 0 }; // sensor 3 -- user 1
                double[] s4 = new double[11] {0,0,0,0, 0, 0, 0, 1.0, 0, 0, 0 }; // sensor 4 -- user 2

                Vector3 p1 = new Vector3(s1[4],s1[5],s1[6]);
                Vector3 p2 = new Vector3(s2[4],s2[5],s2[6]);
                Vector3 p3 = new Vector3(s3[4],s3[5],s3[6]);
                Vector3 p4 = new Vector3(s4[4],s4[5],s4[6]);

                Quaternion q1 = new Quaternion(s1[7], s1[8], s1[9], s1[10]);
                Quaternion q2 = new Quaternion(s2[7], s2[8], s2[9], s2[10]);
                Quaternion q3 = new Quaternion(s3[7], s3[8], s3[9], s3[10]);
                Quaternion q4 = new Quaternion(s4[7], s4[8], s4[9], s4[10]);

                //Console.WriteLine("Rotated point:" + " " + Quaternion.Rotatedpoint(p1, q1));
                Console.WriteLine("Rotated vector:" + " " + Quaternion.RotatedVector(p1, q1));

                /*//test quaternion
                Console.WriteLine("position:" + " " + p1);
                Console.WriteLine("orientation:" + " " + q1);
                Console.WriteLine("conjugate:" + " " + Quaternion.Conjugate(q1));

                 */

                /*//distance
                double distance = Vector3.Distance(p1,p2);
                Console.WriteLine("The distance between the sensor 1 and 2 is:");
                Console.WriteLine();
                Console.WriteLine("Inches:" + " " + distance);
                Console.WriteLine();
                Console.WriteLine("Cm:" + " " + (double)(2.54 * distance));
                */
                

                /*//angle
                Angle an = Vector3.AnglePS(p1,p2);
                Console.WriteLine("The angle between the sensor 1 and 2 is:");
                Console.WriteLine("Degree:" + " " + an.Degrees);
                Console.WriteLine("Radian:" + " " + an.Radians);
                */
                                
                NotificationEventArgs passer_tablet = new NotificationEventArgs(p1, p2, q1, q2);
             
                Console.WriteLine("The angle between the tablet and intruder is:");
                Console.WriteLine("Degree:" + " " + passer_tablet.AngleAtoB.Degrees);
                Console.WriteLine("The distance between the tablet and intruder is:");
                Console.WriteLine("Distance:" + " " + passer_tablet.Distance);


                //Console.WriteLine("Radian:" + " " + ab.Radians);
               /* if(passer_tablet.IsClose)  //notification trigger
                    Console.WriteLine("Someone is looking at your screen!!!");*/

                   passer_tablet.Notify(3);

                //if the tablet is in intrusion mode && privacy control is in automatic model
                //call automatic privacy control, based on distance between intruder and tablet


                //pc is in manual model (people can turn on privacy whenever they want??)
                //call manual privacy control, based on user's behavior

               
                /*
                NotificationEventArgs tablet_user = new NotificationEventArgs(p2, p3, q2, q3);
                NotificationEventArgs tablet_user2 = new NotificationEventArgs(p2, p4, q2, q4);
                NotificationEventArgs user_user = new NotificationEventArgs(p3, p4, q3, q4);
                Console.WriteLine("The angle between the tablet and user is:");
                Console.WriteLine("Degree:" + " " + tablet_user.AngleAtoB.Degrees);

                Angle ViewThreshold = new Angle(60.0, AngleUnit.Degrees);
                double DistanceThreshold_Tablet = 30;
                double DistanceThreshold_User = 30;
                double DistanceThreshold_bothUser = 15;

                //trigger 1: when the user tilt the screen to a certain angle, the content on the tablet changes.
                if (tablet_user.AngleAtoB <= ViewThreshold) 
                    tablet_user.PrivacyControl_grayscale();//different, value changed with angle, recalculate coefficiency


                //trigger 2: User take the tablet closer to the body.
                if (tablet_user.Distance <= DistanceThreshold_Tablet)
                    tablet_user.PrivacyControl_grayscale(); //different, value changed with distance


                //trigger 3: Collaboration. When a user Lean closer to colleagues, the content on the tablet changes.
                if (user_user.Distance <= DistanceThreshold_User)
                    user_user.PrivacyControl_grayscale();
                
                //trigger 4: Collaboration. Users Hold up the tablet to cover their mouth when speaking.
                if (tablet_user.Distance <= DistanceThreshold_bothUser && tablet_user2.Distance <= DistanceThreshold_bothUser)
                    tablet_user.PrivacyControl_brightness();
                */

                Console.ReadKey();
                                     
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error" + "" + ex);
            }
           
        }



        /*//calculate the angle betweeen intruders gazing direction and intruder-tablet line
        public static Angle notifyAngle(double [] intr, double [] tabl){
        
            Vector3 intruder, tablet, line, gazing;
            Quaternion orientation;
            intruder = new Vector3(intr[4],intr[5],intr[6]); //geting the intruder~s position
            tablet = new Vector3(tabl[4],tabl[5],tabl[6]); //getting the tablet position
            line = (tablet - intruder).Normalized;
            
            orientation = new Quaternion(intr[7],intr[8],intr[9],intr[10]); //getting the intruder orientation
            gazing = Quaternion.Rotatedpoint(intruder,orientation);//the gazing direction

            //calculate the angle
            Angle angle = new Angle();
            angle = line.AnglePS(gazing);
            return angle;
        }       
        */
            
        
        
        
        }


    
}
