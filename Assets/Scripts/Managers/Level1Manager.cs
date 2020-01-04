using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NavGame.Core;

public class Level1Manager : MonoBehaviour
{
    public Transform[] BookSpawnPoints;
    public Transform[] EnemySpawnPoints;

    public GameObject[] Books;
    GameObject[] Enemies;

    public GameObject BookPrefab;
    public GameObject EnemyPrefab;

    public float BookRespawnDelay = 10f;

    public float EnemyRespawnDelay = 10f;


    void Start()
    {
        Books = new GameObject[BookSpawnPoints.Length];
        Enemies = new GameObject[EnemySpawnPoints.Length];
        StartCoroutine(SpawnBooks());
        StartCoroutine(SpawnEnemies());
    }

    IEnumerator SpawnBooks()
    {
        while (true)
        {
            for (int i = 0; i < BookSpawnPoints.Length; i++)
            {
                if (Books[i] == null)
                {
                    Books[i] = Instantiate(BookPrefab, BookSpawnPoints[i].position, Quaternion.identity);
                    Collectible collectible = Books[i].GetComponent<Collectible>();
                    int index = i;
                    collectible.OnPickup += col => { Books[index] = null; };
                }
                else
                {
                    Debug.Log("Nao era nulo: " + i);
                }
                yield return new WaitForSeconds(BookRespawnDelay);
            }
        }
    }

    IEnumerator SpawnEnemies()
    {

            for (int i = 0; i < EnemySpawnPoints.Length; i++)
            {
                if (Enemies[i] != null)
                {
                    Enemies[i] = Instantiate(EnemyPrefab, EnemySpawnPoints[i].position, Quaternion.identity) as GameObject;
                }
                yield return new WaitForSeconds(EnemyRespawnDelay);
            }
        
    }
}
