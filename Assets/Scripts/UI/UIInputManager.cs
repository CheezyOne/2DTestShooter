using UnityEngine;

public class UIInputManager : MonoBehaviour
{
    [SerializeField] private GameObject _mobileControls;

    private void Awake()
    {
        SetupUIForMobile();
    }

    private void SetupUIForMobile()
    {
        if (_mobileControls != null)
            _mobileControls.SetActive(Application.isMobilePlatform);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        if (!Application.isPlaying) 
            return;

        SetupUIForMobile();
    }
#endif
}