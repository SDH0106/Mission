using Defective.JSON;
using System;
using System.Collections.Generic;
using UnityEngine;

public partial class User // Save
{
    public void Save()
    {
        JSONObject saveJson = new JSONObject(JSONObject.Type.Object);

        //  변수 형태 유저 데이터 저장

        for (int i = 0; i < UsedFreeRefreshCount.Length; ++i)
        {
            string refreshCounts = $"{UsedFreeRefreshCount[i]},{UsedPaidRefreshCount[i]}";
            saveJson.AddField(UserPropertyCategory.UserMissionRefreshCount.ToString() + i, refreshCounts);
        }

       
        //  컨테이너 형태 유저 데이터 저장
        saveJson.AddField(UserPropertyCategory.UserMissions.ToString(), SaveMissionArray());
        saveJson.AddField(UserPropertyCategory.DailyClearLog.ToString(), GameManager._dailyLogoutTime.ToString("d"));
        saveJson.AddField(UserPropertyCategory.WeeklyClearLog.ToString(), GameManager._weeklyLogoutTime.ToString("d"));
        saveJson.AddField(UserPropertyCategory.MonthlyClearLog.ToString(), GameManager._monthlyLogoutTime.ToString("d"));

        PlayerPrefs.SetString(SavedJsonPrefsKey, saveJson.Print());
        Debug.Log("Data Saved");
    }

    public JSONObject SaveMissionArray()
    {
        List<UserMission> missionlist = new List<UserMission>(_userMissions.Values);
        JSONObject missionJsonArr = new JSONObject(JSONObject.Type.Array);

        for (int i = 0; i < missionlist.Count; i++)
        {
            JSONObject obj = new JSONObject(JSONObject.Type.Object);
            obj.AddField("id", missionlist[i].id.ToString());
            obj.AddField("count", missionlist[i].count.ToString());
            obj.AddField("isComplete", missionlist[i].isComplete.ToString());
            obj.AddField("isReward", missionlist[i].isReward.ToString());

            if (missionlist[i].randomMissionElements != null)
            {
                obj.AddField("randomElementKey", string.Join(',', missionlist[i].randomMissionElements.Keys));
                obj.AddField("randomElementValue", string.Join(',', missionlist[i].randomMissionElements.Values));
            }

            missionJsonArr.Add(obj);
        }

        return missionJsonArr;
    }
}
