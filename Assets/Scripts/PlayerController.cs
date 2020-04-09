#region Namespace Dependencies
using UnityEngine;
#endregion

#region Component Dependences

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerData))]
[RequireComponent(typeof(Rigidbody2D))]

#endregion
public class PlayerController : MonoBehaviour
{

#region Data References
    /*****************************************************************************************
                                        DATA REFERENCES
    *****************************************************************************************/
    private PlayerInput myInput;
    private PlayerData myData;
    private Rigidbody2D myRigidbody;
    private AudioSource gunAudio;
    private AudioSource pipeAudio;
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
        /* Set all required references */
        myInput = this.GetComponent<PlayerInput>();
        myData = this.GetComponent<PlayerData>();
        myRigidbody = this.GetComponent<Rigidbody2D>();
        gunAudio = this.transform.Find("Gun").GetComponent<AudioSource>();
        pipeAudio = this.transform.Find("Pipe").GetComponent<AudioSource>();

        /* Assert that all references were set correctly. Terminate on failure 
            and print a message about it                                        */
        Debug.Assert(myInput != null, "PlayerInput reference was not found!");
        Debug.Assert(myData != null, "PlayerData reference was not found!");
        Debug.Assert(myRigidbody != null, "Player RigidBody2D was not found!");
        Debug.Assert(gunAudio != null, "Player did not find AudioSource on child object 'Gun'!");
        Debug.Assert(pipeAudio != null, "Player did not find AudioSource on child object 'Pipe'!");

        Debug.Log("Player start at " + transform.position.ToString());
        transform.position = myData.GetStartPosition();
    }

    void Update()
    {
        if (myInput.wantPanModeSwitch)
        {
            if (GameStateManager.GetCurrentState() == GameState.Playing)
            {
                GameStateManager.SetState(GameState.PanMode);
            }
            else if (GameStateManager.GetCurrentState() == GameState.PanMode)
            {
                GameStateManager.SetState(GameState.Playing);
            }
        }

        if (GameStateManager.GetCurrentState() != GameState.Playing) 
        {
            myRigidbody.velocity = Vector2.zero;
            transform.position = myData.freezePosition;
            return;
        }
        // Any logic beyond this point only happens in GameState.Playing

        if (myData.isStuck) 
        {
            Freeze();
        }
        else 
        {
            myData.framesUnstuck++;
        }

        if (myInput.wantGunCharge && myData.canShoot) 
        {
            this.ChargeGun(myData.GetChargeSpeed());
        }
        else
        {
            myData.shootChargeEffect.Stop();
        }

        if (myInput.wantShoot && myData.canShoot) 
        {
            this.Shoot(myData.GetShootForce(), myInput.lookDir);
        }

        if (myInput.wantJumpCharge && myData.canJump)
        {
            this.ChargeJump(myData.GetChargeSpeed());
        }
        else 
        {
            myData.jumpChargeEffect.Stop();
        }

        if (myInput.wantJump && myData.canJump)
        {
            this.Jump(myData.GetJumpForce(), myInput.lookDir);
        }

        if (!myData.isStuck && myData.currentWall != null && myData.framesUnstuck > 5) 
        {
            BecomeStuckTo(myData.currentWall);
        }

        UpdateRotation();

        if (myData.shootWait < myData.GetShootCooldown())
        {
            ShootCooldown();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "platform" && collision.gameObject != myData.currentWall) 
        {
            BecomeStuckTo(collision.gameObject);
        }

        if (collision.transform.tag == "winflag")
        {
            GameEvents.PlayerWonEvent.Invoke();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "platform" && collision.gameObject == myData.currentWall) 
        {
            Debug.Assert(collision.gameObject.GetComponent<PlatformLife>(), "GameObject tagged with \"platform\" did not have a PlatformLife component");
            GameEvents.PlayerleftWallEvent.Invoke(collision.gameObject, this.gameObject);
            myData.currentWall = null;
        }
    }

#endregion

#region Local API

    private void ChargeGun(float rate)
    {
        myData.shootCharge += rate * Time.deltaTime;
        if (!myData.shootChargeEffect.isPlaying)
        {
            myData.shootChargeEffect.Play();
        }
        float scale = 0.4f + 0.6f * myData.GetShootForceRatio();
        myData.shootChargeEffect.transform.localScale = new Vector3(scale, scale, 1.0f);
    }

    private void ChargeJump(float rate)
    {
        myData.jumpCharge += rate * Time.deltaTime;
        if (!myData.jumpChargeEffect.isPlaying) 
        {
            myData.jumpChargeEffect.Play();
            SoundPlayer.instance.Play(pipeAudio, myData.GetJumpChargeSound(), 1.0f);
        }
        if (!pipeAudio.isPlaying)
        {
            SoundPlayer.instance.Play(pipeAudio, myData.GetJumpChargeMaxSound(), 1.0f, true);
        }
        myData.jumpChargeEffect.transform.localScale = new Vector3(myData.GetJumpForceRatio(), myData.GetJumpForceRatio(), 1);
    }

    private void Shoot(float force, Vector2 direction)
    {
        SoundPlayer.instance.Play(gunAudio, myData.GetShootSounds()[Random.Range(0, myData.GetShootSounds().Count)], 0.1f + 0.9f * myData.GetShootForceRatio());
        float scale = 0.4f + 0.6f * myData.GetShootForceRatio();
        myData.shootEffect.transform.localScale = new Vector3(scale, scale, 1.0f);
        myData.canShoot = false;
        myData.shootCharge = 0.0f;
        myData.shootWait = 0;
        var bullet = Instantiate(myData.GetBulletPrefab(), myData.gun.transform.position, Quaternion.Euler(transform.rotation.eulerAngles));
        bullet.GetComponent<Bullet>().direction = direction;
        bullet.GetComponent<Bullet>().force = force;
        myData.shootEffect.Play();
    }

    private void Jump(float force, Vector2 direction) 
    {
        SoundPlayer.instance.Play(pipeAudio, myData.GetJumpSound(), 0.1f + 0.9f * myData.GetJumpForceRatio());
        myRigidbody.AddForce(direction * force);
        myData.canJump = false;
        myData.isStuck = false;
        myData.framesUnstuck = 0;
        myData.launchEffect.Play();
    }

    private void BecomeStuckTo(GameObject go) 
    {
        myData.isStuck = true;
        myData.canJump = true;
        myData.currentWall = go;
        myData.jumpCharge = 0.0f;
        myData.stuckPosition = transform.position;
    }

    private void Freeze()
    {
        transform.position = myData.stuckPosition;
        myRigidbody.velocity = Vector2.zero;
        myData.framesUnstuck = 0;
    }

    private void UpdateRotation()
    {
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(myInput.lookDir.y, myInput.lookDir.x));
    }

    private void ShootCooldown()
    {
        myData.shootWait += Time.deltaTime;        
        if (myData.shootWait >= myData.GetShootCooldown())    
        {
            myData.canShoot = true;
        }
    }

#endregion
    
#region Game Event Function Callbacks
    void OnGameStateChange(GameState newState)
    {
        if (newState != GameState.Playing)
        {
            myData.freezeVelocity = myRigidbody.velocity;
            myData.freezePosition = transform.position;

            myData.freezeJumpEffectState = myData.launchEffect.isPlaying;
            myData.freezeJumpChargeEffectState = myData.jumpChargeEffect.isPlaying;
            myData.freezeShootEffectState = myData.shootEffect.isPlaying;
            myData.freezeShootChargeEffectState = myData.shootChargeEffect.isPlaying;
            myData.jumpChargeEffect.Pause();
            myData.launchEffect.Pause();
            myData.shootEffect.Pause();
            myData.shootChargeEffect.Pause();
        }
        
        if (newState == GameState.Playing)
        {
            myRigidbody.velocity = myData.freezeVelocity;
            if (myData.freezeJumpEffectState) myData.launchEffect.Play();
            if (myData.freezeJumpChargeEffectState) myData.jumpChargeEffect.Play();
            if (myData.freezeShootEffectState) myData.shootEffect.Play();
            if (myData.freezeShootChargeEffectState) myData.shootChargeEffect.Play();
        }
    }
#endregion

    
}
