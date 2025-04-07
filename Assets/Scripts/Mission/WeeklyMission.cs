using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WeeklyMission : IMission
{
    public ulong Id { get; set; }
    public string IconId { get; set; }
    public string Name => GetNameByNameId();
    public string NameId { get; set; }
    public string Explain => GetExplainByExplainId();
    public string ExplainId { get; set; }
    public double CompleteCount { get; set; }
    public RewardType RewardType { get; set; }
    public ulong RewardAmount { get; set; }
    public LifeCycleType LifeCycle { get; set; }
    public MissionObjective[] MissionObjectives { get; set; }
    public List<string> MissionExplainElements { get; set; }

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

    public WeeklyMission(ulong id, string iconId, int lifeCycleType, string nameId, string explainId, double completionCount, int rewardType, ulong rewardAmount, List<string> missionExplainElement, MissionObjective[] missionObjectives)
    {
        this.Id = id;
        this.IconId = iconId;
        this.NameId = nameId;
        this.ExplainId = explainId;
        this.CompleteCount = completionCount;
        this.RewardType = (RewardType)rewardType;
        this.RewardAmount = rewardAmount;
        this.LifeCycle = (LifeCycleType)lifeCycleType;
        this.MissionObjectives = missionObjectives;
        this.MissionExplainElements = missionExplainElement;
    }

    public WeeklyMission() { }

    public double GetReward()
    {
        if (MissionObjectives.Any(x => x.condition1 == MissionConditionKeys.MissionCompleteAll))
        {
            if (RewardType == RewardType.Gem)
                return RewardAmount;
            else
                return (3f * (User.Instance.Chapter + 1) * (User.Instance.Chapter + 1));
        }
        else if (MissionObjectives.Any(x => x.condition1 == MissionConditionKeys.PlayTime))
        {
            return RewardAmount;
        }
        else
        {
            if (RewardType == RewardType.Gem)
                return RewardAmount;
            else
                return 2f * (User.Instance.Chapter + 1);
        }
    }

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
        if (CompleteCount <= Count)
        {
            User.Instance.SetMissionComplete(Id, true);
            GameData.Instance.CompleteMissions.Add(Id);

            return true;
        }
        return false;
    }

    public void AddCount(double num)
    {
        if (IsComplete || IsReward) return;
        double addCount = Count + num;
        if (addCount > CompleteCount) addCount = CompleteCount;
        User.Instance.SetMissionCount(Id, addCount);
    }

    public void SetCount(double num)
    {
        if (IsComplete || IsReward) return;
        double setCount = num;
        if (setCount > CompleteCount) setCount = CompleteCount;
        User.Instance.SetMissionCount(Id, setCount);
        CheckComplete();
    }

    void GetMissionCount()
    {
        UserMission mission = User.Instance.GetUserMission(Id);

        _count = mission.count;
        _isComplete = mission.isComplete;
        _isReward = mission.isReward;
    }

    string GetNameByNameId()
    {
        if ((Id / 1000000) % 100 != 01)
            return NameId;

        return string.Format(NameId, Id % 10000 + 1);
    }

    string GetExplainByExplainId()
    {
        if (MissionExplainElements == null)
            return ExplainId;

        return string.Format(ExplainId, MissionExplainElements.ToArray());
    }
}