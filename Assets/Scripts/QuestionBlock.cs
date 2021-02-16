using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestionBlock : HittableBlock
{
    public Item storedItem;
    public byte itemAmount = 0;


    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
    }

    override protected void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (itemAmount > 0)
        {
            Item spawnedItem = Instantiate(storedItem, this.transform.position, this.transform.rotation) as Item;
            spawnedItem.StartCoroutine(spawnedItem.SpawnItem());
            itemAmount--;
        }

        if(itemAmount <= 0)
            animator.SetTrigger("hit");
    }
}
