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

        if(recursive == false) // ��ͻ�� X (���� �ڽĸ� ã�� ���)
        {
            Transform t = go.transform.GetChild(0);

            if(string.IsNullOrEmpty(name) || t.name == name)
            {
                T component = t.GetComponent<T>();
                if(component != null)
                    return component;
            }
        }
        else // ��ͻ�� O (�ڽ��� �ڽı��� ã�� ���)
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
