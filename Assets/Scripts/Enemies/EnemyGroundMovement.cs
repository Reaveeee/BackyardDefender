using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyGroundMovement : MonoBehaviour
{
    EnemyManager enemyManager;
    Rigidbody2D rigidbody;
    GameObject player;
    bool firstStunFrame;
    public float knockbackResistance;
    GameManager gameManager;

    void Start()
    {
        enemyManager = GetComponent<EnemyManager>();
        rigidbody = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
            rigidbody.velocity = new Vector2(enemyManager.target.transform.position.x - transform.position.x, enemyManager.target.transform.position.y - transform.position.y).normalized * enemyManager.speed;
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
            rigidbody.velocity /= knockbackResistance;
        }
    }
}
