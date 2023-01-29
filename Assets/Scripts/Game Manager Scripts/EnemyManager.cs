using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    [SerializeField]
    private GameObject boarPrefab, cannibalPrefab;

    public Transform[] cannibalSpawnPoints, boarSpawnPoints;

    [SerializeField]
    private int boarCount, cannibalCount;

    private int initialCannibalCount, initialBoarCount;

    public float waitBeforeSpawnEnemyTimes = 10f;


    void Awake()
    {
        MakeInstance();
    }

    private void Start()
    {
        initialCannibalCount = cannibalCount;
        initialBoarCount = boarCount;

        SpawnEnemies();

        StartCoroutine("CheckToSpawnEnemies");
    }

    void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void SpawnEnemies()
    {
        SpawnCannibals();
        SpawnBoars();
    }

    void SpawnCannibals()
    {
        int index = 0;

        for (int i = 0; i < cannibalCount; i++)
        {
            if (index >= cannibalSpawnPoints.Length)
            {
                index = 0;
            }

            Instantiate(cannibalPrefab, cannibalSpawnPoints[index].position, Quaternion.identity);

            index++;
        }

        cannibalCount = 0;
    }

    void SpawnBoars()
    {
        int index = 0;

        for (int i = 0; i < boarCount; i++)
        {
            if (index >= boarSpawnPoints.Length)
            {
                index = 0;
            }

            Instantiate(boarPrefab, boarSpawnPoints[index].position, Quaternion.identity);

            index++;
        }

        boarCount = 0;
    }

    IEnumerator CheckToSpawnEnemies()
    {
        yield return new WaitForSeconds(waitBeforeSpawnEnemyTimes);

        SpawnCannibals();
        SpawnBoars();

        StartCoroutine("CheckToSpawnEnemies");
    }

    public void EnemyDied(bool cannibal)
    {
        if (cannibal)
        {
            cannibalCount++;

            if (cannibalCount > initialCannibalCount)
            {
                cannibalCount = initialCannibalCount;
            }
        }
        else
        {
            boarCount++;

            if (boarCount > initialBoarCount)
            {
                boarCount = initialBoarCount;
            }

        }
    }

    public void StopSpawning()
    {
        StopCoroutine("CheckToSpawnEnemies");
    }
}
