using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantManager : MonoBehaviour
{
    GameManager gameManager;
    Field field;
    [SerializeField] bool isOmegaTree;
    [SerializeField] Sprite[] sprites;

    SpriteRenderer spriteRenderer;

    public int growTime;
    public bool harvestable;
    public int worth;
    public int maxHealth;
    public int health;
    
    int growth = 0;
    float growthPercent;
    public bool grownOut = false;
    
    Vector2 maxScale;

    public event Action OnDeath;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.OnGameStateUpdate += HandleGameStateUpdate;
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = maxHealth;

        if (!isOmegaTree)
        {
            maxScale = transform.localScale;
            transform.localScale = new Vector2(0.1f, 0.1f);
            field = GetComponentInParent<Field>();
        }
    }

    void HandleGameStateUpdate()
    {
        if(gameManager.gameState == 0 && !isOmegaTree)
        {
            if(!grownOut)
            {
                growth++;
                growthPercent = growth / (float)growTime;
                transform.localScale = maxScale * growthPercent;
                Mathf.Clamp(transform.localScale.x, 0, maxScale.x);
                Mathf.Clamp(transform.localScale.y, 0, maxScale.y);
                if (growthPercent >= 1)
                {
                    grownOut = true;
                }
            }
        }
    }
    public void sellPlant()
    {
        gameManager.money += worth;
        Destroy(gameObject);
    }

    public void takeDamage(int damage)
    {
        health -= damage;
        updateSprite();
    }

    void updateSprite()
    {
                          spriteRenderer.sprite = sprites[0];
        if (health <= 75) spriteRenderer.sprite = sprites[1];
        if (health <= 50) spriteRenderer.sprite = sprites[2];
        if (health <= 25) spriteRenderer.sprite = sprites[3];
    }

    private void Update()
    {
        if(health <= 0)
        {
            OnDeath.Invoke();
        }
    }
}
