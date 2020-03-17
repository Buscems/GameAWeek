using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    //keeping track of how many times the enemy types have been spawned
    int cycleCount;
    float cycleCountMax;
    int enemyCycles;
    float enemyCyclesMax;

    [Header("Enemies to spawn")]
    public GameObject enemyToSpawn1;
    public GameObject enemyToSpawn2;
    public GameObject enemyToSpawn3;
    public GameObject enemyToSpawn4;
    public GameObject enemyToSpawn5;
    public GameObject enemyToSpawn6;

    [Header("How long until the enemy gets spawned")]
    [Tooltip("The lower the number, the faster the enemies spawn")]
    public float spawnTimer;
    [Tooltip("Start point for the timer, should be the same number as the spawnTimer")]
    public float spawnTimerStart;
    [Space(10)]

    [Header("Interval between enemy spawns")]
    [Tooltip("Time between enemy of the same path")]
    public float enemySpawnInterval;
    [Tooltip("Time between enemy paths")]
    public float enemyWaveInterval;
    [Tooltip("Time in between enemy wave is dead and the boss gets spawned")]
    public float spawnBossTimer;
    [Tooltip("Time it takes to start fast enemy spawn")]
    public Vector2 fastEnemyTimerMinMax;
    float fastEnemyTimer;
    [Tooltip("Time it takes for enemy to spawn after warning sign")]
    public float spawnFastEnemy;
    [Space(10)]

    [Header("Which enemy to spawn")]
    public bool enemyLeftMost;
    public bool enemyLeft;
    public bool enemyRight;
    public bool enemyRightMost;
    bool spawnEnemy1, spawnEnemy2, spawnEnemy3, spawnEnemy4;

    public static bool startGame;

    // Start is called before the first frame update
    void Start()
    {
        enemyCycles = 0;
        enemyLeftMost = true;
        enemyLeft = false;
        enemyRight = false;
        enemyRightMost = false;
        startGame = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (startGame)
        {
            spawnTimer -= Time.deltaTime;

            if (spawnTimer <= 0 && enemyLeftMost)
            {
                spawnEnemy1 = true;
            }
            if (spawnTimer <= 0 && enemyLeft)
            {
                spawnEnemy2 = true;
            }
            if (spawnTimer <= 0 && enemyRight)
            {
                spawnEnemy3 = true;
            }
            if (spawnTimer <= 0 && enemyRightMost)
            {
                spawnEnemy4 = true;
            }

            if (spawnEnemy1)
            {
                StartCoroutine(SpawnEnemy1());
                spawnEnemy1 = false;
            }
            if (spawnEnemy2)
            {
                StartCoroutine(SpawnEnemy2());
                spawnEnemy2 = false;
            }
            if (spawnEnemy3)
            {
                StartCoroutine(SpawnEnemy3());
                spawnEnemy3 = false;
            }
            if (spawnEnemy4)
            {
                StartCoroutine(SpawnEnemy4());
                spawnEnemy4 = false;
            }
        }
    }

    public IEnumerator SpawnEnemy1()
    {
        enemyLeftMost = false;
        enemyLeft = false;
        enemyRight = false;
        enemyRightMost = false;
        Instantiate(enemyToSpawn1, enemyToSpawn1.transform.position, Quaternion.identity);
        spawnTimer = spawnTimerStart;
        //yield return new WaitForSeconds(enemySpawnInterval);
        yield return new WaitForSeconds(enemyWaveInterval);
        int randomNum = Random.Range(0, 4);
        if (randomNum == 0)
        {
            enemyLeftMost = true;
            enemyLeft = false;
            enemyRight = false;
            enemyRightMost = false;
        }
        if (randomNum == 1)
        {
            enemyLeftMost = false;
            enemyLeft = true;
            enemyRight = false;
            enemyRightMost = false;
        }
        if (randomNum == 2)
        {
            enemyLeftMost = false;
            enemyLeft = false;
            enemyRight = true;
            enemyRightMost = false;
        }
        if (randomNum == 3)
        {
            enemyLeftMost = false;
            enemyLeft = false;
            enemyRight = false;
            enemyRightMost = true;
        }
    }
    public IEnumerator SpawnEnemy2()
    {
        enemyLeftMost = false;
        enemyLeft = false;
        enemyRight = false;
        enemyRightMost = false;
        Instantiate(enemyToSpawn2, enemyToSpawn2.transform.position, Quaternion.identity);
        spawnTimer = spawnTimerStart;
        //yield return new WaitForSeconds(enemySpawnInterval);
        yield return new WaitForSeconds(enemyWaveInterval);
        int randomNum = Random.Range(0, 4);
        if (randomNum == 0)
        {
            enemyLeftMost = true;
            enemyLeft = false;
            enemyRight = false;
            enemyRightMost = false;
        }
        if (randomNum == 1)
        {
            enemyLeftMost = false;
            enemyLeft = true;
            enemyRight = false;
            enemyRightMost = false;
        }
        if (randomNum == 2)
        {
            enemyLeftMost = false;
            enemyLeft = false;
            enemyRight = true;
            enemyRightMost = false;
        }
        if (randomNum == 3)
        {
            enemyLeftMost = false;
            enemyLeft = false;
            enemyRight = false;
            enemyRightMost = true;
        }
    }
    public IEnumerator SpawnEnemy3()
    {
        enemyLeftMost = false;
        enemyLeft = false;
        enemyRight = false;
        enemyRightMost = false;
        Instantiate(enemyToSpawn3, enemyToSpawn3.transform.position, Quaternion.identity);
        spawnTimer = spawnTimerStart;
        //yield return new WaitForSeconds(enemySpawnInterval);
        yield return new WaitForSeconds(enemyWaveInterval);
        int randomNum = Random.Range(0, 4);
        if (randomNum == 0)
        {
            enemyLeftMost = true;
            enemyLeft = false;
            enemyRight = false;
            enemyRightMost = false;
        }
        if (randomNum == 1)
        {
            enemyLeftMost = false;
            enemyLeft = true;
            enemyRight = false;
            enemyRightMost = false;
        }
        if (randomNum == 2)
        {
            enemyLeftMost = false;
            enemyLeft = false;
            enemyRight = true;
            enemyRightMost = false;
        }
        if (randomNum == 3)
        {
            enemyLeftMost = false;
            enemyLeft = false;
            enemyRight = false;
            enemyRightMost = true;
        }
    }
    public IEnumerator SpawnEnemy4()
    {
        enemyLeftMost = false;
        enemyLeft = false;
        enemyRight = false;
        enemyRightMost = false;
        Instantiate(enemyToSpawn4, enemyToSpawn4.transform.position, Quaternion.identity);
        spawnTimer = spawnTimerStart;
        //yield return new WaitForSeconds(enemySpawnInterval);
        yield return new WaitForSeconds(enemyWaveInterval);
        int randomNum = Random.Range(0, 4);
        if (randomNum == 0)
        {
            enemyLeftMost = true;
            enemyLeft = false;
            enemyRight = false;
            enemyRightMost = false;
        }
        if (randomNum == 1)
        {
            enemyLeftMost = false;
            enemyLeft = true;
            enemyRight = false;
            enemyRightMost = false;
        }
        if (randomNum == 2)
        {
            enemyLeftMost = false;
            enemyLeft = false;
            enemyRight = true;
            enemyRightMost = false;
        }
        if (randomNum == 3)
        {
            enemyLeftMost = false;
            enemyLeft = false;
            enemyRight = false;
            enemyRightMost = true;
        }
    }

}
