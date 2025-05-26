using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyRabbitMovement : MonoBehaviour
{
    [SerializeField] float jumpTime;
    [SerializeField] float waitTime;

    EnemyManager enemyManager;
    Rigidbody2D rigidbody;
    GameObject player;
    bool firstStunFrame;
    public float knockbackResistance;
    GameManager gameManager;
    bool jumping = false;
    SpriteRenderer sr;

    void Start()
    {
        enemyManager = GetComponent<EnemyManager>();
        rigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        jump();
    }

    void Update()
    {
        if (gameManager.gameOver)
        {
            rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            Destroy(this);
        }
        if (enemyManager.stun < 0)
        {
            if (jumping)
            {
                rigidbody.linearVelocity = new Vector2(enemyManager.target.transform.position.x - transform.position.x, enemyManager.target.transform.position.y - transform.position.y).normalized * enemyManager.speed;
            }
            else rigidbody.linearVelocity = Vector2.zero; 
            if(rigidbody.linearVelocity.x > 0)
            {
                sr.gameObject.transform.localScale = Vector3.one;
            }
            else if(rigidbody.linearVelocity.x < 0)
            {
                sr.gameObject.transform.localScale = Vector3.one - Vector3.right * 2;
            }
            firstStunFrame = true;
        }
        else if(firstStunFrame)
        {
            firstStunFrame = false;
        }
    }

    private void FixedUpdate()
    {
        if(enemyManager.stun > 0)
        {
            rigidbody.linearVelocity /= knockbackResistance;
        }
    }

    void jump()
    {
        jumping = true;
        Invoke("endJump", jumpTime);
        sr.sprite = enemyManager.sprites[1];
    }

    void endJump()
    {
        jumping = false;
        Invoke("jump", waitTime);
        sr.sprite = enemyManager.sprites[0];
    }
}
