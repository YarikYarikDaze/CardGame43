using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;

public class LobbyConnect : NetworkBehaviour
{


    [SerializeField] GameObject main;
    [SerializeField] GameObject ChoiceMenu;
    [SerializeField] GameObject lobbyHost;
    [SerializeField] GameObject lobbyJoin;
    [SerializeField] Button host;
    [SerializeField] Button join;
    [SerializeField] GameObject JoinMenu;
    [SerializeField] TMP_Text hostText;
    [SerializeField] TMP_InputField IP;
    [SerializeField] Button connect;

    [SerializeField] Button proceed;
    [SerializeField] TMP_Text hostConnectedCount;
    [SerializeField] SceneTransitionManager transitionManager;
    [SerializeField] TMP_Text joinConnectedCount;

    public string GetLocalIPv4()
    {
        return Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
    }

    void Awake()
    {
        string[] fours = GetLocalIPv4().Split(".");
        string first = fours[0] + "." + fours[1] + ".";
        string second = (Int32.Parse(fours[2])).ToString("X2") + (Int32.Parse(fours[3])).ToString("X2");
        host.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            main.SetActive(false);
            lobbyHost.SetActive(true);
            lobbyJoin.SetActive(false);
            ChoiceMenu.SetActive(false);
            hostText.text = "The code is: <" + second + ">";
        });

        join.onClick.AddListener(() =>
        {
            ChoiceMenu.SetActive(false);
            JoinMenu.SetActive(true);
        });

        connect.onClick.AddListener(() =>
        {
            // Port.text = "7777";
            string num1 = int.Parse(((IP.text).Substring(0, 2)), System.Globalization.NumberStyles.HexNumber).ToString();
            string num2 = int.Parse(((IP.text).Substring(2, 2)), System.Globalization.NumberStyles.HexNumber).ToString();

            NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>().SetConnectionData(

                (IP.text != "") ? (first + num1 + "." + num2) : "127.0.0.1",
                (ushort)7777
            );
            NetworkManager.Singleton.StartClient();
            Debug.Log(first + IP.text);
            Debug.Log("Get on with it!");
            main.SetActive(false);
            lobbyJoin.SetActive(true);
            lobbyHost.SetActive(false);
        });

        proceed.onClick.AddListener(() =>
        {
            transitionManager.TransitionAllPlayersToGameScene();
        });
    }
    void Update()
    {
        int count = NetworkManager.Singleton.ConnectedClientsIds.Count;
        if (lobbyHost.activeSelf)
        {
            proceed.interactable = count >= 2 && count <= 6;
            hostConnectedCount.text = "Connected players: " + count.ToString();
        }
        if (lobbyJoin.activeSelf)
        {
            joinConnectedCount.text = "Currently in this lobby: " + count.ToString();
        }
    }
}
//10.91.87.137