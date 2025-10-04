using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Health : MonoBehaviour, IDamagable, IKillable
{
    [SerializeField] private Image _healthBarForeground;
    [SerializeField] private TMP_Text _healthText;

    [SerializeField] protected float _maxHealth;

    protected float _currentHealth;

    protected virtual void OnEnable()
    {
        _currentHealth = _maxHealth;
        UpdateHealth();
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0; 
            Die();
        }

        UpdateHealth();
    }

    protected void UpdateHealth()
    {
        _healthBarForeground.fillAmount = _currentHealth / _maxHealth;
        _healthText.text = _currentHealth + "/" + _maxHealth;
    }

    public virtual void Die()
    {

    }
}