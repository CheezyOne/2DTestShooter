using UnityEngine;

public class Injector : MonoBehaviour
{
    [SerializeField] private PlayerMovements _player; //Можно изменить на многократное создание игрока (воскрешение/клоны), но в ТЗ ничего не указано => оставить так
    [SerializeField] private Joystick _joystick;

#if UNITY_EDITOR
    [SerializeField] private bool _isSimulatingMobile;
#endif

    private void Awake()
    {
        if (Application.isMobilePlatform)
        {
            _player.Initialize(new MobileInputService(_joystick));
        }
        else
        {
            _player.Initialize(new DesktopInputService());
        }

#if UNITY_EDITOR
        if (_isSimulatingMobile)
            _player.Initialize(new MobileInputService(_joystick));
#endif
    }
}