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

    public string GetLocalIPv4()
    {
        return Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
    }

    void Awake()
    {
        string[] fours = GetLocalIPv4().Split(".");
        string first = fours[0] + "." + fours[1] + ".";
        string second = fours[2] + "." + fours[3];
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
            NetworkManager.Singleton.gameObject.GetComponent<UnityTransport>().SetConnectionData(
                (IP.text!="") ? (first+IP.text) : "127.0.0.1",
                (ushort)7777
            );
            NetworkManager.Singleton.StartClient();
            Debug.Log("Get on with it!");
            main.SetActive(false);
            lobbyJoin.SetActive(true);
            lobbyHost.SetActive(false);
        });
    }

}
