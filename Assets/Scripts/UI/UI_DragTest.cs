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
        // �̺�Ʈ ����
        this.gameObject.BindEvent(OnDrag, Define.UIEvent.Drag);
    }


    // �巡�� �� ȣ��� �޼���
    private void OnDrag(PointerEventData eventData)
    {
        // ������Ʈ�� ��ġ�� ���콺/��ġ ��ġ�� �°� �̵�
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
}
