using UnityEngine;

public class UIInputManager : MonoBehaviour
{
    [SerializeField] private GameObject _joystick;

    private void Start()
    {
        SetupUIForMobile();
    }

    private void SetupUIForMobile()
    {
        if (_joystick != null)
            _joystick.SetActive(Application.isMobilePlatform);
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