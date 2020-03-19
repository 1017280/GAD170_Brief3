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
            myData.framesUnstuck = 0;
        }
        else 
        {
            myData.framesUnstuck++;
        }

        if (myInput.wantGunCharge && myData.canShoot) 
        {
            this.ChargeGun(myData.GetChargeSpeed());
        }

        if (myInput.wantShoot && myData.canShoot) 
        {
            this.Shoot(myData.GetShootForce(), myInput.lookDir);
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

        if (!myData.isStuck && myData.currentWall != null && myData.framesUnstuck > 5) 
        {
            Stuck(myData.currentWall);
        }

        transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(myInput.lookDir.y, myInput.lookDir.x));

        myData.shootWait += Time.deltaTime;        
        if (myData.shootWait >= myData.GetShootCooldown())    
        {
            myData.canShoot = true;
        }
    }

    private void ChargeGun(float rate)
    {
        myData.shootCharge += rate * Time.deltaTime;
    }

    private void ChargeJump(float rate)
    {
        myData.jumpCharge += rate * Time.deltaTime;
    }

    private void Shoot(float force, Vector2 direction)
    {
        Debug.Log("SHOOT");
        myData.canShoot = false;
        myData.shootCharge = 0.0f;
        myData.shootWait = 0;
        var bullet = Instantiate(myData.GetBulletPrefab(), myData.gun.transform.position, Quaternion.Euler(transform.rotation.eulerAngles));
        bullet.GetComponent<Bullet>().direction = direction;
        bullet.GetComponent<Bullet>().force = force;
    }

    private void Jump(float force, Vector2 direction) 
    {
        Debug.Log("JUMP");
        myRigidbody.AddForce(direction * force);
        myData.canJump = false;
        myData.isStuck = false;
        myData.framesUnstuck = 0;
        myData.launchEffect.Play();
    }

    private void Stuck(GameObject go) 
    {
        Debug.Log("STUCK");
        myData.isStuck = true;
        myData.canJump = true;
        myData.currentWall = go;
        myData.jumpCharge = 0.0f;
        myData.stuckPosition = transform.position;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "platform" && collision.gameObject != myData.currentWall) 
        {
            Stuck(collision.gameObject);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "platform" && collision.gameObject == myData.currentWall) 
        {
            Debug.Log("Leave wall");
            myData.currentWall = null;
        }
    }
}
