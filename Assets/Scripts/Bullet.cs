using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 direction { get; set; }
    public float force { get; set; }

    private bool hitWall = false;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.AddForce(direction * force);
    }

    void Update()
    {
        /*if (!hitWall)
        {
            rb.MovePosition(transform.position + new Vector3(direction.x, direction.y, 0) * force * Time.deltaTime);
        }*/
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "platform") 
        {
            hitWall = true;
        }
    }
}
