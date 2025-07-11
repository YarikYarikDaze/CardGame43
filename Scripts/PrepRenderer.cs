using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.Netcode;

public class PrepRenderer : MonoBehaviour
// bro why the fuck is it all empty
{
    int playerId;
    public int prepCount;
    Vector3[,] basePos;

    [SerializeField] int counter;
    public int[,] prepCards;
    [SerializeField] GameObject[] Breps;
    [SerializeField] GameObject prepPrefab;

    public void Awake()
    {
        prepCount = NetworkManager.Singleton.ConnectedClientsIds.Count;
        Debug.Log(prepCount);
        Vector3[] PlPos = new Vector3[]
        {
            new Vector3(0f, -3f, 0f),       // YOU
            new Vector3(-6f, 0f, -90f),     // LEFT
            new Vector3(-6f, 3f, 180f),     // OPPOSITE LEFT
            new Vector3(0f, 3f, 180f),      // OPPOSITE CENTER
            new Vector3(6f, 3f, 180f),      // OPPOSITE RIGHT
            new Vector3(6f, 0f, 90f)        // RIGHT
        };
        Vector3 empty = new Vector3(0f, 0f, 0f);
        basePos = new Vector3[,]{
            {PlPos[0], empty, empty, empty, empty, empty},
            {PlPos[0], PlPos[3], empty, empty, empty, empty},
            {PlPos[0], PlPos[2], PlPos[4], empty, empty, empty},
            {PlPos[0], PlPos[1], PlPos[3], PlPos[5], empty, empty},
            {PlPos[0], PlPos[1], PlPos[2], PlPos[4], PlPos[5], empty},
            {PlPos[0], PlPos[1], PlPos[2], PlPos[3], PlPos[4], PlPos[5]}
        };
        prepCards = new int[prepCount, 3];
        // Debug.Log(prepCards.GetLength(0) + ";   " + prepCards.GetLength(1));

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
        // Debug.Log(counter + ";   "+ prepCards.GetLength(0) + ";   " + prepCards.GetLength(1));
        for (int i = 0; i < 3; i++)
        {
            prepCards[counter, i] = newPrepCards[i];
        }
        for (int i = 0; i < prepCount; i++)
        {
            // Debug.Log(prepCards[i, 0]+", "+prepCards[i, 1]+", "+prepCards[i, 2]);
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
                basePos[prepCount - 1, shift(i)].x,
                basePos[prepCount - 1, shift(i)].y,
                0f
            );



            Breps[i].transform.rotation = Quaternion.Euler(0f, 0f, basePos[prepCount - 1, shift(i)].z);

            Breps[i].GetComponent<PrepScript>().SetCards(new int[] { prepCards[i, 0], prepCards[i, 1], prepCards[i, 2] });
            Breps[i].GetComponent<PrepScript>().id = i;
        }
    }
    
}
