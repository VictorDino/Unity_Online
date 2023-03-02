using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginScript : MonoBehaviour
{
    [SerializeField] private Button loginButton;
    [SerializeField] private Text loginText;
    [SerializeField] private Text passwordText;
    private void Awake()
    {
        loginButton.onClick.AddListener(Log);
    }
    private void Log()
    {
        NetworkManager._NETWORK_MANAGER.ConnectToServer(loginText.text.ToString(), passwordText.text.ToString());
    }
}
