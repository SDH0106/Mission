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
        string refreshCounts = $"{_usedMissionFreeRefreshCount},{_usedMissionPaidRefreshCount}";
        saveJson.AddField(UserPropertyCategory.UserMissionRefreshCount.ToString(), refreshCounts);

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

            if ((missionlist[i].id / 1000000) % 100 == 01)
            {
                MissionObjective[] missionObjectives = missionlist[i].objectives;
                JSONObject missionObjectJsonArr = new JSONObject(JSONObject.Type.Array);

                if (missionObjectives != null)
                {
                    for (int j = 0; j < missionObjectives.Length; ++j)
                    {
                        JSONObject obj2 = new JSONObject(JSONObject.Type.Object);

                        //Debug.Log(missionlist[i].id + " " + missionObjectives[j].category + " " + missionObjectives[j].categoryValue);
                        obj2.AddField("condition1", missionObjectives[j].condition1.ToString());
                        obj2.AddField("condition2", missionObjectives[j].condition2.ToString());
                        obj2.AddField("objectiveCount", missionObjectives[j].objectiveCount.ToString());
                        obj2.AddField("groupId", missionObjectives[j].groupId.ToString());

                        missionObjectJsonArr.Add(obj2);
                    }

                    obj.AddField("objectives", missionObjectJsonArr);
                }
            }

            missionJsonArr.Add(obj);
        }

        return missionJsonArr;
    }
}
