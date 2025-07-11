using UnityEngine;
using Unity.Netcode;
using System;
using System.Collections.Generic;
using System.Linq;

public class Player : NetworkBehaviour
{
    [SerializeField] int id;                                // Player's ID.
    public           int ID { get { return id; } }
    [SerializeField] bool turn;                             // Determines wether its player's turn or not
    public           bool Turn { get { return turn; } }     // Getter for read-only
    [SerializeField] int maxMoves = 1;                      // Moves per turn
    public           int remainingMoves;                    // Counter of remaining moves

    [Space(20)]

    [SerializeField] HandScript handScript;                 // Hand's script.
    public           PrepRenderer prepRenderer;               // ALL preps' renderer script.
    
    [Space(20)]

    [SerializeField] public GameObject deckPrefab;          // Prefab of a card to instantiate
    public           int[] handCards;                       // Array of cards in each Deck in Hand

    void Awake()
    // On spawn gets HS and ERS
    {
        handScript = transform.GetChild(0).gameObject.GetComponent<HandScript>();
        // is Hand under Player? if yes, and player is a prefab, then we can just preset it
        if (handScript != null)
        {
            handScript.deckPrefab = deckPrefab;
            handScript.playerScript = this;
        }

    }

    void Update()
    // Add LandCard(selected) on A/D if `selected` is set
    {
        if(!prepRenderer)
            prepRenderer = GameObject.FindWithTag("Preper").GetComponent<PrepRenderer>();

        //Debug.Log(GetComponent<NetworkObject>().IsSpawned);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Debug.Log(IsOwner);
        }

        // if (!IsOwner) return;
    }


    public void SetId(int id)
    // Sets ID.
    {
        this.id = id;
    }

    public void Receive(int[] handCards)
    // Receiver of both Hand and Prep cards, called on GMS 
    {
        SetCards(handCards);
    }

    public void TakeTurn(bool turn)
    // Start of this player's turn
    {
        this.turn = turn;
        remainingMoves = turn ? maxMoves : 0;
    }

    void SetCards(int[] newHandCards)
    // Sets Decks to player and passes on to Hand 
    {
        handCards = new int[newHandCards.Length];
        //COLOR 0-1-2 R-Y-B
        Array.Copy(newHandCards, handCards, newHandCards.Length);
        handScript.ReceiveCardsInHand(handCards);
    }


    public void MoveCard(int color, bool left)
    {
        MoveCardServerRpc(color, id, left);
        remainingMoves--;
    }

    [ServerRpc(RequireOwnership = false)]
    void MoveCardServerRpc(int color, int id, bool left, ServerRpcParams rpcParams = default)
    // ARGUMENT COLOR 0-1-2 R-Y-B
    {
        ulong senderId = rpcParams.Receive.SenderClientId;
    
        // Validate sender exists
        if (!NetworkManager.Singleton.ConnectedClients.ContainsKey(senderId))
        {
            // Debug.LogError($"Unknown client ID: {senderId}");
            return;
        }

        GameObject.FindWithTag("GameManager").GetComponent<GameManager>().PlaceCard(color, id, left);
        // Debug.Log("AAAAAA");
        if (!NetworkManager.Singleton.ConnectedClients.ContainsKey((ulong)id))
        {
            // Debug.LogError($"Unknown client ID: {(ulong)id}");
            return;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void CastServerRpc(int id, ServerRpcParams rpcParams = default)
    // unfinished Casting RPC
    {
        GameObject.FindWithTag("GameManager").GetComponent<GameManager>().NewCast(id);
    }

    public void CastSpell()
    {
        if (Turn)
        {
            this.CastServerRpc(this.id);
            remainingMoves--;
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void PassServerRpc(ServerRpcParams rpcParams = default)
    // unfinished Turn End RPC
    {
        GameObject.FindWithTag("GameManager").GetComponent<GameManager>().NewTurn();
    }
    public void EndTurn()
    // End of turn. Envoked from server
    {
        if (!turn) return;

        EndTurnServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    public void EndTurnServerRpc(ServerRpcParams rpcParams = default)
    // Tell server that turn is over.
    {
        GameObject.FindWithTag("GameManager").GetComponent<GameManager>().NewTurn();
    }


    public void Kill(int id)
    {
        if (this.id != id)
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        // Debug.Log("OOOOOOOOAAAAAAAAAAAAAHHH");
    }
}
