using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PromptText : MonoBehaviour
{
    [SerializeField] Shop[] shops;
    [SerializeField] GameObject promptSprite;
    Image promptSpriteImage;

    TextMeshProUGUI textMesh;
    void Start()
    {
        foreach (Shop shop in shops)
        {
            shop.OnPromptAppear += HandlePromptAppear;
            shop.OnPromptDisappear += HandlePromptDisappear;
        }
        textMesh = GetComponent<TextMeshProUGUI>();
        promptSpriteImage = promptSprite.GetComponent<Image>();
    }

    void HandlePromptAppear(string text, Sprite sprite)
    {
        textMesh.text = text;
        textMesh.enabled = true;
        promptSpriteImage.sprite = sprite;
        promptSpriteImage.enabled = true;
    }

    void HandlePromptDisappear()
    {
        textMesh.enabled = false;
        promptSpriteImage.enabled = false;
    }
}
