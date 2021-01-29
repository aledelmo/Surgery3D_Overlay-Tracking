using System;
using IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Request
{
    public class UIManager : MonoBehaviour
    {
        
        [SerializeField] private Button connectButton;
        [SerializeField] private Button disconnectButton;
        [SerializeField] private AsyncReadback asyncReadback;
        [SerializeField] private VideoPlayer vp;
        private bool _connected;
        private bool flag = false;

        private void Start()
        {
            connectButton.onClick.AddListener(ConnectDisconnect);
            connectButton.onClick.AddListener(EventManager.Instance.onConnectionRequest.Invoke);
            disconnectButton.onClick.AddListener(ConnectDisconnect);
            disconnectButton.onClick.AddListener(EventManager.Instance.onDisconnectionRequest.Invoke);
        }

        private void Update()
        {
            if (vp.isPrepared && !flag)
            {
                asyncReadback.enabled = true;
                flag = !flag;
            }
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
