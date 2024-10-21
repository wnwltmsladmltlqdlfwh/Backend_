using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_LobbyScene_Wallet : UI_Scene
{
    enum Buttons
    {
        MenuButton,
        MailButton,
        NoticeButton,
        ElifButton,
        GoldButton,
        CandyButton,
    }

    enum Texts
    {
        ElifText,
        GoldText,
        CandyText,
    }

    protected override void Init()
    {
        base.Init();
        BackendGameData.Instance.onGameDataLoadEvent.AddListener(UpdateGameData);

        Manager.UI.SetCanvas(this.gameObject, true, 100);

        Debug.Log($"Sort Wallet : {GetComponent<Canvas>().sortingOrder}");

        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.MenuButton).gameObject.BindEvent(TestCostsUpdate);
    }

    public void UpdateGameData()
    {
        Bind<Text>(typeof(Texts));
        Text gold = Get<Text>((int)Texts.GoldText);
        Text elif = Get<Text>((int)Texts.ElifText);
        Text candy = Get<Text>((int)Texts.CandyText);

        gold.text = $"{BackendGameData.Instance.UserGameData.gold}";
        elif.text = $"{BackendGameData.Instance.UserGameData.paidElif + BackendGameData.Instance.UserGameData.freeElif}";
        candy.text = $"{BackendGameData.Instance.UserGameData.redCandy}";
    }

    private void TestCostsUpdate(PointerEventData eventData)
    {
        BackendGameData.Instance.UserGameData.paidElif += 1;
        BackendGameData.Instance.SaveUserData(10);
    }
}
