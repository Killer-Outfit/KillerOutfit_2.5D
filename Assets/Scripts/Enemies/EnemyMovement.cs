using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Defined in prefab
    public string movementType;
    public float speed;

    private Transform playerTransform;

    private CharacterController controller;
    public int direction;
    public string state;

    private float vertical;
    private float horizontal;
    private Vector3 movementVector;
    private float wanderTimer;

    public Vector3 attackMoveTarget;


    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        controller = this.GetComponent<CharacterController>();
        direction = -1;
        state = "idle";
        movementVector = new Vector3(0, 0, 0);
        IdleMove();
    }

    // Update is called once per frame
    void Update()
    {
        // Change horizontal and vertical movement values based on state.
        if (state == "idle")
        {
            wanderTimer -= Time.deltaTime;
            if (wanderTimer <= 0)
            {
                IdleMove();
            }
        }
        else if (state == "attacking")
        {
            AttackMove();
        }

        CheckPlayer();
        movementVector.x = direction * horizontal;
        movementVector.z = vertical;
        controller.Move(movementVector.normalized * speed * Time.deltaTime);
    }

    // Updates the current idle movement on a random timer, based on behavior type (aggressive, defensive, stationary).
    void IdleMove()
    {
        wanderTimer = Random.Range(0.5f, 1f);
        vertical = Random.Range(-1f, 1f);
        if (movementType == "aggressive")
        {
            horizontal = Random.Range(1f, -0.25f);
        }
        else if (movementType == "defensive")
        {
            horizontal = Random.Range(0.25f, -0.75f);
        }
        else if (movementType == "stationary")
        {
            horizontal = 0;
            vertical = 0;
        }
    }

    // Moves in front of the player to attack.
    void AttackMove()
    {
        // Set attackMoveTarget to a space just in front of the player, depending on which side the enemy is on
        Vector3 tmp = playerTransform.position;
        tmp.x += -direction * 2;
        attackMoveTarget = tmp;

        float playerX = attackMoveTarget.x;
        float enemyX = this.transform.position.x;
        float hDiff = playerX - enemyX;
        if(Mathf.Abs(hDiff) < 0.05)
        {
            horizontal = 0;
        }
        else
        {
            horizontal = direction * Mathf.Sign(hDiff);
        }

        float playerZ = attackMoveTarget.z;
        float enemyZ = this.transform.position.z;
        float vDiff = playerZ - enemyZ;
        if (Mathf.Abs(vDiff) < 0.05)
        {
            vertical = 0;
        }
        else
        {
            vertical = Mathf.Sign(vDiff);
        }
    }

    // Sets the enemy's direction for movement and facing. -1 is facing LEFT (right of the player), 1 is facing RIGHT (left of the player).
    void CheckPlayer()
    {
        float playerX = playerTransform.position.x;
        float enemyX = this.transform.position.x;
        float diff = Mathf.Sign(playerX - enemyX);
        if (diff != direction)
        {
            direction = -direction;
            this.transform.Rotate(new Vector3(0, 180, 0));
        }
    }

    // Move away from walls.
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Wall")
        {
            movementVector.z = -movementVector.z;
        }
    }
}
