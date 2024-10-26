using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public Sprite[] sprites;
    public float speed;
    SpriteRenderer spriteRenderer;

    float timer = 0;
    int index = 0;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        timer += Time.deltaTime;
        if(timer > 1 / speed)
        {
            timer = 0;
            spriteRenderer.sprite = sprites[index];
            index++;
            if(index > sprites.Length - 1)
            {
                index = 0;
            }
        }
    }
}
