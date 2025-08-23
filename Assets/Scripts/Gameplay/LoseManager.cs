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
        WindowsManager.Instance.OpenWindow(_loseWindow);
    }
}