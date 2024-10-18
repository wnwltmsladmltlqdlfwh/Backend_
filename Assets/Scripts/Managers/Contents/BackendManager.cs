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
            Debug.LogFormat("�ʱ�ȭ ���� : {0}", bro);
        }
        else
        {
            Debug.LogFormat("�ʱ�ȭ ���� : {0}", bro);
        }
    }

    #region ����������
    // ��Ʈ ID�� �ݺ� Ƚ��, ������ ���� ��� ������ �Լ��� �޾� ���� GameData���� ������ �߰��ϴ� �Լ�
    public void GameDataInsert(string selectedProbabilityField, int maxRepeatCount, Param param,
        Action<BackendReturnObject> onCompleted = null)
    {
        if (!Backend.IsLogin)
        {
            Debug.LogError("�α����� �Ǿ����� ����");
            return;
        }
        if(maxRepeatCount <= 0)
        {
            Debug.LogErrorFormat("{0} ���� ������ �߰����� ���߽��ϴ�.", selectedProbabilityField);
            return;
        }

        Backend.GameData.Insert(selectedProbabilityField, param, callback =>
        {
            switch (ErrorCheck(callback))
            {
                case BackendState.Failure:
                    Debug.LogError("���� ����");
                    break;
                case BackendState.Maintainance:
                    Debug.LogError("���� ���� ��");
                    break;
                case BackendState.Retry:
                    Debug.LogWarning("���� ��õ�");
                    GameDataInsert(selectedProbabilityField, maxRepeatCount - 1, param, onCompleted);
                    break;
                case BackendState.Success:
                    Debug.Log("���� �߰� ����");
                    onCompleted?.Invoke(callback);
                    break;
            }
        });
    }

    // ��Ʈ ID�� �ݺ� Ƚ��, ������ ���� ��� ������ �Լ��� �޾� ���� GameData���� ������ �����ϴ� �Լ�
    public void GameDataUpdate(string selectedProbabilityField, string inDate, int maxRepeatCount,
        Param param,Action<BackendReturnObject> onCompleted = null)
    {
        if(!Backend.IsLogin)
        {
            Debug.LogError("�α����� �Ǿ����� ����");
            return;
        }

        if(maxRepeatCount <= 0)
        {
            Debug.LogErrorFormat("{0} ���� ������ �������� ���߽��ϴ�.", selectedProbabilityField);
            return;
        }

        Backend.GameData.UpdateV2(selectedProbabilityField, inDate, Backend.UserInDate,param,callback =>
        {
            switch (ErrorCheck(callback))
            {
                case BackendState.Failure:
                    Debug.LogError("���� ����");
                    break;
                case BackendState.Maintainance:
                    Debug.LogError("���� ���� ��");
                    break;
                case BackendState.Retry:
                    Debug.LogWarning("���� ��õ�");
                    GameDataUpdate(selectedProbabilityField, inDate ,maxRepeatCount - 1, param, onCompleted);
                    break;
                case BackendState.Success:
                    Debug.Log("���� ���� ����");
                    onCompleted?.Invoke(callback);
                    break;
            }
        });
    }

    public BackendState ErrorCheck(BackendReturnObject bro)
    {
        if (bro.IsSuccess())
        {
            Debug.Log("��û ����!");
            return BackendState.Success;
        }
        else
        {
            if(bro.IsClientRequestFailError()) // Ŭ���̾�Ʈ�� �Ͻ����� ��Ʈ��ũ ����
            {
                Debug.LogError("�Ͻ����� ��Ʈ��ũ ����");
                return BackendState.Retry;
            }
            else if(bro.IsServerError()) // ���� �̻� �߻�
            {
                Debug.LogError("���� �̻� �߻�");
                return BackendState.Retry;
            }
            else if(bro.IsMaintenanceError()) // ���� ���°� '����'
            {
                // ���� �˾� + �α��� ȭ������ �̵�
                Debug.Log("���� ������");
                return BackendState.Maintainance;
            }
            else if (bro.IsTooManyRequestByLocalError()) // �ܱⰣ ���� ��û���� 403 Forbbiden �߻�
            {
                // �ܱⰣ�� ���� ��û�� ������ �߻��ϸ�, 5�а� �ڳ� �Լ� ��û ����
                Debug.LogError("�ܱⰣ�� ���� ��û�� ����, 5�а� ��� �Ұ�");
                return BackendState.Failure;
            }
            else if (bro.IsBadAccessTokenError())
            {
                bool isRefreshSuccess = RefreshTheBackendToken(3); // �ִ� 3�� �������� ����

                if(isRefreshSuccess)
                {
                    Debug.LogError("��ū �߱� ����");
                    return BackendState.Retry;
                }
                else
                {
                    Debug.LogError("��ū�� �߱� ���� ���߽��ϴ�.");
                    return BackendState.Failure;
                }
            }

            return BackendState.Retry;
        }
    }

    // �ڳ� ��ū ��߱� Ƚ��
    // maxRepeatCount : ���� ���� ���� �� ��õ��� Ƚ��
    public bool RefreshTheBackendToken(int maxRepeatCount)
    {
        if(maxRepeatCount <= 0)
        {
            Debug.Log("��ū �߱� ����");
            return false;
        }

        BackendReturnObject callback = Backend.BMember.RefreshTheBackendToken();

        if (callback.IsSuccess())
        {
            Debug.Log("��ū �߱� ����");
            return true;
        }
        else
        {
            if(callback.IsClientRequestFailError()) // Ŭ���̾�Ʈ�� �Ͻ����� ��Ʈ��ũ ����
            {
                return RefreshTheBackendToken(maxRepeatCount - 1);
            }
            else if(callback.IsServerError()) // ���� �̻� �߻�
            {
                return RefreshTheBackendToken(maxRepeatCount - 1);
            }
            else if(callback.IsMaintenanceError()) // ���� ���� ����
            {
                //���� �˾� + �α��� ȭ�� �̵�
                return false;
            }
            else if (callback.IsTooManyRequestError()) // �ܱⰣ ���� ��û���� 403 Forbbiden �߻�
            {
                // �ʹ� ���� ��û ������ ��
                return false;
            }
            else
            {
                // ��õ��� �ص� �׼�����ū ��߱��� �Ұ����� ���
                // Ŀ���� �α��� Ȥ�� �䵥���̼ų� �α��� ���� ���� �α����� �����ؾ� �մϴ�.
                // �ߺ� �α����� ��� 401 bad refreshToken ������ �Բ� �߻��� �� �ֽ��ϴ�.
                Debug.Log("���� ���ӿ� ������ �߻��߽��ϴ�. �α��� ȭ������ ���ư��ϴ�." + callback.ToString());
                return false;
            }
        }
    }
    #endregion
    #region ������ �ҷ�����
    public void GetChartData(string seletedProbabilityField, int maxRepeatCount = 10, Action<BackendReturnObject> onCompleted = null)
    {
        if (!Backend.IsLogin)
        {
            Debug.Log("�α����� �Ǿ����� ����");
            return;
        }

        if(maxRepeatCount <= 0)
        {
            Debug.LogError("���� ���� : �ݺ�Ƚ�� �ʰ�");
            return;
        }

        BackendReturnObject bro = Backend.Chart.GetOneChartAndSave(seletedProbabilityField);

        switch (ErrorCheck(bro))
        {
            case BackendState.Failure:
                Debug.Log("���� ����");
                break;
            case BackendState.Maintainance:
                Debug.Log("���� ���� ��");
                break;
            case BackendState.Retry:
                Debug.Log("��õ�");
                GetChartData(seletedProbabilityField, maxRepeatCount - 1, onCompleted);
                break;
            case BackendState.Success:
                Debug.Log("���� ����");
                onCompleted?.Invoke(bro);
                break;
        }
    }
    #endregion
}
