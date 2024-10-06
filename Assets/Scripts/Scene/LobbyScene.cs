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
    Dictionary<int, UI_PopUp> _popups = new Dictionary<int, UI_PopUp>();

    protected override void Init()
    {
        base.Init();

        sceneType = Define.Scenes.Lobby;
        lobbyType = Define.LobbyUI.Main;

        _lobby = Manager.UI.ShowSceneUI<UI_LobbyScene>();

        Debug.Log("·Îºñ Init");
    }

    public override void Clear()
    {
        throw new System.NotImplementedException();
    }
}
