using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.Netcode;

public class EnemyRender : MonoBehaviour
{
    public int enemyCount;
    Vector3[] baseEnemyPos;
    [SerializeField] Transform[] EnemyPrepPos;
    [SerializeField] GameObject[,] EnemyPreps;
    [SerializeField] Transform empty;

    [SerializeField] GameObject prepPrefab;
    [SerializeField] GameObject cardPrefab;

    void Awake()
    {
    }


    void Update()
    {
    }

    public void SetEnemies(int[,] cards)
    {
        VisualizeSetCards(cards);
    }

    void VisualizeSetCards(int[,] cards)
    {
    }
    void VisualizePrep(int minWidth, bool visible = true)
    {
    }

    void OnDestroy()
    {
        Debug.Log("FUCCCCCCCCCCCCCCCCCCCCCCCCK");
    }
}
