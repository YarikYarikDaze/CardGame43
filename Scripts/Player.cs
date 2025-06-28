using UnityEngine;
using Unity.Netcode;
using System;
using System.Collections.Generic;
using System.Linq;

public class Player : MonoBehaviour
{
    [SerializeField] int id;                                // Player's ID.
    [SerializeField] bool turn;                             // Determines wether its player's turn or not
    public           bool Turn { get { return turn; } }     // Getter for read-only
    [SerializeField] int maxMoves = 1;                      // Moves per turn
    [SerializeField] int remainingMoves;                    // Counter of remaining moves
    public           int selected;                          // Color of selected card
    // COLOR 0-1-2 r-y-b

    [Space(20)]

    [SerializeField] HandScript handScript;                 // Hand's script.
    public           EnemyRender enemyRender;               // Enemy renderer script.
    
    [Space(20)]

    [SerializeField] public GameObject deckPrefab;          // Prefab of a card to instantiate
    public           int[] handCards;                       // Array of cards in each Deck in Hand
    [SerializeField] int[] prepCards;                       // Array of cards on Prep
    [SerializeField] GameObject[] prep;                     // Prep's gameobjects???

    void Awake()
    // On spawn gets HS and ERS
    {
        handScript = GameObject.FindWithTag("Hand").GetComponent<HandScript>();
        // is Hand under Player? if yes, and player is a prefab, then we can just preset it
        if (handScript != null)
        {
            handScript.deckPrefab = cardPrefab;
            handScript.playerScript = this;
        }

        enemyRender = GameObject.FindWithTag("EnemyRender").GetComponent<EnemyRender>();
    }

    void Update()
    // Add LandCard(selected) on A/D if `selected` is set
    {
    }

    public void SetId(int id)
    // Sets ID.
    {
        this.id = id;
    }

    public void Receive(int[] handCards, int[] prepCards)
    // Receiver of both Hand and Prep cards, called on GMS 
    {
        SetCards(handCards);
        SetPrep(prepCards);
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

    void SetPrep(int[] newPrepCards)
    // unfinished Prep setter!
    {
        prepCards = new int[newPrepCards.Length];
        Array.Copy(newPrepCards, prepCards, newPrepCards.Length);
    }
    public void LandCard(int color)
    // ARGUMENT COLOR 0-1-2 R-Y-B
    {
        MoveCardServerRpc(color, id, true);
    }

    [ServerRpc]
    void MoveCardServerRpc(int color, int id, bool left)
    // ARGUMENT COLOR 0-1-2 R-Y-B
    {
        GameManager.Instance?.PlaceCard(color, id, left);
    }

    [ServerRpc]
    void CastServerRpc(int id)
    // unfinished Casting RPC
    {
    }

    [ServerRpc]
    void PassServerRpc()
    // unfinished Turn End RPC
    {
    }

    void OnDestroy()
    {
        Debug.Log("OOOOOOOOAAAAAAAAAAAAAHHH");
    }
}
