using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    public Button serverBtn;
    public Button clientBtn;
    public Button hostButton;

    private void Start()
    {
        serverBtn.onClick.AddListener(() => NetworkManager.Singleton.StartServer());
        clientBtn.onClick.AddListener(() => NetworkManager.Singleton.StartClient());
        hostButton.onClick.AddListener(() => NetworkManager.Singleton.StartHost());
    }
}
