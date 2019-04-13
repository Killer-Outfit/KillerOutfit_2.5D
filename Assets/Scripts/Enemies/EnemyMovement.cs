using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    // Defined in prefab
    public string movementType;
    public float speed;

    private Transform playerTransform;
    public float pDist;

    private CharacterController controller;
    private EnemyGeneric enemClass;
    public int direction;
    public string state;

    private float vertical;
    private float horizontal;
    private float wanderTimer;
    private Vector3 movementVector;

    private Vector3 attackMoveTarget;
    public bool wantsToAttack;

    private float stagTimer;
    private float knockSpeed;


    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.Find("Player").transform;
        controller = this.GetComponent<CharacterController>();
        enemClass = this.GetComponent<EnemyGeneric>();
        direction = -1;
        state = "idle";
        wantsToAttack = false;
        movementVector = new Vector3(0, 0, 0);
        IdleMove();
        CheckPlayer();
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
            if(wantsToAttack == true)
            {
                state = "attacking";
            }
        }
        else if (state == "attacking")
        {
            AttackMove();
        }
        else
        {
            horizontal = 0;
            vertical = 0;
        }

        CheckPlayer();
        movementVector = new Vector3(direction * horizontal, 0, vertical);
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

        if (movementType == "aggressive")
        {
            float playerX = attackMoveTarget.x;
            float enemyX = this.transform.position.x;
            float hDiff = playerX - enemyX;
            if (Mathf.Abs(hDiff) < 0.05)
            {
                horizontal = 0;
            }
            else
            {
                horizontal = direction * Mathf.Sign(hDiff);
            }
        }
        else if (movementType == "defensive")
        {
            horizontal = Random.Range(0.25f, -0.75f);
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

        CheckForAttack();
    }

    // Called when the enemy is moving to attack the player. If lined up with the player, do attack sequence.
    void CheckForAttack()
    {
        if(movementType == "aggressive")
        {
            if (horizontal == 0 && vertical == 0)
            {
                enemClass.DoAttack();
            }
        }
        else if (movementType == "defensive")
        {
            if (vertical == 0)
            {
                enemClass.DoAttack();
            }
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
        // Update distance to player, used by the controller
        pDist = Mathf.Abs(Vector3.Distance(new Vector3(playerTransform.position.x, 0, playerTransform.position.z), new Vector3(this.transform.position.x, 0, this.transform.position.z)));
    }

    public void DoAttack()
    {
        state = "doingattack";
        wantsToAttack = false;
        //===play animation===//
    }

    public void ResumeMovement()
    {
        state = "idle";
        //===play animation===//
    }

    public void Stagger(float stuntime = 0.4f)
    {
        state = "stagger";
        //===play animation===//
        stagTimer = stuntime;
        StartCoroutine("Stagger");
    }

    private IEnumerable Stagger()
    {
        for (float i=0; i < stagTimer; i+=Time.deltaTime)
        {
            yield return null;
        }
        ResumeMovement();
    }

    public void Knockdown(float speed = 5f)
    {
        state = "knockdown";
        //===play animation===//
        knockSpeed = speed;
        StartCoroutine("Knockdown");
    }

    private IEnumerable Knockdown()
    {
        while (knockSpeed > 0)
        {
            controller.Move(new Vector3(direction * knockSpeed, 0, 0));
            knockSpeed -= Time.deltaTime;
            yield return null;
        }
        ResumeMovement();
    }

    private IEnumerable GetUp()
    {
        //===play animation===//
        yield return new WaitForSeconds(0.5f);
        ResumeMovement();
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
