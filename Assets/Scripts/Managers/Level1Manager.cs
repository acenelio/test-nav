using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NavGame.Core;
using UnityEngine.UI;

public class Level1Manager : LevelManager
{
    public float EnemySpawnWaitTime = 1f;
    public float EnemySpawnInitialWait = 2f;
    public Text UIStudentText;
    public Text UIBookText;

    public Transform[] BookSpawnPoints;
    public Transform[] EnemySpawnPoints;
    public Transform[] Destinations;

    //GameObject[] Books;
    //GameObject[] Enemies;

    public GameObject BookPrefab;
    public GameObject EnemyPrefab;
    

    public float BookRespawnDelay = 10f;

    public float EnemyRespawnDelay = 10f;

    Level1Data LevelData;

    PlayerRanged PlayerScript;

    System.Random rand = new System.Random();

    bool Phase1;

    void Start()
    {
        LevelData = new Level1Data();
        //Books = new GameObject[BookSpawnPoints.Length];
        //Enemies = new GameObject[EnemySpawnPoints.Length];
        SpawnBooks();
        StartCoroutine(SpawnEnemies());

        GameObject player = PlayerManager.instance.GetPlayer();
        PlayerScript = player.GetComponent<PlayerRanged>();
        RangedCombatController combatController = player.GetComponent<RangedCombatController>();
        combatController.OnRangedAttackCast += DecrementBooks;
        combatController.OnRangedAttackCast += UpdateHudBooks;

        UpdateHud();
        Phase1 = true;
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
        GameObject newBook = Instantiate(BookPrefab, BookSpawnPoints[i].position, Quaternion.identity) as GameObject;
        Book book = newBook.GetComponent<Book>();
        book.OnPickup += () => { StartCoroutine(RespawnBook(i)); };
        book.OnPickup += () => { AddBooks(book.Amount); };
        book.OnPickup += UpdateHudBooks;
    }

    IEnumerator RespawnBook(int i)
    {
        yield return new WaitForSeconds(BookRespawnDelay);
        SpawnBook(i);
    }

    IEnumerator SpawnEnemies()
    {
        yield return new WaitForSeconds(EnemySpawnInitialWait);
        while (Phase1) {
            for (int i = 0; Phase1 && i < EnemySpawnPoints.Length; i++)
            {
                SpawnEnemy(i);
                yield return new WaitForSeconds(EnemySpawnWaitTime);
            }
        }
    }

    void SpawnEnemy(int i)
    {
        GameObject newEnemy = Instantiate(EnemyPrefab, EnemySpawnPoints[i].position, Quaternion.identity) as GameObject;
        KamikazeEnemy script = newEnemy.GetComponent<KamikazeEnemy>();
        
        int randomIndex = rand.Next(Destinations.Length); 
        script.Init(Destinations[randomIndex].position);

        //AggroableEnemy character = Enemies[i].GetComponent<AggroableEnemy>();
        //character.OnDied += () => { StartCoroutine(RespawnEnemy(i)); };
        //character.OnCharacterSaved += AddStudent;
        //character.OnCharacterSaved += UpdateHudStudents;
    }

    IEnumerator RespawnEnemyXXX(int i)
    {
        yield return new WaitForSeconds(EnemyRespawnDelay);
        SpawnEnemy(i);
    }

    void AddStudent()
    {
        LevelData.StudentCount++;
    }

    void AddBooks(int count)
    {
        PlayerScript.Ammo += count;
    }

    void DecrementBooks()
    {
        if (PlayerScript.Ammo > 0)
        {
            PlayerScript.Ammo--;
        }
    }

    void UpdateHudStudents()
    {
        UIStudentText.text = "x " + LevelData.StudentCount;
    }

    void UpdateHudBooks()
    {
        UIBookText.text = "x " + PlayerScript.Ammo;
    }    

    void UpdateHud() 
    {
        UpdateHudStudents();
        UpdateHudBooks();
    }
}
