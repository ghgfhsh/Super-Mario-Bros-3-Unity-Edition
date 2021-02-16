using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : StompableEnemy
{
    public float moveSpeed;
    public bool startDirectionRandom;
    [Tooltip("True starts right, false left")]
    [SerializeField] public bool startDirection = false;

    [SerializeField] protected bool canWalkOffEdges = false;
    [SerializeField] protected Transform edgeCheck;
    protected bool isEdgeCheckDelayActivated = false;

    [HideInInspector] public bool isMoving = false;
    public float gravityScale = 3.5f;
    [HideInInspector] public bool direction; //true is right, false is left

    protected SpriteRenderer spriteRenderer;
    protected Rigidbody2D rb;

    // Start is called before the first frame update
    protected override void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();

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

        StartMoving();

        base.OnEnable();
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

    //private void OnDrawGizmos()
    //{
    //    if (!canWalkOffEdges)
    //        Gizmos.DrawSphere(edgeCheck.position, .2f);
    //}

    protected bool EdgeCheck()
    {

        Collider2D[] gColliders = Physics2D.OverlapCircleAll(edgeCheck.position, .2f, 1 << 8);
        for (int i = 0; i < gColliders.Length; i++)
        {
            if (gColliders[i].gameObject != gameObject)
                return true;
        }

        return false;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

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

        //Avoid walking off edges
        if (!canWalkOffEdges && !EdgeCheck() && !isEdgeCheckDelayActivated)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            edgeCheck.localPosition = new Vector2(-edgeCheck.localPosition.x, edgeCheck.localPosition.y);
            StartCoroutine(edgeCheckDelay());
        }
    }

    IEnumerator edgeCheckDelay()
    {
        isEdgeCheckDelayActivated = true;
        yield return new WaitForSeconds(.1f);
        isEdgeCheckDelayActivated = false;
        yield return null;
    }
}
