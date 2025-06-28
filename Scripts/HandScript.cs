using UnityEngine;
using System;

public class HandScript : MonoBehaviour
{
    [SerializeField] int handSize = 3;              // Amount of decks, and hence amount of colors
    [SerializeField] int[] remainingCards;          // array with amounts of remaining cards

    [Space(20)]
    public           Player playerScript;           // PS, set by player, passed onto Decks for card selection
    public           GameObject deckPrefab;         // prefab of a DECK that's taken from player's cardPrefab?

    void InitializeCards(int size)
    // Resets all the decks in hand in accordance to server
    {
        EmptyHand();

        for (int i = 0; i < size; i++)
        {
            GameObject newDeck = Instantiate(deckPrefab);
            newDeck.GetComponent<CardDeckScript>().hand = this;
            newDeck.GetComponent<CardDeckScript>().color = i;
            // COLOR 0-1-2 R-Y-B

            newDeck.transform.parent = transform;

            newDeck.transform.position = new Vector3(
                transform.position.x + 2 * i, transform.position.y + 2, transform.position.z
            );
            // NOTE: remake positions!!
        }
    }

    public void ReceiveCardsInHand(int[] newCards)
    {
        InitializeCards(handSize);
        remainingCards = new int[newCards.Length];
        Array.Copy(newCards, remainingCards, newCards.Length);
        DisplayDecksInHand();
    }

    void EmptyHand()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    void DisplayDecksInHand()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            CardDeckScript cds = transform.GetChild(i).GetComponent<CardDeckScript>();
            if (cds == null) continue;

            cds.Display(remainingCards[i]);
        }
    }
}
