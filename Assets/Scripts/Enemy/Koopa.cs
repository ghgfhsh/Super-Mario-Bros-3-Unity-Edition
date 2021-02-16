using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Koopa : WalkingEnemy
{
    GameObject shell;

    protected override void Start()
    {
        base.Start();
        shell = transform.parent.GetChild(1).gameObject;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        JumpingEnemy componentToEnable = GetComponent<JumpingEnemy>();
        if (componentToEnable)
            componentToEnable.enabled = true;
    }

    protected override void ResetEnemy()
    {
        gameObject.transform.localPosition = origPos;
        gameObject.transform.localRotation = origRotation;
        transform.parent.gameObject.SetActive(false);
    }


    protected override void Stomped(GameObject o)
    {
        BouncePlayer(o);
        stompSoundEffect.Play();
        health--;
        GetComponent<Animator>().SetTrigger("dead");
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(rb.velocity.x, -.1f);
        if (health <= 0)
        {
            replaceWithShell();
        }
    }

    void replaceWithShell()
    {
        gameObject.SetActive(false);
        shell.transform.localPosition = this.transform.localPosition;
        shell.SetActive(true);
    }

    public override void Killed(bool isRight)
    {
        replaceWithShell();
        shell.GetComponent<TurtleShell>().Killed(isRight);
    }

}
