using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject followObject = null;
    public Vector2 followOffset;
    public float speed = 3f;
    public Vector2 threshold;
    private Rigidbody2D rb;

    private Vector3 velocity = Vector3.zero;
    private Vector3 cameraPosOld;
    public float cameraSmooth;
    private float counter;

    // Start is called before the first frame update

    public void setFollowObject(GameObject _followObject)
    {
        counter = Time.time;
        followObject = _followObject;
        cameraPosOld = transform.position;
        rb = followObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (followObject != null && rb != null)
        {
            Vector2 follow = followObject.transform.position;
            float xDifference = Vector2.Distance(Vector2.right * transform.position.x, Vector2.right * follow.x);
            float yDifference = Vector2.Distance(Vector2.up * transform.position.y, Vector2.up * follow.y);

            Vector3 newPosition = transform.position;

            if (Mathf.Abs(xDifference) >= threshold.x)
            {
                if (newPosition.x >= transform.position.x)
                    newPosition.x = follow.x;
            }
            if (Mathf.Abs(yDifference) >= threshold.y)
            {
                newPosition.y = follow.y;
            }
            float moveSpeed = rb.velocity.magnitude > speed ? rb.velocity.magnitude : speed;

            //newPosition = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);
            transform.position = SuperSmoothLerp(cameraPosOld, transform.position, newPosition, Time.time - counter, cameraSmooth);
            cameraPosOld = transform.position;
            counter = Time.time;

        }
        else
        {
            Debug.Log("Waiting For Follow Object");
        }
    }

    public static Vector3 SuperSmoothLerp(Vector3 followOld, Vector3 targetOld, Vector3 targetNew, float elapsedTime, float lerpAmount)
    {
        Vector3 f = followOld - targetOld + (targetNew - targetOld) / (lerpAmount * elapsedTime);
        Vector3 returnValue = targetNew - (targetNew - targetOld) / (lerpAmount * elapsedTime) + f * Mathf.Exp(-lerpAmount * elapsedTime);
        return new Vector3(returnValue.x, returnValue.y, followOld.z);
    }

    //disabled for now
    //private Vector3 calculateThreshold()
    //{
    //    Rect aspect = Camera.main.pixelRect;
    //    Vector2 t = new Vector2(Camera.main.orthographicSize * aspect.width / aspect.height, Camera.main.orthographicSize);
    //    t.x -= followOffset.x;
    //    t.y -= followOffset.y;
    //    return t;
    //}
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Vector2 border = threshold;
        Gizmos.DrawWireCube(transform.position, new Vector3(border.x * 2, border.y * 2, 1));
    }
}
