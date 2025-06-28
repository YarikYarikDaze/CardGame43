using UnityEngine;
using Unity.Netcode;
using System;
using System.Collections.Generic;
using System.Linq;

public class Player : MonoBehaviour
{
    [SerializeField] int id;                                // Player's ID.
    public           int ID { get { return id; } }
    [SerializeField] bool turn;                             // Determines wether its player's turn or not
    public           bool Turn { get { return turn; } }     // Getter for read-only
    [SerializeField] int maxMoves = 1;                      // Moves per turn
    public           int remainingMoves;                    // Counter of remaining moves
    public           int selected = -1;                     // Color of selected card
    // COLOR 0-1-2 r-y-b

    [Space(20)]

    [SerializeField] HandScript handScript;                 // Hand's script.
    public           PrepRenderer prepRenderer;               // ALL preps' renderer script.
    
    [Space(20)]

    [SerializeField] public GameObject deckPrefab;          // Prefab of a card to instantiate
    public           int[] handCards;                       // Array of cards in each Deck in Hand

    void Awake()
    // On spawn gets HS and ERS
    {
        handScript = GameObject.FindWithTag("Hand").GetComponent<HandScript>();
        // is Hand under Player? if yes, and player is a prefab, then we can just preset it
        if (handScript != null)
        {
            handScript.deckPrefab = deckPrefab;
            handScript.playerScript = this;
        }

        prepRenderer = GameObject.FindWithTag("Preper").GetComponent<PrepRenderer>();
    }

    void Update()
    // Add LandCard(selected) on A/D if `selected` is set
    {
        if (selected == -1) return;

        if (Input.GetKeyDown(KeyCode.A) && turn)
        {
            MoveCardServerRpc(selected, id, true);
            selected = -1;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveCardServerRpc(selected, id, false);
            selected = -1;
        }
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

    public void TakeTurn()
    // Start of this player's turn
    {
        turn = true;
        remainingMoves = maxMoves;
    }

    void SetCards(int[] newHandCards)
    // Sets Decks to player and passes on to Hand 
    {
        handCards = new int[newHandCards.Length];
        //COLOR 0-1-2 R-Y-B
        Array.Copy(newHandCards, handCards, newHandCards.Length);
        handScript.ReceiveCardsInHand(handCards);
    }


    [ServerRpc]
    void MoveCardServerRpc(int color, int id, bool left)
    // ARGUMENT COLOR 0-1-2 R-Y-B
    {
        GameManager.Instance?.PlaceCard(color, id, left);
    }

    [ServerRpc]
    public void CastServerRpc(int id)
    // unfinished Casting RPC
    {
        GameManager.Instance?.NewCast(id);
    }

    [ServerRpc]
    public void PassServerRpc()
    // unfinished Turn End RPC
    {
        GameManager.Instance?.NewTurn();
    }
    public void EndTurn()
    // End of turn. Envoked from server
    {
        turn = false;
        EndTurnServerRpc();
    }

    [ServerRpc]
    public void EndTurnServerRpc()
    // Tell server that turn is over.
    {
        GameManager.Instance?.NewTurn();
    }

    void OnDestroy()
    {
        Debug.Log("OOOOOOOOAAAAAAAAAAAAAHHH");
    }
}
