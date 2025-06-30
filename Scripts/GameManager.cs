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
    [SerializeField] GameObject prepRenderPrefab;  // prefab of an prep renderer

    [SerializeField] int initCardAmount = 8;        // amount of cards to put in every hand

    string[,] pairEffects;

    [ClientRpc]
    void aClientRpc(bool f)
    {
        //Debug.Log("This is"+((f) ? "" : " NOT")+" your server speaking");
    }
    void Update()
    {
        aClientRpc(NetworkManager.Singleton.IsServer);
    }

    // void Awake()
    // {
    //     if (!NetworkManager.Singleton.IsServer) Destroy(gameObject);
    // }
    public override void OnNetworkSpawn()
    // Basically initializer: sets turns, p-count, and cards
    {
        this.InitializeEffects();
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
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            GameObject playerInstance = Instantiate(playerPrefab, new Vector3(0f, -4.75f, 0f), Quaternion.identity);
            NetworkObject netObj = playerInstance.GetComponent<NetworkObject>();
            netObj.SpawnAsPlayerObject(clientId, true);
            playerInstance.GetComponent<Player>().enabled = true;
        }
        // Initializes players and state
        InitializeGameClientRpc();
        SetState(playerCards);

        //var sendParams = new ClientRpcParams();
        //sendParams.Send.TargetClientIds = new[] { NetworkManager.Singleton.ConnectedClientsIds[0] };
    }

    void InitializeEffects()
    {
        pairEffects = new string[3, 3]
        {
            {
                "TakeCard",
                "TakeCard",
                "TakeCard"
            },
            {
                "TakeCard",
                "TakeCard",
                "TakeCard"
            },
            {
                "TakeCard",
                "TakeCard",
                "TakeCard"
            }
        };
    }

    [ClientRpc]
    void InitializeGameClientRpc()
    // Spawns player and renderer on every client
    {
        GameObject prepRender = Instantiate(prepRenderPrefab);
        // GameObject player = Instantiate(playerPrefab, new Vector3(0f, -4.75f, 0f), Quaternion.identity);
        // player.GetComponent<NetworkObject>().Spawn();

        //Debug.Log($"Object scene: {player.scene.name}");
    }

    void SetState(int[,,] cards)
    // loads the states of each player
    {
        ResetPrepClientRpc();
        for (int i = 0; i < playerCount; i++)
        {
            // sendParams are used for us to send only to client no. i. 
            // NOTE: Its using Network's IDs, which is probably read only?
            //       Later on we have to store IDs on a separate array to allow swapping, shifting etc.
            var sendOnly = new ClientRpcParams();
            sendOnly.Send.TargetClientIds = new[] { NetworkManager.Singleton.ConnectedClientsIds[i] };

            // Relays the ID to both player AND his PrepRenderer
            SetIdClientRpc(i, playerCount, sendOnly);

            // Sets hand and prep
            // NOTE: preset amount of cards and colors, 
            //       we have to evaluate by colors and prep size
            int[] handCards = new int[] { cards[i, 0, 0], cards[i, 0, 1], cards[i, 0, 2] };

            // RPCs the Player ID and Cards for player's hand 
            GiveCardsClientRpc(handCards, sendOnly);


            int[] prepCards = new int[] { cards[i, 1, 0], cards[i, 1, 1], cards[i, 1, 2] };
            SetPrepClientRpc(prepCards);


            // Gives a turn to current player
            // if (i == currentTurn)
        }
        SetTurnClientRpc();

        LoadPrepsClientRpc();
    }

    [ClientRpc]
    void GiveCardsClientRpc(int[] handCards, ClientRpcParams clientParams)
    // each client receives only their data!
    {
        Player player = GameObject.FindWithTag("Player").GetComponent<Player>();
        if (!player) return;
        player.Receive(handCards);
    }

    [ClientRpc]
    void SetPrepClientRpc(int[] prepCards)
    // each client receives only their data!
    {
        PrepRenderer preper = GameObject.FindWithTag("Preper").GetComponent<PrepRenderer>();
        if (!preper) return;
        preper.Absorb(prepCards);
    }

    [ClientRpc]
    void LoadPrepsClientRpc()
    // each client receives only their data!
    {
        PrepRenderer preper = GameObject.FindWithTag("Preper").GetComponent<PrepRenderer>();
        if (!preper) return;
        preper.Demonstrate();
    }

    [ClientRpc]
    void ResetPrepClientRpc()
    // each client receives only their data!
    {
        PrepRenderer preper = GameObject.FindWithTag("Preper").GetComponent<PrepRenderer>();
        if (!preper) return;
        preper.Annihilate();
    }

    [ClientRpc]
    void SetIdClientRpc(int id, int playercount, ClientRpcParams clientParams)
    // sets the client's Player Id to player and his PrepRenderer
    {
        GameObject.FindWithTag("Player").GetComponent<Player>().SetId(id);
        GameObject.FindWithTag("Preper").GetComponent<PrepRenderer>().SetId(id, playercount);
    }

    [ClientRpc]
    void SetTurnClientRpc()
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
        Debug.Log(filtered.Length);
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
        int[] targets = new int[] { (id + 1) % playerCount };
        
        SpellEffect newSpell = CreateSpell(playerCards, id, targets);

        HandleNewSpell(newSpell);

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

    int GiveColorToCard()
    {
        return (new System.Random()).Next(3);
    }

    public void GiveCardToPlayer(int id)
    {
        int color = GiveColorToCard();
        this.playerCards[id, 0, color]++;
        SetState(playerCards);
    }

    SpellEffect CreateSpell(int[,,] playerCards, int playerId, int[] targets)
    {
        SpellEffect newSpell = (SpellEffect)ScriptableObject.CreateInstance(this.pairEffects[playerCards[playerId, 0, 0], playerCards[playerId, 0, 1]]);
        newSpell.InitializeSpell(playerId, targets);
        return newSpell;
    }

    void HandleNewSpell(SpellEffect spell)
    {
        spell.OnCast(spell);
    }
}
