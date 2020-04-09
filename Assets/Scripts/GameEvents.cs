using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameEvents 
{
    public delegate void PlayerLeftWallEvent(GameObject wall, GameObject player);
    public static PlayerLeftWallEvent PlayerleftWallEvent = new PlayerLeftWallEvent((GameObject w, GameObject p) => {  });

    public delegate void PlatformLifeChange(PlatformLife platform, int currentLives);
    public static PlatformLifeChange PlatformLifeChangedEvent = new PlatformLifeChange((PlatformLife p, int c) => {  });

    public delegate void UniversalPlatformLifeChange(int currentLives);
    public static UniversalPlatformLifeChange UniversalPlatformLifeChangedEvent = new UniversalPlatformLifeChange((int c) => {  });

    public delegate void PlatformDestroyed(PlatformLife platform);
    public static PlatformDestroyed PlatformDestroyedEvent = new PlatformDestroyed((PlatformLife p) => {});

    public delegate void GameStateChanged(GameState newState);
    public static GameStateChanged GameStateChangedEvent = new GameStateChanged((GameState g) => {});

        public delegate void PlayerWon();
    public static PlayerWon PlayerWonEvent = new PlayerWon(() => {});
}