using UnityEngine;

public class LoseManager : MonoBehaviour
{
    [SerializeField] private LoseWindow _loseWindow;

    private void OnEnable()
    {
        EventBus.OnPlayerDie += OnPlayerDie;
    }

    private void OnDisable()
    {
        EventBus.OnPlayerDie -= OnPlayerDie;
    }

    private void OnPlayerDie()
    {
        SoundsManager.Instance.PlaySound(SoundType.Lose);
        WindowsManager.Instance.OpenWindow(_loseWindow);
    }
}