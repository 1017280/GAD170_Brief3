////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description:
//   Controls the bullet
//  Purpose:
//   To make an object act like a bullet
//  Usage:
//   Attach to an object with a rigidbody and an audio source
////////////////////////////////////////////////////////////////////////////////////////////////////

#region Namespace Dependencies
using System.Collections.Generic;
using UnityEngine;
#endregion

public class Bullet : MonoBehaviour
{
#region Inspector
    [SerializeField] private List<AudioClip> bounceSounds;
    [SerializeField] private float lifeTime = 4.0f;
#endregion

#region Exposed Data

    public Vector2 direction { get; set; }
    public float force { get; set; }

#endregion

#region Local Data
    private Rigidbody2D rb;
    private AudioSource player;
    private float timeLived = 0.0f;
    private Vector2 freezePosition = Vector2.zero;
    private Vector2 freezeVelocity = Vector2.zero;
    bool frozen = false;

#endregion

#region Unity Callback Functions
   
   void OnEnable()
   {
       GameEvents.GameStateChangedEvent += OnGameStateChange;
   }

   void OnDisable()
   {
       GameEvents.GameStateChangedEvent -= OnGameStateChange;
   }
   
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<AudioSource>();

        rb.AddForce(direction * force);
    }

    void Update()
    {

        if (frozen)
        {
            rb.velocity = Vector2.zero;
            transform.position = freezePosition;
            return;
        }

        timeLived += Time.deltaTime;
        if (timeLived >= lifeTime)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        SoundPlayer.instance.Play(player, bounceSounds[Random.Range(0, bounceSounds.Count)], collision.relativeVelocity.magnitude);
    }
#endregion

#region Game Event Callback Functions

void OnGameStateChange(GameState newState)
{
    if (newState != GameState.Playing)
    {
        frozen = true;
        freezePosition = transform.position;
        freezeVelocity = rb.velocity;
    }
    else 
    {
        frozen = false;
        rb.velocity = freezeVelocity;
    }
}

#endregion

}
