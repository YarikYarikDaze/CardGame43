using System;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
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
            // OOOOH, YOU REFER TO SINGLETON, WHILE ITS IN GAMEOBJECT!
            // NetworkManager.Singleton.GetComponent<UnityTransport>().SetConnectionData(
            //     IP.text,
            //     (ushort)int.Parse(Port.text)
            // );
            NetworkManager.Singleton.StartClient();
            main.SetActive(false);
            lobby.SetActive(true);
            lobbyJoin.SetActive(true);
            lobbyHost.SetActive(false);
        });
    }

}
