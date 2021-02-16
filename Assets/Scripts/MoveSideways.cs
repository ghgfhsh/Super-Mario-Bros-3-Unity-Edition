using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveSideways : MonoBehaviour
{
    Rigidbody2D rb;
    public float moveSpeed;
    public bool startDirectionRandom;
    [Tooltip("True starts right, false left")]
    [SerializeField]public bool startDirection = false;

    public bool moveFromStart = false;
    public bool isMoving = false;
    public float gravityScale = 3.5f;
    [HideInInspector] public bool direction; //true is right, false is left

    SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void OnEnable()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (startDirectionRandom)
        {
            float ranValue = Random.value;
            if (ranValue >= .5)
                direction = true;
            else
                direction = false;
        }
        else if (direction != startDirection)
        {
            direction = startDirection;
        }

        if (moveFromStart)
            StartMoving();
    }

    public void StartMoving()
    {
        rb.gravityScale = gravityScale;
        rb.simulated = true;
        if (direction)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            spriteRenderer.flipX = true;
        }
        else
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            spriteRenderer.flipX = false;
        }
        isMoving = true;
    }

    private void FixedUpdate()
    {
        //Handle Turning Around
        if (Mathf.Abs(rb.velocity.x) <= .001f && isMoving)
        {
            if (direction)
            {
                rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
                spriteRenderer.flipX = false;
                direction = false;
            }
            else
            {
                rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
                spriteRenderer.flipX = true;
                direction = true;
            }
        }
    }
}
