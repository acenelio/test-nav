using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NavGame.Core;
using UnityEngine.UI;

public class Level1Manager : MonoBehaviour
{
    public static Level1Manager instance;

    public Text UIStudentText;
    public Text UIBookText;

    public Transform[] BookSpawnPoints;
    public Transform[] EnemySpawnPoints;

    public GameObject[] Books;
    GameObject[] Enemies;

    public GameObject BookPrefab;
    public GameObject EnemyPrefab;

    public float BookRespawnDelay = 10f;

    public float EnemyRespawnDelay = 10f;

    Level1Data LevelData;

    PlayerRanged PlayerScript;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LevelData = new Level1Data();
        Books = new GameObject[BookSpawnPoints.Length];
        Enemies = new GameObject[EnemySpawnPoints.Length];
        SpawnBooks();
        SpawnEnemies();

        GameObject player = PlayerManager.instance.GetPlayer();
        PlayerScript = player.GetComponent<PlayerRanged>();
        RangedCombatController combatController = player.GetComponent<RangedCombatController>();
        combatController.OnRangedAttackCast += DecrementBooks;
        combatController.OnRangedAttackCast += UpdateHudBooks;

        UpdateHud();
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
        Book book = Books[i].GetComponent<Book>();
        book.OnPickup += () => { StartCoroutine(RespawnBook(i)); };
        book.OnPickup += () => { AddBooks(book.Amount); };
        book.OnPickup += UpdateHudBooks;
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
        AggroableEnemy character = Enemies[i].GetComponent<AggroableEnemy>();
        character.OnDied += () => { StartCoroutine(RespawnEnemy(i)); };
        character.OnCharacterSaved += AddStudent;
        character.OnCharacterSaved += UpdateHudStudents;
    }

    IEnumerator RespawnEnemy(int i)
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
