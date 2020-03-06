using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayerFollower : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    private Vector2 velocity;
    void Start()
    {
        Debug.Assert(playerTransform != null, "Player transform not set in camera");
    }

    void FixedUpdate()
    {
        var newPos = Vector2.SmoothDamp(transform.position, playerTransform.position, ref velocity, 0.3f, 20.0f, Time.fixedDeltaTime);
        //var newPos = Vector2.Lerp(transform.position, playerTransform.position, 1.0f * Time.smoothDeltaTime);
        transform.position = new Vector3(newPos.x, newPos.y, -10.0f);        
    }
}
