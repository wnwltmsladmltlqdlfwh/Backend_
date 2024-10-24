using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : BaseScene
{
    public Define.LobbyUI lobbyType
    {
        get; set;
    }

    UI_LobbyScene _lobby = null;
    UI_LobbyScene_Wallet _wallet = null;
    Dictionary<int, UI_PopUp> _popups = new Dictionary<int, UI_PopUp>();

    protected override void Init()
    {
        base.Init();

        sceneType = Define.Scenes.LobbyScene;
        lobbyType = Define.LobbyUI.Main;

        _lobby = Manager.UI.ShowSceneUI<UI_LobbyScene>();
        _wallet = Manager.UI.ShowSceneUI<UI_LobbyScene_Wallet>();

        BackendGameData.Instance.GameDataLoad();

        Debug.Log("�κ� Init");
    }

    public override void Clear()
    {

    }
}
