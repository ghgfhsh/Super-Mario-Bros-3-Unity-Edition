using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public CharacterController2D controller;

	public float runSpeed = 40f;
    public float jumpDelay = 0.25f;                                                     //Allows multiple frames to press jump button
    private float jumpWindowTimer;


    float horizontalMove = 0f;
	bool jump = false;
    bool holdingJump = false;
	bool crouch = false;

    // Update is called once per frame
    void Update () {

		horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
        if (Input.GetButtonDown("Jump"))
        {
            jumpWindowTimer = Time.time + jumpDelay;
        }

        if (Input.GetButton("Jump"))
        {
            holdingJump = true;
        }
        else
        {
            holdingJump = false;
        }

        if (jump)
        {
        }

        if (Input.GetButtonDown("Crouch"))
		{
			crouch = true;
        } else if (Input.GetButtonUp("Crouch"))
		{
			crouch = false;
		}

	}

	void FixedUpdate ()
	{
        if(jumpWindowTimer > Time.time)
        {
            jump = true;
        }

		// Move our character
		controller.Move(horizontalMove * Time.fixedDeltaTime, crouch, jump, holdingJump);
        jump = false;
    }
    

}
