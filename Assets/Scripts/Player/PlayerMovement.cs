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

    [SerializeField] Sprite[] spriteSetIdleDown;
    [SerializeField] Sprite[] spriteSetWalkDown;

    [SerializeField] Sprite[] spriteSetIdleRight;
    [SerializeField] Sprite[] spriteSetWalkRight;

    [SerializeField] Sprite[] spriteSetIdleLeft;
    [SerializeField] Sprite[] spriteSetWalkLeft;

    [SerializeField] Sprite[] spriteSetIdleUp;
    [SerializeField] Sprite[] spriteSetWalkUp;

    int previousDir; //0 = Down, 1 = Right, 2 = Left, 3 = Up 4 = none

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
        if(Input.GetAxisRaw("Vertical") < 0)
        {
            if(previousDir != 0)
            {
                animationManager.setAnimation(spriteSetWalkDown, 4);
                previousDir = 0;
            }   
        }
        else if (Input.GetAxisRaw("Vertical") > 0)
        {
            if (previousDir != 3)
            {
                animationManager.setAnimation(spriteSetWalkUp, 4);
                previousDir = 3;
            }
        }
        else if(Input.GetAxisRaw("Horizontal") > 0)
        {
            if (previousDir != 1)
            {
                animationManager.setAnimation(spriteSetWalkRight, 4);
                previousDir = 1;
            }
        }
        else if(Input.GetAxisRaw("Horizontal") < 0)
        {
            if (previousDir != 2)
            {
                animationManager.setAnimation(spriteSetWalkLeft, 4);
                previousDir = 2;
            }
        }
        else
        {
            if(previousDir != 4)
            {
                animationManager.setAnimation(spriteSetIdleDown, 3);
                previousDir = 4;
            }  
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

    private void FixedUpdate()
    {
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -14.5f, 14.5f), Mathf.Clamp(transform.position.y, -14.5f, 14.5f));
    }

    void HandleMeleeAttack()
    {
        stunTimer = 0.2f;
    }
}
