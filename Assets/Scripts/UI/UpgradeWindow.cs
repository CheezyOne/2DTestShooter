using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum UpgradeType
{
    Heal,
    Speed,
    MaxHealth
}

public class UpgradeWindow : BaseWindow
{
    [SerializeField] private UpgradeUI[] _upgradesUI;

    public override void Init()
    {
        base.Init();
        List<Upgrade> upgradeInfos = new();
        upgradeInfos.AddRange(UpgradesManager.Instance.UpgradeInfos);
        EventBus.OnUpgradeWindowOpen?.Invoke();
        List<UpgradeUI> upgrades = new();
        upgrades.AddRange(_upgradesUI);

        for (int i = upgradeInfos.Count - 1; i >= 0; i--)
        {
            if (upgradeInfos[i].CurrentLevel == upgradeInfos[i].MaxLevel)
            {
                upgradeInfos.RemoveAt(i);
            }
        }

        for (int i = 0; i < upgradeInfos.Count; i++)
        {
            int randomInt = Random.Range(0, upgrades.Count);
            upgrades[randomInt].Init(upgradeInfos[i]);
            upgrades.RemoveAt(randomInt);
        }

        for (int i = 0; i < _upgradesUI.Length; i++)
        {
            if (!_upgradesUI[i].IsInited)
            {
                Destroy(_upgradesUI[i].gameObject);
            }
        }
    }
}