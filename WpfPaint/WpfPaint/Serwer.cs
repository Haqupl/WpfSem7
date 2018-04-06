using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace wpfSyntezator
{
    class Serwer
    {
        IPAddress IpSerwera;
        Int32 port;
        TcpListener _serwer;
        public List<ClientResive> odebraneDane;
        public bool ServerActive { get; private set; }

        Task tServer;

        //public delegate void DataRefreshHandler();
        //public event DataRefreshHandler Refresh;
        public event EventHandler Refresh;

        public Serwer(string Ip, Int32 port)
        {
            IpSerwera = IPAddress.Parse(Ip); ;
            this.port = port;
            odebraneDane = new List<ClientResive>();
        }

        public void StartSerwer()
        {
            tServer = Task.Run(() => startServer());
        }

        private void startServer()
        {

            try
            {
                _serwer = new TcpListener(IpSerwera, port);

                _serwer.Start();
                ServerActive = true;

                Byte[] bytes = new Byte[256];
                String data = null;

                while (ServerActive)
                {
                    Console.Write("Waiting for a connection... ");

                    TcpClient client = _serwer.AcceptTcpClient();
                    Console.WriteLine("Connected!");
                    Task.Run(() => ClientConetcion(client));

                }

            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message);
                throw e;
            }
            finally
            {
                StopSerwer();
            }
        }

        private void ClientConetcion(TcpClient client)
        {
            //var data = getData(client);
            NetworkStream stream = null;
            int lastLenghtData = 0;
            try
            {
                while (client.Connected)
                {
                    if (client.Available > 0 && lastLenghtData < client.Available)
                    {
                        stream = client.GetStream();

                        Console.WriteLine("stream.DataAvailable   " + stream.DataAvailable);
                        if (stream.DataAvailable)
                        {
                            int i;
                            Byte[] bytes = new Byte[256];
                            string data = "";
                            while ((i = stream.Read(bytes, lastLenghtData, bytes.Length)) != 0)
                            {
                                // Translate data bytes to a ASCII string.
                                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                                Console.WriteLine("Received: {0}", data);

                                // Process the data sent by the client.
                                byte[] msg = System.Text.Encoding.ASCII.GetBytes("Serw complete...");
                                // Send back a response.
                                stream.Write(msg, 0, msg.Length);
                            }
                        }
                        lastLenghtData = client.Available;

                    }
                    else
                        System.Threading.Thread.Sleep(200);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                client.Close();
            }

            if (stream != null)
            {
                stream.Close();
            }

        }

        public void StopSerwer()
        {
            if (ServerActive)
            {
                ServerActive = false;
                UnlockWaitingServer();
                _serwer.Pending();
                _serwer.Stop();
                _serwer.Server.Dispose();
            }

        }

        private string getData(TcpClient client)
        {
            int i;
            Byte[] bytes = new Byte[256];
            string data = "";
            NetworkStream stream = client.GetStream();

            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                // Translate data bytes to a ASCII string.
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine("Received: {0}", data);

                // Process the data sent by the client.
                byte[] msg = System.Text.Encoding.ASCII.GetBytes("Serw complete...");
                // Send back a response.
                stream.Write(msg, 0, msg.Length);
            }
            stream.Close();


            //lock (odebraneDane)
            //{
            //    odebraneDane.Add(new ClientResive(data, client.Client.RemoteEndPoint));
            //}


            //if (Refresh != null)
            //{
            //    Refresh(this, new EventArgs());
            //}
            return data;
        }

        void UnlockWaitingServer()
        {
            using (TcpClient cl = new TcpClient())
            {
                cl.Connect(IpSerwera, port);
                cl.Close();
            }
        }

    }
}
