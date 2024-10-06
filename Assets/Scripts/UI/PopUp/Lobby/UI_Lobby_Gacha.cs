using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Lobby_Gacha : UI_PopUp
{
    enum Buttons
    {
        OneTimeGacha,
        TenTimesGacha,
        PopUpClose,
    }

    protected override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.OneTimeGacha).gameObject.BindEvent(OneGacha);
        GetButton((int)Buttons.TenTimesGacha).gameObject.BindEvent(TenGacha);
        GetButton((int)Buttons.PopUpClose).onClick.AddListener(ClosePopUpUI);
    }

    public override void ClosePopUpUI()
    {
        base.ClosePopUpUI();
    }

    private void OneGacha(PointerEventData data)
    {
        Debug.Log("°¡Ã­ 1È¸ ½ÇÇà");
    }

    private void TenGacha(PointerEventData data)
    {
        Debug.Log("°¡Ã­ 10È¸ ½ÇÇà");
    }
}
