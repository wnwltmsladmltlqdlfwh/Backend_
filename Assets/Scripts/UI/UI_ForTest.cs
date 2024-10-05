using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI_ForTest : UI_Base
{
    enum Buttons
    {
        TestButton,
    }

    Canvas canvas = null;

    protected override void Init()
    {
        canvas = GetComponentInParent<Canvas>();

        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.TestButton).gameObject.BindEvent(OnTestButtonTab);
    }

    private void OnTestButtonTab(PointerEventData data)
    {
        var a = GetButton((int)Buttons.TestButton).gameObject.GetComponent<Image>();

        a.color = Random.ColorHSV();

        Debug.Log("¹öÆ° ÅÇ");
    }
}
