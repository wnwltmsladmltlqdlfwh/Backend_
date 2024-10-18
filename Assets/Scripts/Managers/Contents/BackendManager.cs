using BackEnd;
using System;
using UnityEngine;

public enum BackendState
{
    Failure,
    Maintainance,
    Retry,
    Success,
}

public class BackendManager
{
    public void Init()
    {
        BackendReturnObject bro = Backend.Initialize();

        if(bro.IsSuccess())
        {
            Debug.LogFormat("초기화 성공 : {0}", bro);
        }
        else
        {
            Debug.LogFormat("초기화 실패 : {0}", bro);
        }
    }

    #region 데이터저장
    // 차트 ID와 반복 횟수, 연결이 됐을 경우 실행할 함수를 받아 서버 GameData란에 정보를 추가하는 함수
    public void GameDataInsert(string selectedProbabilityField, int maxRepeatCount, Param param,
        Action<BackendReturnObject> onCompleted = null)
    {
        if (!Backend.IsLogin)
        {
            Debug.LogError("로그인이 되어있지 않음");
            return;
        }
        if(maxRepeatCount <= 0)
        {
            Debug.LogErrorFormat("{0} 게임 정보를 추가하지 못했습니다.", selectedProbabilityField);
            return;
        }

        Backend.GameData.Insert(selectedProbabilityField, param, callback =>
        {
            switch (ErrorCheck(callback))
            {
                case BackendState.Failure:
                    Debug.LogError("연결 실패");
                    break;
                case BackendState.Maintainance:
                    Debug.LogError("서버 점검 중");
                    break;
                case BackendState.Retry:
                    Debug.LogWarning("연결 재시도");
                    GameDataInsert(selectedProbabilityField, maxRepeatCount - 1, param, onCompleted);
                    break;
                case BackendState.Success:
                    Debug.Log("정보 추가 성공");
                    onCompleted?.Invoke(callback);
                    break;
            }
        });
    }

    // 차트 ID와 반복 횟수, 연결이 됐을 경우 실행할 함수를 받아 서버 GameData란에 정보를 수정하는 함수
    public void GameDataUpdate(string selectedProbabilityField, string inDate, int maxRepeatCount,
        Param param,Action<BackendReturnObject> onCompleted = null)
    {
        if(!Backend.IsLogin)
        {
            Debug.LogError("로그인이 되어있지 않음");
            return;
        }

        if(maxRepeatCount <= 0)
        {
            Debug.LogErrorFormat("{0} 게임 정보를 수정하지 못했습니다.", selectedProbabilityField);
            return;
        }

        Backend.GameData.UpdateV2(selectedProbabilityField, inDate, Backend.UserInDate,param,callback =>
        {
            switch (ErrorCheck(callback))
            {
                case BackendState.Failure:
                    Debug.LogError("연결 실패");
                    break;
                case BackendState.Maintainance:
                    Debug.LogError("서버 점검 중");
                    break;
                case BackendState.Retry:
                    Debug.LogWarning("연결 재시도");
                    GameDataUpdate(selectedProbabilityField, inDate ,maxRepeatCount - 1, param, onCompleted);
                    break;
                case BackendState.Success:
                    Debug.Log("정보 수정 성공");
                    onCompleted?.Invoke(callback);
                    break;
            }
        });
    }

    public BackendState ErrorCheck(BackendReturnObject bro)
    {
        if (bro.IsSuccess())
        {
            Debug.Log("요청 성공!");
            return BackendState.Success;
        }
        else
        {
            if(bro.IsClientRequestFailError()) // 클라이언트의 일시적인 네트워크 끊김
            {
                Debug.LogError("일시적인 네트워크 끊김");
                return BackendState.Retry;
            }
            else if(bro.IsServerError()) // 서버 이상 발생
            {
                Debug.LogError("서버 이상 발생");
                return BackendState.Retry;
            }
            else if(bro.IsMaintenanceError()) // 서버 상태가 '점검'
            {
                // 점검 팝업 + 로그인 화면으로 이동
                Debug.Log("게임 점검중");
                return BackendState.Maintainance;
            }
            else if (bro.IsTooManyRequestByLocalError()) // 단기간 많은 요청으로 403 Forbbiden 발생
            {
                // 단기간에 많은 요청을 보내면 발생하며, 5분간 뒤끝 함수 요청 중지
                Debug.LogError("단기간에 많은 요청을 보냄, 5분간 사용 불가");
                return BackendState.Failure;
            }
            else if (bro.IsBadAccessTokenError())
            {
                bool isRefreshSuccess = RefreshTheBackendToken(3); // 최대 3번 리프레시 실행

                if(isRefreshSuccess)
                {
                    Debug.LogError("토큰 발급 성공");
                    return BackendState.Retry;
                }
                else
                {
                    Debug.LogError("토큰을 발급 받지 못했습니다.");
                    return BackendState.Failure;
                }
            }

            return BackendState.Retry;
        }
    }

    // 뒤끝 토큰 재발급 횟수
    // maxRepeatCount : 서버 연결 실패 시 재시도할 횟수
    public bool RefreshTheBackendToken(int maxRepeatCount)
    {
        if(maxRepeatCount <= 0)
        {
            Debug.Log("토큰 발급 실패");
            return false;
        }

        BackendReturnObject callback = Backend.BMember.RefreshTheBackendToken();

        if (callback.IsSuccess())
        {
            Debug.Log("토큰 발급 성공");
            return true;
        }
        else
        {
            if(callback.IsClientRequestFailError()) // 클라이언트의 일시적인 네트워크 끊김
            {
                return RefreshTheBackendToken(maxRepeatCount - 1);
            }
            else if(callback.IsServerError()) // 서버 이상 발생
            {
                return RefreshTheBackendToken(maxRepeatCount - 1);
            }
            else if(callback.IsMaintenanceError()) // 서버 상태 점검
            {
                //점검 팝업 + 로그인 화면 이동
                return false;
            }
            else if (callback.IsTooManyRequestError()) // 단기간 많은 요청으로 403 Forbbiden 발생
            {
                // 너무 많은 요청 보내는 중
                return false;
            }
            else
            {
                // 재시도를 해도 액세스토큰 재발급이 불가능한 경우
                // 커스텀 로그인 혹은 페데레이셔녀 로그인 통해 수동 로그인을 진행해야 합니다.
                // 중복 로그인일 경우 401 bad refreshToken 에러와 함께 발생할 수 있습니다.
                Debug.Log("게임 접속에 문제가 발생했습니다. 로그인 화면으로 돌아갑니다." + callback.ToString());
                return false;
            }
        }
    }
    #endregion
    #region 데이터 불러오기
    public void GetChartData(string seletedProbabilityField, int maxRepeatCount = 10, Action<BackendReturnObject> onCompleted = null)
    {
        if (!Backend.IsLogin)
        {
            Debug.Log("로그인이 되어있지 않음");
            return;
        }

        if(maxRepeatCount <= 0)
        {
            Debug.LogError("연결 실패 : 반복횟수 초과");
            return;
        }

        BackendReturnObject bro = Backend.Chart.GetOneChartAndSave(seletedProbabilityField);

        switch (ErrorCheck(bro))
        {
            case BackendState.Failure:
                Debug.Log("연결 실패");
                break;
            case BackendState.Maintainance:
                Debug.Log("서버 점검 중");
                break;
            case BackendState.Retry:
                Debug.Log("재시도");
                GetChartData(seletedProbabilityField, maxRepeatCount - 1, onCompleted);
                break;
            case BackendState.Success:
                Debug.Log("연결 성공");
                onCompleted?.Invoke(bro);
                break;
        }
    }
    #endregion
}
