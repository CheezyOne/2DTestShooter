using UnityEngine;
using System;
using Random = UnityEngine.Random;
using UnityEngine.AI;

public class EnemySpawner : Singleton<EnemySpawner>
{
    [SerializeField] private Transform _player;
    [SerializeField] private float _minSpawnDistance = 3f; 
    [SerializeField] private float _maxSpawnDistance = 5f;
    [SerializeField] private Transform _enemiesHolder;
    [SerializeField] private Vector2 _worldSize;
    [SerializeField] private float _spawnHeight;
    [SerializeField] private WeightedEnemy[] _weightedEnemies;
    [SerializeField] private int _firstWaveAmount;
    [SerializeField] private int _waveIncreaseAmount;
    [SerializeField] private float _navMeshCheckDistance = 1f;
    [SerializeField] private LayerMask _obstacleLayers;

    private const int MAX_SPAWN_ATTEMPTS = 16;

    private int _enemiesThisWave;
    private int _enemiesKilledThisWave;
    private int _waveIndex;

    public int WaveIndex => _waveIndex;
    public int EnemiesLeftThisWave => _enemiesThisWave - _enemiesKilledThisWave;

    protected override void Awake()
    {
        base.Awake();
        SpawnWave();
    }

    public void SpawnWave()
    {
        _enemiesThisWave = _firstWaveAmount + _waveIncreaseAmount * _waveIndex;
        SoundsManager.Instance.PlaySound(SoundType.NewWave);

        for(int i =0;i< _enemiesThisWave; i++)
        {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPosition;
        int attempts = 0;

        do
        {
            spawnPosition = CalculateSpawnPosition();
            spawnPosition.x = Mathf.Clamp(spawnPosition.x, -_worldSize.x / 2, _worldSize.x / 2);
            spawnPosition.z = Mathf.Clamp(spawnPosition.z, -_worldSize.y / 2, _worldSize.y / 2);
            attempts++;

            if (attempts == MAX_SPAWN_ATTEMPTS)
            {
                Debug.LogWarning("Couldn't spawn enemy");
                return;
            }
        }
        while (!IsPositionValid(spawnPosition) && attempts < MAX_SPAWN_ATTEMPTS);

        Enemy newEnemy = PoolManager.Instance.InstantiateObject(GetRandomEnemy(), spawnPosition, Quaternion.identity, _enemiesHolder);
        newEnemy.SetPlayer(_player);
    }

    private bool IsPositionValid(Vector3 position)
    {
        if (!NavMesh.SamplePosition(position, out NavMeshHit hit, _navMeshCheckDistance, NavMesh.AllAreas))
        {
            return false;
        }

        return true;
    }

    private Enemy GetRandomEnemy()
    {
        float totalWeight = 0;

        for(int i=0;i<_weightedEnemies.Length;i++)
        {
            totalWeight += _weightedEnemies[i].Weight;
        }

        float randomValue = Random.Range(0f, totalWeight);

        for (int i = 0; i < _weightedEnemies.Length; i++)
        {
            float cumulativeWeight = 0f;

            foreach (WeightedEnemy weightedEnemy in _weightedEnemies)
            {
                cumulativeWeight += weightedEnemy.Weight;

                if (randomValue < cumulativeWeight)
                {
                    return weightedEnemy.Enemy;
                }
            }
        }

        return _weightedEnemies[_weightedEnemies.Length - 1].Enemy;
    }

    private Vector3 CalculateSpawnPosition()
    {
        Vector2 randomDirection2D = Random.insideUnitCircle.normalized;
        Vector3 randomDirection = new Vector3(randomDirection2D.x, 0, randomDirection2D.y);
        float randomDistance = Random.Range(_minSpawnDistance, _maxSpawnDistance);
        Vector3 spawnPosition = _player.position + randomDirection * randomDistance;
        spawnPosition.y = _spawnHeight;

        return spawnPosition;
    }

    private void OnEnemyDie()
    {
        _enemiesKilledThisWave++;

        if(_enemiesKilledThisWave == _enemiesThisWave)
        {
            _enemiesKilledThisWave = 0;
            _waveIndex++;
            UpgradesManager.Instance.SetUpgrades();
        }
    }

    private void OnEnable()
    {
        EventBus.OnEnemyDie += OnEnemyDie;
    }

    private void OnDisable()
    {
        EventBus.OnEnemyDie -= OnEnemyDie;
    }
}

[Serializable]
public struct WeightedEnemy
{
    public Enemy Enemy;
    public float Weight;
}