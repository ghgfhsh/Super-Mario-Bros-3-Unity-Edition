using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController2D : MonoBehaviour
{
    //inspector variables
	[SerializeField] private float m_JumpForce = 15f;                          // Amount of force added when the player jumps.
    public float jumpTime;
    [Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
	[SerializeField] private Transform m_CeilingCheck;							// A position marking where to check for ceilings
	[SerializeField] private Collider2D m_CrouchDisableCollider;				// A collider that will be disabled when crouching

	const float GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool isGrounded;            // Whether or not the player is grounded.
    private bool isHittingCeiling;
	const float CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D rigidBody;
	private bool isFacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 velocity = Vector3.zero;
    private bool isJumping = false; //used to check if the pllayer just jumped
    private float jumpTimer;
    private float gravityScale; // gets scale from rigidbody
    public Animator animator;

    //Variables used externally
    public bool isGoingUp = false;

    //variables for fall behind
    [HideInInspector]public bool isInFallBehindZone = false;
    [HideInInspector] public bool isBehindObject = false;
    public float holdToFallThroughTime = 4f;
    [SerializeField]private float timeFallenBehind = 8f;
    public SpriteRenderer spriteRenderer;
    private bool isFallenBehind = false;
    private bool isCrouching = false;

    //collision detection
    float oldPosY;
    bool playerisMovingDown = false;

    //sound effects
    [SerializeField] AudioSource jumpSoundEffect;

    private void Awake()
	{
		rigidBody = GetComponent<Rigidbody2D>();
        gravityScale = rigidBody.gravityScale;
        oldPosY = transform.position.y;
    }


	private void FixedUpdate()
	{
        isGrounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] gColliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < gColliders.Length; i++)
		{
			if (gColliders[i].gameObject != gameObject)
				isGrounded = true;
		}

        isHittingCeiling = false;

        Collider2D[] cColliders = Physics2D.OverlapCircleAll(m_CeilingCheck.position, CeilingRadius, m_WhatIsGround);
        for (int i = 0; i < cColliders.Length; i++)
        {
            if (cColliders[i].gameObject != gameObject && !cColliders[i].gameObject.CompareTag("OneWay"))
            {
                isHittingCeiling = true;
                //if (cColliders[i].gameObject.CompareTag("Hittable"))
                //    cColliders[i].gameObject.GetComponent<QuestionBlock>().Hit();
            }

        }

    }


	public void Move(float move, bool crouch, bool jump, bool holdingJump)
	{

        HandleCrouch(move, crouch);

		// Move the character by finding the target velocity
		Vector3 targetVelocity = new Vector2(move * 10f, rigidBody.velocity.y);
		// And then smoothing it out and applying it to the character
		rigidBody.velocity = Vector3.SmoothDamp(rigidBody.velocity, targetVelocity, ref velocity, m_MovementSmoothing);

        //applies speed to animator, and makes sure walk animation isn't playing if you are in the air
        if (isGrounded)
            animator.SetFloat("walkSpeed", Mathf.Abs(move));
        else
            animator.SetFloat("walkSpeed", 0f);

        HandleFlipping(move);

        HandleJump(jump, holdingJump);

    }

    private void HandleFlipping(float move)
    {
        // If the input is moving the player right and the player is facing left...
        if (move > 0 && !isFacingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (move < 0 && isFacingRight)
        {
            // ... flip the player.
            Flip();
        }
    }

    void HandleCrouch(float move, bool crouch)
    {

        // If crouching
        if (crouch && Mathf.Abs(move) < .01 && isGrounded)
        {
            isCrouching = true;
            if (!isFallenBehind && isInFallBehindZone)
            {
                StartCoroutine(FallBehind());
                isFallenBehind = true;
            }

            // Disable one of the colliders when crouching
            //if (m_CrouchDisableCollider != null)
               // m_CrouchDisableCollider.enabled = false;
        }
        else
        {
            isCrouching = false;
            // Enable the collider when not crouching
            //if (m_CrouchDisableCollider != null)
            //  m_CrouchDisableCollider.enabled = true;
        }
    }

    private void HandleJump(bool jump, bool holdingJump)
    {

        // If the player should jump...
        if (jump && isGrounded)
        {

            // Add a vertical force to the player.
            if (!isJumping)
            {
                isGrounded = false;

                jumpSoundEffect.Play();
                isJumping = true;
                jumpTimer = jumpTime;
                animator.SetBool("jumpingUp", true);
            }
        }

        if (holdingJump && isJumping)
            CalculateJump();
        else
        {
            isJumping = false;
            rigidBody.gravityScale = gravityScale;
        }

        //change to jump animation when falling
        if (!isJumping && rigidBody.velocity.y < 0)
        {
            animator.SetBool("jumpingUp", false);
            animator.SetBool("jumpingDown", true);
        }

        //end jump animation
        if (isGrounded)
        {
            animator.SetBool("jumpingDown", false);
        }
    }

    private void CalculateJump()
    {
        
        if (jumpTimer > 0 && !isHittingCeiling)
        {
            rigidBody.gravityScale = 0f;
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, m_JumpForce * Time.deltaTime);
            jumpTimer -= Time.deltaTime;
        }
        else
        {
            isJumping = false;
            rigidBody.gravityScale = gravityScale;
        }
    }


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		isFacingRight = !isFacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

    IEnumerator FallBehind()
    {
        //check every .25seconds if the player has released the crouch button
        float i = holdToFallThroughTime / .25f;
        for (i = 0; i < holdToFallThroughTime; i+=.25f)
        {
            yield return new WaitForSeconds(.25f);
            if (!Input.GetButton("Crouch"))
            {
                isFallenBehind = false;
                yield return null;
            }
        }
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        spriteRenderer.sortingOrder = -1;
        yield return new WaitForSeconds(.2f);
        gameObject.GetComponent<BoxCollider2D>().isTrigger = false;
        StartCoroutine(EndFallBehind());
        yield return null;
    }

    IEnumerator EndFallBehind()
    {
        yield return new WaitForSeconds(timeFallenBehind);
        Debug.Log("Waiting to see player");
        while (isBehindObject)
        {
            yield return new WaitForSeconds(.25f);
        }
        Debug.Log("Player Seen");
        isFallenBehind = false;
        spriteRenderer.sortingOrder = 5;
        yield return null;
    }
}
