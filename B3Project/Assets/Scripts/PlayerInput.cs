using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MouseButton : int
{
    left = 0, right = 1, middle = 2,
}
public class PlayerInput : MonoBehaviour
{
    /*****************************************************************************************
                                            INSPECTOR
    *****************************************************************************************/

    [SerializeField] private string jumpKey = "space";
    [SerializeField] private MouseButton shootButton = MouseButton.left;

    /*****************************************************************************************
                                        READONLY DATA
    *****************************************************************************************/
    public bool wantJumpCharge { get; private set; } = false;
    public bool wantJump { get; private set; } = false;
    public bool wantGunCharge { get; private set; } = false;
    public bool wantShoot { get; private set; } = false;
    public Vector2 lookDir { get; private set; } = new Vector2(0, 0);

    void Update()
    {
        var mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        wantJumpCharge = Input.GetKey(jumpKey);
        wantJump = Input.GetKeyUp(jumpKey);
        wantGunCharge = Input.GetMouseButton((int)shootButton);
        wantShoot = Input.GetMouseButtonUp((int)shootButton);
        lookDir = mouseWorld - transform.position;
    }
}
