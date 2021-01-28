using UnityEngine;
using UnityEngine.Events;

namespace Request
{
    public class EventManager : MonoBehaviour
    {
        public static EventManager Instance;

        private void Awake()
        {
            Application.targetFrameRate = 30;
            
            if (Instance == null)
            {
                Instance = this;
            
                onConnectionRequest = new UnityEvent();
                onDisconnectionRequest = new UnityEvent();
                onClientBusy = new UnityEvent();
                onClientFree = new UnityEvent();
            }
            else
                Destroy(this);
        }

        public UnityEvent onConnectionRequest;
        public UnityEvent onDisconnectionRequest;
        public UnityEvent onClientBusy;
        public UnityEvent onClientFree;
    }
}