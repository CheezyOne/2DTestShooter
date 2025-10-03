using UnityEngine;
using TMPro;

public class LoseWindow : BaseWindow
{
    [SerializeField] private TMP_Text _lastWaveNumber;

    public override void Init()
    {
        base.Init();
        _lastWaveNumber.text = (EnemySpawner.Instance.WaveIndex+1).ToString();
    }
}