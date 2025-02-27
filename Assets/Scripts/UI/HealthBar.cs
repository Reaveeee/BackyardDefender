using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    public EnemyManager enemyManager;
    public PlayerManager playerManager;
    GameManager gameManager;
    Camera cam;
    Vector3 offset;
    Vector3 redBarOffset;
    Vector3 shieldBarOffset;
    RectTransform healthBar;
    RectTransform orangeBar;
    RectTransform shieldBar;

    float orangeBarTimer;
    float orangeBarDelay = .3f;

    int health;
    int maxHealth;
    float shield;
    float maxShield;

    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        healthBar = GetComponentInChildren<HealthBar2>().GetComponent<RectTransform>();
        orangeBar = GetComponentInChildren<HealthBar3>().GetComponent<RectTransform>();
        shieldBar = GetComponentInChildren<HealthBar4>().GetComponent<RectTransform>();
        offset = new Vector3(0, 2, 0);
        if(enemyManager != null)
        {
            enemyManager.OnDamageReceived += HandleDamageReceived;
        }
        else
        {
            playerManager.OnDamage += HandleDamageReceived;
        }
    }

    void Update()
    {
        orangeBarTimer -= Time.deltaTime;
        if (!gameManager.gameOver)
        {
            if(enemyManager != null)
            {
                transform.position = cam.WorldToScreenPoint(enemyManager.gameObject.transform.position + offset);
                health = enemyManager.health;
                maxHealth = enemyManager.maxHealth;
                shield = enemyManager.shield;
                maxShield = enemyManager.maxShield;
            }
            else
            {
                transform.position = cam.WorldToScreenPoint(playerManager.gameObject.transform.position + offset);
                health = playerManager.health;
                maxHealth = playerManager.maxHealth;
                shield = playerManager.shield;
                maxShield = playerManager.maxShield;
            }
        }
        
        if(orangeBarTimer <= 0)
        {
            UpdateOrangeBar();
        }
        shieldBarOffset = new Vector3(-75 + (shield / maxShield * 75), 0, 0);
        shieldBar.position = transform.position + shieldBarOffset;
    }

    void HandleDamageReceived()
    {
        orangeBarTimer = orangeBarDelay;
        redBarOffset = new Vector3(-75 + ((float)health / maxHealth * 75), 0, 0);
        healthBar.position = transform.position + redBarOffset;
        
    }

    void UpdateOrangeBar()
    {
        if(orangeBar.position.x > healthBar.position.x)
        {
            orangeBar.position = orangeBar.position + new Vector3(-0.5f, 0, 0);
        }
    }
}
