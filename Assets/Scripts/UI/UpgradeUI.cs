using TMPro;
using UnityEngine;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _descriptionText;
    [SerializeField] private TMP_Text _currentLevelText;

    private Upgrade _upgradeInfo;

    public bool IsInited { get; private set; }

    public void Init(Upgrade upgradeInfo)
    {
        IsInited = true;
        _upgradeInfo = upgradeInfo;
        _currentLevelText.text = _currentLevelText.text + _upgradeInfo.CurrentLevel;
        _descriptionText.text = _upgradeInfo.Description;
    }

    public void ChoseUpgrade()
    {
        UpgradesManager.Instance.GetUpgrade(_upgradeInfo);
    }
}