////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description:
//   Updates and holds input data relevant to the player
//  Purpose:
//   To be used in PlayerController for all input actions and be a nice interface in the inspector.
//   Also servers the purpose to keep PlayerController more clean and separate logic from data.
//  Usage:
//   Require this component on the PlayerController and hold a reference to it and refer to it
//   for any input data
////////////////////////////////////////////////////////////////////////////////////////////////////

#region Namespace Dependencies
using UnityEngine;
#endregion

enum MouseButton : int
{
    left = 0, right = 1, middle = 2,
}
public class PlayerInput : MonoBehaviour
{
#region Inspector
    /*****************************************************************************************
                                            INSPECTOR
    *****************************************************************************************/

    [SerializeField] private string jumpKey = "space";
    [SerializeField] private MouseButton shootButton = MouseButton.left;
    [SerializeField] private string panModeKey = "r";
#endregion

#region Exposed Data
    /*****************************************************************************************
                                        EXPOSED DATA
    *****************************************************************************************/
    public bool wantJumpCharge { get; private set; } = false;
    public bool wantJump { get; private set; } = false;
    public bool wantGunCharge { get; private set; } = false;
    public bool wantShoot { get; private set; } = false;
    public bool wantPanModeSwitch { get; private set; } = false;
    public Vector2 lookDir { get; private set; } = new Vector2(0, 0);
#endregion

#region Unity Callback Functions
    void Update()
    {
        var mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        wantJumpCharge = Input.GetKey(jumpKey);
        wantJump = Input.GetKeyUp(jumpKey);
        wantGunCharge = Input.GetMouseButton((int)shootButton);
        wantShoot = Input.GetMouseButtonUp((int)shootButton);
        lookDir = mouseWorld - transform.position;
        // Normalize
        lookDir /= lookDir.magnitude;
        wantPanModeSwitch = Input.GetKeyDown(panModeKey);
    }
#endregion
}
