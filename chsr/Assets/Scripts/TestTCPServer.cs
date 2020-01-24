using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UniRx;

using Field;

public class TestTCPServer : MonoBehaviour
{
    public string mIpAddress = "127.0.0.1"; //自分自身を指すIPアドレス
    public int mPortNumber = 2001; //ポート番号は適当
    private TcpListener mListener;
    private TcpClient mClient;
    public Fieldfactory ff;

    public Action action;

    public bool trigger = false;
    public string message = "";

    // Start is called before the first frame update
    public void Start()
    {
        Debug.Log("Start");
        action += GetThread;
        StartServer();
    }

    private void Update()
    {
        if(trigger)
        {
            trigger = false;
            ff.GenerateField(message);
        }    
    }

    private void StartServer()
    {
        action();
        Debug.Log("StartServer");
        var ip = IPAddress.Parse(mIpAddress);
        mListener = new TcpListener(ip, mPortNumber);
        mListener.Start();
        mListener.BeginAcceptSocket(DoAcceptTcpClientCallback, mListener);
    }
    
    public void GetThread()
    {
        Debug.Log(System.Threading.Thread.CurrentThread.ManagedThreadId);
    }

    public void DoAcceptTcpClientCallback(IAsyncResult ar)
    {
        Debug.Log("DoAcceptTcpClientCallback");
        /* 渡されたものを取り出す */
        var listener = (TcpListener)ar.AsyncState;
        mClient = listener.EndAcceptTcpClient(ar);
        print("Connect: " + mClient.Client.RemoteEndPoint);

        /* 接続した人とのネットワークストリームを取得 */
        var stream = mClient.GetStream();
        var reader = new StreamReader(stream, Encoding.UTF8);
        var writer = new StreamWriter(stream, Encoding.UTF8);
        var str = "";

        /* 接続が切れるまで送受信を繰り返す */
        while(mClient.Connected)
        {
            while(!reader.EndOfStream)
            {
                str = reader.ReadLine();
                //Task.Run( () => ff.GenerateField(str));
                Debug.Log(str);
                writer.WriteLine("Receipt complete");
                writer.Flush();
            }

            /* クライアントの接続が切れたら */
            if(mClient.Client.Poll(1000, SelectMode.SelectRead) && (mClient.Client.Available == 0))
            {
                print("Disconnect: " + mClient.Client.RemoteEndPoint);
                mClient.Close();
                break;
            }
        }
        trigger =true;
        message = str;
        mListener.Stop();
        StartServer();
    }

    protected virtual void OnApplicationQuit()
    {
        if(mListener != null) mListener.Stop();
        if(mClient != null) mClient.Close();
    }
}
