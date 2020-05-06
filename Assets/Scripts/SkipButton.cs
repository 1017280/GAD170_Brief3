////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description:
//   Simple script that holds a button click function
//  Purpose:
//   To be set to the unity event in a button
//  Usage:
//   Put script on button that should invoke it on click, and assign function in inspector
////////////////////////////////////////////////////////////////////////////////////////////////////

#region Namespace Dependencies
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#endregion

public class SkipButton : MonoBehaviour
{
    public void OnSkipClick()
    {
        GameEvents.PlayerWonEvent.Invoke();
    }
}
