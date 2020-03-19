using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    /*****************************************************************************************
                                            INSPECTOR
    *****************************************************************************************/

    [SerializeField] private float minJumpForce = 300.0f;
    [SerializeField] private float maxJumpForce = 1500.0f;
    [SerializeField] private float chargeSpeed = 1.0f;
    [SerializeField] private float minShootForce = 300.0f;
    [SerializeField] private float maxShootForce = 1000.0f;
    [SerializeField] private float shootCooldown = 1.0f;
    [SerializeField] private GameObject bulletPrefab;

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
    public GameObject gun { get; private set; }
    public float shootWait { get; set; } = 0;

    public float GetJumpForce()
    {
        return minJumpForce + (maxJumpForce - minJumpForce) * jumpCharge;
    }

    public GameObject GetBulletPrefab()
    {
        return bulletPrefab;
    }

    public float GetShootForce()
    {
        return (maxShootForce - minShootForce) * shootCharge + minShootForce;
    }

    public float GetShootCooldown()
    {
        return shootCooldown;
    }

    void Start()
    {
        launchEffect = GetComponentInChildren<ParticleSystem>();
        gun = transform.Find("Gun").gameObject;
    }

    void LateUpdate()
    {
        lastPosition = transform.position;
        wasStuck = isStuck;   
    }

    public float GetChargeSpeed() { return chargeSpeed; }
}
