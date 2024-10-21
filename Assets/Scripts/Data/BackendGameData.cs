using BackEnd;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UserData
{
    public int level;
    public int experience;
    public int gold;
    public int paidElif;
    public int freeElif;
    public int redCandy;

    public void Reset()
    {
        level = 1;
        experience = 0;
        gold = 0;
        paidElif = 0;
        freeElif = 0;
        redCandy = 0;
    }
}

public class BackendGameData
{
    [System.Serializable]
    public class GameDataLoadEvent : UnityEngine.Events.UnityEvent { }
    public GameDataLoadEvent onGameDataLoadEvent = new GameDataLoadEvent();

    private static BackendGameData instance = null;
    public static BackendGameData Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new BackendGameData();
            }
            return instance;
        }
    }

    private UserData userGameData = new UserData();
    public UserData UserGameData => userGameData;

    private string gameDataRowInDate = string.Empty;

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
        userGameData.Reset();

        Param param = new Param()
        {
            { "level", userGameData.level},
            { "experience", userGameData.experience },
            { "gold", userGameData.gold},
            { "paidElif", userGameData.paidElif },
            { "freeElif", userGameData.freeElif },
            {"redCandy", userGameData.redCandy},
        };
        Debug.LogFormat("���� ���� ������ ������ ��û�մϴ�.");
        Manager.Backend.GameDataInsert(selectedProbabilityField, 10, param);
    }

    //���� ����DB�� �����ϴ� �ڽ��� �����͸� �����ϴ� �Լ�
    public void UpdateUserData(string selectedProbabilityField, string inDate)
    {
        Param param = new Param()
        {
            { "level", userGameData.level},
            { "experience", userGameData.experience },
            { "gold", userGameData.gold},
            { "paidElif", userGameData.paidElif },
            { "freeElif", userGameData.freeElif },
            {"redCandy", userGameData.redCandy},
        };
        Debug.LogFormat("���� ���� ������ ������ ��û�մϴ�.");
        Manager.Backend.GameDataUpdate(selectedProbabilityField, inDate, 10, param);
    }

    public void GameDataLoad()
    {
        Backend.GameData.GetMyData("UserData", new Where(), callback =>
        {
            // ���� ���� �ҷ����� ���� ��
            if (callback.IsSuccess())
            {

                Debug.Log($"���� ���� ������ �ҷ����⿡ �����߽��ϴ�. : {callback}");

                //JSON ������ �Ľ� ����
                try
                {
                    LitJson.JsonData gameDataJson = callback.FlattenRows();

                    if (gameDataJson.Count <= 0)
                    {
                        Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
                    }
                    else
                    {
                        //�ҷ��� ���� ������ ������
                        gameDataRowInDate = gameDataJson[0]["inDate"].ToString();
                        //�ҷ��� ���� ������ userGameData ���� ����
                        userGameData.level = int.Parse(gameDataJson[0]["level"].ToString());
                        userGameData.experience = int.Parse(gameDataJson[0]["experience"].ToString());
                        userGameData.gold = int.Parse(gameDataJson[0]["gold"].ToString());
                        userGameData.paidElif = int.Parse(gameDataJson[0]["paidElif"].ToString());
                        userGameData.freeElif = int.Parse(gameDataJson[0]["freeElif"].ToString());
                        userGameData.redCandy = int.Parse(gameDataJson[0]["redCandy"].ToString());

                        onGameDataLoadEvent?.Invoke();
                    }
                }
                //Json ������ �Ľ� ����
                catch (System.Exception e)
                {
                    //���� ������ �ʱⰪ���� ����
                    userGameData.Reset();
                    //try-catch ���� ���
                    Debug.LogError(e);
                }
            }
            // ���� ��
            else
            {
                Debug.LogError($"���� ���� ������ �ҷ����⿡ �����߽��ϴ�. : {callback}");
            }
        });
    }
}
