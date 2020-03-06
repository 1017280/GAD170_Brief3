using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    /*****************************************************************************************
                                            INSPECTOR
    *****************************************************************************************/

    [SerializeField] private float minJumpForce = 100.0f;
    [SerializeField] private float maxJumpForce = 1500.0f;
    [SerializeField] private float chargeSpeed = 1.0f;
    [SerializeField] private float shootForce = 100.0f;

    /*****************************************************************************************
                                            EXPOSED DATA
    *****************************************************************************************/

    public bool canJump { get; set; } = true;
    public bool isAirborne { get; set; } = false;
    public bool canShoot { get; set; } = true;
    public bool isShooting { get; set; } = false;
    public bool isStuck { get; set; } = false;
    public float shootCharge { get { return _shootCharge; } set { _shootCharge = Mathf.Clamp01(value); }}
    private float _shootCharge = 0.0f;
    public float jumpCharge { get { return _jumpCharge; } set { _jumpCharge = Mathf.Clamp01(value); }}
    private float _jumpCharge = 0.0f;
    public Vector2 lastPosition { get; private set; } = Vector2.zero;
    public bool wasStuck { get; private set; } = false;

    public float GetJumpForce()
    {
        return minJumpForce + (maxJumpForce - minJumpForce) * jumpCharge;
    }

    public float GetShootForce()
    {
        return shootForce * shootCharge;
    }

    void LateUpdate()
    {
        lastPosition = transform.position;
        wasStuck = isStuck;   
    }

    public float GetChargeSpeed() { return chargeSpeed; }
}
