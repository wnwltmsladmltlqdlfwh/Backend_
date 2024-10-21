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
        Debug.Log("�α��� ��û");

        BackendReturnObject bro = Backend.BMember.CustomLogin(_id, _pw);

        if (bro.IsSuccess())
        {
            Debug.Log("�α��� ����" + bro);
            UserDataUpdate();
        }
        else
        {
            Debug.LogError("�α��� ����" + bro);
        }
    }

    public void SignUp(PointerEventData eventData)
    {
        BackendReturnObject bro = Backend.BMember.CustomSignUp(_id, _pw);

        if (bro.IsSuccess())
        {
            Debug.Log("ȸ������ ����" + bro);
        }
        else
        {
            Debug.Log("ȸ������ ����" + bro);
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
