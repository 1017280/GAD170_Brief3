#region Namespace Dependencies
using UnityEngine;
#endregion

public class PlatformLife : MonoBehaviour
{

#region Inspector

    /// <summary> The starting life value for this platform
    /// <para> A "life" in the context of platforms is defined as an allowance for the player to jump off it. </para>
    /// </summary>
    [Tooltip("The amount of \"lives\" to begin with A.K.A. the amount of times the player will be able to jump off it")]
    [SerializeField] private int startingLives = 3;

    /// <summary>The amount of lives at which the player can no longer use this wall</summary>
    [Tooltip("The amount of lives at which the player can no longer use this wall")]
    [SerializeField] private int minLivesThreshold = 0;

    /// <summary>The prefab for the UI Text that will show the amount of remaining lives</summary>
    [Tooltip("The prefab for the UI Text that will show the amount of remaining lives")]
    [SerializeField] private GameObject lifeLabelPrefab;

    /// <summary>The position of which the UI label representing the current amount of lives will be at as a ratio between 0-1</summary>
    [Tooltip("The position of which the UI label representing the current amount of lives will be at as a ratio between 0-1")]
    [SerializeField] private Vector2 labelPositionRatio = new Vector2(0.5f, 0.5f);

#endregion

#region Local Data

    private int currentLives;

#endregion

#region Unity Callback Functions

    void OnEnable() 
    {
        GameEvents.PlayerleftWallEvent += OnPlayerLeftWall;
    }

    void OnDestroy()
    {
        GameEvents.PlayerleftWallEvent -= OnPlayerLeftWall;
    }

    void Start()
    {
        // Just do an if check on the lifeLabelPrefab instead of an assertion, as you should 
        // be able to create platforms without labels for quick testing levels
        if (lifeLabelPrefab != null)
        {
            // Create the label with the canvas as its parent
            var label = Instantiate(lifeLabelPrefab, Vector3.zero, Quaternion.identity, GameObject.Find("Canvas").transform).GetComponent<PlatformLifeLabel>();

            // ..but if a prefab is referenced, it has to be valid
            Debug.Assert(label != null, "Life label prefab does not have the PlatformLifeLabel component");

            label.SetPlatform(this, startingLives);
        }

        // Set default values
        currentLives = startingLives;
    }

    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        var rend = GetComponent<Renderer>();
        Gizmos.DrawWireSphere(GetLabelPosition(), 0.5f);
    }

#endregion

#region Exposed API

    public void RetractLife(int amount = 1)
    {
        currentLives -= amount;
    
        GameEvents.PlatformLifeChangedEvent.Invoke(this, currentLives);
        if (currentLives <= minLivesThreshold)
        {
            Die();
        }
    }

    public void AddLife(int amount = 1) 
    {
        currentLives += amount;
        GameEvents.PlatformLifeChangedEvent.Invoke(this, currentLives);
    }

    public Vector3 GetLabelPosition()
    {
        var rend = GetComponent<Renderer>();
        return transform.position + new Vector3(rend.bounds.size.x * labelPositionRatio.x, rend.bounds.size.y * labelPositionRatio.y);
    }

#endregion

#region Local API

    private void Die() 
    {
        GameEvents.PlatformDestroyedEvent.Invoke(this);
        Destroy(gameObject);
    }

#endregion

#region GameEvent Callback Functions

    void OnPlayerLeftWall(GameObject wall, GameObject player) 
    {
        if (wall == this.gameObject) 
        {
            RetractLife();
        }
    }

#endregion

}








