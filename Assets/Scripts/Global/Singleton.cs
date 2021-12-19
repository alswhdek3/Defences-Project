using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    static public T Instance { get; private set; }

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = (T)this;
            OnAwake();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        if(Instance == (T)this)
        {
            OnStart();
        }
    }

    virtual protected void OnAwake()
    {

    }
    virtual protected void OnStart()
    {

    }
}
