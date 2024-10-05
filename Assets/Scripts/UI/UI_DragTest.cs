using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_DragTest : UI_Base
{
    private RectTransform rectTransform;
    private Canvas canvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    protected override void Init()
    {
        // 이벤트 구독
        this.gameObject.BindEvent(OnDrag, Define.UIEvent.Drag);
    }


    // 드래그 중 호출될 메서드
    private void OnDrag(PointerEventData eventData)
    {
        // 오브젝트의 위치를 마우스/터치 위치에 맞게 이동
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
}
