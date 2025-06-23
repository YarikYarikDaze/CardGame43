using UnityEngine;
using System.Collections.Generic;
using Unity.Netcode;

public class CardDeckScript : MonoBehaviour
{
    public int color;
    public int remaining = 0;
    [SerializeField] Sprite[] nums;
    [SerializeField] Sprite[] possibleColors;
    [SerializeField] GameObject remShow;

    [SerializeField] GameObject hand;

    SpriteRenderer renderer;

    void Update()
    {
    }

    public void InitializeHand()
    {
        this.hand = GameObject.FindWithTag("Hand");
    }
    public void Display(int remaining)
    {
        this.InitializeHand();
        this.renderer = gameObject.GetComponent<SpriteRenderer>();
        this.remaining = remaining;
        this.SetColor();
        this.SetRemaining();
    }

    void SetRemaining()
    {
        remShow.SetActive(true);
        remShow.GetComponent<SpriteRenderer>().sprite = nums[remaining];
    }
    void SetColor()
    {
        renderer.sprite = possibleColors[color];
    }
    void OnMouseOver(){
        if (Input.GetMouseButtonDown(0) && remaining > 0 && this.hand.GetComponent<HandScript>().getPlayer().GetComponent<Player>().Turn)
        {
            this.hand.GetComponent<HandScript>().getPlayer().GetComponent<Player>().LandCard(this.color);
        }
    }
}
