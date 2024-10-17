using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager
{
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    public void LoadScene(Define.Scenes scene)
    {
        Manager.Clear();
        SceneManager.LoadScene(GetSceneType(scene));
    }

    string GetSceneType(Define.Scenes scene)
    {
        string name = System.Enum.GetName(typeof(Define.Scenes), scene);
        return name;
    }

    public bool CheckCurrentScene(Define.Scenes scenes)
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if(currentScene.name == GetSceneType(scenes))
            return true;

        return false;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
