using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingEnemy : MonoBehaviour
{
    public float timeBetweenJumps;
    public float jumpForce = 650f;
    public Transform m_GroundCheck;

    private Rigidbody2D rb;
    private Animator animator;
    private float health;
    [SerializeField] private LayerMask m_WhatIsGround;

    public bool isGrounded;

    // Start is called before the first frame update
    void OnEnable()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        StartCoroutine(Jump());
        health = GetComponent<KillableEnemy>().health;
        Debug.Log(health);
        animator.SetTrigger("flyingMode");
    }

    private void FixedUpdate()
    {
        isGrounded = false;

        // The character is grounded if a circlecast to the groundcheck position hits anything designated as ground
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] gColliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, .2f, m_WhatIsGround);
        for (int i = 0; i < gColliders.Length; i++)
        {
            if (gColliders[i].gameObject != gameObject)
                isGrounded = true;
        }

        if (isGrounded)
            animator.SetBool("isFlying", false);
        else
            animator.SetBool("isFlying", true);

        if (health == 1)
        {
            enabled = false;
        }
    }

    IEnumerator Jump()
    {
        while (enabled)
        {
            rb.AddForce(Vector2.up * jumpForce);
            yield return new WaitForSeconds(timeBetweenJumps);
        }
    }
}
