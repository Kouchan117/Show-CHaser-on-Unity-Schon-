using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

using UniRx;

namespace UnityTCP
{
    public class TestServerController : MonoBehaviour
    {
        private Fieldfactory fieldFactory = default;

        private void Start()
        {
            // TcpServerController(シングトン)を取得
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
                .Subscribe(msg => GetMessage(msg)) // 受け取れる引数はObserverの実装によってさまざま。この場合はstring。
                .AddTo(gameObject);

            // サーバ起動
            server.StartServer();
        }

        // Observerによって呼ばれるコールバック
        // すでにメインスレッドに切り替わっているので、UnityAPIが使える。
        private void GetMessage(string msg)
        {
            Debug.Log(msg);
            fieldFactory.GenerateField(msg);

            //var obj = Instantiate(cubePrefab, new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f)), Quaternion.identity);
            //obj.name = msg;
        }
    }
}