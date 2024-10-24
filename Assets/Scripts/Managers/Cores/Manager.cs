using UnityEngine;

[DefaultExecutionOrder(-1)]
public class Manager : Singleton<Manager>
{
    #region Contents
    BackendManager backend = new BackendManager();
    LoginManager login = new LoginManager();
    public static BackendManager Backend { get { return Instance.backend; } }
    public static LoginManager Login { get { return Instance.login; } }
    #endregion

    #region Cores
    UI_Manager ui = new UI_Manager();
    Scene_Manager scene = new Scene_Manager();
    Input_Manager input = new Input_Manager();
    Pool_Manager pool = new Pool_Manager();
    Resource_Manager resource = new Resource_Manager();
    public static UI_Manager UI { get { return Instance.ui; } }
    public static Scene_Manager Scene { get {  return Instance.scene; } }
    public static Input_Manager Input { get {  return Instance.input; } }
    public static Pool_Manager Pool { get { return Instance.pool; } }
    public static Resource_Manager Resource { get { return Instance.resource; } }
    #endregion

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

    private static void InitManagers()
    {
        Backend.Init();
        Input.Init();
        Pool.Init();
    }

    private void StartManagers()
    {
        Input.StartInputManager();
    }

    public static void Clear()
    {
        Scene.Clear();
        Pool.Clear();
        UI.Clear();
    }

    private void OnDisable()
    {
        Input.OnDisable();
    }
}
