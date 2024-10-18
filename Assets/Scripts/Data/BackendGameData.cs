using BackEnd;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UserData
{
    public int _int = 1;
    public float _float = 3.5f;
    public string _str = string.Empty;
    public Dictionary<string, int> _dic = new Dictionary<string, int>();
    public List<string> _list = new List<string>();
}

public class BackendGameData
{
    public UserData _userData = new UserData();

    public void SaveUserData(int maxRepeatCount)
    {
        string selectedProbabilityField = "UserData";

        if (!Backend.IsLogin)
        {
            Debug.LogError("�α����� �Ǿ����� �ʽ��ϴ�.");
            return;
        }

        if(maxRepeatCount <= 0)
        {
            Debug.LogErrorFormat("{0} ��Ʈ�� ������ �޾ƿ��� ���߽��ϴ�.", selectedProbabilityField);
            return;
        }

        BackendReturnObject bro = Backend.GameData.Get(selectedProbabilityField, new Where());

        switch (Manager.Backend.ErrorCheck(bro))
        {
            case BackendState.Failure:
                Debug.LogError("�ʱ�ȭ ����");
                break;
            case BackendState.Maintainance:
                Debug.LogError("���� ���� ��");
                break;
            case BackendState.Retry:
                Debug.LogWarning("���� ��õ�");
                SaveUserData(maxRepeatCount - 1);
                break;
            case BackendState.Success:
                //���� ����DB�� �ڽ��� ������ ������ ���
                if(bro.GetReturnValuetoJSON() != null)
                {
                    //DB���� �����ϳ� �� �ӿ� �����ͷ��� 0���� ���
                    if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
                    {
                        //���� ����DB�� �ڽ��� �����͸� �߰��Ѵ�.
                        InsertUserData(selectedProbabilityField);
                    }
                    else
                    {
                        //���� ����DB�� �ִ� �ڽ��� �����͸� �����Ѵ�.
                        UpdateUserData(selectedProbabilityField, bro.GetInDate());
                    }
                }
                //���� ����DB�� �������� ���� ���
                else
                {
                    //���� ����DB�� �ڽ��� �����͸� �߰��Ѵ�.
                    InsertUserData(selectedProbabilityField);
                }

                if (Manager.Login._dataSave == false)
                {
                    Manager.Login._dataSave = !Manager.Login._dataSave;
                    Debug.LogFormat("������ ���� �Ϸ� : {0}", Manager.Login._dataSave);
                }
                break;
        }
    }

    // ���� ����DB�� �ڽ��� �����͸� �߰��ϴ� �Լ�
    public void InsertUserData(string selectedProbabilityField)
    {
        Param param = new Param();
        Debug.LogFormat("���� ���� ������ ������ ��û�մϴ�.");
        Manager.Backend.GameDataInsert(selectedProbabilityField, 10, param);
    }

    //���� ����DB�� �����ϴ� �ڽ��� �����͸� �����ϴ� �Լ�
    public void UpdateUserData(string selectedProbabilityField, string inDate)
    {
        Param param = new Param();
        Debug.LogFormat("���� ���� ������ ������ ��û�մϴ�.");
        Manager.Backend.GameDataUpdate(selectedProbabilityField, inDate, 10, param);
    }

    public Param GetUserDataParam()
    {
        Param param = new Param();
        param.Add("_int", _userData._int);
        param.Add("_float", _userData._float);
        param.Add("_str", _userData._str);
        param.Add("_dic", _userData._dic);
        param.Add("_list", _userData._list);
        return param;
    }
}
