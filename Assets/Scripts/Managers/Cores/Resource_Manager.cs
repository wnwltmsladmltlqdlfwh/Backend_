using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Resource_Manager
{
    public T Load<T>(string path) where T : Object
    {
        if(typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.IndexOf('/');

            if(index >= 0)
                name = name.Substring(0, index);

            GameObject go = Manager.Pool.GetOriginal(name);
            if(go != null)
                return go as T;
        }

        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if(original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        if(original.GetComponent<Poolable>() != null)
            return Manager.Pool.Pop(original, parent).gameObject;

        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;
        return go;
    }

    public void Destroy(GameObject obj)
    {
        if (obj == null)
            return;

        Poolable poolable = obj.GetComponent<Poolable>();
        if(poolable != null)
        {
            Manager.Pool.Push(poolable);
            return;
        }

        Object.Destroy(obj);
    }

    public async void Destroy(GameObject obj, float time)
    {
        if(obj == null)
            return;

        await Task.Delay(Mathf.RoundToInt(time * 1000));

        Poolable poolable = obj.GetComponent<Poolable>();
        if (poolable != null)
        {
            Manager.Pool.Push(poolable);
            return;
        }

        Object.Destroy(obj);
    }

    IEnumerator Timer(float time) { yield return new WaitForSeconds(time); }
}
