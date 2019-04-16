using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour
{
    Vector3 movementVector;
    public float movementSpeed;
    public float turningSpeed;
    float vVelocity;
    CharacterController controller;
    public Camera mainCam;
    private Vector3 curPlayerPortPos;
    private bool attacking;
    private bool rightFacing;
    private Vector3 right;
    private Vector3 left;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        right = new Vector3(0, -60, 0);
        left = new Vector3(0, 60, 0);
        rightFacing = true;
        attacking = false;
        controller = GetComponent<CharacterController>();
        vVelocity = -10;
    }

    // Update is called once per frame
    void Update()
    {
        normalMovement();
        curPlayerPortPos = mainCam.WorldToViewportPoint(transform.position);
    }

    private void normalMovement()
    {
        // Get stick inputs
        float horizontal = 0; // Input.GetAxis("LStick X") * movementSpeed * Time.deltaTime;
        float vertical = 0;  // Input.GetAxis("LStick Y") * movementSpeed * Time.deltaTime;

        if(Input.GetAxis("Vertical") > 0 && transform.position.z < 3f)
        {
            vertical = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;

        }
        else if(Input.GetAxis("Vertical") < 0 && transform.position.z > -7f)
        {
            vertical = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        }

        if (Input.GetAxis("Horizontal") > 0 && curPlayerPortPos.x < 1f)
        {
            rightFacing = true;
            horizontal = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        }
        else if (Input.GetAxis("Horizontal") < 0 && curPlayerPortPos.x > 0f)
        {
            rightFacing = false;
            horizontal = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        }


        
        Vector2 inputs = new Vector2(horizontal, vertical);
        inputs = Vector2.ClampMagnitude(inputs, 1);
        movementVector.x = inputs.x;
        movementVector.z = inputs.y;
        vVelocity = 0;
        if (controller.isGrounded == false)
        {
            //Debug.Log("I am on the ground");
            vVelocity = Physics.gravity.y;
        }
        movementVector.y = vVelocity;
        controller.Move(movementVector * Time.deltaTime * movementSpeed);

        if (rightFacing)
        {
            transform.rotation = Quaternion.Euler(0, -70, 0);
        }else
        {
            transform.rotation = Quaternion.Euler(0, 60, 0);
        }

        // play run animation when the player is moving
        if (vertical != 0 || horizontal != 0)
        {
            //anim.Play("HumanoidRun");
            anim.SetBool("isIdle", false);
        }
        else
        {
            //anim.SetTrigger("stopRun");
            anim.SetBool("isIdle", true);
        }

    }
    
    public void setAttacking(bool isAttack)
    {
        attacking = isAttack;
    }
}
