using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseWindow : BaseWindow
{
    [SerializeField] private string _gameplaySceneName;

    public void OnRestartButton()
    {
        SceneManager.LoadScene(_gameplaySceneName);
    }
}