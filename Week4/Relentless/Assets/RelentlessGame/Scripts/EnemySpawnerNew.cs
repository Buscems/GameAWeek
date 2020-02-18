using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.UI;

public class EnemySpawnerNew : MonoBehaviour
{

    public Transform topLeft, topRight, bottomLeft, bottomRight;

    public float spawnInterval;

    public static int currentWave;

    public int baseAmountOfEnemiesSpawnedPerRound;

    int enemiesBeingSpawned;

    int enemiesSpawnedThisRound;

    public GameObject basic, tough, shoot, fast, random;

    bool isSpawning;

    bool toughSpawn, shootSpawn, fastSpawn, randomSpawn;

    bool newWave;

    public float waveInterval;

    public Color[] numberColors;

    public TextMeshProUGUI numbers;

    bool gameStart;

    bool canBegin;

    // Start is called before the first frame update
    void Start()
    {
        gameStart = false;

        currentWave = 1;

    }

    // Update is called once per frame
    void Update()
    {

        if (!gameStart)
        {
            StartCoroutine(StartGame());
        }

        if (canBegin)
        {
            enemiesBeingSpawned = baseAmountOfEnemiesSpawnedPerRound + (int)Mathf.Pow(currentWave, 3);

            if (!isSpawning && !newWave)
            {
                StartCoroutine(SpawnEnemies());
            }
            if (enemiesSpawnedThisRound == enemiesBeingSpawned && !newWave)
            {
                StartCoroutine(NewWave());
            }

            if (currentWave == 5 && !toughSpawn)
            {
                toughSpawn = true;
            }
            if (currentWave == 10 && !shootSpawn)
            {
                shootSpawn = true;
            }
            if (currentWave == 15 && !fastSpawn)
            {
                fastSpawn = true;
            }
            if (currentWave == 20 && !randomSpawn)
            {
                randomSpawn = true;
            }
        }
    }

    IEnumerator StartGame()
    {
        gameStart = true;
        for (int i = 10; i > 0; i--)
        {
            var temp = i;
            temp -= 1;
            numbers.enabled = false;
            numbers.enabled = true;
            //numbers.color = numberColors[temp];
            numbers.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        numbers.enabled = false;
        canBegin = true;
    }

    IEnumerator SpawnEnemies()
    {
        isSpawning = true;
        int temp = Random.Range(0, 4);
        Vector2 spawnPoint = new Vector2(0, 0);
        //leftSide
        if (temp == 0)
        {
            spawnPoint.x = topLeft.position.x;
            spawnPoint.y = Random.Range(bottomLeft.position.y, topLeft.position.y);
        }
        //rightSide
        if (temp == 1)
        {
            spawnPoint.x = topRight.position.x;
            spawnPoint.y = Random.Range(bottomRight.position.y, topRight.position.y);
        }
        //topSide
        if (temp == 2)
        {
            spawnPoint.y = topLeft.position.y;
            spawnPoint.x = Random.Range(topLeft.position.x, topRight.position.x);
        }
        //bottomSide
        if (temp == 3)
        {
            spawnPoint.y = bottomLeft.position.x;
            spawnPoint.x = Random.Range(bottomLeft.position.x, bottomRight.position.x);
        }
        if (!toughSpawn && !shootSpawn && !fastSpawn && !randomSpawn)
        {

            Instantiate(basic, spawnPoint, Quaternion.identity);
        }
        else
        {
            int enemyPicker = Random.Range(0, 100);
            if (toughSpawn && !shootSpawn)
            {
                if (enemyPicker <= 80)
                {
                    Instantiate(basic, spawnPoint, Quaternion.identity);
                }
                else
                {
                    Instantiate(tough, spawnPoint, Quaternion.identity);
                }
            }
            if (toughSpawn && shootSpawn && !fastSpawn)
            {
                if (enemyPicker <= 60)
                {
                    Instantiate(basic, spawnPoint, Quaternion.identity);
                }
                else if (enemyPicker <= 80)
                {
                    Instantiate(tough, spawnPoint, Quaternion.identity);
                }
                else
                {
                    Instantiate(shoot, spawnPoint, Quaternion.identity);
                }
            }
            if (toughSpawn && shootSpawn && fastSpawn && !randomSpawn)
            {
                if (enemyPicker <= 40)
                {
                    Instantiate(basic, spawnPoint, Quaternion.identity);
                }
                else if (enemyPicker <= 60)
                {
                    Instantiate(tough, spawnPoint, Quaternion.identity);
                }
                else if (enemyPicker <= 80)
                {
                    Instantiate(shoot, spawnPoint, Quaternion.identity);
                }
                else
                {
                    Instantiate(fast, spawnPoint, Quaternion.identity);
                }
            }
            if (toughSpawn && shootSpawn && fastSpawn && randomSpawn)
            {
                if (enemyPicker <= 30)
                {
                    Instantiate(basic, spawnPoint, Quaternion.identity);
                }
                else if (enemyPicker <= 50)
                {
                    Instantiate(tough, spawnPoint, Quaternion.identity);
                }
                else if (enemyPicker <= 70)
                {
                    Instantiate(shoot, spawnPoint, Quaternion.identity);
                }
                else if (enemyPicker <= 90)
                {
                    Instantiate(fast, spawnPoint, Quaternion.identity);
                }
                else
                {
                    Instantiate(random, spawnPoint, Quaternion.identity);
                }
            }
        }
        enemiesSpawnedThisRound++;
        yield return new WaitForSeconds(spawnInterval);
        isSpawning = false;
    }

    IEnumerator NewWave()
    {
        newWave = true;
        enemiesSpawnedThisRound = 0;
        while (GameObject.FindGameObjectWithTag("Enemy") != null)
        {
            yield return null;
        }
        currentWave++;
        for (int i = 5; i > 0; i--)
        {
            var temp = i;
            temp -= 1;
            numbers.enabled = false;
            numbers.enabled = true;
            numbers.color = numberColors[temp];
            numbers.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        numbers.enabled = false;
        newWave = false;
    }



}
