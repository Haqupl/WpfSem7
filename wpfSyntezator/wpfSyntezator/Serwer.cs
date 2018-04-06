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

        public event EventHandler Refresh;

        public Serwer(string Ip, Int32 port)
        {
            IpSerwera = IPAddress.Parse(Ip); ;
            this.port = port;
            odebraneDane = new List<ClientResive>();
        }

        public void StartSerwer()
        {
            Task.Run(() => startServer());
        }

        private void startServer()
        {

            try
            {
                _serwer = new TcpListener(IpSerwera, port);

                _serwer.Start();

                Byte[] bytes = new Byte[256];
                String data = null;

                while (true)
                {
                    Console.Write("Waiting for a connection... ");
                    TcpClient client = _serwer.AcceptTcpClient();

                    try
                    {
                        Console.WriteLine("Connected!");
                        data = getData(client);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message + " " + ex.StackTrace);
                    }
                    finally
                    {
                        client.Close();
                    }
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " " + e.StackTrace);
            }
            finally
            {
                StopSerwer();
            }
        }

        public void StopSerwer()
        {
            _serwer.Stop();
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


            lock (odebraneDane)
            {

                var ipExist = odebraneDane.Where(c => ((IPEndPoint)c.IpEndPoint).Address.ToString() == ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()).ToList().Count>0;
               
                odebraneDane.Add(new ClientResive(data, client.Client.RemoteEndPoint, ipExist));
            }

            stream.Close();
            if (Refresh != null)
            {
                Refresh(this, null);
            }
            return data;
        }

    }
}
