using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyTargetTracking : MonoBehaviour
{
    GameManager gameManager;
    public GameObject target;
    GameObject[] possibleTargets = new GameObject[2];
    System.Random random = new System.Random();
    float timer;
    [SerializeField] int minSearchTime;
    [SerializeField] int maxSearchTime;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        possibleTargets[0] = GameObject.Find("OmegaTree");
        possibleTargets[1] = GameObject.Find("Player");
        timer = maxSearchTime;
    }

    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            target = TrackTarget(possibleTargets);
            timer = random.Next(minSearchTime, maxSearchTime + 1);
        }
    }

    GameObject TrackTarget(GameObject[] possibleTargets)
    {
        GameObject target = GameObject.Find("Player");
        float shortestDistance = Mathf.Infinity;
        float calculatedDistance;
        foreach (GameObject possibleTarget in possibleTargets)
        {
            calculatedDistance = gameManager.GetDistance(possibleTarget, gameObject);
            if (calculatedDistance < shortestDistance)
            {
                target = possibleTarget;
                shortestDistance = calculatedDistance;
            }
        }
        return target;
    }
}
