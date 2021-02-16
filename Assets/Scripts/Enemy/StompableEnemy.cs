using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompableEnemy : KillableEnemy
{
    [SerializeField] Transform stompCheck;

    [SerializeField] float stompCheckRadius = .2f;

    [HideInInspector]public float timeToDestroyAfterStomp = 1f;

    [SerializeField] protected AudioSource stompSoundEffect;

    public override void OnChildTriggerEnter(Collider2D collision)
    {
        if (hitBoxesEnabled && checkStomp() && collision.gameObject.layer == 9)
        {
            Stomped(collision.gameObject);
            if (isActiveAndEnabled)
                StartCoroutine(PreventDoubleHit());
        }
        else
            base.OnChildTriggerEnter(collision);
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(stompCheck.position, stompCheckRadius);
    }

    protected void BouncePlayer(GameObject o)
    {
        Rigidbody2D playerRb = o.GetComponent<Rigidbody2D>();

        playerRb.velocity = Vector2.zero;

        playerRb.AddForce(Vector2.up * playerJumpForce);

    }

    protected virtual void Stomped(GameObject o)
    {
        stompSoundEffect.Play();
        BouncePlayer(o);
        StartCoroutine(playDeathStompAnimation());
    }


    bool checkStomp()
    {
        //checks if the player is above the enemy
        // This can be done using layers instead but Sample Assets will not overwrite your project settings.
        Collider2D[] gColliders = Physics2D.OverlapCircleAll(stompCheck.position, stompCheckRadius, 1 << 9);
        for (int i = 0; i < gColliders.Length; i++)
        {
            if (gColliders[i].gameObject != gameObject)
                return true;
        }

        return false;
    }


    IEnumerator playDeathStompAnimation()
    {
        GetComponent<Animator>().SetTrigger("dead");
        health--;
        if (health == 0)
        {
            GetComponent<Rigidbody2D>().simulated = false;
            GetComponent<BoxCollider2D>().enabled = false;
            isKilled = true;
            yield return new WaitForSeconds(timeToDestroyAfterStomp);
            Destroy(gameObject.transform.parent.gameObject);
            yield return null;
        }

    }
}
