using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindAnyObjectByType<T>();
                if (instance == null)
                {
                    var singletonObject = new GameObject(typeof(T).Name);
                    singletonObject.AddComponent<T>();
                    instance = singletonObject.GetComponent<T>();
                }
            }

            DontDestroyOnLoad(instance);
            return instance;
        }
    }
}
