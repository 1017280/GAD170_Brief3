using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipButton : MonoBehaviour
{
    public void OnSkipClick()
    {
        GameEvents.PlayerWonEvent.Invoke();
    }
}
