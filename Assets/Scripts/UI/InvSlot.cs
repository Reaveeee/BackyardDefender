using System;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvSlot : MonoBehaviour
{
    PlayerActions playerActions;
    PlayerManager playerManager;
    public int slot;
    public bool altScrollable;
    public GameObject altScrollingUp;
    public GameObject altScrollingDown;
    TextMeshProUGUI[] bagCounter;
    public Image[] bagSprites;
    ShopItem[] shopItems;
    int shopItemsIndex = 0;

    float animationTime = 0.15f;
    float invsible;
    float lowAlpha;

    void Start()
    {
        playerActions = GameObject.Find("Player").GetComponent<PlayerActions>();
        playerActions.OnInvSwitch += UpdatePosition;
        playerActions.OnLeftClick += UpdateItemNumbers;
        playerManager = GameObject.Find("Player").GetComponent<PlayerManager>();
        shopItems = GameObject.Find("Shops").GetComponentsInChildren<ShopItem>();

        invsible = 0f;
        lowAlpha = 0.3f;
        if(altScrollable)
        {
            bagSprites = new Image[2];
            altScrollingUp.GetComponent<Image>().color = new Color(0, 0, 0, invsible);
            altScrollingDown.GetComponent<Image>().color = new Color(0, 0, 0, invsible);
            bagCounter = GetComponentsInChildren<TextMeshProUGUI>();
            for (int i = 0; i < bagSprites.Length; i++)
            {
                bagSprites[i] = bagCounter[i].transform.parent.GetComponent<Image>();
            }
        }
        for(int i = 0; i < shopItems.Length; i++)
        {
            shopItems[i].OnBuyItem += UpdateItemNumbers;
        }
        Invoke("UpdateItemNumbers", .3f);
    }

    private void UpdateItemNumbers()
    {
        if (altScrollable)
        {
            for(int i = 0; i < bagCounter.Length; i++)
            {
                bagCounter[i].text = Convert.ToString(playerManager.seeds[i]);
                if (playerManager.seeds[i] == 0)
                {
                    bagCounter[i].text = "";
                    LeanTween.color(bagSprites[i].rectTransform, new Color(.7f, .7f, .7f, .7f), .2f).setEaseOutCubic();
                }
                else
                {
                    LeanTween.color(bagSprites[i].rectTransform, Color.white, .2f).setEaseOutCubic();
                }
            }
        }
    }


    void UpdatePosition()
    {
        if(slot == playerActions.invSlot)
        {
            LeanTween.moveY(gameObject, 80, animationTime).setEaseOutCubic();
            LeanTween.scale(gameObject.transform.GetChild(0).gameObject, new Vector2(1.1f, 1.1f), animationTime).setEaseOutCubic();
            if (altScrollable)
            {
                LeanTween.alpha(altScrollingUp.GetComponent<RectTransform>(), lowAlpha, animationTime).setEaseOutCubic();
                LeanTween.alpha(altScrollingDown.GetComponent<RectTransform>(), lowAlpha, animationTime).setEaseOutCubic();
            }   
        }
        else
        {
            LeanTween.moveY(gameObject, 59, animationTime).setEaseOutCubic();
            LeanTween.scale(gameObject.transform.GetChild(0).gameObject, new Vector2(1f, 1f), animationTime).setEaseOutCubic();
            if (altScrollable)
            {
                LeanTween.alpha(altScrollingUp.GetComponent<RectTransform>(), invsible, animationTime).setEaseOutCubic();
                LeanTween.alpha(altScrollingDown.GetComponent<RectTransform>(), invsible, animationTime).setEaseOutCubic();
            }
        }
    }
}
