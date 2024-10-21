using BackEnd;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;

public class LoginManager
{
    public string _id = string.Empty;
    public string _pw = string.Empty;

    public bool _dataSave = false;
    public bool _dataLoad = false;

    public void Login(PointerEventData eventData)
    {
        Debug.Log("로그인 요청");

        BackendReturnObject bro = Backend.BMember.CustomLogin(_id, _pw);

        if (bro.IsSuccess())
        {
            Debug.Log("로그인 성공" + bro);
            UserDataUpdate();
        }
        else
        {
            Debug.LogError("로그인 실패" + bro);
        }
    }

    public void SignUp(PointerEventData eventData)
    {
        BackendReturnObject bro = Backend.BMember.CustomSignUp(_id, _pw);

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
    private ChartBaseData _chartBaseData = new ChartBaseData();

    private void UserDataUpdate()
    {
        BackendGameData.Instance.SaveUserData(10);
        _chartBaseData.LoadData();

        while (_dataSave == false && _dataLoad == false)
        {
            return;
        }

        Manager.Scene.LoadScene(Define.Scenes.LobbyScene);
    }
}
