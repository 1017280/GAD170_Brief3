////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description:
//   Contains the behaviour for how the main camera should work. Follows the player when game state
//   is GameState.running but becomes controllable with WASD QE when in GameState.panMode.
//  Purpose:
//   To follow the player or allow the user to control it in panMode.
//  Usage:
//   Put script on the main scene camera and add the player transform reference in the inspector
////////////////////////////////////////////////////////////////////////////////////////////////////

#region Namespace Dependencies
using UnityEngine;
#endregion

public class CameraController : MonoBehaviour
{
#region Inspector
    ///<summary>A reference to the player this camera will be following</summary>
    [Tooltip("A reference to the player this camera will be following")]
    [SerializeField] private Transform playerTransform;
    ///<summary>The speed of which the camera can zoom in pan mode</summary>
    [Tooltip("The speed of which the camera can zoom in pan mode")]
    [SerializeField] private float zoomSpeed = 50;
    ///<summary>The speed of which the camera pans in pan mode</summary>
    [Tooltip("The speed of which the camera pans in pan mode")]
    [SerializeField] private float panSpeed = 5;
    ///<summary>The minimum size/maximum zoom for the camera in pan mode</summary>
    [Tooltip("The minimum size/maximum zoom for the camera in pan mode")]
    [SerializeField] private float minSize = 5;
    ///<summary>The maximum size/minimum zoom for the camera in pan mode</summary>
    [Tooltip("The minimum maximum/minimum zoom for the camera in pan mode")]
    [SerializeField] private float maxSize = 10;
#endregion
    
#region Local Data
    private Vector2 velocity;
    private bool isFollowingPlayer = true;
    private Camera camera;
    private float targetCamSize;
    private float startSize;
#endregion

#region Unity Callback Functions
    void OnEnable()
    {
        GameEvents.GameStateChangedEvent += OnGameStateChanged;
    }

    void OnDestroy()
    {
        GameEvents.GameStateChangedEvent -= OnGameStateChanged;
    }

    void Start()
    {
        Debug.Assert(playerTransform != null, "Player transform not set in camera");
        camera = GetComponent<Camera>();

        targetCamSize = camera.orthographicSize;
        startSize = camera.orthographicSize;
    }

    void FixedUpdate()
    {
        if (isFollowingPlayer)   
        {
            var newPos = Vector2.SmoothDamp(transform.position, playerTransform.position, ref velocity, 0.3f, 20.0f, Time.fixedDeltaTime);
            transform.position = new Vector3(newPos.x, newPos.y, -10.0f);     
        }

        if (GameStateManager.GetCurrentState() == GameState.PanMode)
        {
            float hAxis = Input.GetAxisRaw("Horizontal");
            float vAxis = Input.GetAxisRaw("Vertical");
            
            transform.position += new Vector3(hAxis, vAxis) * panSpeed * Time.deltaTime;

            float zoom = 0;

            if (Input.GetKey(KeyCode.Q)) 
            {
                zoom = zoomSpeed * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.E)) 
            {
                zoom = -zoomSpeed * Time.deltaTime;
            }

            targetCamSize -= zoom * zoomSpeed * Time.deltaTime;
            targetCamSize = Mathf.Clamp(targetCamSize, minSize, maxSize);
        }

        camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, targetCamSize, 0.1f);
    }

#endregion

#region Game Event Callback Functions

void OnGameStateChanged(GameState newState)
{
    if (newState != GameState.Playing)
    {
        isFollowingPlayer = false;
    }

    if (newState == GameState.PanMode)
    {
        targetCamSize = startSize + (maxSize - minSize) / 2.0f;
    }

    if (newState == GameState.Playing)
    {
        isFollowingPlayer = true;
        targetCamSize = startSize;
    }
}

#endregion

}
