using UnityEngine;

public class RangedEnemy : Enemy // If we want to take cover from him, we may remake Enemy script, but i think it's funnier this way
{
    [SerializeField] private Weapon _weapon;

    protected override void AttackPlayer()
    {
        if (_player == null)
            return;

        _weapon.TryShoot();
    }
}