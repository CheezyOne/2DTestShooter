using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Enemy _enemyPrefab;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _spawnDistanceMultiplier = 1.2f;
    [SerializeField] private Transform _enemiesHolder;
    [SerializeField] private Vector2 _worldSize;
    [SerializeField] private float _spawnHeight;
    [SerializeField] private float _spawnTime;

    private Coroutine _spawnEnemyRoutine;
    private WaitForSeconds _nextEnemySpawnWait;

    private void Awake()
    {
        _nextEnemySpawnWait = new WaitForSeconds(_spawnTime);
        _spawnEnemyRoutine = StartCoroutine(SpawnEnemyRoutine());
    }
    
    private IEnumerator SpawnEnemyRoutine()
    {
        while(true)
        {
            SpawnEnemy();
            yield return _nextEnemySpawnWait;
        }
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPosition;

        do
        {
            spawnPosition = CalculateSpawnPosition();
            spawnPosition.x = Mathf.Clamp(spawnPosition.x, -_worldSize.x / 2, _worldSize.x / 2);
            spawnPosition.z = Mathf.Clamp(spawnPosition.z, -_worldSize.y / 2, _worldSize.y / 2);
        }
        while (IsPositionInCameraView(spawnPosition));

        Enemy newEnemy = Instantiate(_enemyPrefab, spawnPosition, Quaternion.identity, _enemiesHolder);
        newEnemy.SetPlayer(_player);
    }

    private Vector3 CalculateSpawnPosition()
    {
        Bounds cameraBounds = GetCameraBoundsAtHeight(_spawnHeight);
        float expandedWidth = cameraBounds.size.x * _spawnDistanceMultiplier;
        float expandedHeight = cameraBounds.size.z * _spawnDistanceMultiplier;
        Vector3 cameraCenter = cameraBounds.center;

        // Определяем безопасные зоны спавна (вне видимости)
        float leftBound = cameraCenter.x - expandedWidth / 2;
        float rightBound = cameraCenter.x + expandedWidth / 2;
        float topBound = cameraCenter.z + expandedHeight / 2;
        float bottomBound = cameraCenter.z - expandedHeight / 2;

        // Выбираем случайную сторону для спавна
        int side = Random.Range(0, 4);
        Vector3 spawnPos;

        switch (side)
        {
            case 0: // Слева
                spawnPos = new Vector3(
                    leftBound,
                    _spawnHeight,
                    Random.Range(bottomBound, topBound)
                );
                break;

            case 1: // Справа
                spawnPos = new Vector3(
                    rightBound,
                    _spawnHeight,
                    Random.Range(bottomBound, topBound)
                );
                break;

            case 2: // Сверху
                spawnPos = new Vector3(
                    Random.Range(leftBound, rightBound),
                    _spawnHeight,
                    topBound
                );
                break;

            case 3: // Снизу
                spawnPos = new Vector3(
                    Random.Range(leftBound, rightBound),
                    _spawnHeight,
                    bottomBound
                );
                break;

            default:
                spawnPos = Vector3.zero;
                break;
        }

        return spawnPos;
    }

    private Bounds GetCameraBoundsAtHeight(float height)
    {
        Plane plane = new Plane(Vector3.up, new Vector3(0, height, 0));
        Vector3[] screenCorners = new Vector3[4];
        screenCorners[0] = new Vector3(0, 0, 0); // нижний левый
        screenCorners[1] = new Vector3(Screen.width, 0, 0); // нижний правый
        screenCorners[2] = new Vector3(0, Screen.height, 0); // верхний левый
        screenCorners[3] = new Vector3(Screen.width, Screen.height, 0); // верхний правый
        Vector3 min = new Vector3(float.MaxValue, height, float.MaxValue);
        Vector3 max = new Vector3(float.MinValue, height, float.MinValue);

        foreach (Vector3 screenCorner in screenCorners)
        {
            Ray ray = _camera.ScreenPointToRay(screenCorner);
            float enter;

            if (plane.Raycast(ray, out enter))
            {
                Vector3 worldPoint = ray.GetPoint(enter);
                min = Vector3.Min(min, worldPoint);
                max = Vector3.Max(max, worldPoint);
            }
        }

        Bounds bounds = new Bounds();
        bounds.SetMinMax(min, max);
        return bounds;
    }

    bool IsPositionInCameraView(Vector3 worldPosition)
    {
        Vector3 viewportPoint = _camera.WorldToViewportPoint(worldPosition);
        return viewportPoint.z > 0 &&
               viewportPoint.x >= 0 && viewportPoint.x <= 1 &&
               viewportPoint.y >= 0 && viewportPoint.y <= 1;
    }

    private void StopSpawnEnemyRoutine()
    {
        StopCoroutine(_spawnEnemyRoutine);
    }

    private void OnEnable()
    {
        EventBus.OnPlayerDie += StopSpawnEnemyRoutine;
    }

    private void OnDisable()
    {
        EventBus.OnPlayerDie -= StopSpawnEnemyRoutine;
    }
}