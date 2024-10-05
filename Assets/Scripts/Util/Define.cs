using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public enum MouseActions
    {
        Click,
        Press,
        PointerUp,
        PointerDown,
    }

    public enum TouchActions
    {
        Tap,
        Press,
        TouchUp,
        TouchDown,
    }

    public enum UIEvent
    {
        Tab,
        DragBegin,
        Drag,
        DragEnd,
    }
}
