using UnityEngine;
using Unity.Netcode;
using System;
using System.Collections.Generic;
using System.Linq;

public class Player : MonoBehaviour
{


    [SerializeField] Transform handPos;
    [SerializeField] Transform prepPos;
    [SerializeField] GameObject hand;
    [SerializeField] GameObject[] prep;



    bool turn;
    public bool Turn { get { return turn; } }
    int maxMoves = 1;
    int remainingMoves;

    [SerializeField] int[] prepCards;
    public int[] handCards;
    [SerializeField] public GameObject cardPrefab;

    [SerializeField] int id;


    [SerializeField] float mainRadius;
    public Transform selected;

    public EnemyRender enemyRender;

    void Awake()
    {
        InitializeItself();
    }

    void InitializeItself()
    {
        this.hand = GameObject.FindWithTag("Hand");
        enemyRender = GameObject.FindWithTag("EnemyRender").GetComponent<EnemyRender>();
    }

    void Update()
    {
    }

    public void SetId(int id)
    {
        this.id = id;
    }

    public void Receive(int[] handCards, int[] prepCards)
    {
        SetCards(handCards);
        SetPrep(prepCards);
    }

    public void TakeTurn()
    {
        this.turn = true;
    }

    void SetCards(int[] newHandCards)
    {
        this.handCards = new int[newHandCards.Length];
        Array.Copy(newHandCards, this.handCards, newHandCards.Length);
        hand.GetComponent<HandScript>().ReceiveCardsInHand(handCards);    }

    void SetPrep(int[] newPrepCards)
    {
        this.prepCards = new int[newPrepCards.Length];
        Array.Copy(newPrepCards, this.prepCards, newPrepCards.Length);
    }


    public int prepLen()
    {
        return 1;
    }


    public void LandCard(int color)
    {
        this.MoveCardServerRpc(color, id, true);
    }

    [ServerRpc]
    void MoveCardServerRpc(int color, int id, bool left)
    {
        GameManager.Instance?.PlaceCard(color, id, left);
    }

    [ServerRpc]
    void CastServerRpc(int id)
    {
    }

    [ServerRpc]
    void PassServerRpc()
    {
    }

    void OnDestroy()
    {
        Debug.Log("OOOOOOOOAAAAAAAAAAAAAHHH");
    }
}
