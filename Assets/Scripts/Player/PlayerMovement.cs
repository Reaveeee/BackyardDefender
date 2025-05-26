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
    SpriteRenderer inHandSpriteRenderer;
    SpriteRenderer playerSprite;
    int inHandOrderOffset;
    bool lastY;

    [SerializeField] Sprite[] walkUp;
    [SerializeField] Sprite[] walkDown;
    [SerializeField] Sprite[] idleUp;
    [SerializeField] Sprite[] idledown;

    Vector2 previousInputVector;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        playerActions = GetComponent<PlayerActions>();
        playerActions.OnMeleeAttack += HandleMeleeAttack;
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        animationManager = GetComponentInChildren<AnimationManager>();
        inHandSpriteRenderer = GameObject.Find("InHandSprite").GetComponent<SpriteRenderer>();
        playerSprite = GameObject.Find("PlayerSprite").GetComponent<SpriteRenderer>();
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

        animate();

        if (stunTimer < 0 )
        {
            rigidbody.linearVelocity = inputVector.normalized * speedModifier;
        }
        else
        {
            rigidbody.linearVelocity = Vector2.zero;
        }
        previousInputVector = inputVector;
    }

    private void FixedUpdate()
    {
        transform.position = new Vector2(Mathf.Clamp(transform.position.x, -13, 13), Mathf.Clamp(transform.position.y, -13, 13));
    }

    void animate()
    {

        if (previousInputVector != inputVector)
        {
            if (inputVector.x > 0)
            {
                if (inputVector.y > 0)
                {
                    lastY = true;
                    animationManager.setAnimation(walkUp, 5, -1);
                }
                else
                {
                    lastY = false;
                    animationManager.setAnimation(walkDown, 5, -1);
                }
            }
            else if (inputVector.x < 0)
            {
                if (inputVector.y > 0)
                {
                    lastY = true;
                    animationManager.setAnimation(walkUp, 5, 1);
                }
                else
                {
                    lastY = false;
                    animationManager.setAnimation(walkDown, 5, 1);
                }
            }
            else
            {
                if (inputVector.y > 0)
                {
                    lastY = true;
                    animationManager.setAnimation(walkUp, 5);
                }
                else if(inputVector.y < 0)
                {
                    lastY = false;
                    animationManager.setAnimation(walkDown, 5);
                }
            }

            if (inputVector.y == 0 && inputVector.x == 0)
            {
                if (lastY) animationManager.setAnimation(idleUp, 3);
                else animationManager.setAnimation(idledown, 3);
            }
        }

        if (lastY)
        {
            switch (playerActions.invSlot)
            {
                case 0:
                    inHandOrderOffset = 1;
                    break;
                case 1:
                    inHandOrderOffset = -1;
                    break;
                case 2:
                    inHandOrderOffset = -1;
                    break;
                case 3:
                    inHandOrderOffset = 1;
                    break;
            }
        }
        else
        {
            switch (playerActions.invSlot)
            {
                case 0:
                    inHandOrderOffset = -1;
                    break;
                case 1:
                    inHandOrderOffset = 1;
                    break;
                case 2:
                    inHandOrderOffset = 1;
                    break;
                case 3:
                    inHandOrderOffset = -1;
                    break;
            }
        }

            inHandSpriteRenderer.sortingOrder = playerSprite.sortingOrder + inHandOrderOffset;
    }

    void HandleMeleeAttack()
    {
        stunTimer = 0.2f;
    }
}
