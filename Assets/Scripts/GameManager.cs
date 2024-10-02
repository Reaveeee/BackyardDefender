using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public byte gameState = 0; // 0 = Tag, 1 = Nacht
    public int day = 1;

    public int money = 0;
    public int currentEnemies = 0;
    public bool[] activeShops = new bool[4];
    public bool gameOver = false;

    [SerializeField] TextMeshProUGUI daytimeText;
    [SerializeField] TextMeshProUGUI currentDayText;
    [SerializeField] TextMeshProUGUI moneyText;

    [SerializeField] Button endDayButton;
    Image endDayButtonImage;
    TextMeshProUGUI endDayButtonText;
    EnemySpawner enemySpawner;
    GameObject enemyParent;

    public event Action OnGameStateUpdate;

    GameObject player;
    GameObject omegaTree;

    private void Start()
    {
        activeShops[0] = true;
        endDayButtonText = endDayButton.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        endDayButtonImage = endDayButton.GetComponent<Image>();
        currentDayText.text = "Day: 1";
        player = GameObject.Find("Player");
        omegaTree = GameObject.Find("OmegaTree");
        enemySpawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
        enemyParent = GameObject.Find("GroundEnemies");

        player.GetComponent<PlayerManager>().OnDeath += GameOver;
        omegaTree.GetComponent<PlantManager>().OnDeath += GameOver;
    }

    void Update()
    {
        currentEnemies = enemyParent.GetComponentsInChildren<EnemyManager>().Length;
        if(gameState == 1 && currentEnemies > 1)
        {
            daytimeText.text = "Night: " + currentEnemies + " enemies left";
        }
        else if (gameState == 1 && currentEnemies == 1)
        {
            daytimeText.text = "Night: " + currentEnemies + " enemy left";
        }
        else if(gameState != 0 && currentEnemies == 0 && enemySpawner.finishedSpawning)
        {
            gameState = 0;
            Invoke("enableEndDayButton", 3.1f);
            daytimeText.text = "Day";
            day++;
            currentDayText.text = "Day: " + day;
            OnGameStateUpdate.Invoke();
        }

        moneyText.text = "Money: " + money;
    }

    public void EndDay()
    {
        gameState = 1;
        disableEndDayButton();
        OnGameStateUpdate.Invoke();
    }

    void enableEndDayButton()
    {
        endDayButton.enabled = true;
        endDayButtonText.enabled = true;
        endDayButtonImage.enabled = true;
    }
    void disableEndDayButton()
    {
        endDayButton.enabled = false;
        endDayButtonText.enabled = false;
        endDayButtonImage.enabled = false;
    }

    void GameOver()
    {
        if (!gameOver)
        {
            Debug.Log("Trying to load GameOver Scene");
            SceneManager.LoadScene("GameOver", LoadSceneMode.Additive);
        }
        gameOver = true;
    }

    public void HandleEnemyDeath(GameObject enemy)
    {
        money += enemy.GetComponent<EnemyManager>().moneyReward;
    }

    //Utility
    public float GetDistance(GameObject gameObj1, GameObject gameObj2)
    {
        float distance = Mathf.Sqrt(Mathf.Pow(gameObj1.transform.position.x - gameObj2.transform.position.x, 2) + Mathf.Pow(gameObj1.transform.position.y - gameObj2.transform.position.y, 2));
        return distance;
    }
}
