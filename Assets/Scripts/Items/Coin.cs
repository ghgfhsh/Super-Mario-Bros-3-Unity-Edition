using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : Item
{
    public override IEnumerator SpawnItem()
    {
        rb.AddForce(Vector2.up * moveSpeed);
        yield return new WaitForSeconds(moveTime);
        Destroy(rb.gameObject);
        GameManager.Instance.coinCounter++;
        yield return null;
    }
}
