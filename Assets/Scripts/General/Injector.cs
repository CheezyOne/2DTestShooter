using UnityEngine;

public class Injector : MonoBehaviour
{
    [SerializeField] private PlayerMovements _playerMovements; //ћожно изменить на многократное создание игрока (воскрешение/клоны), но в “« ничего не указано => оставить так, но при необходимости создал бы событие создани€ игрока
    [SerializeField] private Joystick _joystick;
    [SerializeField] private Camera _mainCamera;

#if UNITY_EDITOR
    [SerializeField] private bool _isSimulatingMobile;
#endif

    private void Awake()
    {
        if (Application.isMobilePlatform)
        {
            _playerMovements.Initialize(new MobileInputService(_joystick), new MobileRotationService());
        }
        else
        {
            _playerMovements.Initialize(new DesktopInputService(), new DesktopRotationService(_mainCamera));
        }

#if UNITY_EDITOR
        if (_isSimulatingMobile)
            _playerMovements.Initialize(new MobileInputService(_joystick),new MobileRotationService());
#endif
    }
}