using UnityEngine;

public class Util
{
    public static T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        T component = go.GetComponent<T>();

        if (component == null)
            component = go.AddComponent<T>();

        return component;
    }

    public static GameObject FindChild(GameObject go, string name, bool recursive = false)
    {
        Transform t = FindChild<Transform>(go, name, recursive);

        if (t == null)
            return null;

        return t.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name, bool recursive = false) where T : UnityEngine.Object
    {
        if(go == null)
            return null;

        if(recursive == false) // 재귀사용 X (직속 자식만 찾는 경우)
        {
            Transform t = go.transform.GetChild(0);

            if(string.IsNullOrEmpty(name) || t.name == name)
            {
                T component = t.GetComponent<T>();
                if(component != null)
                    return component;
            }
        }
        else // 재귀사용 O (자식의 자식까지 찾는 경우)
        {
            foreach(T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return (T)component;
            }
        }

        return null;
    }
}
