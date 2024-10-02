using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public Sprite[] itemSprites;
    SpriteRenderer[] spriteRenderer;
    PlayerManager playerManager;
    GameManager gameManager;

    int item; // 0 = Peashooter, 1 = Carrot
    int maxItems = 2;
    public int amount;
    public int price;
    bool selected = false;

    bool used = false;

    public event Action OnBuyItem;

    void Start()
    {
        spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.OnGameStateUpdate += HandleGamestateupdate;

        HandleGamestateupdate();
    }

    private void Update()
    {
        if (selected)
        {
            if (Input.GetKey(KeyCode.E) && gameManager.gameState == 0)
            {
                BuyItem();
            }
        }
    }

    void BuyItem()
    {
        if (gameManager.money >= price && !used)
        {
            used = true;
            playerManager.seeds[item] += amount;
            gameManager.money -= price;
            OnBuyItem.Invoke();
            spriteRenderer[0].enabled = false;
            spriteRenderer[1].enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        selected = true;
        LeanTween.scale(spriteRenderer[0].gameObject, new Vector3(1.1f, 1.1f, 1), .1f); 
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        selected = false;
        LeanTween.scale(spriteRenderer[0].gameObject, new Vector3(1f, 1f, 1), .1f);
    }

    void HandleGamestateupdate()
    {
        if(gameManager.gameState == 0)
        {
            used = false;
            spriteRenderer[0].enabled = true;
            spriteRenderer[1].enabled = true;
            item = UnityEngine.Random.Range(0, maxItems);
            amount = UnityEngine.Random.Range(1, 10);
            price = amount * 5;
            spriteRenderer[0].sprite = itemSprites[item];
        }
    }
}
