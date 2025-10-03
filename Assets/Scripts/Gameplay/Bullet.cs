using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private TrailRenderer _trail;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private int _lifetime;

    private bool _isHit;

    public Rigidbody Rigidbody => _rigidbody;
    public float BulletSpeed => _bulletSpeed;

    private float _damage;

    private void Awake()
    {
        PoolManager.Instance.DestroyObject(gameObject, _lifetime);
    }

    public void SetDamage(float damage)
    {
        _damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_isHit)
            return;

        _isHit = true;

        if (other.TryGetComponent(out Health health))
        {
            health.TakeDamage(_damage);
        }

        PoolManager.Instance.DestroyObject(gameObject);
    }

    private void OnDisable()
    {
        _trail.Clear();
        _isHit = false;
        _rigidbody.velocity = Vector3.zero;
    }
}