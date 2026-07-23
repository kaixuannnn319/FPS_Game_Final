using System;

// Central place for game-wide events. Call GameEvents.PlayerRespawned()
// from wherever your player's respawn logic lives.
public static class GameEvents
{
    public static event Action OnPlayerRespawn;

    public static void PlayerRespawned()
    {
        OnPlayerRespawn?.Invoke();
    }
}