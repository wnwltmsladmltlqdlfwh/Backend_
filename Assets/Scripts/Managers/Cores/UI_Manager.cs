using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class UI_Manager
{
    int _order = 10; // 최근까지 사용한 sort order 저장

    Stack<UI_PopUp> _popupStack = new Stack<UI_PopUp>();
    UI_Scene _sceneUI = null;

    public GameObject Root
    {
        get
        {
            GameObject root = GameObject.Find("@UI_Root");
            if (root == null)
                root = new GameObject { name = "@UI_Root" };

            return root;
        }
    }

    public void SetCanvas(GameObject go, bool sort = true, int sortingorder = 0)
    {
        Canvas canvas = Util.GetOrAddComponent<Canvas>(go);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.overrideSorting = true;

        if(sort)
        {
            if (sortingorder != 0)
            {
                canvas.sortingOrder = sortingorder;
            }
            else
            {
                canvas.sortingOrder = _order;
                _order++;
            }
        }
        else
        {
            canvas.sortingOrder = 0;
        }
    }

    public T MakeWorldSpaceUI<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if(string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Manager.Resource.Instantiate($"UI/WorldSpace/{name}");

        if (parent != null)
            go.transform.SetParent(parent);

        Canvas canvas = go.GetOrAddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;

        return Util.GetOrAddComponent<T>(go);
    }

    public T MakeSubItem<T>(Transform parent = null, string name = null) where T : UI_Base
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Manager.Resource.Instantiate($"UI/SubItem/{name}");

        if (parent != null)
            go.transform.SetParent(parent);

        return Util.GetOrAddComponent<T>(go);
    }

    public T ShowSceneUI<T>(string name = null) where T : UI_Scene
    {
        if (string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Manager.Resource.Instantiate($"UI/Scene/{name}");
        T sceneUI = Util.GetOrAddComponent<T>(go);
        _sceneUI = sceneUI;

        go.transform.SetParent(Root.transform);

        return sceneUI;
    }

    public T ShowPopUpUI<T>(string name = null) where T : UI_PopUp
    {
        if(string.IsNullOrEmpty(name))
            name = typeof(T).Name;

        GameObject go = Manager.Resource.Instantiate($"UI/PopUp/{name}");
        T popup = Util.GetOrAddComponent<T>(go);
        _popupStack.Push(popup);

        go.transform.SetParent(Root.transform);
        
        return popup;
    }

    public void ClosePopUpUI()
    {
        if (_popupStack.Count == 0)
            return;

        UI_PopUp popUp = _popupStack.Pop();
        Manager.Resource.Destroy(popUp.gameObject);
        popUp = null;

        _order--;
    }

    public void ClosePopUpUI(UI_PopUp popUp)
    {
        if(_popupStack.Count == 0)
            return;

        if(_popupStack.Peek() != popUp)
        {
            Debug.Log("Close Popup Failed");
            return;
        }

        ClosePopUpUI();
    }

    public void CloseAllPopUp()
    {
        while (_popupStack.Count > 0)
            ClosePopUpUI();
    }

    public void Clear()
    {
        CloseAllPopUp();
        _sceneUI = null;
    }
}
