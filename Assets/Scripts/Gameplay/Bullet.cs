using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private int _lifetime;

    public Rigidbody Rigidbody => _rigidbody;
    public float BulletSpeed => _bulletSpeed;

    private float _damage;

    private void Awake()
    {
        Destroy(gameObject, _lifetime);
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out EnemyHealth enemyHealth))
        {
            enemyHealth.TakeDamage(_damage);
            Destroy(gameObject);
        }
    }
}