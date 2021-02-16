using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallThroughTrigger : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        CharacterController2D characterController = collision.gameObject.GetComponent<CharacterController2D>();
        if (characterController)
        {
            characterController.isInFallBehindZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        CharacterController2D characterController = collision.gameObject.GetComponent<CharacterController2D>();
        if (characterController)
        {
            characterController.isInFallBehindZone = false;
        }
    }
}
