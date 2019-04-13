using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Overmind : MonoBehaviour
{
    private List<GameObject> enemMelee;
    private GameObject MeleeL;
    private GameObject MeleeR;
    private List<GameObject> enemRanged;
    private GameObject RangedL;
    private GameObject RangedR;

    private float aggroTimerMelee;
    private float aggroTimerRanged;

    // Start is called before the first frame update
    void Start()
    {
        aggroTimerMelee = Random.Range(1f, 2f);
        aggroTimerRanged = Random.Range(2f, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        aggroTimerMelee -= Time.deltaTime;
        if(aggroTimerMelee <= 0)
        {
            CallMeleeAttack();
        }
        aggroTimerRanged -= Time.deltaTime;
        if(aggroTimerRanged <= 0)
        {
            CallRangedAttack();
        }
        UpdateAttackers();
    }

    // Calls a melee attack from the closest enemy to the player
    private void CallMeleeAttack()
    {
        GameObject closestL = null;
        GameObject closestR = null;
        float distL = 9999;
        float distR = 9999;

        // Get the closest enemies on the left and right
        foreach(GameObject e in enemMelee)
        {
            if(e.GetComponent<EnemyMovement>().direction == 1) // enemy is on the LEFT
            {
                if(e.GetComponent<EnemyMovement>().pDist < distL)
                {
                    closestL = e;
                    distL = e.GetComponent<EnemyMovement>().pDist;
                }
            }
            else // enemy is on the RIGHT
            {
                if (e.GetComponent<EnemyMovement>().pDist < distR)
                {
                    closestR = e;
                    distR = e.GetComponent<EnemyMovement>().pDist;
                }
            }
        }

        // No enemies exist, or all enemy slots are taken
        if(closestR == null && closestL == null || (MeleeR != null && MeleeL != null))
        {
            return;
        }

        // Special cases if enemies are all on one side
        if(closestL == null)
        {
            if (closestR != null && MeleeR == null)
            {
                MeleeR = closestR;
            }
            return;
        }
        if (closestR == null)
        {
            if (closestL != null && MeleeL == null)
            {
                MeleeL = closestL;
            }
            return;
        }

        // Attack a side with no current attackers. If both sides have attackers, do nothing.
        if (MeleeR == null && MeleeL == null)
        {
            int rand = Random.Range(0, 2);
            if (rand == 0)
            {
                MeleeR = closestR;
                return;
            }
            else if (rand == 1)
            {
                MeleeL = closestL;
                return;
            }
        }
        else if (MeleeR == null && MeleeL != null)
        {
            MeleeR = closestR;
            return;
        }
        else if (MeleeL == null && MeleeR != null)
        {
            MeleeL = closestL;
            return;
        }
    }

    private void CallRangedAttack()
    {
        GameObject closestL = null;
        GameObject closestR = null;
        float distL = 9999;
        float distR = 9999;

        // Get the closest enemies on the left and right
        foreach (GameObject e in enemRanged)
        {
            if (e.GetComponent<EnemyMovement>().direction == 1) // enemy is on the LEFT
            {
                if (e.GetComponent<EnemyMovement>().pDist < distL)
                {
                    closestL = e;
                    distL = e.GetComponent<EnemyMovement>().pDist;
                }
            }
            else // enemy is on the RIGHT
            {
                if (e.GetComponent<EnemyMovement>().pDist < distR)
                {
                    closestR = e;
                    distR = e.GetComponent<EnemyMovement>().pDist;
                }
            }
        }

        // No enemies exist, or all enemy slots are taken
        if (closestR == null && closestL == null || (RangedR != null && RangedL != null))
        {
            return;
        }

        // Special cases if enemies are all on one side
        if (closestL == null)
        {
            if (closestR != null && RangedR == null)
            {
                RangedR = closestR;
            }
            return;
        }
        if (closestR == null)
        {
            if (closestL != null && RangedL == null)
            {
                RangedL = closestL;
            }
            return;
        }

        // Attack a side with no current attackers. If both sides have attackers, do nothing.
        if (RangedR == null && RangedL == null)
        {
            int rand = Random.Range(0, 2);
            if (rand == 0)
            {
                RangedR = closestR;
                return;
            }
            else if (rand == 1)
            {
                RangedL = closestL;
                return;
            }
        }
        else if (RangedR == null && RangedL != null)
        {
            RangedR = closestR;
            return;
        }
        else if (RangedL == null && RangedR != null)
        {
            RangedL = closestL;
            return;
        }
    }

    private void UpdateAttackers()
    {
        // Check all attacking enemies. If they are not currently moving to attack or attacking, free those enemy slots.
        if(MeleeL != null)
        {
            if(MeleeL.GetComponent<EnemyMovement>().state != "attacking" && MeleeL.GetComponent<EnemyMovement>().state != "doingattack")
            {
                MeleeL = null;
            }
        }
        if (MeleeR != null)
        {
            if (MeleeR.GetComponent<EnemyMovement>().state != "attacking" && MeleeR.GetComponent<EnemyMovement>().state != "doingattack")
            {
                MeleeR = null;
            }
        }
        if (RangedL != null)
        {
            if (RangedL.GetComponent<EnemyMovement>().state != "attacking" && RangedL.GetComponent<EnemyMovement>().state != "doingattack")
            {
                RangedL = null;
            }
        }
        if (RangedR != null)
        {
            if (RangedR.GetComponent<EnemyMovement>().state != "attacking" && RangedR.GetComponent<EnemyMovement>().state != "doingattack")
            {
                RangedR = null;
            }
        }
    }

    public void AddMelee(GameObject obj)
    {
        enemMelee.Add(obj);
    }

    public void RemoveMelee(GameObject obj)
    {
        enemMelee.Remove(obj);
    }

    public void AddRanged(GameObject obj)
    {
        enemRanged.Add(obj);

    }

    public void RemoveRanged(GameObject obj)
    {
        enemRanged.Remove(obj);
    }
}
