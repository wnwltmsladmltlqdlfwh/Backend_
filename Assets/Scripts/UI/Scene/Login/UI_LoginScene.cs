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

        GetButton((int)Buttons.LoginButton).gameObject.BindEvent(Login, Define.UIEvent.Tab);
        GetButton((int)Buttons.SignButton).gameObject.BindEvent(SignUp, Define.UIEvent.Tab);
    }

    private void Login(PointerEventData eventData)
    {
        string id = Get<InputField>((int)InputFields.IdInputField).text;
        string pw = Get<InputField>((int)InputFields.PwInputField).text;

        Debug.Log("로그인 요청");
        
        BackendReturnObject bro = Backend.BMember.CustomLogin(id, pw);

        if(bro.IsSuccess())
        {
            Debug.Log("로그인 성공" + bro);
            UserDataUpdate();
            //Manager.Scene.LoadScene(Define.Scenes.LobbyScene);
        }
        else
        {
            Debug.LogError("로그인 실패" + bro);
        }
    }

    private void SignUp(PointerEventData eventData)
    {
        string id = Get<InputField>((int)InputFields.IdInputField).text;
        string pw = Get<InputField>((int)InputFields.PwInputField).text;

        BackendReturnObject bro = Backend.BMember.CustomSignUp(id, pw);

        if (bro.IsSuccess())
        {
            Debug.Log("회원가입 성공" + bro);
        }
        else
        {
            Debug.Log("회원가입 실패" + bro);
        }
    }

    private BackendGameData _backendGameData = new BackendGameData();

    private void UserDataUpdate()
    {
        _backendGameData.SaveUserData(10);
    }
}
