using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.Netcode;

public class PrepRenderer : MonoBehaviour
// bro why the fuck is it all empty
{
    int playerId;
    public int prepCount=2;
    Vector3[] basePos;

    [SerializeField] int counter;
    public int[,] prepCards;
    [SerializeField] GameObject[] Breps;
    [SerializeField] GameObject prepPrefab;

    public void Awake()
    {
        basePos = new Vector3[]{
            new Vector3(0f, -3f, 0f),
            new Vector3(0f, 3f, 180f),
            new Vector3(-6f, 0f, -90f),
            new Vector3(6f, 0f, 90f)
        };
        prepCards = new int[2, 3];
        Debug.Log(prepCards.GetLength(0) + ";   " + prepCards.GetLength(1));

    }

    public void SetId(int newId, int allplayers)
    {
        playerId = newId;
        prepCount = allplayers;
    }
    public void Annihilate()
    {
        foreach (GameObject gm in Breps)
        {
            Destroy(gm);
        }
        counter = 0;
    }
    public void Absorb(int[] newPrepCards)
    {
        Debug.Log(counter + ";   "+ prepCards.GetLength(0) + ";   " + prepCards.GetLength(1));
        for (int i = 0; i < 3; i++)
        {
            prepCards[counter, i] = newPrepCards[i];
        }
        for (int i = 0; i < prepCount; i++)
        {
            Debug.Log(prepCards[i, 0]+", "+prepCards[i, 1]+", "+prepCards[i, 2]);
        }
        counter++;
    }
    int shift(int i) {
        return (prepCount * 2 - playerId + i) % prepCount;
    }
    public void Demonstrate()
    {
        //Debug.Log("Hey!");
        Breps = new GameObject[prepCount];
        for (int i = 0; i < prepCount; i++)
        {
            Breps[i] = Instantiate(prepPrefab);
            Breps[i].transform.position = new Vector3(
                basePos[shift(i)].x,
                basePos[shift(i)].y,
                0f
            );
        
            Breps[i].transform.rotation = Quaternion.Euler(0f, 0f, basePos[shift(i)].z);

            Breps[i].GetComponent<PrepScript>().SetCards(new int[]{prepCards[i, 0],prepCards[i, 1],prepCards[i, 2]});
        }
    }
    
}
