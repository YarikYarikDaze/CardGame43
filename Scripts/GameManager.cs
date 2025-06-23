using System;
using UnityEngine;
using Unity.Netcode;


public class GameManager : NetworkBehaviour
{
    [SerializeField] int[,,] playerCards;        // 1 red, 2 yellow, 3 blue. i - players, j - hand/prep, k - cards
    [SerializeField] int currentTurn;               // turn counter;
    [SerializeField] int playerCount;

    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject enemyRenderPrefab;

    [SerializeField] int initCardAmount = 8;

    public static GameManager Instance { get; private set; }

    public override void OnNetworkSpawn()
    {
        if (Instance == null && (IsServer || IsHost)) Instance = this;
        else Destroy(gameObject);

        playerCount = NetworkManager.Singleton.ConnectedClientsIds.Count;

        currentTurn = 0;
        playerCards = new int[playerCount, 2, 3];

        Debug.Log("dancer 0! And the array is " + ((playerCards == null) ? "null!" : "not null!"));


        for (int i = 0; i < playerCount; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                playerCards[i, 0, j] = 0;
                playerCards[i, 1, j] = 0;
            }
        }

        System.Random rnd = new System.Random();
        for (int i = 0; i < playerCount; i++)
        {
            for (int j = 0; j < initCardAmount; j++)
            {
                int k = rnd.Next(3);
                playerCards[i, 0, k] += 1;

                Debug.Log("PC:" + i + "-" + 0 + "-" + k + ": " + playerCards[i, 0, k]);
            }
        }

        InitializeGameClientRpc();
        SetState(playerCards);
        //var sendParams = new ClientRpcParams();
        //sendParams.Send.TargetClientIds = new[] { NetworkManager.Singleton.ConnectedClientsIds[0] };
    }

    [ClientRpc]
    void InitializeGameClientRpc()
    {
        GameObject enemyRender = Instantiate(enemyRenderPrefab);


        GameObject player = Instantiate(playerPrefab, new Vector3(0f, -5f, 0f), Quaternion.identity);
        Debug.Log($"Object scene: {player.scene.name}");
    }

    void SetState(int[,,] cards)
    {
        for (int i = 0; i < playerCount; i++)
        {
            var sendParams = new ClientRpcParams();
            sendParams.Send.TargetClientIds = new[] { NetworkManager.Singleton.ConnectedClientsIds[i] };
            int[] handCards = new int[] { cards[i, 0, 0], cards[i, 0, 1], cards[i, 0, 2] };
            int[] prepCards = new int[] { cards[i, 1, 0], cards[i, 1, 1], cards[i, 0, 2] };
            this.GiveCardsClientRpc(handCards, prepCards, sendParams);
            this.SetIdClientRpc(i, sendParams);
            if (i == currentTurn)
            {
                this.SetTurnClientRpc(sendParams);
            }
        }
    }

    [ClientRpc]
    void GiveCardsClientRpc(int[] handCards, int[] prepCards, ClientRpcParams clientParams)
    {
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if(!player) return;
        Debug.Log("lancer 1! And the array is " + ((playerCards == null) ? "null!" : "not null!"));

        player.Receive(handCards, prepCards);
    }

    [ClientRpc]
    void SetIdClientRpc(int id, ClientRpcParams clientParams)
    {
        GameObject.FindWithTag("Player").GetComponent<Player>().SetId(id);
    }

        [ClientRpc]
    void SetTurnClientRpc(ClientRpcParams clientParams)
    {
        GameObject.FindWithTag("Player").GetComponent<Player>().TakeTurn();
    }

    public void PlaceCard(int color, int id, bool left)
    {
        this.playerCards[id, 0, color - 1]--;
        this.playerCards[id, 1, 0] = color;
        SetState(playerCards);
    }

    public void NewCast(int id)
    {
        for (int i = 0; i < 3; i++)
        {
            playerCards[id, 1, i] = 0;
        }
        
        SetState(playerCards);
    }

    public void NewTurn()
    {
        currentTurn = (currentTurn++) % playerCount;

        SetState(playerCards);
    }

}
