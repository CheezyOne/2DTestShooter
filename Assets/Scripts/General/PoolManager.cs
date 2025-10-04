using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using System.Collections;

public class PoolManager : Singleton<PoolManager>
{
    [SerializeField] private Transform _poolObjectsHolder;

    public List<ObjectPool> _objectPools = new();
    private Dictionary<GameObject, Coroutine> _activeCoroutines = new Dictionary<GameObject, Coroutine>();

    private void OnDisable()
    {
        foreach (ObjectPool ObjectPool in _objectPools)
        {
            ObjectPool.InactiveObjects.Clear();
        }

        foreach (var coroutine in _activeCoroutines.Values)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
        }
        _activeCoroutines.Clear();
    }

    public T InstantiateObject<T>(T objectToSpawn, Vector3 spawnPosition, Quaternion spawnRotation, Transform parent = null) where T : Component
    {
        ObjectPool pool = null;

        foreach (ObjectPool objectPool in _objectPools)
        {
            if (objectPool.PoolName == objectToSpawn.gameObject.name)
            {
                pool = objectPool;
                break;
            }
        }

        if (pool == null)
        {
            pool = new ObjectPool() { PoolName = objectToSpawn.gameObject.name };
            _objectPools.Add(pool);
        }

        GameObject poolObject = pool.InactiveObjects.FirstOrDefault();
        T spawnableObject;

        if (poolObject == null)
        {
            if (parent == null)
            {
                spawnableObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation, _poolObjectsHolder);
            }
            else
            {
                spawnableObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation, parent);
            }
        }
        else
        {
            spawnableObject = poolObject.GetComponent<T>();
            spawnableObject.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
            pool.InactiveObjects.Remove(spawnableObject.gameObject);
            spawnableObject.gameObject.SetActive(true);
        }

        return spawnableObject;
    }

    public void DestroyObject(GameObject objectToReturn, float delay = 0)
    {
        if (objectToReturn == null || !objectToReturn.activeInHierarchy)
            return;

        if (_activeCoroutines.ContainsKey(objectToReturn))
        {
            StopCoroutine(_activeCoroutines[objectToReturn]);
            _activeCoroutines.Remove(objectToReturn);
        }

        if (delay == 0)
        {
            ReturnObjectToPool(objectToReturn);
        }
        else
        {
            Coroutine coroutine = StartCoroutine(DestroyObjectRoutine(objectToReturn, delay));
            _activeCoroutines[objectToReturn] = coroutine;
        }
    }

    private void ReturnObjectToPool(GameObject objectToReturn)
    {
        if (objectToReturn == null) 
            return;

        string realObjectName = objectToReturn.name.Replace("(Clone)", ""); 
        ObjectPool pool = _objectPools.FirstOrDefault(p => p.PoolName == realObjectName);

        if (pool == null)
        {
            Debug.LogWarning($"There's no pool for: {realObjectName}");
            Destroy(objectToReturn); 
        }
        else
        {
            objectToReturn.SetActive(false);
            if (!pool.InactiveObjects.Contains(objectToReturn))
                pool.InactiveObjects.Add(objectToReturn);
        }
    }

    private IEnumerator DestroyObjectRoutine(GameObject objectToReturn, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (_activeCoroutines.ContainsKey(objectToReturn))
        {
            _activeCoroutines.Remove(objectToReturn);
            ReturnObjectToPool(objectToReturn);
        }
    }
}

[Serializable]
public class ObjectPool
{
    public string PoolName;
    public List<GameObject> InactiveObjects = new();
}