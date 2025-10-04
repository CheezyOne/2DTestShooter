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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SoundsManager.Instance.PlaySound(SoundType.Lose);
        WindowsManager.Instance.OpenWindow(_loseWindow);
    }
}