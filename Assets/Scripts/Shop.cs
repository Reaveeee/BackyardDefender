using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    GameManager gameManager;
    BoxCollider2D collider;
    GameObject buildingSite;

    float startPos;
    float hiddenPos;
    public bool built = false;
    bool selected = false;

    [SerializeField] int shopIndex;
    [SerializeField] int buildingPrize;
    [SerializeField] Sprite eButtonSprite;

    public event Action<string, Sprite> OnPromptAppear;
    public event Action OnPromptDisappear;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.OnGameStateUpdate += HandleGamestateUpdate;
        buildingSite = GameObject.Find("BuildingSite " + shopIndex);

        collider = GetComponent<BoxCollider2D>();

        startPos = transform.position.y;
        hiddenPos = startPos - 5;
        gameObject.transform.position = new Vector3(transform.position.x, hiddenPos);
        Invoke("HandleGamestateUpdate", .1f);
        CloseShop();
    }

    private void Update()
    {
        if(selected && Input.GetKeyDown(KeyCode.E) && gameManager.money >= buildingPrize)
        {
            gameManager.activeShops[shopIndex] = true;
            gameManager.money -= buildingPrize;
        }
    }

    void HandleGamestateUpdate()
    {
        if(gameManager.activeShops[shopIndex] == true)
        {
            if (gameManager.gameState == 0)
            {
                LeanTween.moveY(gameObject, startPos, 3);
                Destroy(buildingSite);
                built = true;
                Invoke("OpenShop", 3);
            }
            else if (gameManager.gameState == 1)
            {
                LeanTween.moveY(gameObject, hiddenPos, 3);
                Invoke("CloseShop", 0);
            }
        }
        if(gameManager.gameState == 1)
        {
            selected = false;
        }
    }

    void OpenShop()
    {
        collider.isTrigger = false;
        collider.offset = new Vector2(0, 0);
    }

    void CloseShop()
    {
        collider.isTrigger = true;
        collider.offset = new Vector2(0, 5);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(gameManager.gameState == 0)
        {
            selected = true;
            OnPromptAppear.Invoke(buildingPrize + "$", eButtonSprite);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        selected = false;
        OnPromptDisappear.Invoke();
    }
}
