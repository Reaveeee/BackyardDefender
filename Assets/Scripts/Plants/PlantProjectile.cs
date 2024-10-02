using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantProjectile : MonoBehaviour
{
    Rigidbody2D rigidbody;

    public float speed;
    public int damage;
    public int pierce;
    public GameObject target;

    float lifetime = 0;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = new Vector2(target.transform.position.x - transform.position.x, target.transform.position.y - transform.position.y).normalized * speed;
    }

    private void Update()
    {
        lifetime += Time.deltaTime;
        if (lifetime > 30)
        {
            Destroy(gameObject);
        }
    }
}
