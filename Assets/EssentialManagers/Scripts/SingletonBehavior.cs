using UnityEngine;

public abstract class SingletonBehavior<T> : MonoBehaviour where T : Component
{
    public static T instance;

    protected virtual void Awake()
    {
        if (instance != null & instance != this)
        {
            
            Destroy(gameObject);
        }

        else
        {
            instance = this as T;
        }
    }
}