using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Spawner : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    private bool spawnToggle = true;
    private bool isGameObjectAlive = false;
    private GameObject childObject;
    private Rigidbody2D childObjectRb;
    private bool isKilled = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        childObject = transform.GetChild(0).gameObject;
        childObjectRb = childObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (spriteRenderer.isVisible && spawnToggle && !isGameObjectAlive)
        {
            childObject.SetActive(true);
            Debug.Log("Spawned Object");
            spawnToggle = false;
            isGameObjectAlive = true;
        }

        if (!spriteRenderer.isVisible && !isGameObjectAlive)
        {
            spawnToggle = true;
        }

        if (childObject.activeSelf == false)
        {
                isGameObjectAlive = false;
        }
    }
}
