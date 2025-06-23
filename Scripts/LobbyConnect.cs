using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;

public class LobbyConnect : MonoBehaviour
{
    [SerializeField] GameObject main;
    [SerializeField] GameObject lobby;
    [SerializeField] GameObject lobbyHost;
    [SerializeField] GameObject lobbyJoin;
    [SerializeField] Button host;
    [SerializeField] Button join;
    
    void Awake()
    {
        host.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartHost();
            main.SetActive(false);
            lobby.SetActive(true);
            lobbyHost.SetActive(true);
            lobbyJoin.SetActive(false);
        });
        join.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.StartClient();
            main.SetActive(false);
            lobby.SetActive(true);
            lobbyJoin.SetActive(true);
            lobbyHost.SetActive(false);
        });
    }

}
