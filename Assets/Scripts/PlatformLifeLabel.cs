#region Namespace Dependencies
using UnityEngine;
using UnityEngine.UI;
#endregion

[RequireComponent(typeof(Text))]
public class PlatformLifeLabel : MonoBehaviour
{
#region Local Data
    private PlatformLife targetPlatform;
    private Text label;
#endregion

#region Unity Callback Events
    void OnEnable()
    {
        GameEvents.PlatformLifeChangedEvent += OnPlatformLifeChanged;
        GameEvents.UniversalPlatformLifeChangedEvent += OnThisPlatformLifeChanged;
        GameEvents.PlatformDestroyedEvent += OnPlatformDestroyed;
    }

    void OnDestroy()
    {
        GameEvents.PlatformLifeChangedEvent -= OnPlatformLifeChanged;
        GameEvents.UniversalPlatformLifeChangedEvent -= OnThisPlatformLifeChanged;
        GameEvents.PlatformDestroyedEvent -= OnPlatformDestroyed;
    }

    void Awake()
    {
        label = this.GetComponent<Text>();
    }

    void Update()
    {
        var rend = targetPlatform.GetComponent<Renderer>();
        transform.position = targetPlatform.GetLabelPosition();//Camera.main.ScreenToWorldPoint(targetPlatform.GetLabelPosition());
    }

    public void SetPlatform(PlatformLife platform, int startLives) 
    {
        targetPlatform = platform;
        label.text = startLives.ToString();
    }

#endregion

#region Game Event Callback Functions
    void OnPlatformLifeChanged(PlatformLife platform, int currentLives)
    {
        if (targetPlatform == platform)
        {
            OnThisPlatformLifeChanged(currentLives);
        }
    }

    void OnThisPlatformLifeChanged(int currentLives)
    {
        label.text = currentLives.ToString();
    }

    void OnPlatformDestroyed(PlatformLife platform)
    {
        if (targetPlatform == platform)
        {
            Destroy(gameObject);
        }
    }

#endregion
}
