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
            Debug.LogError("로그인이 되어있지 않습니다.");
            return;
        }

        if(maxRepeatCount <= 0)
        {
            Debug.LogErrorFormat("{0} 차트의 정보를 받아오지 못했습니다.", selectedProbabilityField);
            return;
        }

        BackendReturnObject bro = Backend.GameData.Get(selectedProbabilityField, new Where());

        switch (Manager.Backend.ErrorCheck(bro))
        {
            case BackendState.Failure:
                Debug.LogError("초기화 실패");
                break;
            case BackendState.Maintainance:
                Debug.LogError("서버 점검 중");
                break;
            case BackendState.Retry:
                Debug.LogWarning("연결 재시도");
                SaveUserData(maxRepeatCount - 1);
                break;
            case BackendState.Success:
                //게임 정보DB에 자신의 정보가 존재할 경우
                if(bro.GetReturnValuetoJSON() != null)
                {
                    //DB에는 존재하나 그 속에 데이터량이 0줄일 경우
                    if (bro.GetReturnValuetoJSON()["rows"].Count <= 0)
                    {
                        //게임 정보DB에 자신의 데이터를 추가한다.
                        InsertUserData(selectedProbabilityField);
                    }
                    else
                    {
                        //게임 정보DB에 있는 자신의 데이터를 갱신한다.
                        UpdateUserData(selectedProbabilityField, bro.GetInDate());
                    }
                }
                //게임 정보DB에 존재하지 않을 경우
                else
                {
                    //게임 정보DB에 자신의 데이터를 추가한다.
                    InsertUserData(selectedProbabilityField);
                }

                if (Manager.Login._dataSave == false)
                {
                    Manager.Login._dataSave = !Manager.Login._dataSave;
                    Debug.LogFormat("데이터 저장 완료 : {0}", Manager.Login._dataSave);
                }
                break;
        }
    }

    // 게임 정보DB에 자신의 데이터를 추가하는 함수
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
        Debug.LogFormat("게임 정보 데이터 삽입을 요청합니다.");
        Manager.Backend.GameDataInsert(selectedProbabilityField, 10, param);
    }

    //게임 정보DB에 존재하는 자신의 데이터를 갱신하는 함수
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
        Debug.LogFormat("게임 정보 데이터 수정을 요청합니다.");
        Manager.Backend.GameDataUpdate(selectedProbabilityField, inDate, 10, param);
    }

    public void GameDataLoad()
    {
        Backend.GameData.GetMyData("UserData", new Where(), callback =>
        {
            // 게임 정보 불러오기 성공 시
            if (callback.IsSuccess())
            {

                Debug.Log($"게임 정보 데이터 불러오기에 성공했습니다. : {callback}");

                //JSON 데이터 파싱 성공
                try
                {
                    LitJson.JsonData gameDataJson = callback.FlattenRows();

                    if (gameDataJson.Count <= 0)
                    {
                        Debug.LogWarning("데이터가 존재하지 않습니다.");
                    }
                    else
                    {
                        //불러운 게임 정보의 고유값
                        gameDataRowInDate = gameDataJson[0]["inDate"].ToString();
                        //불러운 게임 정보를 userGameData 변수 저장
                        userGameData.level = int.Parse(gameDataJson[0]["level"].ToString());
                        userGameData.experience = int.Parse(gameDataJson[0]["experience"].ToString());
                        userGameData.gold = int.Parse(gameDataJson[0]["gold"].ToString());
                        userGameData.paidElif = int.Parse(gameDataJson[0]["paidElif"].ToString());
                        userGameData.freeElif = int.Parse(gameDataJson[0]["freeElif"].ToString());
                        userGameData.redCandy = int.Parse(gameDataJson[0]["redCandy"].ToString());

                        onGameDataLoadEvent?.Invoke();
                    }
                }
                //Json 데이터 파싱 실패
                catch (System.Exception e)
                {
                    //유저 정보를 초기값으로 설정
                    userGameData.Reset();
                    //try-catch 에러 출력
                    Debug.LogError(e);
                }
            }
            // 실패 시
            else
            {
                Debug.LogError($"게임 정보 데이터 불러오기에 실패했습니다. : {callback}");
            }
        });
    }
}
