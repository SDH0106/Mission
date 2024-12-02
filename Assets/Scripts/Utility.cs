using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Utility
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="missionExplainElements">�̼� ������ �� ���ڸ� ��� List. Format�� ���ڿ� ���� �Ҵ��ϱ� ���� string ������ List�� ����</param>
    /// <param name="randomMissionElements">���� �̼� ���ڵ��� ��� Dictionary. ������ ������ ���� �ڷ����� �ٸ��ų� ���� ���� �����ؾ� �ϴ� ��쵵 �����ϰ�, ������ ������ ���� AddField�� string���� ��ȯ�ؾ� �ϱ⿡ string���� Dictionary�� ����</param>
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
