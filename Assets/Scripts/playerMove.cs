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
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        vVelocity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        normalMovement();
    }

    private void normalMovement()
    {
        //get stick inputs
        float horizontal = Input.GetAxis("LStick X") * movementSpeed * Time.deltaTime;
        float vertical = Input.GetAxis("LStick Y") * movementSpeed * Time.deltaTime;

        if (Input.GetAxis("Horizontal") != 0)
        {
            horizontal = Input.GetAxis("Horizontal") * movementSpeed * Time.deltaTime;
        }
        if (Input.GetAxis("Vertical") != 0)
        {
            vertical = Input.GetAxis("Vertical") * movementSpeed * Time.deltaTime;
        }

        Vector2 inputs = new Vector2(horizontal, vertical);
        inputs = Vector2.ClampMagnitude(inputs, 1);
        movementVector.x = inputs.x;
        movementVector.z = inputs.y;
        movementVector.y = vVelocity;
        controller.Move(movementVector * Time.deltaTime * movementSpeed);
        if (controller.isGrounded)
        {
            //Debug.Log("I am on the ground");
            vVelocity = 0;
        }
    }
}
