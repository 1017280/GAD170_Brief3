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
            transform.position = myData.stuckPosition;
            myRigidbody.velocity = Vector2.zero;
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
            Debug.Log("JUMP (" + myData.GetJumpForce().ToString() + ") " + myInput.lookDir.ToString() + " " + myInput.lookDir.magnitude);
            this.Jump(myData.GetJumpForce(), myInput.lookDir);
        }


        transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(myInput.lookDir.y, myInput.lookDir.x));

        
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
        myData.isStuck = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "platform" && collision.gameObject != myData.currentWall) 
        {
            Debug.Log("STUCK");
            myData.isStuck = true;
            myData.canJump = true;
            myData.currentWall = collision.gameObject;
            myData.jumpCharge = 0.0f;
            myData.stuckPosition = transform.position;
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "platform" && collision.gameObject == myData.currentWall) 
        {
            myData.currentWall = null;
        }
    }
}
