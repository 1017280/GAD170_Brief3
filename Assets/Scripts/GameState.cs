////////////////////////////////////////////////////////////////////////////////////////////////////
//  Description:
//   Wraps around a hidden GameState value and controls what happens when the state is changed.
//  Purpose:
//   To control the current game state and dispatch an event for when its changed                 
//  Usage:
//   Call SetState() to set the current state and GetCurrentState() to get the current state.
//   Subscribe to GameEvents.GameStateChangedEvent to be notifed when the event is changed
////////////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;

public enum GameState
{
    Paused, Playing, PanMode
}

public static class GameStateManager
{

    public static void SetState(GameState state) 
    {
        if (currentState != state)
        {
            GameEvents.GameStateChangedEvent.Invoke(state);
        }
        currentState = state;
    }

    public static GameState GetCurrentState()
    {
        return currentState;
    }

    private static GameState currentState = GameState.Playing;
}




