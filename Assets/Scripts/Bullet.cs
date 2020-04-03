#region Namespace Dependencies
using System.Collections.Generic;
using UnityEngine;
#endregion

public class Bullet : MonoBehaviour
{
#region Inspector
    [SerializeField] private List<AudioClip> bounceSounds;
    [SerializeField] private float lifeTime = 4.0f;
    public Vector2 direction { get; set; }
    public float force { get; set; }
#endregion

#region Local Data
    private Rigidbody2D rb;
    private AudioSource player;
    private float timeLived = 0.0f;
#endregion

#region Unity Callback Functions
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GetComponent<AudioSource>();

        rb.AddForce(direction * force);
    }

    void Update()
    {
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
}
