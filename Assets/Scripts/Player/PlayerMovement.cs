using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rigidbody;
    Vector2 inputVector;
    float speedModifier = 5;
    PlayerActions playerActions;
    float stunTimer;
    GameManager gameManager;
    AnimationManager animationManager;
    [SerializeField] Sprite[] spriteSetIdle;
    [SerializeField] Sprite[] spriteSetWalkDown;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        playerActions = GetComponent<PlayerActions>();
        playerActions.OnMeleeAttack += HandleMeleeAttack;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        animationManager = GetComponentInChildren<AnimationManager>();
    }

    void Update()
    {
        if (gameManager.gameOver)
        {
            rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            Destroy(this);
        }

        stunTimer -= Time.deltaTime;
        inputVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") < 0)
        {
            animationManager.sprites = spriteSetWalkDown;
            animationManager.speed = 4;
        }
        else
        {
            animationManager.sprites = spriteSetIdle;
            animationManager.speed = 3;
        }
        if(stunTimer < 0 )
        {
            rigidbody.velocity = inputVector.normalized * speedModifier;
        }
        else
        {
            rigidbody.velocity = Vector2.zero;
        }
    }

    void HandleMeleeAttack()
    {
        stunTimer = 0.2f;
    }
}
