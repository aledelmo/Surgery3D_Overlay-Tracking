using IO;
using UnityEngine;
using System.Threading;

namespace Request
{
    public class Client : MonoBehaviour
    {
        [SerializeField] private string host;
        [SerializeField] private string port;
        private Listener _listener;
        private Thread _thread;

        public AsyncReadback ar;

        private void Start()
        {
            _listener = new Listener(host, port, HandleMessage);
            
            EventManager.Instance.onConnectionRequest.AddListener(StartServer);
            EventManager.Instance.onClientFree.AddListener(SendRequest);
            EventManager.Instance.onDisconnectionRequest.AddListener(StopServer);
        }

        private static void StartServer()
        {
            EventManager.Instance.onClientFree.Invoke();
        }
        
        private void SendRequest()
        {
            EventManager.Instance.onClientBusy.Invoke();
            _thread = new Thread(() => _listener.RequestMessageAsync(ar.frame));
            _thread.Start();
        }

        private static void StopServer()
        {
            EventManager.Instance.onClientBusy.Invoke();
        }

        private void HandleMessage(string message)
        {
            Debug.Log(message);
            EventManager.Instance.onClientFree.Invoke();
        }
    }
}