using System;

public static class EventBus
{
    public static Action OnPlayerDie;
    public static Action OnReloadComplete;
    public static Action<int> OnWaveSpawned;
    public static Action OnEnemyDie;
    public static Action OnUpgradeWindowOpen;
    public static Action OnUpgradeWindowClose;
    public static Action OnPlayerHealed;
    public static Action OnPlayerMaxHPIncreased;
    public static Action OnPlayerSpeedIncreased;
}