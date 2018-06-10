using Communication;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace WebApplication2.Models
{
    public class communicationModel
    {
        private static communicationModel instance;
        private static object m_lock = new object();
        private bool _isConnected;
        private NetworkStream stream;
        private BinaryReader reader;
        private BinaryWriter writer;

        private communicationModel() { }

        /// <summary>
        /// get instance of singlton.
        /// </summary>
        public static communicationModel Instance
        {
            get
            {
                lock (m_lock)
                {
                    if (instance == null)
                    {
                        instance = new communicationModel();
                        instance._isConnected = false;
                        instance.reader = null;
                        instance.writer = null;
                        instance.stream = null;
                    }
                }
                return instance;
            }
        }

        public bool isConnected
        {
            get
            {
                return _isConnected;
            }
            set
            {
                _isConnected = value;
            }
        }
        /// <summary>
        /// connect to server.
        /// </summary>
        public static void Connect()
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
            TcpClient client = new TcpClient();
            //try to connect
            try
            {
                client.Connect(ep);
            }
            //connection failed
            catch (Exception)
            {
                return;
            }
            //connection success
           Instance.isConnected = true;

            // get streams
            Instance.stream = client.GetStream();
            Instance.reader = new BinaryReader(Instance.stream);
            Instance.writer = new BinaryWriter(Instance.stream);
        }

        /// <summary>
        /// write a message to server
        /// </summary>
        /// <param name="command"></param>
        public static void Write(string command)
        {
            if (Instance.writer != null)
            {
                Instance.writer.Write(command);
            }
        }
        /// <summary>
        /// read a message from server.
        /// </summary>
        /// <returns></returns>
        public static string read()
        {
            if (Instance.reader != null)
            {
                string output = Instance.reader.ReadString();
                return output;
            }
            return null;
        }
    }
}