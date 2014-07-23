using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ListenPortG4;

namespace Prj_Orientation_Notification  
{
    public struct Quaternion
    {
                private double q0;
                private double q1;
                private double q2;
                private double q3;

             
               //constructor

               public Quaternion(double q0, double q1, double q2,double q3)   {    
        	         this.q0 = q0;
                     this.q1 = q1;
                     this.q2 = q2;
                     this.q3 = q3;	                
                }

                //quaternion multiplication
       
            public static Quaternion QMultiply(Quaternion left, Quaternion right){
	                Quaternion ans;
	                double d1,d2,d3,d4;

	                d1 = left.q0*right.q0;
	                d2 = -left.q1*right.q1;
	                d3 = -left.q2 * right.q2; 
                    d4 = -left.q3 * right.q3; 
                    ans.q0 = d1+ d2+ d3+ d4; 
  
                    d1 =  left.q0 * right.q1; 
                    d2 =  right.q0 * left.q1; 
                    d3 =  left.q2 * right.q3; 
                    d4 = -left.q3 * right.q2; 
                    ans.q1 =  d1+ d2+ d3+ d4; 
  
                    d1 =  left.q0 * right.q2; 
                    d2 =  right.q0 * left.q2; 
                    d3 =  left.q3 * right.q1; 
                    d4 = -left.q1 * right.q3; 
                    ans.q2 =  d1+ d2+ d3+ d4; 
  
                    d1 =  left.q0 * right.q3; 
                    d2 =  right.q0 * left.q3; 
                    d3 =  left.q1 * right.q2; 
                    d4 = -left.q2 * right.q1; 
                    ans.q3 =  d1+ d2+ d3+ d4; 
              
                    return ans; 
            }
       
        //complex conjugate of a quaternion
           public static Quaternion Conjugate(Quaternion q) {
                    Quaternion ans;
                    ans.q0 = q.q0;
                    ans.q1 = -q.q1;
                    ans.q2 = -q.q2;
                    ans.q3 = -q.q3;
                    return ans;
                }

        //to apply a quaternion-rotation to a vector, v'=q*v*q', which calculate the vector of rotated point
          public static Vector3 Rotatedpoint(Vector3 input, Quaternion q){

                    Vector3 vn = new Vector3(input);
                    vn.Normalize();
                    Vector3 res = new Vector3();

                    Quaternion vecQuat, resQuat;
                    vecQuat.q0 = 0.0f;
                    vecQuat.q1 = vn.X;
                    vecQuat.q2 = vn.Y;
                    vecQuat.q3 = vn.Z;

                    //calculate vector*q'
                    resQuat = QMultiply(vecQuat, Conjugate(q));
                    //calculate q * vector*q'
                    resQuat = QMultiply(q, resQuat);

                    res.X = resQuat.q1;
                    res.Y = resQuat.q2;
                    res.Z = resQuat.q3;
                    return res;
                }

          //calculate roteted vector
          public static Vector3 RotatedVector(Vector3 v, Quaternion q) {
                   Vector3 rev = new Vector3();
                   /*method from http://www.mathworks.com/help/aeroblks/quaternionrotation.html
                    rev.X = v.X*(1-2*Math.Pow(q.q2,2)-2*Math.Pow(q.q3,2)) + v.Y*2*(q.q1*q.q2+q.q0*q.q3) + v.Z*2*(q.q1*q.q3-q.q0*q.q2);
                    rev.Y = v.X*2*(q.q1*q.q2-q.q0*q.q3) + v.Y*(1-2*Math.Pow(q.q1,2)-2*Math.Pow(q.q3,2)) + v.Z*2*(q.q2*q.q3+q.q0*q.q1);
                    rev.Z = v.X*2*(q.q1*q.q3+q.q0*q.q2) + v.Y*2*(q.q2*q.q3-q.q0*q.q1) + v.Z*(1-2*Math.Pow(q.q1,2)-2*Math.Pow(q.q2,2));*/


                   //algorithm from http://www.cprogramming.com/tutorial/3d/quaternions.html
                   rev.X = v.X * (1 - 2 * Math.Pow(q.q2, 2) - 2 * Math.Pow(q.q3, 2)) + v.Y * 2 * (q.q1 * q.q2 - q.q0 * q.q3) + v.Z * 2 * (q.q1 * q.q3 + q.q0 * q.q2);
                   rev.Y = v.X * 2 * (q.q1 * q.q2 + q.q0 * q.q3) + v.Y * (1 - 2 * Math.Pow(q.q1, 2) - 2 * Math.Pow(q.q3, 2)) + v.Z * 2 * (q.q2 * q.q3 + q.q0 * q.q1);
                   rev.Z = v.X * 2 * (q.q1 * q.q3 - q.q0 * q.q2) + v.Y * 2 * (q.q2 * q.q3 - q.q0 * q.q1) + v.Z * (1 - 2 * Math.Pow(q.q1, 2) - 2 * Math.Pow(q.q2, 2));

                   return rev;
          }
        




          //tostring
          public override string ToString()
          {
              return q0.ToString("N2") + ", " + q1.ToString("N2") + ", " + q2.ToString("N2") + ", " + q3.ToString("N2");
          }


    }
}
