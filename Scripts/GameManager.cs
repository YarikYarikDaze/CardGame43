using System;
using System.Linq;
using UnityEngine;
using Unity.Netcode;
using System;
using System.Collections.Generic;
using System.Collections;

public class GameManager : NetworkBehaviour
{
    // ABOUT CARD VALUES
    // HAND/DECKS: 3 values, representing the amount of cards on each deck
    // Hence, 0-1-2 R-Y-B
    //
    // PREP/CARDS: 3 values, representing the color of each card in this prep,
    // INCLUDING LACK OF A CARD!
    // Hence, 0-1-2-3 X-R-Y-B
    int[,,] playerCards;           // i - players, j - hand/prep, k - cards
    [SerializeField] int currentTurn;               // turn counter
    [SerializeField] int playerCount;               // total player count
    [SerializeField] GameObject playerPrefab;       // prefab of a player
    [SerializeField] GameObject prepRenderPrefab;  // prefab of an prep renderer

    [SerializeField] int initCardAmount = 8;        // amount of cards to put in every hand

    List<SpellEffect>[] spellEffectsOnPlayers;      // for now there conditions on players are stored

    SpellManager spellManager;                      // spell manager

    int target;                          // indexes of targets for current cast

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
        this.InitializeSpellManager();
        playerCount = NetworkManager.Singleton.ConnectedClientsIds.Count;
        playerCards = new int[playerCount, 2, 3];
        spellEffectsOnPlayers = new List<SpellEffect>[playerCount];
        currentTurn = 0;


        for (int i = 0; i < playerCount; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                playerCards[i, 0, j] = 0;
                playerCards[i, 1, j] = 0;
            }

            spellEffectsOnPlayers[i] = new List<SpellEffect>();
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

    void InitializeSpellManager()
    {
        this.spellManager = gameObject.GetComponent<SpellManager>();
        this.spellManager.InitializeItself(this);
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
            SetTurnClientRpc(i == currentTurn, sendOnly);
        }

        LoadPrepsClientRpc(currentTurn);
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
    void LoadPrepsClientRpc(int order)
    // each client receives only their data!
    {
        PrepRenderer preper = GameObject.FindWithTag("Preper").GetComponent<PrepRenderer>();
        if (!preper) return;
        preper.Demonstrate(order);
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
    void SetTurnClientRpc(bool turn, ClientRpcParams clientParams)
    // sets the turn
    {
        GameObject.FindWithTag("Player").GetComponent<Player>().TakeTurn(turn);
    }

    public void PlaceCard(int color, int id, bool left)
    // ARGUMENT COLOR 0-1-2 R-Y-B
    {
        int newColor = color + 1;
        // COLOR 1-2-3 R-Y-B

        int[] oldPlacement = { playerCards[id, 1, 0], playerCards[id, 1, 1], playerCards[id, 1, 2] };

        var filtered = oldPlacement.Where(n => n != 0).ToArray();
        //Debug.Log(filtered.Length);
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
                return;
                newPlacement = oldPlacement;
                break;
        }

        for (int i = 0; i < 3; i++)
        {
            playerCards[id, 1, i] = newPlacement[i];
        }

        playerCards[id, 0, color]--;

        ChangePlayerRemainingMoves(id, -1);

        SetState(playerCards);
    }

    public void SpellCasted()
    {
        this.SetState(playerCards);
    }

    public void NewCast(int id)
    // unfinished casting, right now only erases the cards in prep
    {
        if (CantCastSpell(playerCards, id)) return;

        int[] cards = new int[] { playerCards[id, 1, 0], playerCards[id, 1, 1], playerCards[id, 1, 2] };

        spellManager.InstantiateSpell(id, cards);

        ChangePlayerRemainingMoves(id, -1);
    }

    void ChangePlayerRemainingMoves(int index, int change)
    {
        var sendOnly = new ClientRpcParams();
        sendOnly.Send.TargetClientIds = new[] { NetworkManager.Singleton.ConnectedClientsIds[index] };
        ChangePlayerRemainingMovesClientRpc(change, sendOnly);
    }

    [ClientRpc]
    void ChangePlayerRemainingMovesClientRpc(int change, ClientRpcParams clientParams)
    {
        GameObject.FindWithTag("Player").GetComponent<Player>().ChangeActionsLeft(change);
    }

    public void GetTarget(int index, SpellEffect spell)
    {
        if (spell.IsSelfCasted())
        {
            spellManager.InitializeSpell(spell, index, index);
            return;
        }
        PlayerChooseTarget(index, spell);
    }

    public void ClearPrepOfAPlayer(int index)
    {
        for (int i = 0; i < 3; i++)
        {
            playerCards[index, 1, i] = 0;
        }
        SetState(this.playerCards);
    }

    void PlayerChooseTarget(int index, SpellEffect spell)
    {
        this.target = -1;
        var sendOnly = new ClientRpcParams();
        sendOnly.Send.TargetClientIds = new[] { NetworkManager.Singleton.ConnectedClientsIds[index] };
        ChooseTargetClientRpc(sendOnly);

        StartCoroutine(WaitUntilTargetsIsChosen(spell, index));
    }

    IEnumerator WaitUntilTargetIsChosen(SpellEffect spell, int index)
    {
        yield return new WaitUntil(() => target != -1);

        spellManager.InitializeSpell(spell, index, this.target);
    }

    [ClientRpc]
    void ChooseTargetsClientRpc(int N, ClientRpcParams clientParams)
    // sets the turn
    {
        GameObject.FindWithTag("Player").GetComponent<Player>().ChooseTarget();
    }

    public void AcceptTargetFromPlayer(int newTarget)
    {
        this.target = newTarget;
    }

    bool CantCastSpell(int[,,] playerCards, int id)
    {
        if (playerCards[id, 1, 0] == 0 || playerCards[id, 1, 1] == 0) return true;
        return false;
    }

    public void NewTurn()
    // next turn!
    {
        // NOTE: add Previous() and Next()
        currentTurn = NextPlayer(currentTurn);
        spellManager.TraverseEffectsOnTurn(currentTurn);
        SetState(playerCards);
    }

    int GiveColorToCard()
    {
        return (new System.Random()).Next(3);
    }

    public void GiveCardToPlayer(int index)
    // Gives Card to <index> player. Color defined by GiveColorToCard()
    {
        int color = GiveColorToCard();
        this.playerCards[index, 0, color]++;
        SetState(playerCards);
    }

    public List<SpellEffect> GetEffectsOnPlayer(int index)
    {
        return this.spellEffectsOnPlayers[index];
    }

    public void SetEffectsOnPlayer(int index, List<SpellEffect> newEffects)
    {
        this.spellEffectsOnPlayers[index] = new List<SpellEffect>(newEffects);
        Debug.Log(spellEffectsOnPlayers[index].Count);
    }

    public void AddEffect(int index, SpellEffect newSpell)
    {
        this.spellEffectsOnPlayers[index].Add(newSpell);
    }

    public void ForceEndPlayerTurn()
    {
        this.NewTurn();
    }

    public int PreviousPlayer(int index)
    {
        return (index - 1) % playerCount;
    }

    public int NextPlayer(int index)
    {
        return (index + 1) % playerCount;
    }


    public int ChooseCard(int index)
    {
        int CardsOnHand = GetCardsCountPrep(index);
        if (CardsOnHand == 0) return -1;

        int CardNumber = (new System.Random()).Next(CardsOnHand);

        return CardNumber;
    }

    public int GetCardsCountPrep(int index)
    {
        int CardsOnHand = 0;
        for (int i = 0; i < 3; i++)
        {
            if (playerCards[index, 1, i] != 0) CardsOnHand++;
        }
        return CardsOnHand;
    }

    public int RemoveCardFromPrep(int index, int cardNumber)
    {
        int previousColor = playerCards[index, 1, cardNumber];
        for (int i = cardNumber; i < 2; i++)
        {
            playerCards[index, 1, i] = playerCards[index, 1, i + 1];
        }
        playerCards[index, 1, 2] = 0;
        return previousColor;
    }

    public void AddCardToPrep(int index, int cardNumber, int color)
    {
        playerCards[index, 1, cardNumber] = color;
    }

    public int GetRandomPlayer()
    {
        return (new System.Random()).Next(playerCount);
    }

    public void ReturnSpellToPrep(int index)
    {
        int CardCount = GetCardsCountPrep(index);
        for (int i = 0; i < CardCount; i++)
        {
            playerCards[index, 0, playerCards[index, 1, i]]++;
            playerCards[index, 1, i] = 0;
        }
    }
}
