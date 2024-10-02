using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantShooting : MonoBehaviour
{
    PlantTargetTracking targetTracking;
    PlantManager plantManager;
    GameManager gameManager;
    GameObject shotProjectile;
    PlantProjectile shotProjectileScript;
    Field field;

    public GameObject projectile;
    public float reloadTime;
    public float range;

    float nextShot;

    void Start()
    {
        targetTracking = GetComponent<PlantTargetTracking>();
        plantManager = GetComponent<PlantManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        nextShot = reloadTime;
        field = GetComponentInParent<Field>();
    }

    void Update()
    {
        if (gameManager.gameOver)
        {
            Destroy(this);
        }
        if (plantManager.grownOut && gameManager.gameState == 1)
        {
            nextShot -= Time.deltaTime;
            try
            {
                if (nextShot <= 0 && gameManager.GetDistance(gameObject, targetTracking.target) <= range)
                {
                    nextShot = reloadTime;
                    Shoot();
                }
            }
            catch { }
        }
    }

    void Shoot()
    {
        shotProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
        shotProjectileScript = shotProjectile.GetComponent<PlantProjectile>();
        shotProjectileScript.target = targetTracking.target;
        shotProjectileScript.damage = Mathf.RoundToInt(shotProjectileScript.damage * field.humidity);
        if(shotProjectileScript.damage == 0)
        {
            shotProjectileScript.damage = 1;
        }
    }
}
