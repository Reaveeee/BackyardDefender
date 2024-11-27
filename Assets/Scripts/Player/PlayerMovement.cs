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

        if (previousInputVector != inputVector)
        {
            if(inputVector.x > 0)
            {
                if (inputVector.y > 0)
                {
                    animationManager.setAnimation(walkUp, 5, -1);
                }
                else
                {
                    animationManager.setAnimation(walkDown, 5, -1);
                }
            }
            else if(inputVector.x < 0)
            {
                if (inputVector.y > 0)
                {
                    animationManager.setAnimation(walkUp, 5, 1);
                }
                else
                {
                    animationManager.setAnimation(walkDown, 5, 1);
                }
            }

            if(inputVector.y == 0 && inputVector.x == 0)
            {
                animationManager.setAnimation(idledown, 3);
            }
        }

        if (stunTimer < 0 )
        {
            rigidbody.velocity = inputVector.normalized * speedModifier;
        }
        else
        {
            rigidbody.velocity = Vector2.zero;
        }
        previousInputVector = inputVector;
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
