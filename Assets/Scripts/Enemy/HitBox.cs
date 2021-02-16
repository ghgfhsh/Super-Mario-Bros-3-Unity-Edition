using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
        //enemy = GetComponent<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        enemy.OnChildTriggerEnter(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        enemy.OnChildTriggerStay(collision);
    }

}
