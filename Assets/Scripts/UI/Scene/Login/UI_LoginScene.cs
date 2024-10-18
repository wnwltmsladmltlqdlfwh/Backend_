using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using UnityEngine.EventSystems;

public class UI_LoginScene : UI_Scene
{
    enum InputFields
    {
        IdInputField,
        PwInputField,
    }
    enum Buttons
    {
        LoginButton,
        SignButton,
    }
    enum LoginText
    {
        LoginText,
    }

    protected override void Init()
    {
        base.Init();

        Bind<InputField>(typeof(InputFields));
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(LoginText));

        Get<InputField>((int)InputFields.IdInputField).onValueChanged.AddListener(IdValueChanged);
        Get<InputField>((int)InputFields.PwInputField).onValueChanged.AddListener(PwValueChanged);

        GetButton((int)Buttons.LoginButton).gameObject.BindEvent(Manager.Login.Login, Define.UIEvent.Tab);
        GetButton((int)Buttons.SignButton).gameObject.BindEvent(Manager.Login.SignUp, Define.UIEvent.Tab);
    }

    private void IdValueChanged(string value)
    {
        Manager.Login._id = value;
    }
    private void PwValueChanged(string value)
    {
        Manager.Login._pw = value;
    }
}
