using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class outfits : MonoBehaviour
{
    // Outfit anims
    public AnimationClip[] attacks;
    // Outfit damage values
    public float[] attackDamage;
    // The mesh for the outfit
    public Mesh outfitMesh;
    // The outfit's material
    public Material outfitMaterial;
    // The player skin renderer to use(top, mid, bottom dependent)
    public SkinnedMeshRenderer outfitSkinRenderer;
    // The outfit menu skin renderer to use(top, mid, bottom dependent)
    public SkinnedMeshRenderer outfitMenuSkinRenderer;
    // Outfit type must be (Top, Bot, Misc)
    public string outfitType;
    // Attack type must be (punch, Kick, misc)
    public string attackType;

    // Colliders for this outfit's attacks
    public Collider[] attackColliders;

    // Attack variables for phase timing, movement speed, acceleration and active hitbox. All arrays for each attack need to be the same size.
    [Header("Attack1")]
    public float[] attack1Time;
    public float[] attack1Move;
    public float[] attack1Acc;
    public bool[] attack1Active;
    [Header("Attack2")]
    public float[] attack2Time;
    public float[] attack2Move;
    public float[] attack2Acc;
    public bool[] attack2Active;
    [Header("Attack3")]
    public float[] attack3Time;
    public float[] attack3Move;
    public float[] attack3Acc;
    public bool[] attack3Active;

    // Multiplier to change an attack animation
    public float[] animSpeedMultiplier;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Get the length of a certain phase of an attack
    public float GetPhaseTime(int attack, int phase)
    {
        if (attack == 0)
        {
            return attack1Time[phase];
        }
        else if (attack == 1)
        {
            return attack2Time[phase];
        }
        else
        {
            return attack3Time[phase];
        }
    }

    // Get the movespeed of a certain phase of an attack
    public float GetPhaseMove(int attack, int phase)
    {
        if (attack == 0)
        {
            return attack1Move[phase];
        }
        else if (attack == 1)
        {
            return attack2Move[phase];
        }
        else
        {
            return attack3Move[phase];
        }
    }

    // Get the acceleration of a certain phase of an attack
    public float GetPhaseAcc(int attack, int phase)
    {
        if (attack == 0)
        {
            return attack1Acc[phase];
        }
        else if (attack == 1)
        {
            return attack2Acc[phase];
        }
        else
        {
            return attack3Acc[phase];
        }
        
    }

    // Get the active hitbox flag of a certain phase of an attack
    public bool GetPhaseActive(int attack, int phase)
    {
        if (attack == 0)
        {
            return attack1Active[phase];
        }
        else if (attack == 1)
        {
            return attack2Active[phase];
        }
        else
        {
            return attack3Active[phase];
        }
        
    }

    // Get the number of phases in an attack.
    public int GetPhases(int attack)
    {
        if (attack == 0)
        {
            return attack1Time.Length;
        }
        else if (attack == 1)
        {
            return attack2Time.Length;
        }
        else
        {
            return attack3Time.Length;
        }
        
    }
}
