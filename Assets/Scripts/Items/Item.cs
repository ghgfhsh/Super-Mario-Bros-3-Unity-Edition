using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public ItemType itemType;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float moveTime;
    bool canPickUp = false;

    [SerializeField] protected AudioSource spawnSound;

    public Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public enum ItemType
    {
        mushroom,
        leaf,
        coin
    }

    public abstract IEnumerator SpawnItem();

    public void ActivatePickup()
    {
        canPickUp = true;
        BoxCollider2D[] boxColliders = GetComponents<BoxCollider2D>();
        foreach (BoxCollider2D boxCollider in boxColliders){
            boxCollider.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canPickUp)
            return;

        GameObject collisionObject = collision.gameObject;
        if (!collisionObject.CompareTag("Player"))
        {
            return;
        }

        collisionObject.GetComponent<Player>().PickupItem(itemType);
        Destroy(gameObject);
    }

}
