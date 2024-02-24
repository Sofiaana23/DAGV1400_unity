using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class charactermovement : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGrounded;
    public float playerSpeed = 1f;
    private float gravityValue = -9.8f;
    private float jumpHeight = .1f;

    private float characterGroundedHeight = 1.08f;

    private bool isCrouched;
    private bool isSprinting;
    private float sizeChangeBuffer;
    public float rotateSpeed;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        isGrounded = true;
        playerVelocity = new Vector3(0.0f,0.0f,0.0f);
        isCrouched = false;
        isSprinting = false;
        sizeChangeBuffer = 0f;
        rotateSpeed = 5f;
    }

    private void GroundCharacter(){
        // Fixes the character's grounding
        if (controller.transform.position.y < characterGroundedHeight )
        {
            // Need to move character back to the top of the ground
            playerVelocity.y = .01f;
        }
        else if (controller.transform.position.y <= characterGroundedHeight+.1f && playerVelocity.y <= 0)
        {
            // Character is close enough to the ground.  Stop moving.  Set Grounded to True
            playerVelocity.y = 0;
            isGrounded = true;
        }
    }

    private void JumpCharacter(){
        // Changes the height position of the player..
        Debug.Log("Controller Position: " + controller.transform.position);
        Debug.Log("Controller Velocity: " + playerVelocity);
        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            Debug.Log("Pressed Space");
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            isGrounded = false;
        }
        else if (!isGrounded)
        {
            Debug.Log("Player is Airborn");
            playerVelocity.y += gravityValue * Time.deltaTime;
        }
        else 
        {
            //Debug.Log("Player is on the ground");
            playerVelocity.y = 0f;
        }
        
    }

    private void MoveCharacter(){
        playerVelocity.x = Input.GetAxis("Horizontal") * playerSpeed;
        playerVelocity.z = Input.GetAxis("Vertical") * playerSpeed;
    }

    private void CrouchCharacter(){
        if (Input.GetKey(KeyCode.LeftControl) && !isCrouched){
            Debug.Log("Entering Crouch");
            // shrink character by 1/4
            sizeChangeBuffer = -.5f;
            // slow down character movement
            playerSpeed *= .5f;
            isCrouched = true;
        }
        // key is released while character is still crouched
        else if (!Input.GetKey(KeyCode.LeftControl) && isCrouched){
            Debug.Log("Exiting Crouch");
            playerSpeed *= 2f;
            isCrouched = false;
            sizeChangeBuffer = .5f;
        }
    }

    private void SprintCharacter(){
        if (Input.GetKey(KeyCode.LeftShift) && !isSprinting){
            Debug.Log("Entering Sprint");
            playerSpeed *= 2f;
            isSprinting = true;
        }
        // key is released but character is still sprinting
        else if (!Input.GetKey(KeyCode.LeftShift) && isSprinting){
            Debug.Log("Exiting Sprint");
            playerSpeed *=.5f;
            isSprinting = false;
        }
    }

    void FixedUpdate()
    {
        // These 3 functions prepare the playerVelocity Vector so that one move completes the action
        GroundCharacter();
        // Only Allow Crouching if the character is grounded
        if (isGrounded)
            CrouchCharacter();
        // Only allow jumping if the character is not crouched
        if (!isCrouched)
            JumpCharacter();
        if (isGrounded && !isCrouched)
            SprintCharacter();
        MoveCharacter();

        if (Mathf.Abs(sizeChangeBuffer) > .1){
            float sizeChange = sizeChangeBuffer / 5;
            Vector3 scale = transform.localScale;
            scale.y += sizeChange;
            transform.localScale = scale;
            sizeChangeBuffer -= sizeChange;
        }
        else {
            sizeChangeBuffer = 0;
        }
        // This executes the Move
        controller.Move(playerVelocity);


    }

    void Update(){
        // Rotate the Character
        transform.Rotate(new Vector3(0f, Input.GetAxis("Mouse X"), 0f) * rotateSpeed);
    }
}

