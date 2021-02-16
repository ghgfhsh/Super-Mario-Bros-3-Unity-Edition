using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleShell : WalkingEnemy
{
    Animator animator;

    [SerializeField] private AudioSource kickSound;

    protected override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    protected override void OnEnable()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        stompSoundEffect.Play(); // this makes sure bounce sound effect plays through koopa dying
        StartCoroutine(PreventDoubleHit());
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if(!isMarkedForDeath)
            animator.SetFloat("velocity", Mathf.Abs(rb.velocity.x));
        else
            animator.SetFloat("velocity", 0f); //stops spinning when the shell is gonna die
    }

    protected override void ResetEnemy()
    {
        GameObject koopa = transform.parent.GetChild(0).gameObject;
        koopa.transform.localPosition = origPos;
        koopa.transform.localRotation = origRotation;
        koopa.SetActive(true);
        koopa.transform.parent.gameObject.SetActive(false);
        isMoving = false;
        gameObject.SetActive(false);
    }

    protected override void HitPlayer(GameObject o)
    {
        //if the shell is moving hit the player, otherwise start moving the opposite direction as the player
        if (isMoving)
            base.HitPlayer(o);
        else
        {
            kickSound.Play();
            if (o.transform.position.x > gameObject.transform.position.x)
                direction = false;
            else
                direction = true;

            StartMoving();
        }
    }

    protected override void Stomped(GameObject o)
    {
        BouncePlayer(o);

        //if moving stop moving, if not, then start moving based on hit player script
        if (isMoving)
        {
            stompSoundEffect.Play();
            isMoving = false;
            rb.velocity = Vector2.zero;
        }
        else
            HitPlayer(o);
    }

    public override void Killed(bool isRight)
    {
        base.Killed(isRight);
    }

}
