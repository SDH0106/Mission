using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum MissionType
{
    MissionType1, MissionType2, MissionType3, SpecificMonsterKill, ClearWithSpecificMercenary, DestroyBuilding, SideEventSuccess, MonsterKillBySpecificWeapon
}

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
    string Explain { get; }
    MissionType Type { get; set; }
    LifeCycleType LifeCycleType { get; set; }
    double CompletionCount { get; set; }

    RewardType RewardType { get; set; }
    ulong RewardAmount { get; set; }

    double Count { get; }
    bool IsComplete { get; }
    bool IsReward { get; }

    void AddCount(double num);
    void MissionReward();

    List<string> RandomMissionExplainElements { get; set; }

    Dictionary<RandomMission.MissionElementTypes, string> RandomMissionElements { get; set; }
}