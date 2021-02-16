using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    Animator animator;
    BoxCollider2D collider;

    public Transform ceilingCheck;
    public Transform groundCheck;

    PlayerState playerState = PlayerState.small;

    public bool isPlayerInvincible = false;

    //sounds
    [SerializeField] AudioSource powerUpEffect;

    private void Start()
    {
        if (!isLocalPlayer)
        {
            gameObject.GetComponent<PlayerMovement>().enabled = false;
            gameObject.GetComponent<CharacterController2D>().enabled = false;
            gameObject.GetComponent<BoxCollider2D>().enabled = false;
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            Destroy(rb);
        }
        else
        {
            animator = GetComponent<Animator>();
            collider = GetComponent<BoxCollider2D>();
            GameManager.Instance.playerState = playerState;
        }
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        NetworkServer.Spawn(gameObject, connectionToClient);
        if (isLocalPlayer)
        {
            Camera.main.GetComponent<CameraController>().setFollowObject(gameObject);
        }
        else
        {
        }
    }

    public void PickupItem(Item.ItemType itemType)
    {
        if (!isLocalPlayer)
            return;

        switch (itemType)
        {
            case Item.ItemType.mushroom:
                Debug.Log("Picking up Mushroom");
                if (playerState == PlayerState.small)
                {
                    SwitchState(PlayerState.big);
                }
                break;
            case Item.ItemType.leaf:
                Debug.Log("Picking up Leaf");
                break;
            default:
                Debug.LogError("Item type does not exist");
                break;
        }

        powerUpEffect.Play();
    }

    private void SwitchState(PlayerState newState)
    {
        switch (newState)
        {
            case PlayerState.small:
                animator.SetTrigger("smallMario");
                collider.size = new Vector2(collider.size.x, collider.size.y / 1.8f);
                groundCheck.localPosition = new Vector2(groundCheck.localPosition.x, groundCheck.localPosition.y / 1.8f);
                ceilingCheck.localPosition = new Vector2(ceilingCheck.localPosition.x, ceilingCheck.localPosition.y / 1.8f);
                playerState = PlayerState.small;
                break;
            case PlayerState.big:
                animator.SetTrigger("bigMario");
                collider.size = new Vector2(collider.size.x, collider.size.y * 1.8f);
                groundCheck.localPosition = new Vector2(groundCheck.localPosition.x, groundCheck.localPosition.y * 1.8f);
                ceilingCheck.localPosition = new Vector2(ceilingCheck.localPosition.x, ceilingCheck.localPosition.y * 1.8f);
                playerState = PlayerState.big;
                break;
            default:
                Debug.LogError("Couldn't resolve Player State");
                break;
        }

        GameManager.Instance.playerState = playerState;
    }

    public void HurtPlayer()
    {
        if (isPlayerInvincible)
            return;

        switch (playerState)
        {
            case PlayerState.small:
                Debug.Log("Player Dead af");
                break;
            case PlayerState.big:
                SwitchState(PlayerState.small);
                break;
            default:
                Debug.LogError("Couldn't resolve Player State");
                break;
        }

        if (playerState != PlayerState.small)
        {
            StartCoroutine(playerInvincible());
        }

    }

    public enum PlayerState
    {
        small,
        big
    }

    IEnumerator playerInvincible()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        isPlayerInvincible = true;

        for (int i = 0; i < 9; i++)
        {
            if(i % 2 != 0)
                spriteRenderer.color = new Color(0, 0, 0, 0);
            else
                spriteRenderer.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(.25f);
        }

        isPlayerInvincible = false;

        yield return null;
    }

}
