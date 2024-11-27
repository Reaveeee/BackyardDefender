using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    [SerializeField] Sprite[] sprites;

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
        playerActions.OnCreateField += handleCreateField;
        handleCreateField();
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
                if(playerActions.invSlot == 1 && playerManager.water >= 1)
                {
                    if(humidity < maxHumidity)
                    {
                        playerManager.water -= 1;
                    }
                    humidity = maxHumidity;
                    GameObject.Find("WaterCounterText").GetComponent<TextMeshProUGUI>().text = playerManager.water.ToString();
                }
                else if(playerActions.invSlot== 2)
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

    private void handleCreateField()
    {
        GameObject up = GameObject.Find("Field: " + transform.position.x + "_" + (transform.position.y + 1));
        GameObject down = GameObject.Find("Field: " + transform.position.x + "_" + (transform.position.y - 1));
        GameObject left = GameObject.Find("Field: " + (transform.position.x - 1) + "_" + transform.position.y);
        GameObject right = GameObject.Find("Field: " + (transform.position.x + 1) + "_" + transform.position.y);
        SpriteRenderer spriteRender = GetComponent<SpriteRenderer>();
        if (up != null && right != null && left != null && down != null) { spriteRender.sprite = sprites[1]; return; }
        if (up == null && right == null && left == null && down == null) { spriteRender.sprite = sprites[0]; return; }
        if (up != null && right == null && left == null && down == null) { spriteRender.sprite = sprites[2]; return; }
        if (up != null && right != null && left == null && down == null) { spriteRender.sprite = sprites[3]; return; }
        if (up != null && right == null && left != null && down == null) { spriteRender.sprite = sprites[4]; return; }
        if (up != null && right == null && left == null && down != null) { spriteRender.sprite = sprites[5]; return; }
        if (up != null && right != null && left != null && down == null) { spriteRender.sprite = sprites[6]; return; }
        if (up != null && right != null && left == null && down != null) { spriteRender.sprite = sprites[7]; return; }
        if (up != null && right == null && left != null && down != null) { spriteRender.sprite = sprites[8]; return; }
        if (up == null && right != null && left == null && down == null) { spriteRender.sprite = sprites[9]; return; }
        if (up == null && right != null && left != null && down == null) { spriteRender.sprite = sprites[10]; return; }
        if (up == null && right != null && left == null && down != null) { spriteRender.sprite = sprites[11]; return; }
        if (up == null && right != null && left != null && down != null) { spriteRender.sprite = sprites[12]; return; }
        if (up == null && right == null && left != null && down == null) { spriteRender.sprite = sprites[13]; return; }
        if (up == null && right == null && left != null && down != null) { spriteRender.sprite = sprites[14]; return; }
        if (up == null && right == null && left == null && down != null) { spriteRender.sprite = sprites[15]; return; }
    }
}
