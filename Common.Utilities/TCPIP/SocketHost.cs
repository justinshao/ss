using Common.Utilities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Common.Utilities.TCPIP
{
    public class SocketHost
    {
        public event RecivedMsgHandle OnRecivedMessage;
        public delegate void RecivedMsgHandle(string ip, string msg);
        IPAddress _ServerIP;
        private IPEndPoint _ServerFullAddr;
        private Socket _Socket;
        int _Port;
        public bool StartListen(string ip, int port)
        {
            try
            {
                _Port = port;
                _ServerIP = IPAddress.Parse(ip);   //IP  
                _ServerFullAddr = new IPEndPoint(_ServerIP, _Port);//设置IP，端口  
                _Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                //指定本地主机地址和端口号  
                _Socket.Bind(_ServerFullAddr);

                Thread thread = new Thread(new ThreadStart(DoListen));
                thread.IsBackground = true;
                thread.Start();
                return true;
            }
            catch (Exception ex)
            {
                LogerHelper.Loger.Error("StartListen" + ex);
                return false;
            }
        }
        Dictionary<string, Client> _dicClientSocket = new Dictionary<string, Client>();
        private void DoListen()
        {
            byte[] message = new byte[1024];
            while (true)
            {
                try
                {
                    _Socket.Listen(500);//backlog 参数指定队列中最多可容纳的等待接受的传入连接数。  
                    Socket ServerSocket = _Socket.Accept();//为新建连接创建新的socket。sock这个socket是用来监听的，当他有连接请求的时候，将地址给新的socket来接收，这样不影响他继续监听原本的socket。  
                                                           //int bytes = newSocket.Receive(message);//用刚才chuangjian接收数据  

                    Client client = new Client(ServerSocket);
                    client.thread = new Thread(new ParameterizedThreadStart(ServerReceive));
                    client.thread.IsBackground = true;
                    client.thread.Start(client);
                    Thread.Sleep(200);
                }
                catch (SocketException ex)
                {
                    LogerHelper.Loger.Error("DoListen" + ex);
                }
            }
        }

        private void ServerReceive(object ThreadData)//服务器开始接收信息
        {
            Client client = (Client)ThreadData;
            try
            {
                while (client.online)
                {
                    if (client.socket == null || client.socket.Available < 1)
                    {
                        Thread.Sleep(200);
                        continue;
                    }
                    byte[] data = new byte[1024];
                    int bytes = client.socket.Receive(data);//用刚才chuangjian接收数据  
                    var msg = Encoding.Default.GetString(data, 0, bytes);//对接收字节编码（S与C 两端编码格式必须一致不然中文乱码）（当接收的字节大于1024的时候 这应该是循环接收，测试就没有那样写了）  
                    if (msg != "")
                    {
                        string[] arr = msg.Split('|');
                        switch (arr[0])
                        {
                            case "Online":
                                //client.name = arr[1];
                                Add(client);
                                break;
                            case "Exit":
                                Remove(client);
                                break;
                            case "SendMessage":
                                if (OnRecivedMessage != null && arr.Length > 1)
                                {
                                    OnRecivedMessage(client.remoteIP, arr[1]);
                                }
                                break;
                        }
                    }
                    Thread.Sleep(200);
                }
            }
            catch (Exception e)
            {
                LogerHelper.Loger.Error(e);
            }
        }

        private void Add(Client client)
        {
            if (!_dicClientSocket.ContainsKey(client.remoteIP))
            {
                _dicClientSocket.Add(client.remoteIP, client);
            }
            else
            {

                _dicClientSocket[client.remoteIP] = client;
            }
        }

        private void Remove(Client client)
        {
            if (_dicClientSocket.ContainsKey(client.remoteIP))
            {
                client.online = false;
                if (client.socket != null)
                {
                    client.socket.Close();
                }
                Thread.Sleep(200);
                if (client.thread.IsAlive)
                {
                    client.thread.Abort();
                }
                Thread.Sleep(200);
                _dicClientSocket.Remove(client.remoteIP);
            }
        }
        public byte[] PreVarReceive(Socket s)
        {
            int total = 0, recv;
            byte[] datasize = new byte[4];
            recv = s.Receive(datasize, 0, 4, 0);
            int size = BitConverter.ToInt32(datasize, 0);
            int dataleft = size;
            byte[] data = new byte[size];
            while (total < size)
            {
                recv = s.Receive(data, total, dataleft, 0);
                if (recv == 0)
                {
                    data = Encoding.Default.GetBytes("Exit");
                    break;
                }
                total += recv;
                dataleft -= recv;
            }
            return data;
        }

        public int PreVarSend(Socket s, byte[] data)
        {
            int total = 0;
            int size = data.Length;
            int dataleft = size;
            int sent;
            byte[] datasize = new byte[4];
            datasize = BitConverter.GetBytes(size);
            sent = s.Send(datasize);
            while (total < size)
            {
                sent = s.Send(data, total, dataleft, SocketFlags.None);
                total += sent;
                dataleft -= sent;
            }
            return total;
        }

        public void Send(string strdata)
        { 
            try
            {
                var list = _dicClientSocket.ToList();
                foreach (var item in list)
                {
                    Send(item.Value, strdata);
                }
            }
            catch (Exception e)
            {
                LogerHelper.Loger.Error(e);
            }
        }

        public void Send(Client c, string msg)
        {
            try
            {
                byte[] data = new byte[1024];
                data = Encoding.UTF8.GetBytes(msg);
                int sent = PreVarSend(c.socket, data);
            }
            catch (Exception e)
            {
                LogerHelper.Loger.Error(e);
            }
        }
    }


    public class Client
    {
        public bool online;
        public Socket socket;
        public Thread thread;
        private string _name;
        private string _remoteIP;
        public string name
        {
            set { _name = value; }
            get { return _name; }
        }
        public string remoteIP
        {
            get { return _remoteIP; }
        }

        public Client(Socket s)
        {
            online = true;
            socket = s;
            _remoteIP = ((IPEndPoint)socket.RemoteEndPoint).Address.ToString();
        }
    }

}
