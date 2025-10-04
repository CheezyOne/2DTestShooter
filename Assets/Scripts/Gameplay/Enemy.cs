using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _pushForce;
    [SerializeField] private float _damage;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _stopAttackRange;
    [SerializeField] private float _attackRate;
    [SerializeField] private float _destinationCheckTime;

    private Rigidbody _playerRigidbody;
    private PlayerHealth _playerHealth;
    private WaitForSeconds _destinationWait;
    private Coroutine _destinationRoutine;
    private bool _isAttacking;
    private float _attackTime;

    protected Transform _player;

    private void Awake()
    {
        _destinationWait = new WaitForSeconds(_destinationCheckTime);
    }

    public void SetPlayer(Transform player)
    {
        _player = player;
        _playerHealth = player.GetComponent<PlayerHealth>();
        _playerRigidbody = _player.GetComponent<Rigidbody>();
        _destinationRoutine = StartCoroutine(DestinationRoutine());
    }
    
    private IEnumerator DestinationRoutine()
    {
        while (true)
        {
            if (_player == null)
                yield break;

            _agent.SetDestination(_player.position);
            yield return _destinationWait;
        }
    }

    private void Update()
    {
        if (_player == null)
        {
            if(_destinationRoutine!=null)
                StopCoroutine(_destinationRoutine);

            return;
        }

        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);

        if (distanceToPlayer <= _attackRange && !_isAttacking)
        {
            _agent.isStopped = true;
            _isAttacking = true;
            return;
        }
        
        if (distanceToPlayer > _attackRange && _isAttacking)
        {
            _attackTime = 0;
            _agent.isStopped = false;
            _isAttacking = false;
            return;
        }

        _attackTime += Time.deltaTime;

        if (_isAttacking)
        {
            Vector3 direction = (_player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _rotationSpeed);

            if(_attackTime >= _attackRate)
            {
                _attackTime = 0;
                AttackPlayer();
            }
        }
    }

    protected virtual void AttackPlayer()
    {
        if (_playerHealth == null)
            return;

        _playerHealth.TakeDamage(_damage);
        Vector3 knockbackDirection = (_player.position - transform.position).normalized;
        knockbackDirection.y = 0;
        _playerRigidbody.AddForce(knockbackDirection * _pushForce, ForceMode.Impulse);
    }
}