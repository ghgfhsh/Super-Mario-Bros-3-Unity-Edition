using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastBoxCollider : MonoBehaviour
{
    const float skinWidth = 0.15f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;
    public float distance = .2f;

    public LayerMask collisionMask;
    public HitResults hitResults;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    BoxCollider2D collider;
    RaycastOrigins raycastOrigins;

    private void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    private void Update()
    {
        UpdateRayCastOrigins();

        //draw vertical rays
        for (int i = 0; i < verticalRayCount; i++)
        {
            if (i == 0 || i == horizontalRayCount - 1)
                continue;
            DrawTopRays(i);
            DrawBottomRays(i);
        }

        //draw horizontal rays
        for (int i = 0; i < horizontalRayCount; i++)
        {
            if (i == 0 || i == horizontalRayCount - 1)
                continue;

            DrawRightRays(i);
            DrawLeftRays(i);
        }
    }

    void DrawTopRays(int i)
    {
        Debug.DrawRay(raycastOrigins.bottomLeft + Vector2.right * verticalRaySpacing * i, Vector2.up * -distance, Color.red);
        Vector2 rayOrigin = raycastOrigins.bottomLeft + Vector2.right * (verticalRaySpacing * i);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, distance, collisionMask);

        if (hit)
        {
            hitResults.down = true;
        }
        else
        {
            hitResults.down = false;
        }
    }

    void DrawBottomRays(int i)
    {

        Debug.DrawRay(raycastOrigins.topLeft + Vector2.right * verticalRaySpacing * i, Vector2.up * distance, Color.red);
        Vector2 rayOrigin = raycastOrigins.topLeft + Vector2.right * (verticalRaySpacing * i);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, distance, collisionMask);

        if (hit)
        {
            hitResults.up = true;
        }
        else
        {
            hitResults.up = false;
        }
    }

    void DrawRightRays(int i)
    {

        Debug.DrawRay(raycastOrigins.bottomRight + Vector2.up * horizontalRaySpacing * i, Vector2.right * distance, Color.red);
        Vector2 rayOrigin = raycastOrigins.bottomRight + Vector2.up * (horizontalRaySpacing * i);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right, distance, collisionMask);


        if (hit)
        {
            hitResults.right = true;
        }
        else
        {
            hitResults.right = false;
        }
    }

    void DrawLeftRays(int i)
    {
        Debug.DrawRay(raycastOrigins.bottomLeft + Vector2.up * horizontalRaySpacing * i, Vector2.right * -distance, Color.red);
        Vector2 rayOrigin = raycastOrigins.bottomLeft + Vector2.up * (horizontalRaySpacing * i);
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.right, distance, collisionMask);


        if (hit)
        {
            hitResults.left = true;
        }
        else
        {
            hitResults.left = false;
        }
    }
    void UpdateRayCastOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);

    }

    void CalculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1f);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1f);

    }

    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct HitResults
    {
        public bool up, down, right, left;
    }
}
