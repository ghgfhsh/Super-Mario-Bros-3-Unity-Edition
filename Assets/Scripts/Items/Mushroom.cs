using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : Item
{
    [SerializeField] private Item leaf;

    protected override void Awake()
    {
        if(GameManager.Instance.playerState == Player.PlayerState.big)
        {
            Item spawnedItem = Instantiate(leaf, this.transform.position, this.transform.rotation) as Item;
            spawnedItem.StartCoroutine(spawnedItem.SpawnItem());
            Destroy(gameObject);
        }
        else
            base.Awake();

    }

    public override IEnumerator SpawnItem()
    {
        rb.velocity = transform.up * moveSpeed;
        yield return new WaitForSeconds(moveTime);
        rb.velocity = Vector2.zero;

        MoveSideways moveSideways = rb.gameObject.GetComponent<MoveSideways>();
        rb.gameObject.GetComponent<Item>().ActivatePickup();
        if (moveSideways)
            moveSideways.StartMoving();
        yield return null;
    }
}
