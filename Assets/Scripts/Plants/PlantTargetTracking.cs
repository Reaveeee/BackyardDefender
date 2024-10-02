using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantTargetTracking : MonoBehaviour
{
    EnemyManager[] groundTargets;
    GameObject groundTargetsParent;
    public GameObject target;
    GameManager gameManager;
    PlantManager plantManager;

    void Start()
    {
        groundTargetsParent = GameObject.Find("GroundEnemies");
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        plantManager = GetComponent<PlantManager>();
    }

    void Update()
    {
        if (plantManager.grownOut)
        {
            groundTargets = groundTargetsParent.GetComponentsInChildren<EnemyManager>();
            target = FindNewTarget();
        }
    }

    GameObject FindNewTarget()
    {
        float shortestDistance = 100;
        for(int i = 0; i < groundTargets.Length; i++)
        {
            float distance = gameManager.GetDistance(gameObject, groundTargets[i].gameObject);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                target = groundTargets[i].gameObject;
            }
        }
        return target;
    }
}
