using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{

    [SerializeField] private GameObject door;

    private SpriteRenderer rend;
    private SpriteRenderer doorRend;

    void Start()
    {
        rend = this.GetComponent<SpriteRenderer>();
        doorRend = door.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        doorRend.color = rend.color;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "bullet")
        {
            Destroy(door);
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}
