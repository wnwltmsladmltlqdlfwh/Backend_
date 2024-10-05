using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public abstract class UI_Base : MonoBehaviour
{
    Dictionary<Type, UnityEngine.Object[]> _objects = new Dictionary<Type, UnityEngine.Object[]>();

    private void Start()
    {
        Init();
    }

    protected abstract void Init();

    protected void Bind<T>(Type type) where T : UnityEngine.Object
    {
        string[] getname = Enum.GetNames(type);
        UnityEngine.Object[] objects = new UnityEngine.Object[getname.Length];
        _objects.Add(typeof(T), objects);

        for(int i = 0; i < getname.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
            {
                objects[i] = Util.FindChild(gameObject, getname[i], true);
            }
            else
            {
                objects[i] = Util.FindChild<T>(gameObject, getname[i], true);
            }

            if (objects[i] == null)
                Debug.LogError($"Failed to Bind UI {objects[i].name}");
        }
    }

    protected T Get<T>(int index) where T : UnityEngine.Object
    {
        UnityEngine.Object[] objects = null;

        if (_objects.TryGetValue(typeof(T), out objects) == false)
            return null;

        return objects[index] as T;
    }

    protected GameObject GetObject(int idx) { return Get<GameObject>(idx); }
    protected UnityEngine.UI.Text GetText(int idx) { return Get<UnityEngine.UI.Text>(idx); }
    protected UnityEngine.UI.Button GetButton(int idx) { return Get<UnityEngine.UI.Button>(idx); }
    protected UnityEngine.UI.Image GetImage(int idx) { return Get<UnityEngine.UI.Image>(idx); }

    public static void BindEvent(GameObject go, Action<PointerEventData> action, Define.UIEvent evtType = Define.UIEvent.Tab)
    {
        UI_EventHandler evt = Util.GetOrAddComponent<UI_EventHandler>(go);

        switch (evtType)
        {
            case Define.UIEvent.Tab:
                evt.OnTabHandler -= action;
                evt.OnTabHandler += action;
                break;
            case Define.UIEvent.DragBegin:
                evt.OnBeginDragHandler -= action;
                evt.OnBeginDragHandler += action;
                break;
            case Define.UIEvent.Drag:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
            case Define.UIEvent.DragEnd:
                evt.OnDragHandler -= action;
                evt.OnDragHandler += action;
                break;
        }
    }
}
