using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO.Pipes;
using System.Diagnostics;

namespace testLayers
{
    public class Program
    {
        /// <summary>
        ///  WRITER 
        /// </summary>
        public static bool cond = false;
        public static int aux = 0;
               
        public static void Main(string[] args)
        {
          /*  int count;
            byte[] byteArray;
            char[] charArray;
            UnicodeEncoding uniEncoding = new UnicodeEncoding();

            // Create the data to write to the stream. 
            byte[] firstString = uniEncoding.GetBytes(
                "Invalid file path characters are: ");
            byte[] secondString = uniEncoding.GetBytes(
                Path.GetInvalidPathChars());

            using (MemoryStream memStream = new MemoryStream(100))
            {
                using (BinaryWriter writer = new BinaryWriter(memStream))
                {
                    
                            writer.Write(500);                     
                    
                }
               
            }      */    
            
        }



        

      /*  public static void writeProcess() {
            // create a memory-mapped file of length 1000 bytes and give it a 'map name' of 'test'
            MemoryMappedFile mmf = MemoryMappedFile.CreateNew("test", 100000);
            // write an integer value
            MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor();
            cond = true;

            while (cond == true)
            {
                Console.WriteLine("Write a value :");
                string value = "";
                value = Console.ReadLine().ToString();

                accessor.Write(aux, Convert.ToInt16(value));

                Console.WriteLine("Memory-mapped file created! " + " " + "value added" + " " + value);
                Console.ReadLine(); // pause till enter key is pressed
                // dispose of the memory-mapped file object and its accessor
                aux++;

                if (Console.ReadKey().ToString() == "exit")
                {
                    accessor.Dispose();
                    mmf.Dispose();
                    cond = false;
                }
                Console.ReadKey();
            }
        }*/

        
    }
}

