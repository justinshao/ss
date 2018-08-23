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
    class State
    {
        public System.Net.EndPoint RemoteEP;

        public System.Net.Sockets.Socket Socket
        {
            get;
            private set;
        }

        public byte[] Buffer
        {
            get;
            private set;
        }

        public State(System.Net.Sockets.Socket socket)
        {
            this.Buffer = new byte[2000];
            this.Socket = socket;
            this.RemoteEP = new System.Net.IPEndPoint(System.Net.IPAddress.Any, 0);
        }
    }
    public class SocketClient
    {
        IPAddress _ServerIP;
        string _IP;
        DateTime lastConnect = DateTime.Now;
        private IPEndPoint _ServerFullAddr;
        private Socket _Socket;
        int _Port;
        bool _IsConnected = false;
        public int MyLostTime = 0;

        public delegate void OnReceiveMessageHandle(string msg);
        public event OnReceiveMessageHandle OnReceiveMessageChanged;


        public delegate void OnConnectedChangedHandle(bool isConnected);
        public event OnConnectedChangedHandle OnConnectedChanged;


        public bool Connect(string ip, int port)
        {
            try
            {
                _IP = ip;
                _Port = port;
                _ServerIP = IPAddress.Parse(ip);
                _ServerFullAddr = new IPEndPoint(_ServerIP, port);//设置IP，端口   
                _Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //指定本地主机地址和端口号  
                _Socket.Connect(_ServerFullAddr);
                State state = new State(this._Socket);
                this._Socket.BeginReceiveFrom(state.Buffer, 0, state.Buffer.Length, System.Net.Sockets.SocketFlags.None, ref state.RemoteEP, new System.AsyncCallback(this.EndReceiveFromCallback), state);
                _IsConnected = true;
                StartHeartBeat();
                //心跳
                return true;

            }
            catch (Exception ex)
            {
                LogerHelper.Loger.Error("Connect" + ex);
                _IsConnected = false;
                return false;
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

        private List<byte> sendBuffer = new List<byte>(); 
        private void EndReceiveFromCallback(System.IAsyncResult iar)
        { 

            State state = iar.AsyncState as State;
            System.Net.Sockets.Socket socket = state.Socket;
            try
            {
                int byteRead = socket.EndReceiveFrom(iar, ref state.RemoteEP);
                var readbuf = new byte[state.Buffer.Length];
                byte[] datasize = new byte[4];  
                System.Array.Copy(state.Buffer, datasize, datasize.Length);
                int size = BitConverter.ToInt32(datasize, 0);
                if(size>0)
                { 
                    socket.BeginReceiveFrom(state.Buffer, 0, size, System.Net.Sockets.SocketFlags.None, ref state.RemoteEP, new System.AsyncCallback(this.EndReceiveFromCallback), state);
                }
                else
                {
                    System.Array.Copy(state.Buffer, readbuf, readbuf.Length);
                    this.UnPackage(readbuf); 
                }
            }
            catch (Exception e)
            {
                LogerHelper.Loger.Error(e);
                this.RecOpen();
            }
        }
        private void UnPackage(byte[] ReadBuf)
        { 
            string str = System.Text.Encoding.UTF8.GetString(ReadBuf).Replace("\0","").Trim();
            if (OnReceiveMessageChanged != null)
            {
                OnReceiveMessageChanged(str);
            }
        }
        /// <summary>
        /// 设备重连
        /// </summary>
        /// <returns></returns>
        public bool RecOpen()
        {
            try
            {
                if (_Socket != null && _Socket.Connected)
                {
                    this._Socket.Shutdown(SocketShutdown.Both);
                    this._Socket.Close();
                    this._Socket = null;
                }
                _IsConnected = false;
                return Connect(_IP, _Port);
            }
            catch (Exception e)
            {
                LogerHelper.Loger.Error("RecOpen:" + e);
                return false;
            }
        } 

        public void StartHeartBeat()
        {
            Thread sendEcho = new Thread(new ThreadStart(SocketSend));
            sendEcho.IsBackground = true;
            sendEcho.Start();
        }

        /// 每隔1秒发送一次数据，如果外发了10次请求暗号后仍不见服务器的回应，则认为客户端已经与服务器断开联系了
        public void SocketSend()
        {
            while (true)
            {
                Thread.Sleep(5000);
                try
                {
                    _Socket.Send(Encoding.UTF8.GetBytes("Online|"));
                    _IsConnected = true;
                    OnConnectedChanged(true);
                }
                catch (SocketException)
                {
                    _IsConnected = false;
                    OnConnectedChanged(false);
                }
                
            }
        }

        public bool SendMsg(string msg)
        {
            if (!this._IsConnected)
            {
                return false;
            }
            byte[] byteSend = System.Text.Encoding.Default.GetBytes(msg);
            byte[] message = new byte[1024];
            
            try
            {
                //发送数据  
                _Socket.Send(byteSend);
                return true;
            }
            catch (Exception ex)
            {
                LogerHelper.Loger.Error("SendMsg:" + ex);
                RecOpen();
                return false;
            }
        }
    }
}
