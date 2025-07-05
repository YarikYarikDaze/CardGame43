using UnityEngine;
using System.Collections.Generic;
using Unity.Netcode;

public class CardDeckScript : MonoBehaviour
{
    public int color;                           // color id
    // COLOR 0-1-2 r-y-b
    public int remaining = 0;                   // the number of remaining cards under the deck
    
    [Space(20)]

    [SerializeField] Sprite[] possibleNums;     // array of digit sprites on the deck to show amount of cards
    [SerializeField] Sprite[] possibleColors;   // array of possible card colors

    [Space(20)]

    public HandScript hand;                     // literal gameobject of hand, 
    [SerializeField] GameObject remCounter;     // the GameObject that shows the amount of remaining cards

    [Space(20)]

    SpriteRenderer deckRenderer;                // the deck's Sprite Renderer 

    void Update()
    {
    }

    public void Display(int remInit)
    // doesnt seem to be referenced???
    // deck initializer is updated on server calls (trickling down to player, then here)
    {
        deckRenderer = gameObject.GetComponent<SpriteRenderer>();
        remaining = remInit;
        SetColor();
        SetRemaining();
    }

    void SetRemaining()
    // sets current amount to the counter
    {
        remCounter.SetActive(true);
        remCounter.GetComponent<SpriteRenderer>().sprite = possibleNums[remaining];
    }
    void SetColor()
    // sets the deck's appropriate card color
    {
        deckRenderer.sprite = possibleColors[color];
        // COLOR 0-1-2 r-y-b
    }
    void OnMouseOver()
    // for some reason calls LandCard on click. todo: change to Select?
    {
        if (Input.GetMouseButtonDown(0) && remaining > 0 && hand.playerScript.Turn && hand.playerScript.remainingMoves > 0)
        {
            hand.playerScript.Select(color);
            // COLOR 0-1-2 r-y-b
            // Debug.Log("Fingers in His Ass");
        }
        else
        {
            // Debug.Log(Input.GetMouseButtonDown(0) + " " + (remaining > 0) + " " + hand.playerScript.Turn + " " + (hand.playerScript.remainingMoves > 0));
        }
    }
}
