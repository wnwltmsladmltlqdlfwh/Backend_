using System.Collections;
using System.Collections.Generic;
using Unity.XR.OpenVR;
using UnityEngine;

public class LoginScene : BaseScene
{
    UI_LoginScene _login = null;

    protected override void Init()
    {
        base.Init();

        sceneType = Define.Scenes.LoginScene;

        _login = Manager.UI.ShowSceneUI<UI_LoginScene>();
    }

    public override void Clear()
    {

    }
}
