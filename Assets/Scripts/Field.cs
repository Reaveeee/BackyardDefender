using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour
{
    GameManager gameManager;

    GameObject player;
    PlayerActions playerActions;
    PlayerManager playerManager;
    SpriteRenderer spriteRenderer;
    GameObject plantPrefab;

    public float humidity = 0;
    float maxHumidity = 1;
    float humidityPercent;
    float colorChange;
    bool planted = false;
    GameObject plant;
    PlantManager plantManager;

    Color baseColor;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GameObject.Find("Player");
        playerActions = player.GetComponent<PlayerActions>();
        playerManager = player.GetComponent<PlayerManager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        baseColor = spriteRenderer.color;

        gameManager.OnGameStateUpdate += handleGamestateUpdate;
    }

    void Update()
    {
        plantPrefab = playerActions.seed;
        humidityPercent = humidity / maxHumidity;
        colorChange = humidityPercent / 2;

        spriteRenderer.color = new Color(baseColor.r - colorChange, baseColor.g - colorChange, baseColor.b - colorChange);
    }

    private void OnMouseOver()
    {
        if(gameManager.GetDistance(gameObject, player) <= 3 && gameManager.gameState == 0)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(playerActions.invSlot== 1)
                {
                    humidity = maxHumidity;
                }else if(playerActions.invSlot== 2)
                {
                    if (!planted)
                    {
                        if (playerManager.seeds[playerActions.seedSlot] > 0)
                        {
                            plant = Instantiate(plantPrefab, transform.position, Quaternion.identity, gameObject.transform);
                            plantManager = plant.GetComponent<PlantManager>();
                            planted = true;
                            playerManager.seeds[playerActions.seedSlot]--;
                        }
                    }
                    else
                    {
                        if (plantManager.grownOut && plantManager.harvestable)
                        {
                            plantManager.sellPlant();
                            planted = false;
                        }
                    }
                }
            }
        }
    }

    private void handleGamestateUpdate()
    {
        if(gameManager.gameState == 0)
        {
            if (humidity > 0)
            {
                humidity -= 0.25f;
            }
            Mathf.Clamp(humidity, 0, 1);
        }
    }
}
