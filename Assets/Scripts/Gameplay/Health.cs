using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour, IDamagable, IKillable
{
    [SerializeField] private float _maxHealth;
    [SerializeField] private Image _healthBarForeground;

    private float _currentHealth;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            Die();
        }

        _healthBarForeground.fillAmount = _currentHealth / _maxHealth;
    }

    public virtual void Die()
    {

    }
}