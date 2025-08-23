using UnityEngine;

public class PlayerHealth : Health
{
    [SerializeField] private GameObject _player;

    public override void Die()
    {
        EventBus.OnPlayerDie?.Invoke();
        Destroy(_player);
    }
}