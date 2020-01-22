using System;
using System.IO;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UniRx;
using UniRx.Triggers;

namespace UnityTCP
{
    [DisallowMultipleComponent]
    public class TcpServerController : MonoBehaviour
    {
        public static TcpServerController Instance { private set; get; } = default;

        [SerializeField] private string address = "127.0.0.1";
        [SerializeField] private int port = 2001;
        [SerializeField] private bool isAutoStartServerOnPlay = false;

        private TcpListener listener = default;
        private TcpClient client = default;

        // --------------------
        // Public Observable
        // --------------------

        private Subject<Unit> onStartServer = default, onStopServer = default;
        private Subject<EndPoint> onConnectClient = default, onDisconnectClient = default;
        private Subject<string> onGetMessage = default;

        public IObservable<Unit> OnStartServerAsObservable()
        {
            return onStartServer ?? (onStartServer = new Subject<Unit>());
        }

        public IObservable<Unit> OnStopServerAsObservable()
        {
            return onStopServer ?? (onStopServer = new Subject<Unit>());
        }

        public IObservable<EndPoint> OnConnectClientAsObservable()
        {
            return onConnectClient ?? (onConnectClient = new Subject<EndPoint>());
        }

        public IObservable<EndPoint> OnDisonnectClientAsObservable()
        {
            return onDisconnectClient ?? (onDisconnectClient = new Subject<EndPoint>());
        }

        public IObservable<string> OnGetMessageAsObservable()
        {
            return onGetMessage ?? (onGetMessage = new Subject<string>());
        }

        // --------------------
        // Public Methods
        // --------------------

        public void StartServer()
        {
            var ip = IPAddress.Parse(address);
            listener = new TcpListener(ip, port);
            listener.Start();
            listener.BeginAcceptSocket(OnAcceptSocket, listener);
            if (onStartServer != null)
                onStartServer.OnNext(Unit.Default);
        }

        public void StopServer()
        {
            if (listener != null) listener.Stop();
            if (client != null) client.Close();
            if (onStopServer != null)
                onStopServer.OnNext(Unit.Default);
        }

        // --------------------
        // Callbacks
        // --------------------

        private void OnAcceptSocket(IAsyncResult ar)
        {
            var li = (TcpListener)ar.AsyncState;
            client = li.EndAcceptTcpClient(ar);

            if (onConnectClient != null)
                onConnectClient.OnNext(client.Client.RemoteEndPoint);

            var stream = client.GetStream();
            var reader = new StreamReader(stream, Encoding.UTF8);
            var writer = new StreamWriter(stream, Encoding.UTF8);

            while (client.Connected)
            {
                while (!reader.EndOfStream)
                {
                    if (onGetMessage != null)
                        onGetMessage.OnNext(reader.ReadLine());
                        writer.WriteLine("success");
                        writer.Flush();
                }

                if (client.Client.Poll(1000, SelectMode.SelectRead) && (client.Client.Available == 0))
                {
                    if (onDisconnectClient != null)
                        onDisconnectClient.OnNext(client.Client.RemoteEndPoint);
                    client.Close();
                    break;
                }
            }
            StopServer();
        }

        // --------------------
        // Unity Callbacks
        // --------------------

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        protected virtual void Start()
        {
            if (isAutoStartServerOnPlay)
                StartServer();
        }

        protected virtual void OnApplicationQuit()
        {
            StopServer();
        }
    }
}