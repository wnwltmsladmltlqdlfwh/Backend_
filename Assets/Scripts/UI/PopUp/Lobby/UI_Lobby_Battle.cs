using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Lobby_Battle : UI_PopUp
{
    enum MenuButtons
    {
        PopupClose,
    }

    enum AboutCharactor
    {
        PickCharactors,
        CharListView,
        CharListViewport,
        CharListContent,
        SortingButton,
        PowerButton,
        FilterButton,
        StartButton,
    }

    Button button;

    protected override void Init()
    {
        base.Init();

        Bind<RectTransform>(typeof(AboutCharactor));
        Get<RectTransform>((int)AboutCharactor.SortingButton).gameObject.BindEvent(SortingButtonAct);
        Get<RectTransform>((int)AboutCharactor.PowerButton).gameObject.BindEvent(PowerButtonsAct);
        Get<RectTransform>((int)AboutCharactor.FilterButton).gameObject.BindEvent(FilterButtonAct);
        Get<RectTransform>((int)AboutCharactor.StartButton).gameObject.BindEvent(StartButtonAct);

        Bind<Button>(typeof(MenuButtons));
        GetButton((int)MenuButtons.PopupClose).onClick.AddListener(ClosePopUpUI);
    }

    #region for CharactorWindow
    void SortingButtonAct(PointerEventData data)
    {
        string downArrow = "��";
        string upArrow = "��";

        if (Get<RectTransform>((int)AboutCharactor.SortingButton).GetComponentInChildren<Text>().text == downArrow)
            Get<RectTransform>((int)AboutCharactor.SortingButton).GetComponentInChildren<Text>().text = upArrow;
        else
            Get<RectTransform>((int)AboutCharactor.SortingButton).GetComponentInChildren<Text>().text = downArrow;

        Debug.Log($"���Ĺ�ư ��ġ {Get<RectTransform>((int)AboutCharactor.SortingButton).GetComponentInChildren<Text>().text}");
    }

    void PowerButtonsAct(PointerEventData data)
    {
        Debug.Log("������ ��ư ��ġ");
    }

    void FilterButtonAct(PointerEventData data)
    {
        Debug.Log("���� ��ư ��ġ");
    }

    void StartButtonAct(PointerEventData data)
    {
        Manager.Scene.LoadScene(Define.Scenes.BattleScene);
        Debug.Log("��� ��ư ��ġ");
    }
    #endregion

    public override void ClosePopUpUI()
    {
        base.ClosePopUpUI();
    }
}
