using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SubItem_CharactorCard : UI_Base
{
    enum GameObjects
    {
        InfoText,
        TypeImage,
        RoleImage,
        PositionImage,
        Star,
    }

    protected override void Init()
    {
    }

    public void SetUIOptions()
    {
        Bind<GameObject>(typeof(GameObjects));

        Get<GameObject>((int)GameObjects.InfoText).GetComponent<Text>().text = "테스트 텍스트";
        Get<GameObject>((int)GameObjects.TypeImage).GetComponent<Image>().color = Color.yellow;
        Get<GameObject>((int)GameObjects.RoleImage).GetComponent<Image>().color = Color.red;
        Get<GameObject>((int)GameObjects.PositionImage).GetComponent<Image>().color = Color.gray;
        var a = Get<GameObject>((int)GameObjects.Star).GetComponentsInChildren<Image>();
        
        for(int i = 0; i < a.Length; i++)
        {
            if(i < 3)
            {
                a[i].color = Color.yellow;
            }
            else
            {
                a[i].color = Color.gray;
            }
        }
    }
}
