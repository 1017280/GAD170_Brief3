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




