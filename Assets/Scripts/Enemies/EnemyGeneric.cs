using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// General script for enemies.
public class EnemyGeneric : MonoBehaviour
{
    // Stats
    public float health;
    public float maxHP;
    public float damage;
    public GameObject overmind;

    // Called when the player hits the enemy.
    public void TakeDamage(float atk)
    {
        GetComponent<EnemyMovement>().Stagger(0.5f);
        Damage(atk);
    }

    // Reduce HP
    public void Damage(float atk)
    {
        health -= atk;
        if(health <= 0)
        {
            Die();
        }
    }

    // Default death behavior. Overridden.
    public void Die()
    {
        Destroy(this.gameObject);
    }

    // Enemy starts their attack. Called by EnemyMovement when in position to attack.
    public void DoAttack()
    {
        GetComponent<EnemyMovement>().DoAttack();
        StartCoroutine("Attack");
    }

    // Default attack to be overridden.
    public IEnumerable Attack()
    {
        yield return null;
        GetComponent<EnemyMovement>().ResumeMovement();
    }
}
