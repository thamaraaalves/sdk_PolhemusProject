using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using testLayers;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO.Pipes;
using System.Diagnostics;

namespace testLayers2
{   
    class Program
    {       
        /// <summary>
        /// READER
        /// </summary>
        public static bool cond = false;
        public static int aux = 0;

        static void Main(string[] args)
        {
        /*    int count;
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
                // Set the position to the beginning of the stream.
                memStream.Seek(0, SeekOrigin.Begin);

                // Read the first 20 bytes from the stream.
                byteArray = new byte[memStream.Length];
                count = memStream.Read(byteArray, 0, 20);

                // Read the remaining bytes, byte by byte. 
                while (count < memStream.Length)
                {
                    byteArray[count++] =
                        Convert.ToByte(memStream.ReadByte());
                }

                // Decode the byte array into a char array 
                // and write it to the console.
                charArray = new char[uniEncoding.GetCharCount(
                    byteArray, 0, count)];
                uniEncoding.GetDecoder().GetChars(
                    byteArray, 0, count, charArray, 0);
                Console.WriteLine(charArray);
                Console.ReadKey();
           }*/
        }

        
        //it works , but not good        
      /*  public static void Read() {         
            MemoryMappedFile mmf = MemoryMappedFile.OpenExisting("test");         
            MemoryMappedViewAccessor accessor = mmf.CreateViewAccessor();

            while (cond == true)
            {
                int value = accessor.ReadInt32(aux);
                // print it to the console
                Console.WriteLine("The value is {0}", value);
                aux++;
            }

            if (Console.ReadKey().ToString() == "exit")
            {
                accessor.Dispose();
                mmf.Dispose();
                cond = false;
            }
            Console.ReadKey();
        }*/
    }  
}
