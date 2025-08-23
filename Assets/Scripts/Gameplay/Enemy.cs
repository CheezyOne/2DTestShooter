using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _pushForce;
    [SerializeField] private float _damage;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _attackRange;
    [SerializeField] private float _attackRate;
    [SerializeField] private float _destinationCheckTime;

    private Transform _player;
    private Rigidbody _playerRigidbody;
    private PlayerHealth _playerHealth;
    private WaitForSeconds _attackWait;
    private WaitForSeconds _destinationWait;
    private Coroutine _attackRoutine;
    private Coroutine _destinationRoutine;
    private bool _isAttacking;

    private void Start()
    {
        _attackWait = new WaitForSeconds(_attackRate);
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

        if (distanceToPlayer <= _attackRange-0.1f && !_isAttacking)
        {
            StartAttack();
        }
        else if (distanceToPlayer > _attackRange+0.1f && _isAttacking)
        {
            StopAttack();
        }
    }

    private void StartAttack()
    {
        _isAttacking = true;
        _agent.isStopped = true; 
        _attackRoutine = StartCoroutine(AttackRoutine());
    }

    private void StopAttack()
    {
        _isAttacking = false;
        _agent.isStopped = false; 

        if (_attackRoutine != null)
            StopCoroutine(_attackRoutine);
    }

    private IEnumerator AttackRoutine()
    {
        while (_isAttacking && _playerHealth != null)
        {
            AttackPlayer();
            yield return _attackWait;
        }
    }

    private void AttackPlayer()
    {
        if (_playerHealth == null)
            return;

        _playerHealth.TakeDamage(_damage);

        Vector3 knockbackDirection = (_player.position - transform.position).normalized;
        knockbackDirection.y = 0;
        _playerRigidbody.AddForce(knockbackDirection * _pushForce, ForceMode.Impulse);
    }
}