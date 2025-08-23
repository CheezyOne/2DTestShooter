using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<T>();
            }

            return _instance;
        }
    }

    protected virtual void Awake()
    {
        _instance = (T)this;
    }

    private void OnDestroy()
    {
        _instance = null;
    }
}
