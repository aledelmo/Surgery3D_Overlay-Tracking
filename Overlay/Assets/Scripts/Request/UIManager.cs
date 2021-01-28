using UnityEngine;
using UnityEngine.UI;

namespace Request
{
    public class UIManager : MonoBehaviour
    {
        
        [SerializeField] private Button connectButton;
        [SerializeField] private Button disconnectButton;
        private bool _connected;

        private void Start()
        {
            connectButton.onClick.AddListener(ConnectDisconnect);
            connectButton.onClick.AddListener(EventManager.Instance.onConnectionRequest.Invoke);
            disconnectButton.onClick.AddListener(ConnectDisconnect);
            disconnectButton.onClick.AddListener(EventManager.Instance.onDisconnectionRequest.Invoke);
        }

        private void ConnectDisconnect()
        {
            connectButton.interactable = _connected;
            disconnectButton.interactable = !_connected;
            _connected = !_connected;
        }

        public void QuitApp()
        {
            Application.Quit();
        }
    }
}
