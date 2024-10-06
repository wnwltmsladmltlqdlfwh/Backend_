using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Define.Scenes sceneType { get; protected set; } = Define.Scenes.Unknown;

    private void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        object obj = GameObject.FindObjectOfType(typeof(EventSystem));

        if(obj != null)
        {
            
        }
    }

    public abstract void Clear();
}
