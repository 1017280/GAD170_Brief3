////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description:
//   Contains simple behaviour for the Key. Is destroyed on collision with a bullet and also
//   destroys associated door. Syncs the door color with its own
//  Purpose:
//   To be used as the key in the Key/Door mechanic. Needs to be destroyed to unlock its door.
//  Usage:
//   Attach this to the key object, add the door object reference in the inspector and set the
//   color in the sprite renderer.
////////////////////////////////////////////////////////////////////////////////////////////////////

#region Namespace Dependencies
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

public class Key : MonoBehaviour
{
#region Inspector
    [SerializeField] private GameObject door;
#endregion

#region Local data
    private SpriteRenderer rend;
    private SpriteRenderer doorRend;
#endregion

#region Unity Callback Functions
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
#endregion
}
