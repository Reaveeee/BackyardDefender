using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    Rigidbody2D rigidbody;
    PlantProjectile plantProjectile;
    MeleeAttackObject meleeAttackObject;
    GameManager gameManager;
    GameObject player;
    GameObject damagePopup;
    Camera cam;
    EnemyTargetTracking targetTracking;

    public int maxHealth;
    public int maxShield;
    public int armor;
    public float stunResistance;

    public int health;
    public float shield;
    public int speed;
    public float stun;
    public int moneyReward;
    public GameObject HealthBar;
    public GameObject target;
    public GameObject damagePopupPrefab;
    GameObject damagePopupParent;

    float hitTimer;
    int receivedDamage;

    [SerializeField] float attackDelay;
    [SerializeField] int attackDamage;
    float attackDelayTimer;

    public Sprite[] sprites;

    public event Action OnDamageReceived;
    public event Action<GameObject> OnDeath;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        health = maxHealth;
        shield = maxShield;
        rigidbody = GetComponent<Rigidbody2D>();
        target = GameObject.Find("OmegaTree");
        player = GameObject.Find("Player");
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        damagePopupParent = GameObject.Find("DamagePopups");
        targetTracking = GetComponent<EnemyTargetTracking>();
        attackDelayTimer = attackDelay;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("GroundProjectile"))
        {
            plantProjectile = collision.gameObject.GetComponent<PlantProjectile>();
            TakeDamageProjectile();
        }
        else if (collision.gameObject.CompareTag("GroundMelee"))
        {
            meleeAttackObject = collision.gameObject.GetComponent<MeleeAttackObject>();
            TakeDamageMelee();
        }
    }

    void TakeDamageProjectile()
    {
        hitTimer = 0;
        stun = 3 / stunResistance;
        rigidbody.linearVelocity = plantProjectile.GetComponent<Rigidbody2D>().linearVelocity.normalized * 3;
        if (plantProjectile.pierce <= 0 )
        {
            Destroy(plantProjectile.gameObject);
        }
        else
        {
            plantProjectile.pierce--;
        }

        if(shield > 0)
        {
            receivedDamage = plantProjectile.damage;
            shield -= receivedDamage;
            DisplayDamagePopup(Color.cyan);
            if (shield < 0)
            {
                shield = 0;
            }
        }
        else
        {
            receivedDamage = plantProjectile.damage / armor;
            health -= receivedDamage;
            DisplayDamagePopup(Color.red);
        }

        if (health <= 0)
        {
            Die();
        }
        OnDamageReceived.Invoke();
    }

    void TakeDamageMelee()
    {
        hitTimer = 0;
        stun = 3 / stunResistance;
        rigidbody.linearVelocity = (transform.position - player.transform.position).normalized * 3;

        if (shield > 0)
        {
            receivedDamage = meleeAttackObject.damage;
            shield -= receivedDamage;
            DisplayDamagePopup(Color.cyan);
            if (shield < 0)
            {
                shield = 0;
            }
        }
        else
        {
            receivedDamage = meleeAttackObject.damage / armor;
            health -= receivedDamage;
            DisplayDamagePopup(Color.red);
        }

        if (health <= 0)
        {
            Die();
        }
        OnDamageReceived.Invoke();
    }

    void Die()
    {
        OnDeath.Invoke(gameObject);
        Destroy(HealthBar);
        Destroy(gameObject);
    }

    void DisplayDamagePopup(Color color)
    {
        damagePopup = Instantiate(damagePopupPrefab, cam.WorldToScreenPoint(transform.position + new Vector3(0, 1)), Quaternion.identity, damagePopupParent.transform);
        damagePopup.GetComponent<TextMeshProUGUI>().text = receivedDamage.ToString();
        damagePopup.GetComponent<TextMeshProUGUI>().color = color;
        damagePopup.GetComponent<DamagePopup>().position = transform.position + new Vector3(0, 1);
    }

    private void Update()
    {
        if (gameManager.gameOver)
        {
            Destroy(this);
        }

        attackDelayTimer -= Time.deltaTime;
        if (health <= 0)
        {
            gameManager.currentEnemies--;
            Destroy(gameObject);
        }
        stun -= Time.deltaTime;
        hitTimer += Time.deltaTime;
        if(hitTimer > 5)
        {
            if(shield < maxShield)
            {
                shield += Time.deltaTime * maxShield;
            }
            else
            {
                shield = maxShield;
            }
        }
        if(targetTracking.target != null)
        {
            target = targetTracking.target;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject == target)
        {
            if(attackDelayTimer <= 0)
            {
                if(target == player)
                {
                    PlayerManager playerManager = collision.gameObject.GetComponent<PlayerManager>();
                    playerManager.health -= attackDamage;
                    attackDelayTimer = attackDelay;
                }
                else
                {
                    PlantManager plantManager = collision.gameObject.GetComponent<PlantManager>();
                    plantManager.takeDamage(attackDamage);
                    attackDelayTimer = attackDelay;
                }
            }
        }
    }
}