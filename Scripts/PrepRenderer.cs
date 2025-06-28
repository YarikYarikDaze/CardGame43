using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.Netcode;

public class PrepRenderer : MonoBehaviour
// bro why the fuck is it all empty
{
    int playerId;
    public int prepCount;
    Vector3[] basePos;

    int counter;
    public int[,] prepCards;
    [SerializeField] GameObject[] Breps;
    [SerializeField] GameObject prepPrefab;

    public void SetId(int newId, int allplayers)
    {
        playerId = newId;
        basePos = new Vector3[]{
            new Vector3(0f, -4f, 0f),
            new Vector3(0f, 4f, 180f),
            new Vector3(-6f, 0f, -90f),
            new Vector3(6f, 0f, 90f)
        };
        prepCount = allplayers;
        prepCards = new int[prepCount,3];
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
    int shift() {
        return (prepCount * 2 - playerId) % prepCount;
    }
    public void Demonstrate()
    {
        Breps = new GameObject[prepCount];
        for (int i = 0; i < prepCount; i++)
        {
            Breps[i] = Instantiate(prepPrefab);
            Breps[i].transform.position = new Vector3(
                basePos[shift()].x,
                basePos[shift()].y,
                0f
            );

            Breps[i].transform.rotation = Quaternion.Euler(0f, 0f, basePos[shift()].z);

            Breps[i].GetComponent<PrepScript>().SetCards(new int[]{prepCards[i, 0],prepCards[i, 1],prepCards[i, 2]});
        }
    }
    
}
