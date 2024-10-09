using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LobbyScene_Wallet : UI_Scene
{
    enum Buttons
    {
        MenuButton,
        MailButton,
        NoticeButton,
        CashButton,
        GoldButton,
        CandyButton,
    }

    protected override void Init()
    {
        base.Init();
        Manager.UI.SetCanvas(this.gameObject, true, 100);

        Debug.Log($"Sort Wallet : {GetComponent<Canvas>().sortingOrder}");
        Bind<Button>(typeof(Buttons));
    }


}
