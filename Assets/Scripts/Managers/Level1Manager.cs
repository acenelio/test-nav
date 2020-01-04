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
        SpawnBooks();
        SpawnEnemies();
    }

    void SpawnBooks()
    {
        for (int i = 0; i < BookSpawnPoints.Length; i++)
        {
            SpawnBook(i);
        }
    }

    void SpawnBook(int i)
    {
        Books[i] = Instantiate(BookPrefab, BookSpawnPoints[i].position, Quaternion.identity) as GameObject;
        Collectible collectible = Books[i].GetComponent<Collectible>();
        collectible.OnPickup += col => { StartCoroutine(RespawnBook(i)); };
    }

    IEnumerator RespawnBook(int i)
    {
        yield return new WaitForSeconds(BookRespawnDelay);
        SpawnBook(i);
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < EnemySpawnPoints.Length; i++)
        {
            SpawnEnemy(i);
        }
    }

    void SpawnEnemy(int i)
    {
        Enemies[i] = Instantiate(EnemyPrefab, EnemySpawnPoints[i].position, Quaternion.identity) as GameObject;
        Character character = Enemies[i].GetComponent<Character>();
        character.OnDied += () => { StartCoroutine(RespawnEnemy(i)); };
    }

    IEnumerator RespawnEnemy(int i)
    {
        yield return new WaitForSeconds(EnemyRespawnDelay);
        SpawnEnemy(i);
    }
}
