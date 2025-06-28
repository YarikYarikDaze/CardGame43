using System;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;

public class LobbyConnect : NetworkBehaviour
{
    

    [SerializeField] GameObject main;
    [SerializeField] GameObject lobby;
    [SerializeField] GameObject ChoiceMenu;
    [SerializeField] GameObject lobbyHost;
    [SerializeField] GameObject lobbyJoin;
    [SerializeField] Button host;
    [SerializeField] Button join;
    [SerializeField] GameObject JoinMenu;

    [SerializeField] TMP_Text IP;
    [SerializeField] TMP_Text Port;
    [SerializeField] Button connect;

    void Awake()
    {
        host.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            main.SetActive(false);
            lobby.SetActive(true);
            lobbyHost.SetActive(true);
            lobbyJoin.SetActive(false);
            ChoiceMenu.SetActive(false);
        });

        join.onClick.AddListener(() =>
        {
            ChoiceMenu.SetActive(false);
            JoinMenu.SetActive(true);
        });

        connect.onClick.AddListener(() =>
        {
            // IP.text = "127.0.0.1";
            // Port.text = "7777";
            NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>().SetConnectionData(
                "10.91.5.148",
                (ushort)7777
            );
            NetworkManager.Singleton.StartClient();
            main.SetActive(false);
            lobby.SetActive(true);
            lobbyJoin.SetActive(true);
            lobbyHost.SetActive(false);
        });
    }

}
