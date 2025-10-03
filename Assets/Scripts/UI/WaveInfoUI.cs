using UnityEngine;
using TMPro;

public class WaveInfoUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _waveNumber;
    [SerializeField] private TMP_Text _enemiesLeft;

    private void Update()
    {
        if (EnemySpawner.Instance == null)
            return;

        _waveNumber.text = (EnemySpawner.Instance.WaveIndex + 1).ToString();
        _enemiesLeft.text = EnemySpawner.Instance.EnemiesLeftThisWave.ToString();
    }
}