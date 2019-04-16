using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Subscript for enemy 2. Attack data for ranged "push".
public class Enemy2 : EnemyGeneric
{
    public GameObject proj; // Projectile prefab
    public Transform handTransform; // Transform of the rig's hand, used for positioning.

    private void Start()
    {
        health = maxHP;
        overmind = GameObject.Find("Overmind");
        overmind.GetComponent<Overmind>().AddRanged(this.gameObject);
    }

    // Attack timing
    private IEnumerable Attack()
    {
        yield return new WaitForSeconds(0.5f);
        Instantiate(proj, handTransform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        GetComponent<EnemyMovement>().ResumeMovement();
    }

    private void Die()
    {
        overmind.GetComponent<Overmind>().RemoveRanged(this.gameObject);
        Destroy(this.gameObject);
    }
}
