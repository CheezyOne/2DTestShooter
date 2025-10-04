using System;
using UnityEngine;

public class UpgradesManager : Singleton<UpgradesManager>
{
    [SerializeField] private Upgrade[] _upgradeInfos;
    [SerializeField] private UpgradeWindow _upgradeWindow;

    public Upgrade[] UpgradeInfos => _upgradeInfos;

    private int _maxUpgradedCount;

    public void SetUpgrades()
    {
        for (int i = 0; i < _upgradeInfos.Length; i++)
        {
            if (_upgradeInfos[i].CurrentLevel == _upgradeInfos[i].MaxLevel)
            {
                _maxUpgradedCount++;
            }
        }

        if (_maxUpgradedCount == _upgradeInfos.Length)
        {
            ContinueGame();
            _maxUpgradedCount = 0;
            return;
        }

        _maxUpgradedCount = 0;
        WindowsManager.Instance.OpenWindow(_upgradeWindow);
    }

    private void ContinueGame()
    {
        EventBus.OnUpgradeWindowClose?.Invoke();
        EnemySpawner.Instance.SpawnWave();
        WindowsManager.Instance.CloseCurrentWindow();
    }

    public void GetUpgrade(Upgrade upgradeInfo)
    {
        for (int i = 0; i < _upgradeInfos.Length; i++)
        {
            if (upgradeInfo.UpgradeType == _upgradeInfos[i].UpgradeType)
            {
                _upgradeInfos[i].CurrentLevel++;
            }
        }

        switch (upgradeInfo.UpgradeType)
        {
            case UpgradeType.Heal:
                {
                    EventBus.OnPlayerHealed?.Invoke();
                    break;
                }
            case UpgradeType.Speed:
                {
                    EventBus.OnPlayerSpeedIncreased?.Invoke();
                    break;
                }
            case UpgradeType.MaxHealth:
                {
                    EventBus.OnPlayerMaxHPIncreased?.Invoke();
                    break;
                }
            default:
                {
                    Debug.LogWarning("There's no such upgrade");
                    break;
                }
        }

        ContinueGame();
    }
}

[Serializable]
public struct Upgrade
{
    public int CurrentLevel;
    public int MaxLevel;
    public string Description;
    public UpgradeType UpgradeType;
}