using System;
using System.Collections.Generic;
using System.Linq;
using static UnityEngine.UI.Slider;
using System.Threading;

public class MissionReporter
{
    private Action<string, MissionObjective[], double> AddMissionCount;
    private Action<string, MissionObjective[], double> SetMissionCount;

    public MissionReporter(Action<string, MissionObjective[], double> AddMissionCount, Action<string, MissionObjective[], double> SetMissionCount)
    {
        this.AddMissionCount += AddMissionCount;
        this.SetMissionCount += SetMissionCount;
    }

    public void ReportMonsterKill(ulong monsterId, int chapter, int bodyId, double count)
    {
        List<MissionObjective> objectives = new List<MissionObjective>() {
            new MissionObjective(MissionConditionKeys.MonsterKill, MissionConditionKeys.Null), new MissionObjective(MissionConditionKeys.BodyId, bodyId.ToString("D10")),
            new MissionObjective(MissionConditionKeys.Chapter, chapter.ToString()), new MissionObjective(MissionConditionKeys.MonsterId, monsterId.ToString("D10")),
            new MissionObjective(MissionConditionKeys.MonsterType, (monsterId / 100000 % 10).ToString())
        };

        AddMissionCount(MissionConditionKeys.MonsterKill, objectives.ToArray(), count);
    }

    public void ReportMissionComplete(ulong missionId)
    {
        ulong missionIdSubCategory = (missionId / 10000) % 100;

        MissionObjective[] objectives = {
            new MissionObjective(MissionConditionKeys.MissionCompleteCount, MissionConditionKeys.Null),
            new MissionObjective(MissionConditionKeys.MissionIdSubCategory, missionIdSubCategory.ToString()),
            new MissionObjective(MissionConditionKeys.MissionId, missionId.ToString("D10"))
            };

        AddMissionCount(MissionConditionKeys.MissionCompleteCount, objectives, 1);

        MissionObjective[] objectives2 = {
            new MissionObjective(MissionConditionKeys.MissionCompleteAll, MissionConditionKeys.Null),
            new MissionObjective(MissionConditionKeys.MissionIdSubCategory, missionIdSubCategory.ToString())
            };

        SetMissionCount(MissionConditionKeys.MissionCompleteAll, objectives2, GetCompleteMissionCount(missionIdSubCategory));
    }

    double GetCompleteMissionCount(ulong missionCategory)
    {
        return GameData.Instance.Missions.Where(x => (x.Value.Id / 10000) % 100 == missionCategory && x.Value.IsComplete).Count();
    }

    public void ReportGameStart(int chapter, int bodyId, double count)
    {
        MissionObjective[] objectives = {
            new MissionObjective(MissionConditionKeys.GamePlay, MissionConditionKeys.Null),
            new MissionObjective(MissionConditionKeys.Chapter, chapter.ToString()),
            new MissionObjective(MissionConditionKeys.BodyId, bodyId.ToString())
        };

        AddMissionCount(MissionConditionKeys.GamePlay, objectives, count);
    }

    public void ReportGameClear(int chapter, ulong petIds, int bodyId, double count)
    {
        List<MissionObjective> objectives = new List<MissionObjective>();

        objectives.Add(new MissionObjective(MissionConditionKeys.GameClear, MissionConditionKeys.Null, 0, 0));
        objectives.Add(new MissionObjective(MissionConditionKeys.Chapter, chapter.ToString(), 0, 0));
        objectives.Add(new MissionObjective(MissionConditionKeys.PetId, petIds.ToString("D10"), 0, 0));
        objectives.Add(new MissionObjective(MissionConditionKeys.BodyId, bodyId.ToString(), 0, 0));

        AddMissionCount(MissionConditionKeys.GameClear, objectives.ToArray(), count);
    }

    public void ReportTimeSlip(int chapter, double count)
    {
        MissionObjective[] objectives = {
            new MissionObjective( MissionConditionKeys.TimeSlip, MissionConditionKeys.Null),
            new MissionObjective( MissionConditionKeys.Chapter, chapter.ToString())
        };

        AddMissionCount(MissionConditionKeys.TimeSlip, objectives, count);
    }

    public void ReportStatUpgrade(ulong statId, double count)
    {
        MissionObjective[] objectives = {
            new MissionObjective( MissionConditionKeys.StatUpgrade, MissionConditionKeys.Null),
            new MissionObjective( MissionConditionKeys.StatId, statId.ToString("D10"))
        };

        AddMissionCount(MissionConditionKeys.StatUpgrade, objectives, count);
    }

    public void ReportWeaponUpgrade(double count)
    {
        MissionObjective[] objectives = {
            new MissionObjective( MissionConditionKeys.WeaponUpgrade, MissionConditionKeys.Null)
        };

        AddMissionCount(MissionConditionKeys.WeaponUpgrade, objectives, count);
    }

    public void ReportOneMinuteElapsed(int count)
    {
        MissionObjective[] objectives = {
            new MissionObjective( MissionConditionKeys.PlayTime, MissionConditionKeys.Null)
        };

        AddMissionCount(MissionConditionKeys.PlayTime, objectives, count);
    }

    public void ReportCostumeAcquired(ulong costumeId, int costumeRarity, double count)
    {
        MissionObjective[] objectives = {
            new MissionObjective( MissionConditionKeys.CostumeAcquired, MissionConditionKeys.Null),
            new MissionObjective( MissionConditionKeys.CostumeId, costumeId.ToString("D10")),
            new MissionObjective( MissionConditionKeys.CostumeRarity, costumeRarity.ToString())
        };

        AddMissionCount(MissionConditionKeys.CostumeAcquired, objectives, count);
    }

    public void ReportTopRankReached(int rank, double count)
    {
        MissionObjective[] objectives = {
            new MissionObjective( MissionConditionKeys.RankingReached, rank.ToString())
        };

        AddMissionCount(MissionConditionKeys.RankingReached, objectives, count);
    }

    public void ReportTutorialClear()
    {
        MissionObjective[] objectives = {
            new MissionObjective( MissionConditionKeys.TutorialClear, MissionConditionKeys.Null)
        };

        AddMissionCount(MissionConditionKeys.TutorialClear, objectives, 1);
    }

    public void ReportWarpClear(int chapter)
    {
        MissionObjective[] objectives = {
            new MissionObjective( MissionConditionKeys.Warp, MissionConditionKeys.Null),
            new MissionObjective( MissionConditionKeys.Chapter, chapter.ToString())
        };

        AddMissionCount(MissionConditionKeys.Warp, objectives, 1);
    }

    public void ReportItemAcquire(ulong itemId, double count)
    {
        MissionObjective[] objectives = {
            new MissionObjective( MissionConditionKeys.ItemAcquired, itemId.ToString("D10")),
        };

        AddMissionCount(MissionConditionKeys.ItemAcquired, objectives, count);
    }

    public void ReportQuestUpgrade(ulong questId)
    {
        MissionObjective[] objectives = {
            new MissionObjective( MissionConditionKeys.QuestUpgrade, MissionConditionKeys.Null),
            new MissionObjective( MissionConditionKeys.QuestId, questId.ToString("D10"))
        };

        AddMissionCount(MissionConditionKeys.QuestUpgrade, objectives, 1);
    }

    public void ReportQuestComplete(ulong questId)
    {
        MissionObjective[] objectives = {
            new MissionObjective( MissionConditionKeys.QuestComplete, MissionConditionKeys.Null),
            new MissionObjective( MissionConditionKeys.QuestId, questId.ToString("D10"))
        };

        AddMissionCount(MissionConditionKeys.QuestComplete, objectives, 1);
    }
}
