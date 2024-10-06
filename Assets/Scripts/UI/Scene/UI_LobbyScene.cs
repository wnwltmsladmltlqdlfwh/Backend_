using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LobbyScene : UI_Scene
{
    private enum Buttons
    {
        GachaButton,
        CharactorButton,
        CardButton,
        HouseButton,
        BattleButton,
    }

    protected override void Init()
    {
        base.Init();
        
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.GachaButton).gameObject.BindEvent(GachaTab, Define.UIEvent.Tab);
        GetButton((int)Buttons.CharactorButton).gameObject.BindEvent(CharactorTab, Define.UIEvent.Tab);
        GetButton((int)Buttons.CardButton).gameObject.BindEvent(CardTab, Define.UIEvent.Tab);
        GetButton((int)Buttons.HouseButton).gameObject.BindEvent(HouseTab, Define.UIEvent.Tab);
        GetButton((int)Buttons.BattleButton).gameObject.BindEvent(BattleTab, Define.UIEvent.Tab);
    }

    private void GachaTab(PointerEventData data)
    {
        string gachaPath = $"Lobby/{typeof(UI_Lobby_Gacha).Name}";
        Manager.UI.ShowPopUpUI<UI_Lobby_Gacha>(gachaPath);
        Debug.Log("For Test Tab : Gacha");
    }

    private void CharactorTab(PointerEventData data)
    {
        Debug.Log("For Test Tab : Charactor");
    }

    private void CardTab(PointerEventData data)
    {
        Debug.Log("For Test Tab : Card");
    }

    private void HouseTab(PointerEventData data)
    {
        Debug.Log("For Test Tab : House");
    }

    private void BattleTab(PointerEventData data)
    {
        Debug.Log("For Test Tab : Battle");
    }
}
