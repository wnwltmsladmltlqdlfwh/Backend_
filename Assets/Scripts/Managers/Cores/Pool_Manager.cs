using BackEnd.Functions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pool_Manager
{
    #region Pool
    class Pool
    {
        public GameObject Original { get; private set; } // 풀링할 오브젝트의 원본
        public Transform Root { get; set; } // 풀링한 오브젝트들이 비활성화 시 부모 Transform

        Stack<Poolable> _poolStack = new Stack<Poolable>(); // 오브젝트 풀링 스택

        public void Init(GameObject origin, int count = 5)
        {
            Original = origin;
            Root = new GameObject().transform;
            Root.name = $"{origin.name}_Root";

            for (int i = 0; i < count; i++)
                Push(Create());
        }

        Poolable Create() // 원본 오브젝트 생성
        {
            GameObject go = Object.Instantiate<GameObject>(Original);
            go.name = Original.name;
            return go.GetOrAddComponent<Poolable>();
        }

        public void Push(Poolable poolable) // 사용종료, 스택에 복귀
        {
            if (poolable == null)
                return;

            poolable.transform.parent = Root;
            poolable.gameObject.SetActive(false);
            poolable.isUsing = false;

            _poolStack.Push(poolable);
        }

        public Poolable Pop(Transform parent) // 사용, 스택에서 빼온다.
        {
            Poolable poolable;

            if(_poolStack.Count > 0)
                poolable = _poolStack.Pop();
            else
                poolable = Create();

            poolable.gameObject.SetActive(true);

            //DontDestroyOnLoad 해제
            if(parent == null)
                poolable.transform.parent = Manager.Scene.CurrentScene.transform;

            poolable.transform.parent = parent;
            poolable.isUsing = true;

            return poolable;
        }
    }
    #endregion

    Dictionary<string, Pool> _pool = new Dictionary<string, Pool>();
    Transform _root;

    public void Init()
    {
        if(_root == null)
        {
            _root = new GameObject { name = "@Pool_Root" }.transform;
            Object.DontDestroyOnLoad(_root);
        }
    }

    public void CreatePool(GameObject origin, int count = 0) // 풀링할 오브젝트가 없을때 생성
    {
        Pool pool = new Pool();
        pool.Init(origin, count);
        pool.Root.parent = _root;

        _pool.Add(origin.name, pool);
    }

    public void Push(Poolable poolable) // 오브젝트가 사용 종료
    {
        string name = poolable.gameObject.name;

        if(_pool.ContainsKey(name) == false)
        {
            GameObject.Destroy(poolable.gameObject);
            return;
        }

        _pool[name].Push(poolable);
    }

    public Poolable Pop(GameObject original, Transform parent = null) // 오브젝트 사용
    {
        if(_pool.ContainsKey(original.name) == false)
        {
            CreatePool(original);
        }

        return _pool[original.name].Pop(parent);
    }

    public GameObject GetOriginal(string name) // 오리지널 오브젝트를 _pool 딕셔너리에서 가져옴
    {
        if(_pool.ContainsKey(name) == false)
            return null;

        return _pool[name].Original;
    }
    
    public void Clear()
    {
        foreach(Transform child in _root)
            GameObject.Destroy(child.gameObject);

        _pool.Clear();
    }
}
