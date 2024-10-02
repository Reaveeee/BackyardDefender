using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PriceLabel : MonoBehaviour
{
    [SerializeField] GameObject itemObject;
    [SerializeField] Vector3 offset;
    [SerializeField] bool amountMode;
    Shop shop;

    GameManager gameManager;
    TextMeshProUGUI textMesh;

    Camera cam;
    ShopItem shopItem;

    void Start()
    {
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        shopItem = itemObject.GetComponent<ShopItem>();
        textMesh = GetComponent<TextMeshProUGUI>();
        shop = shopItem.transform.parent.GetComponent<Shop>();

        gameManager.OnGameStateUpdate += HandleGameStateUpdate;
        shopItem.OnBuyItem += HandleBuyItem;
        HandleGameStateUpdate();
    }

    void Update()
    {
        transform.position = cam.WorldToScreenPoint(itemObject.transform.position + offset);
        if(!shop.built)
        {
            textMesh.enabled = false;
        }
    }

    void UpdatePriceText()
    {
        if(!amountMode)
        {
            textMesh.text = shopItem.price.ToString();
        }
        else
        {
            textMesh.text = shopItem.amount.ToString();
        }
        if(gameManager.gameState == 0)
        {
            textMesh.enabled = true;
        }

    }

    void HandleGameStateUpdate()
    {
        if(gameManager.gameState == 0)
        {
            Invoke("UpdatePriceText", 3);
        }
        else if (gameManager.gameState == 1)
        {
            textMesh.enabled = false;
        }
    }

    void HandleBuyItem()
    {
        textMesh.enabled = false;
    }
}
