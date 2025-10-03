using UnityEngine;

public class DeathEffect : MonoBehaviour
{
    [SerializeField] private float _lifeTime;

    private void OnEnable()
    {
        PoolManager.Instance.DestroyObject(gameObject, _lifeTime);
    }
}