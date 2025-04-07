using Defective.JSON;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public partial class User //Load
{
    public void Load()
    {
        JSONObject loadJson = new JSONObject(PlayerPrefs.GetString(SavedJsonPrefsKey, ""));

        loadJson.Clear();

        string valueString = "";

        loadJson.GetField(out valueString, UserPropertyCategory.UserMissionRefreshCount.ToString(), "0,0");
        string[] splitString = valueString.Split(',');
        int.TryParse(splitString[0], out _usedMissionFreeRefreshCount);
        int.TryParse(splitString[1], out _usedMissionPaidRefreshCount);

        loadJson.GetField(out valueString, UserPropertyCategory.DailyClearLog.ToString(), "12/27/2016");
        GameManager._dailyLogoutTime = System.DateTime.Parse(valueString);

        loadJson.GetField(out valueString, UserPropertyCategory.WeeklyClearLog.ToString(), "12/27/2016");
        GameManager._weeklyLogoutTime = System.DateTime.Parse(valueString);

        loadJson.GetField(out valueString, UserPropertyCategory.MonthlyClearLog.ToString(), "12/27/2016");
        GameManager._monthlyLogoutTime = System.DateTime.Parse(valueString);

        // 미션 로드
        LoadUserMissionJson(loadJson.GetField(UserPropertyCategory.UserMissions.ToString()));
    }

    public void LoadUserMissionJson(JSONObject json)
    {
        _userMissions = new Dictionary<ulong, UserMission>();

        if (json != null)
        {
            for (int i = 0; i < json.count; i++)
            {
                UserMission mission = new UserMission();
                JSONObject obj = json[i];
                string valueStr = "";
                obj.GetField(out valueStr, "id", "0");
                ulong.TryParse(valueStr, out mission.id);
                obj.GetField(out valueStr, "count", "0");
                double.TryParse(valueStr, out mission.count);
                obj.GetField(out valueStr, "isComplete", "0");
                bool.TryParse(valueStr, out mission.isComplete);
                obj.GetField(out valueStr, "isReward", "0");
                bool.TryParse(valueStr, out mission.isReward);

                if ((mission.id / 1000000) % 100 == 01)
                {
                    JSONObject missionObjectives = json[i].GetField("objectives");

                    if (missionObjectives != null)
                    {
                        MissionObjective[] missionObj = new MissionObjective[missionObjectives.count];

                        for (int j = 0; j < missionObjectives.count; j++)
                        {
                            JSONObject missionObjective = missionObjectives[j];

                            string valueStr2 = "";
                            missionObjective.GetField(out valueStr2, "condition1", MissionConditionKeys.Null);
                            missionObj[j].condition1 = valueStr2;
                            missionObjective.GetField(out valueStr2, "condition2", MissionConditionKeys.Null);
                            missionObj[j].condition2 = valueStr2;
                            missionObjective.GetField(out valueStr2, "objectiveCount", "0");
                            double.TryParse(valueStr2, out missionObj[j].objectiveCount);
                            missionObjective.GetField(out valueStr2, "groupId", "0");
                            int.TryParse(valueStr2, out missionObj[j].groupId);

                            //Debug.Log(mission.id + " " + missionObj[j].category + " " + missionObj[j].categoryValue);
                        }

                        mission.objectives = missionObj;
                    }
                }

                if (mission.id != 0 && !_userMissions.ContainsKey(mission.id))
                {
                    _userMissions.Add(mission.id, mission);
                }
            }
        }
    }
}
