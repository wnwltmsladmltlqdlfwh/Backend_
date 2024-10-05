using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[DefaultExecutionOrder(-1)]
public class Manager : Singleton<Manager>
{
    #region

    #endregion

    #region cores
    UI_Manager ui = new UI_Manager();
    Scene_Manager scene = new Scene_Manager();
    Input_Manager input = new Input_Manager();
    public static UI_Manager UI { get { return Instance.ui; } }
    public static Scene_Manager Scene { get {  return Instance.scene; } }
    public static Input_Manager Input { get {  return Instance.input; } }
    #endregion

    public int a;

    private void Awake()
    {
        InitManagers();
    }

    private void OnEnable()
    {
        Input.OnEnable();
    }

    private void Start()
    {
        StartManagers();
    }

    private void Update()
    {

    }

    private void InitManagers()
    {
        UI.InitUIManager();
        Scene.InitSceneManager();
        Input.InitInputManager();
    }

    private void StartManagers()
    {
        Input.StartInputManager();
    }

    private void OnDisable()
    {
        Input.OnDisable();
    }
}
