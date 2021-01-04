
using System;
using UnityEngine;

public abstract class Singleton<T>: MonoBehaviour
{
    public static T Instance;
    
    protected virtual void Awake()
    {
        GameObject go = gameObject;
        
        if (Instance != null)
        {
            Destroy(go);
            return;
        }

        Instance = go.GetComponent<T>();
        DontDestroyOnLoad(go);
        RegisterListeners();
    }

    protected virtual void OnDestroy()
    {
        UnregisterListeners();
    }

    protected abstract void UnregisterListeners();
    protected abstract void RegisterListeners();
    
} 