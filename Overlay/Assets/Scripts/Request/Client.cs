using System.Collections;
using IO;
using UnityEngine;
using System.Threading;

namespace Request
{
    public class Client : MonoBehaviour
    {
        public Payload payload;
        public AsyncReadback asyncReadback;
        
        [SerializeField] private string host;
        [SerializeField] private string port;
        private Listener _listener;
        private Thread _thread;

        private void Start()
        {
            _listener = new Listener(host, port, HandleMessage);
            
            EventManager.Instance.onConnectionRequest.AddListener(StartServer);
            EventManager.Instance.onClientFree.AddListener(SendRequest);
            EventManager.Instance.onDisconnectionRequest.AddListener(StopServer);
        }

        private void StartServer()
        {
            asyncReadback.enabled = true;
            StartCoroutine(Wait(0.4f));
        }
        
        private static IEnumerator Wait(float time)
        {
            yield return new WaitForSeconds(time);
            EventManager.Instance.onClientFree.Invoke();
        }
        
        private void SendRequest()
        {
            while (string.IsNullOrEmpty(asyncReadback.Frame)) {}
            
            EventManager.Instance.onClientBusy.Invoke();
            var message = payload.Encode();
            _thread = new Thread(() => _listener.RequestMessageAsync(message));
            _thread.Start();
            
        }

        private void StopServer()
        {
            asyncReadback.enabled = false;
            EventManager.Instance.onClientBusy.Invoke();
        }

        private void HandleMessage(string message)
        {
            payload.Decode(message);
            EventManager.Instance.onClientFree.Invoke();
        }
    }
}