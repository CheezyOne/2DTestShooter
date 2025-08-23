using System;
using UnityEngine;

public static class EventBus
{
    public static Action OnBannerAdShown;

    public static Action OnFigureTaken;
    public static Action OnFigureReleased;
    public static Action OnFigurePlaced;
    public static Action OnFiguresGeneration;

    public static Action OnContinuePlayingReward;
    public static Action OnClearRowsReward;
    public static Action OnChangeFiguresReward;

    public static Action OnAllPatternsCollected;
    public static Action<Sprite, Vector3> OnPatternCollected;

    public static Action OnScoreAdd;
}