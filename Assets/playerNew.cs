using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerNew : MonoBehaviour
{
    CharacterController controller;
    private float maxHealth;
    private float currentHealth;
    [HideInInspector]
    public string state;
    [HideInInspector]
    public bool isAttacking;
    private string attackType;
    private string inputQueue;
    private int currentHitNum;

    public outfits top;
    public outfits misc;
    public outfits bot;

    Animator anim;
    AnimatorOverrideController animatorOverrideController;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        maxHealth = 100;
        currentHealth = maxHealth;
        state = "idle";
        attackType = "";
        inputQueue = "";
        anim = GetComponent<Animator>();
        currentHitNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("XButton") || Input.GetMouseButtonDown(0))
        {
            //Instantiate(laser, transform.position, transform.rotation);
            inputQueue = "punch";
        }
        else if (Input.GetButtonDown("YButton") || Input.GetMouseButtonDown(1))
        {
            inputQueue = "kick";
        }
        else if (Input.GetButtonDown("AButton") || Input.GetKeyDown(KeyCode.Space))
        {
            inputQueue = "misc";
        }

        if (state == "idle" || state == "run")
        {
            gameObject.GetComponent<playerMove>().setAttacking(false);
            CheckQueue();
        }
    }

    public void CheckQueue()
    {
        if (inputQueue != "")
        {
            if (inputQueue == "punch")
            {
                pressX();
            }
            else if (inputQueue == "kick")
            {
                pressY();
            }
            else if (inputQueue == "misc")
            {
                pressA();
            }
            inputQueue = "";
        }
        else // No inputs at the moment
        {
            currentHitNum = 0;
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                state = "run";
            }else
            {
                anim.SetBool("strut", false);
                state = "idle";
                anim.SetTrigger("backtoIdle");
            }
            gameObject.GetComponent<playerMove>().setAttacking(false);
            
        }
    }
    // Activate punch
    public void pressX()
    {
        Debug.Log("pressed x");
        anim.SetTrigger("punch");
        attackType = "punch";
        state = "attacking";
        gameObject.GetComponent<playerMove>().setAttacking(true);
        StartCoroutine("launchAttack");
    }

    // Activate kick
    public void pressY()
    {
        anim.SetTrigger("kick");
        attackType = "kick";
        state = "attacking";
        gameObject.GetComponent<playerMove>().setAttacking(true);
        StartCoroutine("launchAttack");
    }

    // Activate misc attack
    public void pressA()
    {
        anim.SetTrigger("miscAttack");
        attackType = "misc";
        state = "attacking";
        gameObject.GetComponent<playerMove>().setAttacking(true);
        StartCoroutine("launchAttack");
    }

    // Make the attack activate
    IEnumerator launchAttack()
    {
        int happened = 0;
        float startTime = 0f;
        bool hit;
        outfits currentOutfitItem = null;
        // Set the collider being used based on current attack type
        Collider attack = null;
        if (attackType == "punch")
        {
            currentOutfitItem = top;
        }
        else if (attackType == "kick")
        {
            currentOutfitItem = bot;
        }
        else if (attackType == "misc")
        {
            currentOutfitItem = misc;
        }

        attack = currentOutfitItem.attackColliders[currentHitNum];

        // Go through each phase of the attack based on the outfit attack stats
        for (int i = 0; i < currentOutfitItem.GetPhases(currentHitNum); i++)
        {
            startTime += Time.deltaTime;
            // Reset hit counter and set speed
            hit = false;
            //GetComponent<playerMove>().movementSpeed = currentOutfitItem.GetPhaseMove(currentHitNum, i);
            //GetComponent<PlayerMove>().collideMaxSpeed = currentOutfitItem.GetPhaseMove(currentHitNum, i);
            //GetComponent<PlayerMove>().turningSpeed = currentOutfitItem.GetPhaseTurnSpeed(currentHitNumber, i);

            // Go through this phase's timer
            for (float j = 0; j < currentOutfitItem.GetPhaseTime(currentHitNum, i); j += Time.deltaTime)
            {
                // Apply acceleration
                //GetComponent<playerMove>().movementSpeed += currentOutfitItem.GetPhaseAcc(currentHitNum, i);

                // if this phase is an active hitbox and hasn't hit an enemy yet, try to hit an enemy
                if (currentOutfitItem.GetPhaseActive(currentHitNum, i) && hit == false)
                {
                    Collider[] cols = Physics.OverlapBox(attack.bounds.center, attack.bounds.extents, attack.transform.rotation, LayerMask.GetMask("Hitbox"));
                    foreach (Collider c in cols)
                    {
                        if (c.tag == "Enemy")
                        {
                            // Decrease the hit target's health based on the attack's damage
                            c.SendMessageUpwards("DecreaseHealth", currentOutfitItem.attackDamage[currentHitNum]);
                            hit = true;
                        }
                    }
                }
                yield return null;
            }
        }

        //GetComponent<PlayerMove>().DefaultTurn();
        //GetComponent<PlayerMove>().DefaultSpeed();
        currentHitNum++;
        if (currentHitNum == 3)
        {
            currentHitNum = 0;
        }
        CheckQueue();
    }
}
