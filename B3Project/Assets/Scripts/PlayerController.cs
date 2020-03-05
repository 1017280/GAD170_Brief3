using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerData))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{

    /*****************************************************************************************
                                        DATA REFERENCES
    *****************************************************************************************/
    private PlayerInput myInput;
    private PlayerData myData;
    private Rigidbody2D myRigidbody;

    void Start()
    {
        myInput = this.GetComponent<PlayerInput>();
        myData = this.GetComponent<PlayerData>();
        myRigidbody = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (myData.isStuck) 
        {
            transform.position = myData.lastPosition;
        }

        if (myInput.wantGunCharge && myData.canShoot) 
        {
            this.ChargeGun(myData.GetChargeSpeed());
        }

        if (myInput.wantShoot && myData.canShoot) 
        {
            this.Shoot(myData.GetShootForce());
        }

        if (myInput.wantJumpCharge && myData.canJump)
        {
            this.ChargeJump(myData.GetChargeSpeed());
        }

        if (myInput.wantJump && myData.canJump)
        {
            Debug.Log("JUMP");
            this.Jump(myData.GetJumpForce(), myInput.lookDir);
        }


        transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(myInput.lookDir.y, myInput.lookDir.x));
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10.0f);
        
    }

    private void ChargeGun(float rate)
    {
        myData.shootCharge += rate * Time.deltaTime;
    }

    private void ChargeJump(float rate)
    {
        myData.jumpCharge += rate * Time.deltaTime;
    }

    private void Shoot(float force)
    {
        myData.canShoot = false;
        myData.shootCharge = 0.0f;
    }

    private void Jump(float force, Vector2 direction) 
    {
        myRigidbody.AddForce(direction * force);
        myData.canJump = false;
        myData.jumpCharge = 0.0f;
        myData.isStuck = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "platform") 
        {
            Debug.Log("STUCK");
            myData.isStuck = true;
            myData.canJump = true;
        }
    }
}
