using UnityEngine;
using System;

public class HandScript : MonoBehaviour
{
    GameObject player;

    int[] cardsInHand;

    GameObject cardPrefab;

    GameObject[] cards;

    void InitializePlayer()
    {
        if (player == null)
        {
            this.player = GameObject.FindWithTag("Player");
        }
    }

    void InitializeCards()
    {
        if (cards == null)
        {
            this.cardPrefab = player.GetComponent<Player>().cardPrefab;
            this.cards = new GameObject[3];
            for (int i = 0; i < cards.Length; i++)
            {
                cards[i] = Instantiate(cardPrefab);
                cards[i].GetComponent<CardDeckScript>().color = i + 1;
                cards[i].transform.parent = transform;
                cards[i].transform.position = new Vector3(
                    transform.position.x + 2 * i, transform.position.y + 2, transform.position.z
                );
            }
        }
    }

    void InitializeItself()
    {
        InitializePlayer();
        InitializeCards();
    }

    public void ReceiveCardsInHand(int[] newCards)
    {
        this.InitializeItself();
        this.cardsInHand = new int[newCards.Length];
        Array.Copy(newCards, cardsInHand, newCards.Length);
        DisplayCardsInHand();
    }

    void DisplayCardsInHand()
    {
        for (int i = 0; i < cards.Length; i++)
        {
            cards[i].GetComponent<CardDeckScript>().Display(cardsInHand[i]);
        }
    }

    public GameObject getPlayer()
    {
        return this.player;
    }
}
