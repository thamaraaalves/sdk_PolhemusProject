using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMath
{
    class Program
    {
        public static int widthP;
        public static int numberC;
        
        
        static void Main(string[] args)
        {
            //width pixel          
            //to create columns    
            Console.WriteLine("Write the size of column:");
            widthP = Convert.ToInt16(Console.ReadLine());

            Console.WriteLine("Write the number of columns:");
            numberC = Convert.ToInt16(Console.ReadLine());   
         
            Console.WriteLine("Write the number of pixel to search the column:");
            int pos = Convert.ToInt16(Console.ReadLine());

            Console.WriteLine("The Column is" + " " +  Search(pos));
            Console.ReadKey();
                           
        }

        public static void CreatColumns(int w, int n_c)
        {
            int quant = w * n_c;
            int[] c = new int[quant];           

            //function to create columns
            for (int i = 0; i <quant; i++)
            {
                c[i] = i;
            }     

        }

        public static int Search(int pos)
        {
            CreatColumns(widthP, numberC);
            return pos / numberC;
        }

    }
}
