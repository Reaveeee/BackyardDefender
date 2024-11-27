using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Sprite[] sprites;
    private float speed;
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

    public void setAnimation(Sprite[] pSprites, float pSpeed, float scaleX)
    {
        transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
        sprites = pSprites;
        speed = pSpeed;
        timer = math.INFINITY;
    }
    public void setAnimation(Sprite[] pSprites, float pSpeed)
    {
        sprites = pSprites;
        speed = pSpeed;
        timer = math.INFINITY;
    }

    public void setAnimation(float scaleX)
    {
        transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
    }

    public Sprite[] getAnimation()
    {
        return sprites;
    }
}
