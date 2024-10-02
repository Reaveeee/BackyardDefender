using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlobalLight : MonoBehaviour
{
    GameManager gameManager;
    Light2D light;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gameManager.OnGameStateUpdate += ChangeLighting;
        light = GetComponent<Light2D>();
    }

    void ChangeLighting()
    {
        Debug.Log("Gamestate updated to " + gameManager.gameState);
        if(gameManager.gameState == 0)
        {
            LeanTween.value(0.5f, 1, 3).setOnUpdate(UpdateLight);
        }
        else
        {
            LeanTween.value(1, 0.5f, 3).setOnUpdate(UpdateLight);
        }
    }

    private void UpdateLight(float value)
    {
        light.intensity = value;
    }
}
