using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityTCP;
using UniRx;

namespace Field
{
    public class FieldManager : MonoBehaviour
    {
        private enum Mode { ServerMode, ReaderMode }
        [SerializeField] private Mode mode = Mode.ServerMode;

        [SerializeField] private string logPath = default;
        [SerializeField] private float readSpan = 1f;

        private Fieldfactory fieldFactory = default;

        private void Awake()
        {
            fieldFactory = GetComponent<Fieldfactory>();
        }

        private void Start()
        {
            switch(mode)
            {
                case Mode.ServerMode: InitServerMode(); break;
                case Mode.ReaderMode: InitReaderMode(); break;
            }
        }

        private void InitServerMode()
        {
            Debug.Log("Start ServerMode");

            var server = TcpServerController.Instance;

            server.OnStartServerAsObservable() // StartServerが呼ばれたときに発火するObserver
                .Subscribe(_ => Debug.Log("StartServer")) // Subscrive(_ =>)の中に、発火されたときに実行したい処理を書く。
                .AddTo(gameObject); // 特定のオブジェクトに登録(おまじない)

            server.OnStopServerAsObservable() // StopServerが呼ばれたときに発火するObserver
                .Subscribe(_ => Debug.Log("StopServer"))
                .AddTo(gameObject);

            // StopServerが呼ばれたときにサーバを再起動する
            server.OnStopServerAsObservable()
                .Subscribe(_ => server.StartServer()) // Subscrive(_ =>)でメソッドも呼べる。
                .AddTo(gameObject);

            server.OnConnectClientAsObservable() // クライアントが接続したときに発火するObserver
                .Subscribe(ep => Debug.Log("Connect:" + ep)) // Subscrive(ep =>) 左辺に仮引数を入れて受け取ることができる。
                .AddTo(gameObject);

            server.OnDisonnectClientAsObservable()
                .Subscribe(ep => Debug.Log("Disconnect:" + ep))　// クライアントが切断したときに発火するObserver
                .AddTo(gameObject);

            server.OnGetMessageAsObservable() // メッセージを受け取ったときに発火するObserver
                .ObserveOnMainThread() // メインスレッドに切り替え
                .Subscribe(msg => LoadField(msg)) // 受け取れる引数はObserverの実装によってさまざま。この場合はstring。
                .AddTo(gameObject);

            // サーバ起動
            server.StartServer();
        }

        private void InitReaderMode()
        {
            Debug.Log("Start ReaderMode");
            StreamReader reader = new StreamReader(logPath, Encoding.UTF8);
            StartCoroutine(readLogCoroutine(reader));
        }

        private IEnumerator readLogCoroutine(StreamReader reader)
        {
            while (!reader.EndOfStream)
            {
                var str = reader.ReadLine();
                LoadField(str);
                yield return new WaitForSeconds(readSpan);
            }
            Debug.Log("Finish Logfile");
            yield break;
        }

        private void LoadField(string msg)
        {
            Debug.Log(msg);
            fieldFactory.GenerateField(msg);
        }
    }
}