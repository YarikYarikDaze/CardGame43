using System;
using System.Linq;
using UnityEngine;
using Unity.Netcode;


public class GameManager : NetworkBehaviour
{
    // ABOUT CARD VALUES
    // HAND/DECKS: 3 values, representing the amount of cards on each deck
    // Hence, 0-1-2 R-Y-B
    //
    // PREP/CARDS: 3 values, representing the color of each card in this prep,
    // INCLUDING LACK OF A CARD!
    // Hence, 0-1-2-3 X-R-Y-B
    [SerializeField] int[,,] playerCards;           // i - players, j - hand/prep, k - cards
    [SerializeField] int currentTurn;               // turn counter
    [SerializeField] int playerCount;               // total player count
    [SerializeField] GameObject playerPrefab;       // prefab of a player
    [SerializeField] GameObject enemyRenderPrefab;  // prefab of an enemy renderer

    [SerializeField] int initCardAmount = 8;        // amount of cards to put in every hand

    public static GameManager Instance { get; private set; }  // GameManager Singleton class to call

    public override void OnNetworkSpawn()
    // Basically initializer: sets turns, p-count, and cards
    {
        if (Instance == null && (IsServer || IsHost)) Instance = this;
        else Destroy(gameObject);


        playerCount = NetworkManager.Singleton.ConnectedClientsIds.Count;
        playerCards = new int[playerCount, 2, 3];
        currentTurn = 0;


        for (int i = 0; i < playerCount; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                playerCards[i, 0, j] = 0;
                playerCards[i, 1, j] = 0;
            }
        }

        // gives away cards players' hands with cards
        System.Random rnd = new System.Random();
        for (int i = 0; i < playerCount; i++)
        {
            for (int j = 0; j < initCardAmount; j++)
            {
                int k = rnd.Next(3);
                playerCards[i, 0, k]++;
            }
        }

        // Initializes players and state
        InitializeGameClientRpc();
        SetState(playerCards);

        //var sendParams = new ClientRpcParams();
        //sendParams.Send.TargetClientIds = new[] { NetworkManager.Singleton.ConnectedClientsIds[0] };
    }

    [ClientRpc]
    void InitializeGameClientRpc()
    // Spawns player and renderer on every client
    {
        GameObject enemyRender = Instantiate(enemyRenderPrefab);
        GameObject player = Instantiate(playerPrefab, new Vector3(0f, -5f, 0f), Quaternion.identity);
        //Debug.Log($"Object scene: {player.scene.name}");
    }

    void SetState(int[,,] cards)
    // loads the states of each player
    {
        for (int i = 0; i < playerCount; i++)
        {
            // sendParams are used for us to send only to client no. i. 
            // NOTE: Its using Network's IDs, which is probably read only?
            //       Later on we have to store IDs on a separate array to allow swapping, shifting etc.
            var sendOnly = new ClientRpcParams();
            sendOnly.Send.TargetClientIds = new[] { NetworkManager.Singleton.ConnectedClientsIds[i] };

            // Sets hand and prep
            // NOTE: preset amount of cards and colors, 
            //       we have to evaluate by colors and prep size
            int[] handCards = new int[] { cards[i, 0, 0], cards[i, 0, 1], cards[i, 0, 2] };
            int[] prepCards = new int[] { cards[i, 1, 0], cards[i, 1, 1], cards[i, 0, 2] };

            // RPCs the Player ID and Cards for player's hand 
            GiveCardsClientRpc(handCards, prepCards, sendOnly);
            SetIdClientRpc(i, sendOnly);

            // Gives a turn to current player
            if (i == currentTurn)
                SetTurnClientRpc(sendOnly);
        }
    }

    [ClientRpc]
    void GiveCardsClientRpc(int[] handCards, int[] prepCards, ClientRpcParams clientParams)
    // each client receives only their data!
    {
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if(!player) return;
        player.Receive(handCards, prepCards);
    }

    [ClientRpc]
    void SetIdClientRpc(int id, ClientRpcParams clientParams)
    // sets the client's Player Id.
    {
        GameObject.FindWithTag("Player").GetComponent<Player>().SetId(id);
    }

    [ClientRpc]
    void SetTurnClientRpc(ClientRpcParams clientParams)
    // sets the turn
    {
        GameObject.FindWithTag("Player").GetComponent<Player>().TakeTurn();
    }

    public void PlaceCard(int color, int id, bool left)
    // ARGUMENT COLOR 0-1-2 R-Y-B
    {
        playerCards[id, 0, color]--;

        int newColor = color + 1;
        // COLOR 1-2-3 R-Y-B

        int[] oldPlacement = { playerCards[id, 1, 0], playerCards[id, 1, 1], playerCards[id, 1, 2] };

        var filtered = oldPlacement.Where(n => n != 0).ToArray();

        int[] newPlacement;

        switch (filtered.Length)
        {
            case 0:
                newPlacement = new int[] { newColor, 0, 0 };
                break;
            case 1:
                newPlacement = left ? new int[] { newColor, filtered[0], 0 }
                                    : new int[] { filtered[0], newColor, 0 };
                break;
            case 2:
                newPlacement = left ? new int[] { newColor, filtered[0], filtered[1] }
                                    : new int[] { filtered[0], filtered[1], newColor };
                break;
            default:
                newPlacement = oldPlacement;
                break;
        }

        for (int i = 0; i < 3; i++)
        {
            playerCards[id, 1, i] = newPlacement[i];
        }

        SetState(playerCards);
    }

    public void NewCast(int id)
    // unfinished casting, right now only erases the cards in prep
    {
        for (int i = 0; i < 3; i++)
        {
            playerCards[id, 1, i] = 0;
        }
        

        SetState(playerCards);
    }

    public void NewTurn()
    // next turn!
    {
        currentTurn = (currentTurn++) % playerCount;

        SetState(playerCards);
    }

}
