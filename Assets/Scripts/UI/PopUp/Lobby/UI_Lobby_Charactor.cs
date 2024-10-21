using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Lobby_Charactor : UI_PopUp
{
    enum GameObjects
    {
        GridPanel,
        PopUpClose,
    }

    protected override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));

        GameObject gridPanel = Get<GameObject>((int)GameObjects.GridPanel);
        Get<GameObject>((int)GameObjects.PopUpClose).GetComponent<Button>().onClick.AddListener(ClosePopUpUI);

        foreach(Transform child in gridPanel.transform)
        {
            Manager.Resource.Destroy(child.gameObject);
        }

        // ����Ʈ�� �÷��̾� ���� ����Ʈ, �̺��� ����Ʈ�� ����� SetUIOptions�� ������ �־��ָ鼭 ����
        for(int i = 0; i < 20; i++)
        {
            GameObject item = Manager.UI.MakeSubItem<UI_SubItem_CharactorCard>(gridPanel.transform).gameObject;

            UI_SubItem_CharactorCard charCard = item.GetComponent<UI_SubItem_CharactorCard>();
            charCard.SetUIOptions();
        }
    }

    public override void ClosePopUpUI()
    {
        base.ClosePopUpUI();
    }
}
