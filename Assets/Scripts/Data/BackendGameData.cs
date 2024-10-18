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
        Param param = new Param();
        Debug.LogFormat("게임 정보 데이터 삽입을 요청합니다.");
        Manager.Backend.GameDataInsert(selectedProbabilityField, 10, param);
    }

    //게임 정보DB에 존재하는 자신의 데이터를 갱신하는 함수
    public void UpdateUserData(string selectedProbabilityField, string inDate)
    {
        Param param = new Param();
        Debug.LogFormat("게임 정보 데이터 수정을 요청합니다.");
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
