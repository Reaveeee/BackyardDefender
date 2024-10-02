using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    GameManager gameManager;

    public GameObject standardEnemyPrefab;
    public GameObject groundEnemyParent;
    public GameObject healthBarPrefab;
    public GameObject healthBarParent;

    GameObject enemy;
    GameObject healthBar;
    float timer;
    float nextEnemy;
    int enemiesSpawned = 0;
    int spawnPos;
    int spawnOffset = 14;
    int enemiesThisWave;
    public bool finishedSpawning = false;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.OnGameStateUpdate += HandleGamestateUpdate;
    }

    void Update()
    {
        if(enemiesSpawned < enemiesThisWave && gameManager.gameState == 1)
        {
            timer -= Time.deltaTime;
            if(timer <= 0 )
            {
                spawnEnemy();
            }
        }
        finishedSpawning = enemiesSpawned == enemiesThisWave && gameManager.gameState == 1;
    }
    void spawnEnemy()
    {
        nextEnemy = Random.Range(5f / enemiesThisWave, 10f / enemiesThisWave);
        timer = nextEnemy;
        spawnPos = Random.Range(0, 4);
        switch(spawnPos)
        {
            case 0:
                enemy = Instantiate(standardEnemyPrefab, new Vector2(0, spawnOffset), Quaternion.identity, groundEnemyParent.transform);
                break;
            case 1:
                enemy = Instantiate(standardEnemyPrefab, new Vector2(spawnOffset, 0), Quaternion.identity, groundEnemyParent.transform);
                break;
            case 2:
                enemy = Instantiate(standardEnemyPrefab, new Vector2(0, -spawnOffset), Quaternion.identity, groundEnemyParent.transform);
                break;
            case 3:
                enemy = Instantiate(standardEnemyPrefab, new Vector2(-spawnOffset, 0), Quaternion.identity, groundEnemyParent.transform);
                break;
        }
        healthBar = Instantiate(healthBarPrefab, healthBarParent.transform);
        healthBar.GetComponent<HealthBar>().enemyManager = enemy.GetComponent<EnemyManager>();
        enemy.GetComponent<EnemyManager>().HealthBar = healthBar;
        enemy.GetComponent<EnemyManager>().OnDeath += gameManager.HandleEnemyDeath;
        enemiesSpawned++;
    }

    void HandleGamestateUpdate()
    {
        enemiesThisWave = Mathf.RoundToInt(Mathf.Pow(gameManager.day, 1.5f));
        enemiesSpawned = 0;
        timer = 3;
    }
}
