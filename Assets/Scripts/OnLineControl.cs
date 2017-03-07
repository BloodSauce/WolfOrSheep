using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System;
using System.Text;

public class OnLineControl{
    public bool isConnect;
    private Socket client;
    private IPEndPoint ipEndPoint;
    public byte[] msgRec;
    private byte[] msgSend;
    public int msgLength;
    public OnLineControl() {
        client= new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        ipEndPoint = new IPEndPoint(IPAddress.Parse("123.207.141.38"), 66);
        msgRec = new byte[1024];
        msgSend = new byte[1024];
    }
    //连接服务器
    public bool Connect() {
        try {
            IAsyncResult result = client.BeginConnect(ipEndPoint, new AsyncCallback(ConnectCallBack), client);
            //client.Connect(ipEndPoint);
            bool success = result.AsyncWaitHandle.WaitOne(10000, true);
            if (success) {
                isConnect = true;
                client.Send(Encoding.ASCII.GetBytes("s"));
                Thread recThread = new Thread(new ThreadStart(ReceiveMsg));
                recThread.IsBackground = true;
                recThread.Start();
                return true;
            }
            else {
                Close();
                return false;
            }
        }
        catch (Exception e) {
            Debug.Log(e.ToString());
            return false;
        }
    }
    private void ConnectCallBack(IAsyncResult i) {

    }
    //发送消息
    public void SendMsg(string order) {
        msgSend = Encoding.ASCII.GetBytes(order);
        if (!client.Connected) {
            client.Close();
            return;
        }
        try {
            IAsyncResult asySend = client.BeginSend(msgSend, 0, msgSend.Length, SocketFlags.None, new AsyncCallback(SendCallBack), client);
            bool success = asySend.AsyncWaitHandle.WaitOne(5000, true);
            if (!success) {
                client.Close();
            }
        }
        catch(Exception e) {
        }
    }
    private void SendCallBack(IAsyncResult asySend) {

    }
    //接受消息
    public void ReceiveMsg() {
        while (true) {
            if (!client.Connected) {
                client.Close();
            }
            try {
                msgLength = client.Receive(msgRec);
                if (msgLength < 0) {
                    client.Close();
                }
            }
            catch(Exception e) {
                client.Close();
            }
        }         
    }
    //清空记录
    public void Clean() {
        msgLength = 0;
    }
    //关闭
    public void Close() {
        if (client != null && client.Connected) {
            client.Shutdown(SocketShutdown.Both);
            client.Close();
        }
        client = null;
    }
}
