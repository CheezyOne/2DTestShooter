using System;

public static class EventBus
{
    public static Action OnPlayerDie;
    public static Action OnReloadComplete;
    public static Action<int> OnWaveSpawned;
}