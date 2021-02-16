using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public string enemyName;

    protected bool hitBoxesEnabled = true;

    [HideInInspector] public Vector2 origPos;

    [HideInInspector] public Quaternion origRotation;

    protected virtual void Start()
    {
        origPos = transform.localPosition;
        origRotation = transform.localRotation;
    }


    protected virtual void FixedUpdate()
    {
        if (!GetComponent<SpriteRenderer>().isVisible)
        {
            ResetEnemy();
        }

    }

    public virtual void OnChildTriggerEnter(Collider2D collision)
    {

        if (hitBoxesEnabled)
        {
            if (collision.gameObject.layer == 9)
            {
                    Debug.Log("Hit Player");
                    HitPlayer(collision.gameObject);
                //the if statement stops an error if the object is gone before this is ready to be activated

            }

            if (isActiveAndEnabled)
                StartCoroutine(PreventDoubleHit());
        }
        else
            return;
    }

    public virtual void OnChildTriggerStay(Collider2D collision)
    {

    }


    protected virtual void ResetEnemy()
    {
        gameObject.transform.localPosition = origPos;
        gameObject.transform.localRotation = origRotation;
        gameObject.SetActive(false);
    }

    protected virtual void HitPlayer(GameObject o)
    {
        o.GetComponent<Player>().HurtPlayer();
        StartCoroutine(PreventDoubleHit());
    }

    //solves an issue where the player hits twice instantly 
    protected IEnumerator PreventDoubleHit()
    {
        hitBoxesEnabled = false;
        yield return new WaitForSeconds(.1f);
        hitBoxesEnabled = true;
        yield return null;
    }

}
