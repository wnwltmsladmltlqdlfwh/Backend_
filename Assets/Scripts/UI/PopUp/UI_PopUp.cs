using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PopUp : UI_Base
{
    protected override void Init()
    {
        Manager.UI.SetCanvas(gameObject, true);
    }

    public virtual void ClosePopUpUI()
    {
        Manager.UI.ClosePopUpUI(this);
    }
}
