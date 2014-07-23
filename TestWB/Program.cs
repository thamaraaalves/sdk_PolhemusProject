﻿﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Nugget;
/*using Alchemy;
using Alchemy.Classes;*/
//using Fleck;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using tcpServer;

namespace TestWB
{
    public class Program
    {

       
          static void Main(string[] args)
            {

                TcpListener server = null;
                try
                {
                    // Set the TcpListener on port 13000.
                    Int32 port = 9000;
                    IPAddress localAddr = IPAddress.Parse("192.168.0.15");

                    // TcpListener server = new TcpListener(port);
                    server = new TcpListener(localAddr, port);

                    // Start listening for client requests.
                    server.Start();

                    // Buffer for reading data
                    Byte[] bytes = new Byte[361];
                    String data = null;

                    // Enter the listening loop. 
                    while (true)
                    {
                        Console.Write("Waiting for a connection... ");

                        // Perform a blocking call to accept requests. 
                        // You could also user server.AcceptSocket() here.
                        TcpClient client = server.AcceptTcpClient();
                        Console.WriteLine("Connected!");

                        data = null;

                        // Get a stream object for reading and writing
                        NetworkStream stream = client.GetStream();

                        int i;

                        // Loop to receive all the data sent by the client. 
                        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            // Translate data bytes to a ASCII string.
                            data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                            Console.WriteLine(data.Length + " " + "bytes read");

                            // Process the data sent by the client.
                            data = data.ToUpper();

                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);

                            // Send back a response.
                           // stream.Write(msg, 0, msg.Length);
                           // Console.WriteLine("Sent: {0}", data);
                        }

                        // Shutdown and end connection
                        client.Close();
                    }
                }
                catch (SocketException e)
                {
                    Console.WriteLine("SocketException: {0}", e);
                }
                finally
                {
                    // Stop listening for new clients.
                    server.Stop();
                }


                Console.WriteLine("\nHit enter to continue...");
                Console.Read();
                /*FleckLog.Level = LogLevel.Debug;
                
              //  FleckLog.Level = LogLevel.Info;
                var allSockets = new List<IWebSocketConnection>();

               // FleckLog.Level = LogLevel.Warn;


                var server = new WebSocketServer("ws://192.168.0.15:9000");
                server.Start(socket =>
                {
                    socket.OnOpen = () => allSockets.Add(socket);
                    socket.OnClose = () => allSockets.Remove(socket);
                    socket.OnBinary = test =>
                    {
                        byte[] message = test;
                        Console.WriteLine(message.ToString());
                    };
                    
                    socket.OnMessage = message =>
                    {
                        foreach (var s in allSockets.ToList())
                            s.Send(message);
                    };
                });
                var input = Console.ReadLine();
                while (input != "exit")
                {
                    foreach (var socket in allSockets.ToList())
                    {
                        socket.Send(input);
                    }
                    input = Console.ReadLine();
                }*/
            }
        
    }
}
