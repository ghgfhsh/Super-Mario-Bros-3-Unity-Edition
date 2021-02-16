using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableBlock : MonoBehaviour
{
    protected Animator animator;
    EdgeCollider2D edgeCollider;

    public float bounceForce;
    [SerializeField]public float nextHitTimer = 0.2f; //This makes sure you can't hit two things at once
    [SerializeField] protected AudioSource bumpSound;

    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>(); //this is fine to use as there will only be on object to loop through
        edgeCollider = GetComponent<EdgeCollider2D>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
            return;

        if (!GameManager.Instance.blockBeingHit)
        {
            GameManager.Instance.blockBeingHit = true;
            bumpSound.Play();
            StartCoroutine(blockNextHitTimer());
        }
        else
        {
            return;
        }
    }

    IEnumerator blockNextHitTimer()
    {
        yield return new WaitForSeconds(nextHitTimer);
        GameManager.Instance.blockBeingHit = false;
        StopCoroutine(blockNextHitTimer());
    }
}
