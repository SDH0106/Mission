using System.Collections.Generic;
using UnityEngine;

public class DailyMission : IMission
{
    public ulong Id { get; set; }
    public string IconId { get; set; }
    //public string Name => RandomMissionExplainElements == null ? Locale.GetText(nameId, "main") : Locale.GetTextFormat(nameId, "main", Id % 10000 + 1);
    public string Name => RandomMissionExplainElements == null ? TempDatas.MissionNameDict[nameId] : string.Format(TempDatas.MissionNameDict[nameId], Id % 10000 + 1);
    private readonly string nameId;
    //public string Explain => RandomMissionExplainElements == null ? Locale.GetText(explainId, "main") : Locale.GetTextFormat(explainId, "main", RandomMissionExplainElements.ToArray());
    public string Explain => RandomMissionExplainElements == null ? TempDatas.MissionExplainDic[explainId] : string.Format(TempDatas.MissionExplainDic[explainId], RandomMissionExplainElements.ToArray());
    private readonly string explainId;
    public MissionType Type { get; set; }
    public LifeCycleType LifeCycleType { get; set; }
    public double CompletionCount { get; set; }

    public RewardType RewardType { get; set; }
    public ulong RewardAmount { get; set; }

    public List<string> RandomMissionExplainElements { get; set; }
    public Dictionary<RandomMission.MissionElementTypes, string> RandomMissionElements { get; set; }

    double _count;
    bool _isComplete;
    bool _isReward;

    public double Count
    {
        get
        {
            GetMissionCount();
            return _count;
        }
    }
    public bool IsComplete
    {
        get
        {
            GetMissionCount();
            return _isComplete;
        }
    }
    public bool IsReward
    {
        get
        {
            GetMissionCount();
            return _isReward;
        }
    }

    public DailyMission(ulong id, string iconId, int lifeCycleType, string nameId, string explainId, int type, double completionCount, int rewardType, ulong rewardAmount, List<string> missionExplainElements = null, Dictionary<RandomMission.MissionElementTypes, string> randomMissionElements = null)
    {
        this.Id = id;
        this.IconId = iconId;
        this.nameId = nameId;
        this.explainId = explainId;
        this.Type = (MissionType)type;
        this.CompletionCount = completionCount;
        this.RewardType = (RewardType)rewardType;
        this.RewardAmount = rewardAmount;
        this.LifeCycleType = (LifeCycleType)lifeCycleType;
        this.RandomMissionExplainElements = missionExplainElements;
        this.RandomMissionElements = randomMissionElements;
    }

    public DailyMission() { }


    public double GetReward()
    {
        int lastNormalAreaId = GameData.Instance.CalculateLastUnlockedArea();

        if (RewardType == RewardType.Gem)
            return RewardAmount;
        else if (RewardType == RewardType.Coin)
            return RewardAmount;
        else
        {
            if (GameData.Instance.RandomMissions.ContainsKey(Id) && RewardType == RewardType.Relic)
                return Mathf.Clamp(10 + (lastNormalAreaId - 10) * 2.5f, 10, 50);

            return 2f * (lastNormalAreaId + 1);
        }
    }
/*
    public ExecutionStatus GetExecutionStatus()
    {
        switch (Type)
        {
            //  MainLobby
            case MissionType.CostumeCollection:
                return ExecutionStatus.GiftBox;

            //  쟁탈전
            case MissionType.ScrambleModePlay:
                return ExecutionStatus.Scramble;

            //  방어전
            case MissionType.DefenceModePlay:
                return ExecutionStatus.Defence;

            //  보스전
            case MissionType.BossModePlay:
            case MissionType.BossModeClear:
                return ExecutionStatus.Boss;

            //  PvP
            case MissionType.DualModeClear:
                return ExecutionStatus.Dual;

            //  길드전
            case MissionType.TerritoryWarModeClear:
                return ExecutionStatus.GuildWar;

            //  Raid
            case MissionType.RaidModePlay:
                return ExecutionStatus.Raid;

            //  일반모드 -> BloodyFriday 에서는 방어전 진입으로
            case MissionType.MonsterKill:
            case MissionType.TimeSlip:
            case MissionType.WeaponUpgrade:
            case MissionType.QuestComplete:
            case MissionType.QuestUpgrade:
            case MissionType.SpecificMonsterKill:
            case MissionType.ClearWithSpecificMercenary:
            case MissionType.DestroyBuilding:
            case MissionType.SideEventSuccess:
            case MissionType.MonsterKillBySpecificWeapon:
                return ExecutionStatus.Defence;

            // 그외 딱히 상태가 없는경우
            case MissionType.AllClear:
            case MissionType.Attendance:
            case MissionType.DailyMission:
            case MissionType.WeeklyMission:
            default:
                return ExecutionStatus.Null;
        }
    }*/

    public void MissionReward()
    {
        if (User.Instance.GetUserMission(Id).isReward)
            return;

        if (RewardType == RewardType.Relic)
            User.Instance.Relic += GetReward();
        else if (RewardType == RewardType.Gem)
            User.Instance.Gem += GetReward();
        else if (RewardType == RewardType.Coin)
            User.Instance.Coin += GetReward();

        User.Instance.SetMissionReward(Id, true);
    }

    public bool CheckComplete()
    {
        if (IsComplete) return true;
        if (CompletionCount <= Count)
        {
            User.Instance.SetMissionComplete(Id, true);
            GameData.Instance.CompleteMissions.Add(Id);

            IMission mission = GameData.Instance.GetMission(GameData.Instance.BasicMissions[0]);
            mission.AddCount(1);

            return true;
        }
        return false;
    }

    public void AddCount(double num)
    {
        if (IsComplete || IsReward) return;
        double addCount = Count + num;
        if (addCount > CompletionCount) addCount = CompletionCount;
        User.Instance.SetMissionCount(Id, addCount);
    }

    void GetMissionCount()
    {
        UserMission mission = User.Instance.GetUserMission(Id);

        _count = mission.count;
        _isComplete = mission.isComplete;
        _isReward = mission.isReward;
    }
}

