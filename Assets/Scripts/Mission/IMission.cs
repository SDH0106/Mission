using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum RewardType
{
    Gem, Relic, Coin
}

public enum LifeCycleType
{
    infinity, Daily, Weekly, Monthly
}

public interface IMission
{
    ulong Id { get; set; }
    string IconId { get; set; }
    string Name { get; }
    string NameId { get; set; }
    string Explain { get; }
    string ExplainId { get; set; }
    LifeCycleType LifeCycle { get; set; }
    double CompleteCount { get; set; }

    RewardType RewardType { get; set; }
    ulong RewardAmount { get; set; }

    double Count { get; }
    bool IsComplete { get; }
    bool IsReward { get; }

    void AddCount(double num);
    void SetCount(double num);
    void MissionReward();

    List<string> MissionExplainElements { get; set; }

    MissionObjective[] MissionObjectives { get; set; }
}