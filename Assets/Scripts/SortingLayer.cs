using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortingLayer : MonoBehaviour
{
    SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        spriteRenderer.sortingOrder = (int) (-transform.position.y * 10);
    }
}
