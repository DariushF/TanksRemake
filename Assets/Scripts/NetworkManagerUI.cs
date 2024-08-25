using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{
    public Button serverBtn;
    public Button clientBtn;
    public Button hostButton;

    public TMP_InputField joinCodeInputField;

    private void Start()
    {
        // serverBtn.onClick.AddListener(() => RelayManager.Instance.StartServer());
        clientBtn.onClick.AddListener(() => RelayManager.Instance.JoinRelay(joinCodeInputField.text));
        hostButton.onClick.AddListener(() => RelayManager.Instance.CreateRelay());
    }
}
