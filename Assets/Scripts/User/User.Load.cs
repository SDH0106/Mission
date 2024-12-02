using Defective.JSON;
using System;
using System.Collections.Generic;
using UnityEngine;

public partial class User //Load
{
    public void Load()
    {
        JSONObject loadJson = new JSONObject(PlayerPrefs.GetString(SavedJsonPrefsKey, ""));

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

                string keyStr = "";
                obj.GetField(out keyStr, "randomElementKey", null);
                obj.GetField(out valueStr, "randomElementValue", null);
                if (!string.IsNullOrEmpty(keyStr))
                {
                    string[] randomElementKey = keyStr.Split(",");
                    string[] randomElementValue = valueStr.Split(",");

                    Dictionary<RandomMission.MissionElementTypes, string> tempDic = new Dictionary<RandomMission.MissionElementTypes, string>();

                    for (int j = 0; j < randomElementKey.Length; ++j)
                    {
                        RandomMission.MissionElementTypes type = (RandomMission.MissionElementTypes)Enum.Parse(typeof(RandomMission.MissionElementTypes), randomElementKey[j]);
                        tempDic.Add(type, randomElementValue[j]);
                    }

                    mission.randomMissionElements = tempDic;
                }

                if (mission.id != 0 && _userMissions.ContainsKey(mission.id) == false)
                {
                    _userMissions.Add(mission.id, mission);
                }
            }
        }
    }
}
