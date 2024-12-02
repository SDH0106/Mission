using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Utility
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="missionExplainElements">미션 설명문에 들어갈 인자를 담는 List. Format의 인자에 값을 할당하기 위해 string 형으로 List에 저장</param>
    /// <param name="randomMissionElements">랜덤 미션 인자들을 담는 Dictionary. 저장할 값들이 서로 자료형이 다르거나 여러 값을 저장해야 하는 경우도 존재하고, 데이터 저장을 위해 AddField시 string으로 변환해야 하기에 string으로 Dictionary에 저장</param>
    public static IMission CreateMission(ulong id, string iconId, int lifeCycleType, string nameId, string explainId, int missionType, double completeCount, int rewardType, ulong rewardAmount, List<string> missionExplainElements = null, Dictionary<RandomMission.MissionElementTypes, string> randomMissionElements = null)
    {
        ulong type = (id / 10000) % 100; ;

        switch (type)
        {
            case 01:
                return new DailyMission(id, iconId, lifeCycleType, nameId, explainId, missionType, completeCount, rewardType, rewardAmount, missionExplainElements, randomMissionElements);
            case 02:
                return new WeeklyMission(id, iconId, lifeCycleType, nameId, explainId, missionType, completeCount, rewardType, rewardAmount, missionExplainElements, randomMissionElements);
            case 03:
                return new MonthlyMission(id, iconId, lifeCycleType, nameId, explainId, missionType, completeCount, rewardType, rewardAmount, missionExplainElements, randomMissionElements);
            default:
                Debug.LogError("mission type not found. [" + id + "]");
                return null;
        }
    }

    public static void ShowAlertWindow(string title, string content, UnityAction closeAction)
    {
        UIAlertWindow.Show(title, content, closeAction);
    }

    public static void ShowAlertWindow(string title, string content, UnityAction yesAction, UnityAction noAction)
    {
        UIAlertWindow.Show(title, content, yesAction, noAction);
    }
}
