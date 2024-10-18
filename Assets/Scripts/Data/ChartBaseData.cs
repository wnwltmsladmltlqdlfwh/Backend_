using BackEnd;
using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChartBaseData
{
    int _int;
    float _float;
    string _string;
    bool _bool;

    public void LoadData()
    {
        Manager.Backend.GetChartData("145029", 10, LoadChartByServer);

        if (Manager.Login._dataLoad == false)
        {
            Manager.Login._dataLoad = !Manager.Login._dataLoad;
            Debug.LogFormat("데이터 로드 완료 : {0}", Manager.Login._dataLoad);
        }
    }

    private void LoadChartByServer(BackendReturnObject bro)
    {
        JsonData json = bro.FlattenRows();

        for (int i = 0; i < json.Count; i++)
        {
            _int = int.Parse(json[i]["Int"].ToString());
            _float = float.Parse(json[i]["Float"].ToString());
            _string = json[i]["String"].ToString();
            _bool = bool.Parse(json[i]["Bool"].ToString());

            Debug.LogFormat("int {0}, float {1}, string {2}, bool {3}", _int, _float, _string, _bool);
        }
        Debug.Log("정보 받아오기 성공");
    }
}
