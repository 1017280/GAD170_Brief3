#region Namespace Dependencies
using System.Collections.Generic;
using UnityEngine;
#endregion

public class PlayerData : MonoBehaviour
{
#region Inspector
    /*****************************************************************************************
                                            INSPECTOR
    *****************************************************************************************/

    ///<summary>The minimum jump force for the shortest jump charge</summary>
    [Tooltip("The minimum jump force for the shortest jump charge")]
    [SerializeField] private float minJumpForce = 300.0f;
    ///<summary>The maximum jump force for the longest jump charge</summary>
    [Tooltip("The maximum jump force for the longest jump charge")]
    [SerializeField] private float maxJumpForce = 1500.0f;
    ///<summary>The speed of which jumps and shots are charged</summary>
    [Tooltip("The speed of which jumps and shots are charged")]
    [SerializeField] private float chargeSpeed = 1.0f;
    ///<summary>The minimum shoot force for the shortest charge</summary>
    [Tooltip("The minimum shoot force for the shortest charge")]
    [SerializeField] private float minShootForce = 300.0f;
    ///<summary>The maximum shoot force for the longest charge</summary>
    [Tooltip("The maximum shoot force for the longest charge")]
    [SerializeField] private float maxShootForce = 1000.0f;
    ///<summary>The time it takes before the player is able to shoot again</summary>
    [Tooltip("The time it takes before the player is able to shoot again")]
    [SerializeField] private float shootCooldown = 1.0f;
    ///<summary>The prefab of the bullet to be shot</summary>
    [Tooltip("The prefab of the bullet to be shot")]
    [SerializeField] private GameObject bulletPrefab;
    ///<summary>The pool of sounds to select a random sound to play from when shooting</summary>
    [Tooltip("The pool of sounds to select a random sound to play from when shooting")]
    [SerializeField] private List<AudioClip> shootSounds;
    ///<summary>The sound to be played when the player jumps</summary>
    [Tooltip("The sound to be played when the player jumps")]
    [SerializeField] private AudioClip jumpSound;
    ///<summary>The buildup sound to be played when the player starts charging</summary>
    [Tooltip("The buildup sound to be played when the player starts charging")]
    [SerializeField] private AudioClip jumpChargeSound;
    ///<summary>The seamless sound for when the player is in max charge</summary>
    [Tooltip("The seamless sound for when the player is in max charge")]
    [SerializeField] private AudioClip jumpChargeMaxSound;
    ///<summary>The position for the player to start at. This is to prevent a seemingly Unity bug where for some reason the player start position is changed on full scene reload</summary>
    [Tooltip("The position where the player will start")]
    [SerializeField] private Vector2 startPosition;

#endregion

#region Exposed Data
    /*****************************************************************************************
                                            EXPOSED DATA
    *****************************************************************************************/

    public bool canJump { get; set; } = true;
    public bool isAirborne { get; set; } = false;
    public bool canShoot { get; set; } = true;
    public bool isShooting { get; set; } = false;
    public bool isStuck { get; set; } = false;
    public int framesUnstuck { get; set; } = 0;
    public float shootCharge { get { return _shootCharge; } set { _shootCharge = Mathf.Clamp01(value); }}
    private float _shootCharge = 0.0f;
    public float jumpCharge { get { return _jumpCharge; } set { _jumpCharge = Mathf.Clamp01(value); }}
    private float _jumpCharge = 0.0f;
    public Vector2 lastPosition { get; private set; } = Vector2.zero;
    public Vector2 stuckPosition { get; set; }
    public bool wasStuck { get; private set; } = false;
    public GameObject currentWall { get; set; } = null;
    public ParticleSystem launchEffect { get; private set; } = null;
    public ParticleSystem jumpChargeEffect { get; private set; } = null;
    public ParticleSystem shootEffect { get; private set; } = null;
    public ParticleSystem shootChargeEffect { get; private set; } = null;
    public GameObject gun { get; private set; }
    public float shootWait { get; set; } = 0;
    public Vector2 freezeVelocity { get; set; } = Vector2.zero;
    public Vector3 freezePosition { get; set; } = Vector3.zero;
    public bool freezeJumpEffectState { get; set; } = false;
    public bool freezeJumpChargeEffectState { get; set; } = false;
    public bool freezeShootEffectState { get; set; } = false;
    public bool freezeShootChargeEffectState { get; set; } = false;
#endregion

#region Exposed API
    public float GetJumpForce()
    {
        return minJumpForce + (maxJumpForce - minJumpForce) * jumpCharge;
    }

    public float GetJumpForceRatio()
    {
        return (GetJumpForce() - minJumpForce) / (maxJumpForce - minJumpForce);
    }

    public GameObject GetBulletPrefab()
    {
        return bulletPrefab;
    }

    public float GetShootForce()
    {
        return (maxShootForce - minShootForce) * shootCharge + minShootForce;
    }

    public float GetShootForceRatio()
    {
        return (GetShootForce() - minShootForce) / (maxShootForce - minShootForce);
    }

    public float GetShootCooldown()
    {
        return shootCooldown;
    }

    public List<AudioClip> GetShootSounds()
    {
        return shootSounds;
    }

    public AudioClip GetJumpSound()
    {
        return jumpSound;
    }

    public AudioClip GetJumpChargeSound()
    {
        return jumpChargeSound;
    }

    public AudioClip GetJumpChargeMaxSound()
    {
        return jumpChargeMaxSound;
    }

    public Vector2 GetStartPosition()
    {
        return startPosition;
    }

    public float GetChargeSpeed() { return chargeSpeed; }
#endregion
    
#region Unity Callback Functions
    void Start()
    {
        launchEffect = transform.Find("LaunchEffect").GetComponent<ParticleSystem>();
        jumpChargeEffect = transform.Find("JumpChargeEffect").GetComponent<ParticleSystem>();
        shootEffect = transform.Find("ShootEffect").GetComponent<ParticleSystem>();
        shootChargeEffect = transform.Find("ShootChargeEffect").GetComponent<ParticleSystem>();
        gun = transform.Find("Gun").gameObject;
    }

    void LateUpdate()
    {
        lastPosition = transform.position;
        wasStuck = isStuck;   
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(startPosition, 0.5f);
    }

#endregion
}
