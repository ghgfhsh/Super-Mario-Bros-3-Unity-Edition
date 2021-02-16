using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class KillableEnemy : Enemy
{
    [SerializeField] protected byte startingHealth = 1;
    public float playerJumpForce = 650f;
    [HideInInspector] public byte health;
    [HideInInspector] public bool isKilled = false;
    [SerializeField] AudioSource deathSoundEffect;

    protected float deathVelocityX = 2f;
    protected const float deathVelocityY = 10f;
    protected bool isMarkedForDeath = false;

    protected override void Start()
    {
        base.Start();
        health = startingHealth;
    }

    protected virtual void OnEnable()
    {
        health = startingHealth;
    }

    protected override void FixedUpdate()
    {
        if(!isMarkedForDeath)
            base.FixedUpdate();
    }

    public override void OnChildTriggerEnter(Collider2D collision)
    {
        if (collision.gameObject.layer == 13)
        {
            TurtleShell turtleShell = collision.transform.parent.gameObject.GetComponent<TurtleShell>();
            if (turtleShell && turtleShell.isMoving)
            {
                if (collision.transform.position.x >= transform.position.x)
                    Killed(true);
                else
                    Killed(false);
            }
        }
        else
        {
            base.OnChildTriggerEnter(collision);
        }
    }

    public override void OnChildTriggerStay(Collider2D collision)
    {
        base.OnChildTriggerStay(collision);
        if (collision.gameObject.layer == 13)
        {
            TurtleShell turtleShell = collision.transform.parent.gameObject.GetComponent<TurtleShell>();
            if (turtleShell && turtleShell.isMoving)
            {
                if (collision.transform.position.x >= transform.position.x)
                    Killed(true);
                else
                    Killed(false);
            }
        }
    }

    public virtual void Killed(bool isRight)
    {
        StartCoroutine(playDeathAnimation(isRight));
    }


    protected virtual IEnumerator playDeathAnimation(bool isRight)
    {
        isMarkedForDeath = true;
        deathSoundEffect.Play();
        GetComponent<BoxCollider2D>().enabled = false;
        transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
        JumpingEnemy j = GetComponent<JumpingEnemy>();
        if (j)
            j.enabled = false;
        GetComponent<SpriteRenderer>().flipY = true;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (isRight)
            deathVelocityX = -deathVelocityX;

        rb.velocity = Vector2.zero;
        rb.velocity = new Vector2(deathVelocityX, deathVelocityY);

        GameObject parentObject;
        if (transform.parent.transform.parent)
            parentObject = transform.parent.transform.parent.gameObject;
        else
            parentObject = transform.parent.gameObject;

        parentObject.GetComponent<Spawner>().enabled = false;

        yield return new WaitForSeconds(2f);

        Destroy(parentObject);

        yield return null;
    }

}
