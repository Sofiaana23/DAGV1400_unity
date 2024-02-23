using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class charactermovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool grounded;
    public float playerSpeed = 1f;
    private float gravityValue = -9.8f;
    private float jumpHeight = .1f;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        grounded = true;
        playerVelocity = new Vector3(0.0f,0.0f,0.0f);
    }

    private void GroundCharacter(){
        // Fixes the character's grounding
        if (controller.transform.position.y < 1.3f )
        {
            // Need to move character back to the top of the ground
            playerVelocity.y = .01f;
        }
        else if (controller.transform.position.y < 1.4f && playerVelocity.y <= 0)
        {
            // Character is close enough to the ground.  Stop moving.  Set Grounded to True
            playerVelocity.y = 0;
            grounded = true;
        }
    }

    private void JumpCharacter(){
        // Changes the height position of the player..
        Debug.Log("Controller Position: " + controller.transform.position);
        Debug.Log("Controller Velocity: " + playerVelocity);
        if (Input.GetKey(KeyCode.Space) && grounded)
        {
            Debug.Log("Pressed Space");
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            grounded = false;
        }
        else if (!grounded)
        {
            Debug.Log("Player is Airborn");
            playerVelocity.y += gravityValue * Time.deltaTime;
        }
        else 
        {
            Debug.Log("Player is on the ground");
            playerVelocity.y = 0f;
        }
        
    }

    private void MoveCharacter(){
        playerVelocity.x = Input.GetAxis("Horizontal") * playerSpeed;
        playerVelocity.z = Input.GetAxis("Vertical") * playerSpeed;
    }

    void FixedUpdate()
    {
        // These 3 functions prepare the playerVelocity Vector so that one move completes the action
        GroundCharacter();
        JumpCharacter();
        MoveCharacter();

        // This executes the Move
        controller.Move(playerVelocity);
    }
    }

