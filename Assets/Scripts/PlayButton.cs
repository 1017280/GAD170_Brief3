////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description:
//   Simple script that holds a button click function
//  Purpose:
//   To be set to the unity event in a button
//  Usage:
//   Put script on button that should invoke it on click, and assign function
////////////////////////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    public void OnPlayClick()
    {
        SceneManager.LoadScene("Level1");
    }
}
