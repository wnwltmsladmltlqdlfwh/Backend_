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
        
    }

    Canvas canvas = null;

    protected override void Init()
    {
        canvas = GetComponentInParent<Canvas>();

    }

    private void OnTestButtonTab(PointerEventData data)
    {

    }
}
